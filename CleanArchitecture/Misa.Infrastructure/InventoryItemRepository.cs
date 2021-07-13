using Dapper;
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




        #endregion
    }
}
