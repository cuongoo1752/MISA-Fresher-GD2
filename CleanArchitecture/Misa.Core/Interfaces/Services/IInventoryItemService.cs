using Misa.Core.Entities;
using Misa.Core.Entities.Category;
using Misa.Core.Entities.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Interfaces.Services
{
    public interface IInventoryItemService : IEntityService<InventoryItem> 
    {
        /// <summary>
        /// Hàm tạo ra câu lệnh Query gửi đến Database để lấy danh sách hàng hóa theo phân trang, tìm kiếm, sắp xếp
        /// </summary>
        /// <param name="page">Dữ liệu phân trang</param>
        /// <returns>Danh sách hàng hóa</returns>
        public Task<ActionServiceResult> GetOptionPage(OptionPage inputPage);
        /// <summary>
        /// Tạo Mã SKUCode lớn nhất
        /// </summary>
        /// <param name="SKUCodeInput">Mã SKU nhận vào</param>
        /// <returns>Mã SKU lớn nhất</returns>
        public Task<string> CreateSKUCodeMax(string SKUCodeInput); 
    }
}
