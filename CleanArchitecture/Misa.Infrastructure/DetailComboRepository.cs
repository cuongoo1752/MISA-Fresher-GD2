using Dapper;
using Dapper.Transaction;
using Microsoft.Extensions.Configuration;
using Misa.Core.Entities.Category;
using Misa.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Infrastructure
{
    public class DetailComboRepository : BaseEntityRepository<DetailCombo>, IDetailComboRepository
    {
        #region DECLARE
        string _connectionString;
        IConfiguration _configuration;
        DynamicParameters Parameters;
        #endregion

        #region CONSTRUCTOR

        public DetailComboRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            Parameters = new DynamicParameters();
            InitConnection();
        }

        public async  Task<int> DeleteInsertDetailCombo(Guid deleteComboId, List<DetailCombo> insertDetailCombos)
        {
            int rowEffect = 0;

            dbConnection.Open();

            using (var transaction = dbConnection.BeginTransaction())
            {
                //Xóa
                if(deleteComboId != Guid.Empty)
                {
                    Parameters.Add($"@m_ComboID", deleteComboId);
                    Parameters.Add($"@m_InventoryItemID", null);
                    //Thực hiện xóa danh sách đối tượng theo ID 
                    rowEffect += await transaction.ExecuteAsync($"Proc_DeleteDetailCombo", param: Parameters, commandType: CommandType.StoredProcedure);
                }

                //Thêm
                foreach (var item in insertDetailCombos)
                {
                    //Thêm tiền tố m vào tất cả các đối tượng
                    MappingProcParamatersValueWithObject(item);

                    //Thực hiện thêm đối tượng ở cơ sở dữ liệu
                    rowEffect += await transaction.ExecuteAsync($"Proc_InsertDetailCombo", param: Parameters, commandType: CommandType.StoredProcedure);
                }

                transaction.Commit();
            }

            return rowEffect;
        }

        void MappingProcParamatersValueWithObject(DetailCombo entity)
        {
            // Lấy ra properties của đối tượng:
            var properties = typeof(DetailCombo).GetProperties();

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

        #endregion
    }
}
