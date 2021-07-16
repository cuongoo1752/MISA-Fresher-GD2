using Misa.Core.Entities;
using Misa.Core.Entities.Category;
using Misa.Core.Entities.DataController;
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
        #region GET
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
        public Task<string> GetSKUCodeMax(string SKUCodeInput);

        /// <summary>
        /// Lấy thông tin hàng hóa theo ID
        /// </summary>
        /// <param name="InventoryItemID">Mã hàng hóa cần lấy thông tin</param>
        /// <returns>Chi tiết hàng hóa gồm: Hàng hóa chính, hàng hóa các màu, Các màu sắc trong hàng hóa</returns>
        public Task<DetailItem> GetMerchandiseByID(Guid InventoryItemID);
        #endregion

        #region POST
        /// <summary>
        /// Thêm mới danh sách hàng hóa thường hoặc dịch vụ 
        /// </summary>
        /// <param name="detailItem">Đối tượng chứa hàng hóa chính, các hàng hóa theo màu sắc</param>
        /// <returns>Số bản ghi thành công</returns>
        /// Created By LMCUONG(16/07/2021)
        public Task<int> InsertMerchandises(DetailItem detailItem);
        #endregion

        #region PUT
        /// <summary>
        /// Chỉnh sửa danh sách hàng hóa thường hoặc dịch vụ
        /// </summary>
        /// <param name="detailItem">Đối tượng chứa hàng hóa chính, các hàng hóa theo màu sắc cần chỉnh sửa</param>
        /// <returns>Số bản ghi thay đổi</returns>
        /// Created By LMCUONG(16/07/2021)
        public Task<int> UpdateMerchandises(DetailItem detailItem);
        #endregion

        #region DELETE
        #endregion
    }
}
