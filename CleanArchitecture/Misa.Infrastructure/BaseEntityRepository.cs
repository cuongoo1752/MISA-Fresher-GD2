using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Misa.Core.Entities;
using System.Reflection;

namespace Misa.Infrastructure
{
    public class BaseEntityRepository<TEntity> : IEntityRespository<TEntity>where TEntity: Entity
    {
        #region DECLARE
        string _connectionString;
        IConfiguration _configuration;
        DynamicParameters Parameters;
        string _className = typeof(TEntity).Name;
        public IDbConnection dbConnection { get; set; }
        #endregion

        #region CONSTRUCTOR 
        public BaseEntityRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            InitConnection();
        }
        #endregion

        #region METHOD
        /// <summary>
        /// Khơi tại kết nối với cở sở dữ liệu
        /// </summary>
        public void InitConnection()
        {
            Parameters = new DynamicParameters();
            dbConnection = new MySqlConnection(_connectionString);
        }

        #region Method: GetALL, GetById, GetPading
        async Task<IEnumerable<TEntity>>  IEntityRespository<TEntity>.GetAll()
        {
            //Lấy danh sách đối tượng từ cơ sở dữ liệu
            var entities = await dbConnection.QueryAsync<TEntity>($"Proc_Get{_className}s", commandType: CommandType.StoredProcedure);
            return entities;
        }

        async Task<TEntity> IEntityRespository<TEntity>.GetByid(Guid entityId)
        {
            //Thêm mã đối tượng 
            Parameters.Add($"@m_{_className}Id", entityId);

            //Lấy đối tượng từ cở sở dữ liệu bằng mã đối tượng
            var entity = await dbConnection.QueryFirstOrDefaultAsync<TEntity>($"Proc_Get{_className}ById", param: Parameters, commandType: CommandType.StoredProcedure);
            return entity;
        }

        async Task<IEnumerable<TEntity>> IEntityRespository<TEntity>.GetPaging(int pageIndex, int pageSize)
        {
            //Thêm trường thứ tự trang và số lượng trang
            Parameters.Add("@pageIndex", pageIndex);
            Parameters.Add("@pageSize", pageSize);

            //Lấy danh sách đối tượng them phân trang
            var entities = await dbConnection.QueryAsync<TEntity>($"Proc_Get{_className}sPaging", param: Parameters, commandType: CommandType.StoredProcedure);
            return entities;
        }

        public async Task<int?> Insert(TEntity entity)
        {
            //Thêm Mã mới vào mã đối tượng
            entity.GetType().GetProperty($"{_className}Id").SetValue(entity, Guid.NewGuid());
            
            //Thực hiện gán các phương thức của đối tượng và trường dữ liệu chuẩn bị truyền xuống database
            MappingProcParamatersValueWithObject(entity);
            
            //Thực hiện thêm đối tượng ở cơ sở dữ liệu
            var rowEffect = await dbConnection.ExecuteAsync($"Proc_Insert{_className}", param: Parameters, commandType: CommandType.StoredProcedure);
            return rowEffect;
        }

        public async Task<int?> Update(Guid entityId, TEntity entity)
        {
            //Thêm Mã mới vào mã đối tượng
            entity.GetType().GetProperty($"{_className}Id").SetValue(entity, entityId);

            //Thực hiện gán các phương thức của đối tượng và trường dữ liệu chuẩn bị truyền xuống database
            MappingProcParamatersValueWithObject(entity);

            //Thực hiện thêm đối tượng ở cơ sở dữ liệu
            var rowEffect = await dbConnection.ExecuteAsync($"Proc_Update{_className}", param: Parameters, commandType: CommandType.StoredProcedure);
            return rowEffect;
        }
        public async Task<int> Delete(Guid customerId)
        {
            //Thêm mã đối tượng vào biến chuẩn bị truyền xuống Procerduce
            Parameters.Add($"@m_{_className}Id", customerId);

            // Thực hiện xóa dữ liệu ở cơ sở dữ liệu
            var rowEffect = await dbConnection.ExecuteAsync($"Proc_Delete{_className}ById", param: Parameters, commandType: CommandType.StoredProcedure);
            return rowEffect;
        }
        /// <summary>
        /// Map các trường của đối tượng thành các trường của Procerduce trong cơ sở dữ liệu
        /// </summary>
        /// <param name="entity">Đối tượng</param>
        /// Created By: LMCUONG(10/06/2021)
        void MappingProcParamatersValueWithObject(TEntity entity)
        {
            // Lấy ra properties của đối tượng:
            var properties = typeof(TEntity).GetProperties();

            // Duyện từng properties
            foreach (var property in properties)
            {
                //Lấy ra value
                var value = property.GetValue(entity);

                //Lấy ra tên của Property
                var propertyName = property.Name;

                //Đặt tên cho tham số đầu vào
                var procParameterName = $"@m_{propertyName}";
                Parameters.Add(procParameterName, value);

            }
        }

        public async Task<bool> CheckEmployeeCodeExist(string entityCode, Guid? entityId = null)
        {
            //Thêm thông tin mã khách hàng vào biến chuẩn bị truyền xuống cở sở dữ liệu
            Parameters.Add($"@m_{_className}Code", entityCode);

            //Thêm thông tin mã Code khách hàng vào biến chuẩn bị truyền xuống cở sở dữ liệu
            Parameters.Add($"@m_{_className}Id", entityId);

            //Kiểm tra mã nhân viên có bị trung hay không
            var isExits = await dbConnection.ExecuteScalarAsync<bool>($"Proc_Check{_className}CodeExits", param: Parameters, commandType: CommandType.StoredProcedure);
            return isExits;
        }

        public async Task<bool> CheckPhoneNumberExist(string PhoneNumber, Guid? entityId = null)
        {
            //Thêm số điện thoại di động cần kiểm tra
            Parameters.Add("@m_MobilePhoneNumber", PhoneNumber);

            //Thêm mã nhân viên cần kiểm tra
            Parameters.Add($"@m_{_className}Id", entityId);

            var isExits = false;
            //Truy cập vào cơ sở dữ liệu kiểm tra xem mã có bị trùng hay không
            isExits = await dbConnection.ExecuteScalarAsync<bool>($"Proc_Check{_className}PhoneNumberExits", param: Parameters, commandType: CommandType.StoredProcedure);
            return isExits;
        }
        #endregion

        #endregion
    }
}
