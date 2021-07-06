using Misa.Core.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Interfaces.Services
{
    public interface IEmployeeService:IEntityService<Employee>
    {
        /// <summary>
        ///Tạo ra mã nhân viên mới lớn hơn tất cả các mã nhân viên trong hệ thống
        /// </summary>
        /// <returns>Chuỗi mã nhân viên mới</returns>
        Task<string> GetNewBiggestEmployeeCode();

        /// <summary>
        /// Hàm tạo ra một luồng xuất dữ liệu thành file Excel
        /// </summary>
        /// <returns>Luồng Stream chưa dữ liệu Excels</returns>
        public Task<MemoryStream> ExportDataToExcel();

        /// <summary>
        /// Hàm trả về một Object chứa Danh sách nhân viên ở trang hiện tại và tổng tất cả nhân viên
        /// </summary>
        /// <param name="pageIndex">Thứ tự trang hiện tại</param>
        /// <param name="pageSize">Tổng số lượng phần tử của trang</param>
        /// <returns>Trả về kiểm dữ liệu chứa data là danh sách nhân viên và length là độ dài của trang</returns>
        /// Created By: LMCUONG(15/06/2021)
        Task<ResultPading> GetEmployeesPaging(int pageIndex, int pageSize);
        /// <summary>
        /// Hàm trả một Object chưa Danh sách nhân viên được tìm kiếm theo từ khóa và đang ở trang hiện tại, cùng với tổng số lượng phần tử được tìm kiểm
        /// </summary>
        /// <param name="keyWord">Từ khóa cần tìm kiếm</param>
        /// <param name="pageIndex">Thứ tự trang</param>
        /// <param name="pageSize">Tổng số lượng phần tử trong trang</param>
        /// <returns>Trả về kiểm dữ liệu chưa data là danh sách nhân viên là length là độ dài của trang</returns>
        /// Create By: LMCUONG(15/06/2021)
        Task<ResultPading> GetSearchPaging(string keyWord, int pageIndex, int pageSize);
    }
}
