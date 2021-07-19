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
        /// <summary>
        /// Chèn mã Code vào cơ sở dữ liệu
        /// </summary>
        /// <param name="tableName">Tên bảng</param>
        /// <param name="codeName">Tên mã Code</param>
        /// <param name="prefix">Tiền tố</param>
        /// <param name="value">Giá trị mã Code cần chèn</param>
        /// <returns>Số bản ghi thay đổi: 1 - thành công, 0 - không thay đổi</returns>
        /// Create By: LMCUONG(19/07/2021)
        public Task<int> InsertCodeMax(string tableName, string codeName, string prefix, long value);
        /// <summary>
        /// Lấy ra mã hiện tại lớn nhất trong cơ sở dữ liệu
        /// </summary>
        /// <param name="tableName">Tên mảng</param>
        /// <param name="codeName">Tên mã Code</param>
        /// <returns>Giá trị mã code lớn nhất</returns>
        /// Created By: LMCUONG(19/07/2021)
        public Task<long> GetCodeMax(string tableName, string codeName);
        /// <summary>
        /// Lấy danh sách đối tượng theo mã ParentID
        /// </summary>
        /// <param name="ParentID">mã ParentID cần tìm</param>
        /// <returns>Danh sách đối tượng</returns>
        public Task<IEnumerable<InventoryItem>> GetInventoryItemByParentID(Guid ParentID);
        /// <summary>
        /// Xóa háng hóa tất cả các loại theo danh sách ID, nếu xóa không thành công trả về ngoại lệ
        /// </summary>
        /// <param name="ListID">Danh sách ID đối tượng cần xóa</param>
        /// <returns>Số bản ghi thành công</returns>
        public Task<int> DeleteInventoryItemByListID(List<Guid> ListID);
        /// <summary>
        /// Kiểm tra mã SKU có bị trùng hay không
        /// </summary>
        /// <param name="SKUCode">Mã SKU cần kiểm tra</param>
        /// <param name="inventoryItemId">Id đối tượng với update</param>
        /// <returns>true - nếu trùng, false nếu không trùng</returns>
        public  Task<bool> CheckSKUCodeExist(string SKUCode, Guid? inventoryItemId = null);
        /// <summary>
        /// Kiểm tra mã vạch có bị trùng hay không
        /// </summary>
        /// <param name="SKUCode">Mã vạch cần kiểm tra</param>
        /// <param name="inventoryItemId">Id đối tượng với update</param>
        /// <returns>true - nếu trùng, false - nếu không trùng</returns>
        public Task<bool> CheckBarCodeExist(string BarCode, Guid? inventoryItemId = null);

        /// <summary>
        /// Thêm, Sửa, Xóa danh sách các bản ghi nhận vào
        /// </summary>
        /// <param name="insertItems">Các bản ghi thêm</param>
        /// <param name="updateItems">Các bản ghi sửa</param>
        /// <param name="deleteItems">Các bản ghi xóa</param>
        /// <returns>Sô bản ghi bị ảnh hưởng</returns>
        /// CreatedBy LMCUONG(16/07/2021)
        public Task<int> InsertUpdateDeleteMerchandise(List<InventoryItem> insertItems, List<InventoryItem> updateItems, List<InventoryItem> deleteItems);
        /// <summary>
        /// Lấy ra danh sách hàng hóa thuộc Combo có ComboID
        /// </summary>
        /// <param name="ComboID">ID của Combo cần lấy</param>
        /// <returns>Danh sách hàng hóa thuộc Combo</returns>
        public Task<IEnumerable<InventoryItem>> GetInventoryItemByComboID(Guid ComboID);
    }
}
