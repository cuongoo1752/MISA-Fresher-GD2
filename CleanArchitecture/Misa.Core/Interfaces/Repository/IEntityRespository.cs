using Misa.Core.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Interfaces.Repository
{
    public interface IEntityRespository<TEntity> where TEntity : Entity
    {
        #region DECLARE
        IDbConnection dbConnection { get; set; }
        #endregion

        #region METHOD
        /// <summary>
        /// Lấy ra toàn bộ danh sách đối tượng
        /// </summary>
        /// <returns>Danh sách đối tượng</returns>
        /// Created By: LMCUONG(10/06/2021)
        Task<IEnumerable<TEntity>> GetAll();
        /// <summary>
        /// Lấy ra đối tượng theo Id
        /// </summary>
        /// <param name="entityId">Id của đối tượng</param>
        /// <returns>Đối tượng</returns>
        /// Created By: LMCUONG(10/06/2021)
        Task<TEntity> GetByid(Guid entityId);
        /// <summary>
        /// Lấy ra danh sách đối tượng theo phân trang
        /// Phân trang dựa theo thứ tự trang và tổng số phần tử một trang
        /// </summary>
        /// <param name="pageIndex">Thứ tự trang</param>
        /// <param name="pageSize">Tổng số phần tử một trang</param>
        /// <returns>Danh sách đối tượgng</returns>
        /// Created By: LMCUONG(10/06/2021)
        Task<IEnumerable<TEntity>> GetPaging(int pageIndex, int pageSize);
        /// <summary>
        /// Thêm một đối tượng mới
        /// </summary>
        /// <param name="entity">Đối tượng mới</param>
        /// <returns>Số dòng ảnh hưởng: lớn hơn 0 thêm thành công, bằng 0 không thành công</returns>
        /// Created By: LMCUONG(10/06/2021)
        Task<int?> Insert(TEntity entity);
        /// <summary>
        /// Sửa đối tượng đã có trong hệ thống
        /// </summary>
        /// <param name="entityId">Mã đối tượng</param>
        /// <param name="entity">Thông tin đối tượng đã sửa</param>
        /// <returns>Số dòng ảnh hưởng: lớn hơn 0 sửa thành công, bằng 0 không thành công</returns>
        /// Created By: LMCUONG(10/06/2021)
        Task<int?> Update(Guid entityId, TEntity entity);
        /// <summary>
        /// Xóa một đối tượng trong hệ thống
        /// </summary>
        /// <param name="entityId">Id đối tượng</param>
        /// <returns>Số dòng ảnh hưởng: lớn hơn 0 xóa thành công, bằng 0 không thành công</returns>
        /// Created By: LMCUONG(10/06/2021)
        Task<int> Delete(Guid entityId);

        /// <summary>
        /// Kiểm tra mã khách hàng đã tồn tại hay chưa
        /// </summary>
        /// <param name="customerCode">Mã khách hàng</param>
        /// <returns>true - chỉ kết quả đã tồn tại</returns>
        /// CreateBy LMCUONG(23/05/2021)
        Task<bool> CheckEmployeeCodeExist(string entityCode, Guid? entityId = null);
        /// <summary>
        /// Kiểm tra số điện thoại đã tồn tại hay chưa
        /// </summary>
        /// <param name="phoneNumber">Số điện thoại</param>
        /// <returns>true - chỉ kết quả đã tồn tại</returns>
        /// CreateBy LMCUONG(23/05/2021)
        Task<bool> CheckPhoneNumberExist(string PhoneNumber, Guid? entityId = null);
        #endregion
    }
}
