using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Core.Entities;
using Misa.Core.Interfaces.Repository;
using Misa.Core.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Misa.CukCuk.Web.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class EmployeeController : EntityController<Employee>
    {
        #region DECLARE
        IEmployeeRepository _employeeRepository;
        IEmployeeService _employeeServices;
        #endregion

        #region CONSTRUCTOR
        public EmployeeController(IEmployeeRepository employeeRepository, IEmployeeService employeeServices) : base(employeeRepository, employeeServices)
        {
            _employeeRepository = employeeRepository;
            _employeeServices = employeeServices;
        }
        #endregion

        #region METHOD
        /// <summary>
        /// Lấy ra danh sách nhân viên dựa vào từ khóa tìm kiểm
        /// Tìm kiểm dựa vào trường Mã nhân viên và họ tên
        /// </summary>
        /// <param name="keyWord">từ khóa tìm kiếm</param>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: LMCUONG(10/06/2021)
        [HttpGet("search")]
        public async Task<IActionResult> GetSearch(string keyWord)
        {
            //Lấy ra danh sách nhân viên thỏa mãn theo từ khóa
            var employees = await _employeeRepository.GetSearch(keyWord);

            //Nếu có danh sách nhân viên khác rỗng
            if (employees.Count() > 0)
            {
                //Trả về danh sách nhân viên
                return Ok(employees);
            }
            //Nếu không trả về không có dữ liệu
            return NoContent();
        }
        /// <summary>
        /// Tìm kiểm nhân viên theo từ khóa và phân trang
        /// Tìm kiểm dựa vào trường Mã nhân viên và họ tên
        /// Phân trang dựa trang thứ tự trang và số lượng phần tử mỗi trang
        /// </summary>
        /// <param name="keyWord">Từ khóa</param>
        /// <param name="pageIndex">Thứ tự trang</param>
        /// <param name="pageSize">Số lượng phần tử mỗi trang</param>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: LMCUONG(10/06/2021)
        [HttpGet("searchPaging")]
        public async Task<IActionResult> GetSearchPaging(string keyWord, int pageIndex, int pageSize)
        {
            var result = await _employeeServices.GetSearchPaging(keyWord, pageIndex, pageSize);
            if (result != null)
            {
                return Ok(result);
            }
            return NoContent();
        }
        /// <summary>
        /// Lấy danh sách nhân viên cùng nhóm nhân viên
        /// </summary>
        /// <param name="Department">Mã nhóm nhân viên</param>
        /// <returns>Danh sách nhân viên</returns>
        /// Created By: LMCUONG(10/06/2021)
        [HttpGet("Department/{DepartmentId}")]
        public async Task<IActionResult> GetEmployeeByDepartment(Guid DepartmentId)
        {
            var employees = await _employeeRepository.GetEmployeeByDepartment(DepartmentId);
            if (employees.Count() > 0)
            {
                return Ok(employees);
            }
            return NoContent();
        }
        /// <summary>
        /// Lấy ra mã nhân viên mới dùng cho nhân viên tiếp theo
        /// </summary>
        /// <returns>chuỗi là mã nhân viên</returns>
        [HttpGet("NewEmployeeCode")]
        public async Task<IActionResult> GetNewEmployeeCode()
        {
            var newEmployeeCode = await _employeeServices.GetNewBiggestEmployeeCode();
            if (newEmployeeCode != null)
            {
                return Ok(newEmployeeCode);
            }
            return NoContent();
        }
        /// <summary>
        /// Lấy ra danh sách nhân viên theo phân trang
        /// </summary>
        /// <param name="pageIndex">Thứ tự trang</param>
        /// <param name="pageSize">Tổng số lượng phần tử một trang</param>
        /// <returns>Danh sách nhân viên</returns>
        [HttpGet("paging")]
        public async Task<IActionResult> GetEmloyeesByPaging(int pageIndex, int pageSize)
        {
            var result = await _employeeServices.GetEmployeesPaging(pageIndex, pageSize);
            if (result != null)
            {
                return Ok(result);
            }
            return NoContent();
        }



        /// <summary>
        /// Xuất file excel ra xuống Client
        /// </summary>
        /// <param name="cancellationToken">Biến có thể hủy hành động</param>
        /// <returns>Trả về một file Excel chứa danh sách nhân viên</returns>
        [HttpGet("exportExcel")]
        public async Task<IActionResult> ExportExcel(CancellationToken cancellationToken)
        {
            string excelName = $"Danh_sach_nhan_vien.xlsx";
            return File(await _employeeServices.ExportDataToExcel(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
        #endregion

    }
}



