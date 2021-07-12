using Misa.Core.Entities.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Interfaces.Repository
{
    public interface IInventoryItemRepository:IEntityRespository<InventoryItem>
    {
        /// <summary>
        /// Trả về danh sách hàng hóa từ câu lệnh Query trong Database
        /// </summary>
        /// <param name="query">Câu lệnh Query</param>
        /// <returns>Danh sách hàng hóa</returns>
        /// Created by: LMCUONG(12/07/2021)
        public Task<IEnumerable<InventoryItem>> GetInventoryItemsByQuery(string query);
    }
}
