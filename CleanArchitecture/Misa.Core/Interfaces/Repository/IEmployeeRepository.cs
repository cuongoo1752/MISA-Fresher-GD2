using Misa.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Interfaces.Repository
{
    public interface IEmployeeRepository:IEntityRespository<Employee>
    {
        /// <summary>
        /// Lấy ra tất cả nhân viên được tìm kiếm theo từ khóa 
        /// Tìm kiếm theo Mã nhân viên và tên nhân viên
        /// </summary>
        /// <param name="keyWord">Từ khóa nhập vào</param>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: LMCUONG(10/06/2021)
        Task<IEnumerable<Employee>> GetSearch(string keyWord);
        /// <summary>
        /// Lấy ra tất cả nhân viên được tìm kiếm theo từ khóa 
        /// Tìm kiếm theo Mã nhân viên và tên nhân viên
        /// Danh sách nhân viên được giới thiện theo phân trang
        /// </summary>
        /// <param name="keyWord">Từ khóa nhập vào</param>
        /// <param name="pageIndex">Thự tự trang</param>
        /// <param name="pageSize">Số lương phần tử trong trang</param>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: LMCUONG(10/06/2021)
        Task<IEnumerable<Employee>> GetSearchPaging(string keyWord, int pageIndex, int pageSize);
        /// <summary>
        /// Lấy ra tất cả nhân viên có cùng nhóm
        /// </summary>
        /// <param name="CustmerGroupId">Mã nhóm nhân viên</param>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: LMCUONG(10/06/2021)
        Task<IEnumerable<Employee>> GetEmployeeByDepartment(Guid DepartmentId);

        /// <summary>
        /// Lấy mã khách hàng lớn nhất trong hệ thống
        /// </summary>
        /// <returns>Chuỗi mã khách hàng</returns>
        /// Create By: LMCUONG(15/06/2021)
        public Task<string> GetBiggestEmployeeCode();
        /// <summary>
        /// Lấy ra danh sách nhân viên theo phân trang
        /// </summary>
        /// <param name="pageIndex">Thứ tự trang</param>
        /// <param name="pageSize">Tổng số bản ghi trên một trang</param>
        /// <returns>Danh sách nhân viên</returns>
        /// Create By: LMCUONG(15/06/2021)
        Task<IEnumerable<Employee>> GetEmployeesPaging(int pageIndex, int pageSize);
    }
}
