using Dapper;
using Dapper.Transaction;
using Microsoft.Extensions.Configuration;
using Misa.Core.Entities.Category;
using Misa.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Infrastructure
{
    public class InventoryItemRepository : BaseEntityRepository<InventoryItem>, IInventoryItemRepository
    {
        #region DECLARE
        string _connectionString;
        IConfiguration _configuration;
        DynamicParameters Parameters;
        #endregion

        #region CONSTRUCTOR

        public InventoryItemRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            Parameters = new DynamicParameters();
            InitConnection();
        }

        public async Task<IEnumerable<InventoryItem>> GetInventoryItemsByQuery(string query)
        {
            Parameters.Add($"m_queryInput", query);
            var inventoryItems = await dbConnection.QueryAsync<InventoryItem>($"Proc_GetInventoryItemsByQuery", param: Parameters, commandType: CommandType.StoredProcedure);
            return inventoryItems;
        }

        public async Task<string> GetSKUCodeMax(string prefix)
        {
            Parameters.Add($"m_SKUCode", prefix);
            string SKUCodeMax = await dbConnection.ExecuteScalarAsync<string>($"Proc_GetSKUCodeMax", param: Parameters, commandType: CommandType.StoredProcedure);
            return SKUCodeMax;
        }

        public async Task<IEnumerable<InventoryItem>> GetInventoryItemByParentID(Guid ParentID)
        {
            Parameters.Add($"m_ParentID", ParentID);
            var inventoryItems = await dbConnection.QueryAsync<InventoryItem>($"Proc_GetInventoryItemByParentID", param: Parameters, commandType: CommandType.StoredProcedure);
            return inventoryItems;
        }

        public async Task<int> DeleteInventoryItemByListID(List<Guid> ListID)
        {
            int rowEffect = 0;
            // This called method will get a connection, and open it if it's not yet open.
            
                dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {
                    foreach (var ID in ListID)
                    {
                        Parameters.Add($"@m_InventoryItemID", ID);
                        rowEffect += await transaction.ExecuteAsync($"Proc_DeleteInventoryItem", param: Parameters, commandType: CommandType.StoredProcedure);
                    }

                    transaction.Commit();
                }

            
            //Thêm mã đối tượng vào biến chuẩn bị truyền xuống Procerduce
            

            // Thực hiện xóa dữ liệu ở cơ sở dữ liệu
            return rowEffect;
        }

        public async Task<bool> CheckSKUCodeExist(string SKUCode, Guid? inventoryItemId = null)
        {
            //Thêm thông tin mã khách hàng vào biến chuẩn bị truyền xuống cở sở dữ liệu
            Parameters.Add($"@m_SKUCode", SKUCode);

            //Thêm thông tin mã Code khách hàng vào biến chuẩn bị truyền xuống cở sở dữ liệu
            Parameters.Add($"@m_InventoryItemId", inventoryItemId);

            //Kiểm tra mã nhân viên có bị trung hay không
            var isExits = await dbConnection.ExecuteScalarAsync<bool>($"Proc_CheckSKUCodeExits", param: Parameters, commandType: CommandType.StoredProcedure);
            return isExits;
        }

        public async Task<bool> CheckBarCodeExist(string BarCode, Guid? inventoryItemId = null)
        {
            //Thêm số điện thoại di động cần kiểm tra
            Parameters.Add("@m_BarCode", BarCode);

            //Thêm mã nhân viên cần kiểm tra
            Parameters.Add($"@m_InventoryItemId", inventoryItemId);

            var isExits = false;
            //Truy cập vào cơ sở dữ liệu kiểm tra xem mã có bị trùng hay không
            isExits = await dbConnection.ExecuteScalarAsync<bool>($"Proc_CheckBarCodeExits", param: Parameters, commandType: CommandType.StoredProcedure);
            return isExits;
        }

        public async Task<int> InsertUpdateDeleteMerchandise(List<InventoryItem> insertItems, List<InventoryItem> updateItems, List<InventoryItem> deleteItems)
        {
            int rowEffect = 0;

            dbConnection.Open();

            using (var transaction = dbConnection.BeginTransaction())
            {
                //Thêm
                foreach (var item in insertItems)
                {
                    //Thực hiện gán các phương thức của đối tượng và trường dữ liệu chuẩn bị truyền xuống database
                    MappingProcParamatersValueWithObject(item);

                    //Thực hiện thêm đối tượng ở cơ sở dữ liệu
                    rowEffect += await transaction.ExecuteAsync($"Proc_InsertInventoryItem", param: Parameters, commandType: CommandType.StoredProcedure);
                }

                //Sửa
                foreach (var item in updateItems)
                {
                    //Thực hiện gán các phương thức của đối tượng và trường dữ liệu chuẩn bị truyền xuống database
                    MappingProcParamatersValueWithObject(item);

                    //Thực hiện thêm đối tượng ở cơ sở dữ liệu
                    rowEffect += await transaction.ExecuteAsync($"Proc_UpdateInventoryItem", param: Parameters, commandType: CommandType.StoredProcedure);
                }

                //Xóa
                foreach (var item in deleteItems)
                {
                    Parameters.Add($"@m_InventoryItemID", item.InventoryItemID);
                    rowEffect += await transaction.ExecuteAsync($"Proc_DeleteInventoryItem", param: Parameters, commandType: CommandType.StoredProcedure);
                }

                transaction.Commit();
            }

            // Thực hiện xóa dữ liệu ở cơ sở dữ liệu
            return rowEffect;
        }

        void MappingProcParamatersValueWithObject(InventoryItem entity)
        {
            // Lấy ra properties của đối tượng:
            var properties = typeof(InventoryItem).GetProperties();

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

        public async Task<IEnumerable<InventoryItem>> GetInventoryItemByComboID(Guid ComboID)
        {
            Parameters.Add($"@m_ComboID", ComboID);

            var inventoryItems = await dbConnection.QueryAsync<InventoryItem>($"Proc_GetInventoryItemsByComboID", param: Parameters, commandType: CommandType.StoredProcedure);

            return inventoryItems;
        }

        #endregion
    }
}
