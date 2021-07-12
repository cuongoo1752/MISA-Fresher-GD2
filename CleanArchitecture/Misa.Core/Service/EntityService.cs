using Misa.Core.Attributes;
using Misa.Core.Entities;
using Misa.Core.Enum;
using Misa.Core.Exceptions;
using Misa.Core.Interfaces.Repository;
using Misa.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Misa.Core.Service
{
    public class EntityService<TEntity> : IEntityService<TEntity> where TEntity: Entity
    {
        #region DECLARE
        IEntityRespository<TEntity>  _baseRespository;
        #endregion

        #region CONSTRUCTOR
        public EntityService(IEntityRespository<TEntity> baseRespository)
        {
            _baseRespository = baseRespository;
        }
        #endregion

        #region METHOD
        public async Task<int?> Insert(TEntity entity)
        {
            //Kiểm tra dữ liệu hợp lệ hay không
            if(await VadidateObject(entity) == true)
            {
                // Nếu hợp lệ chuyển thông tin xuống Respository
                return await _baseRespository.Insert(entity);

            }

            // Nếu không hợp trả về null
            return null;
        }


        public async Task<int?> Update(Guid entityId, TEntity entity)
        {

            //Kiểm tra dữ liệu hợp lệ hay không
            if (await VadidateObject(entity, entityId) == true)
            {
                //Nếu dữ liệu hợp lệ, chuyển thông tin xuống Respository
                return await _baseRespository.Update(entityId, entity);
            }

            //Nếu không hợp lệ trả về null
            return null;
        }
        /// <summary>
        /// Kiểm tra tính hợp lệ của dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng cần kiểm tra thông tin</param>
        /// <param name="entityId">Mã của đối tượng(nếu có)</param>
        /// <returns>Trả về true - nếu hợp lệ, false - nếu không hợp lệ</returns>
        /// Created By: LMCUONG(19/06/2021)
        private async Task<bool> VadidateObject(TEntity entity, Guid? entityId = null)
        {
            //Biến kết quả kiểm tra dữ liệu
            var isValid = true;

            //Lấy các phương thức trong đối tượng
            var properties = entity.GetType().GetProperties();

            //Duyệt từng phương thức trong đối tượng
            foreach (var prop in properties)
            {
                //Giá trị của phương thức đang xét
                var propValue = prop.GetValue(entity);

                //Tên của phương thức đang xét
                var propName = prop.Name;

                //Kiểu dữ liệu của phương thức đang xét
                var propType = prop.PropertyType;

                //Lấy ra các trường đã đánh dấu cần kiểm tra check rỗng
                var classAttributeRequired = prop.GetCustomAttributes(typeof(MISARequired), true);

                //Lấy ra trường email cần format
                var classAtttributeEmailFormat = prop.GetCustomAttributes(typeof(MISAEmailFormat), true);

                //Lấy ra trường cần đánh 
                var classAttributeMaxLength = prop.GetCustomAttributes(typeof(MISAMaxLength), true);

                //Kiểm tra thông tin bắt buộc nhập
                if (classAttributeRequired.Length > 0)
                {
   

                    //Kiểm tra dữ liệu thời gian
                    if (propType == typeof(DateTime?) && propValue == null)
                    {
                        isValid = false;
                    }

                    //Kiểm tra dữ liệu dạng chuỗi
                    else if (propType == typeof(string) && (propValue.ToString() == string.Empty || propValue == null))
                    {
                        isValid = false;
                    }

                    //Các kiểm tra các trường còn lại
                    else if(propValue == null)
                    {
                        isValid = false;
                    }

                    //Nếu có trường cần xét có giá trị null
                    if (isValid == false)
                    {
                        //Trả lại ngoại lệ thông báo Trường dữ liệu đang trống
                        var msg = string.Format(Properties.Resources.VadidateMsg_RequiredField, Properties.Resources.ResourceManager.GetString(propName));
                        throw new VadidateException(msg, propName);
                    }


                }
                // Kiểm tra định dạng Email
                if (classAtttributeEmailFormat.Length > 0 && propType == typeof(string) && (propValue != null || propValue.ToString() != string.Empty))
                {
                    //Định dạng email thỏa mãn
                    isValid = Regex.IsMatch(propValue.ToString(), @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                    
                    //Nếu không mãn định dạng
                    if (isValid == false)
                    {
                        //Trả lại ngoại lệ thông báo Trường dữ liệu đang trống
                        var msg = Properties.Resources.VadidateMsg_FormatEmail;
                        throw new VadidateException(msg, propName);
                    }
                }
                //Kiểm tra độ dài của chuỗi
                if (classAttributeMaxLength.Length > 0)
                {
                    //Lấy độ dài quy định
                    var length = (classAttributeMaxLength[0] as MISAMaxLength).MaxLength;

                    //Thông báo đã xác định
                    var msg = (classAttributeMaxLength[0] as MISAMaxLength).Msg;

                    //Nếu độ dài vượt quá giới hạn
                    if (propValue.ToString().Trim().Length > length)
                    {
                        //Nếu không có thông báo xác định
                        if (string.IsNullOrEmpty(msg))
                        {
                            //Trả lại ngoại lệ về Độ dài trường quá dài 
                            msg = string.Format(Properties.Resources.VadidateMsg_MaxLength, Properties.Resources.ResourceManager.GetString(propName).ToLower(), length);
                        }
                        throw new VadidateException(msg, propName);
                    }
                }

            }



            // Kiểm tra các trường thông tin đặc thù cho từng đối tượng
            if (isValid == true)
            {
                isValid = await VadidateCustom(entity, entityId);
            }

            return isValid;
        }

        /// <summary>
        /// Kiểm tra dữ liệu các trường thông tin đặc thù của đối tượng
        /// </summary>
        /// <param name="entity">Đối tượng cần kiểm tra</param>
        /// <param name="entityId">Mã đối tượng cần kiểm tra</param>
        /// <returns>Trả về kết quả kiểm tra: true - thỏa, false - không thỏa mãn</returns>
        /// Created By: LMCUONG(15/02/2021)
        public virtual async Task<bool> VadidateCustom(TEntity entity, Guid? entityId = null)
        {
            return true;
        }
        #endregion
    }
}
