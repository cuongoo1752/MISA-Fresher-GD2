using Microsoft.AspNetCore.Mvc;
using Misa.Core.Entities;
using Misa.Core.Enum;
using Misa.Core.Exceptions;
using Misa.Core.Interfaces.Repository;
using Misa.Core.Interfaces.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace Misa.Core.Service
{
    public class EmployeeService : EntityService<Employee>, IEmployeeService
    {
        #region DECLARE
        IEmployeeRepository _employeeRespository;
        #endregion

        #region CONSTRUCTOR
        public EmployeeService(IEmployeeRepository employeeRespository) : base(employeeRespository)
        {
            _employeeRespository = employeeRespository;
        }
        #endregion

        #region METHOD
        public async Task<string> GetNewBiggestEmployeeCode()
        {
            //Lấy ra chuổi chứa mã nhân viên cao nhất trong hệ thống
            var bigEmployeeCode = await _employeeRespository.GetBiggestEmployeeCode();

            //Khởi tạo một chuỗi rỗng
            string stringNumberCode = string.Empty;

            //Tiến hàng lọc số trong chuỗi
            for (int i = 0; i < bigEmployeeCode.Length; i++)
            {
                if (Char.IsDigit(bigEmployeeCode[i]))
                    stringNumberCode += bigEmployeeCode[i];
            }

            int? numberCode = null;

            //Chuyển được lọc vào biến mới kiểu int: numberCode
            if (stringNumberCode.Length > 0)
                numberCode = int.Parse(stringNumberCode);

            //Tăng số đã lọc lên, để không bị trùng mã cũ
            numberCode++;

            //Thêm tiền tố cho mã nhân viên
            var newBiggestEmployeeCode = "NV" + numberCode.ToString();


            return newBiggestEmployeeCode;
        }

        public override async Task<bool> VadidateCustom(Employee employee, Guid? entityId = null)
        {
            //Kiểm tra mã nhân viên có trùng không
            var isDuplicateCustomerCode = false;

            //Nếu đang thêm nhân viên mới
            if (employee.EntityState == EntityState.Add)
            {
                //Kiểm tra mã với trường hợp thêm nhân viên mới
                isDuplicateCustomerCode = await _employeeRespository.CheckEmployeeCodeExist(employee.EmployeeCode);
            }
            //Nếu đang sửa nhân viên
            else
            {
                //Kiểm tra mã với trường hợp sửa nhân viên
                isDuplicateCustomerCode = await _employeeRespository.CheckEmployeeCodeExist(employee.EmployeeCode, entityId);
            }

            //Nếu mã bị trùng 
            if (isDuplicateCustomerCode == true)
            {
                //Trả lại ngoại lệ mã đã bị trùng
                throw new VadidateException(string.Format(Properties.Resources.VadidateMsg_EmployeeCodeExits, employee.EmployeeCode));
            }



            //Kiểm tra xem số điện thoại cho trùng không
            var isDuplicatePhoneNumeber = false;

            //Nếu đang thêm nhân viên mới
            if (employee.EntityState == EntityState.Add && employee.MobilePhoneNumber != null)
            {
                //Kiểm tra số điện thoại với trường thêm nhân viên
                isDuplicatePhoneNumeber = await _employeeRespository.CheckPhoneNumberExist(employee.MobilePhoneNumber);
            }
            //Nếu đang sửa nhân viên mới
            else
            {
                //Kiểm tra số điện thoại với trường hợp sửa nhân viên
                isDuplicatePhoneNumeber = await _employeeRespository.CheckPhoneNumberExist(employee.MobilePhoneNumber, entityId);
            }

            //Nếu số điện thoại bị trùng
            if (isDuplicatePhoneNumeber == true)
            {
                //Trả lại ngoại lệ số điện thoại đã bị trùng trong hệ thống
                throw new VadidateException(string.Format(Properties.Resources.VadidateMsg_PhoneNumberExits, employee.MobilePhoneNumber));
            }

            return true;
        }
    
        public  async Task<MemoryStream> ExportDataToExcel()
        {
            await Task.Yield();
            // Lấy toàn bộ dữ liệu nhân viên
            var employees = await _employeeRespository.GetAll();
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                // Tạo trang worksheet mới và đặt tên
                var workSheet = package.Workbook.Worksheets.Add("DANH SÁCH NHÂN VIÊN");

                // Gán các giá trị mặc định
                workSheet.Cells.Style.WrapText = true;
                workSheet.DefaultRowHeight = 18;

                // Kích thước độ rộng một cột có sẵn trong file Excel
                int widthOneColum = 9;
                // Kích thước độ dài một hàng có sẵn trong file Excel
                int heighOneRow = 9;
                // Định dạnh chiều rộng cột trong file Excel
                workSheet.Column(1).Width = widthOneColum * 0.75;
                workSheet.Column(2).Width = widthOneColum * 2;
                workSheet.Column(3).Width = widthOneColum * 3;
                workSheet.Column(4).Width = widthOneColum * 1.5;
                workSheet.Column(5).Width = widthOneColum * 2;
                workSheet.Column(6).Width = widthOneColum * 3.2;
                workSheet.Column(7).Width = widthOneColum * 3.2;
                workSheet.Column(8).Width = widthOneColum * 2;
                workSheet.Column(9).Width = widthOneColum * 3;
                workSheet.Column(3).AutoFit();

                // Định dạng chiều dài của 2 hàng đầu
                workSheet.Row(1).Height = heighOneRow * 2.5;
                workSheet.Row(2).Height = heighOneRow * 2.5;

                SetStyleHeaderExcel(ref workSheet);

                //Duyệt từng dòng và trả dữ liệu tương ứng
                int index = 0;
                int indexRow = 4;
                foreach (var employee in employees)
                {
                    // Gán dữ liệu từng ô từ dữ liệu khách hàng
                    workSheet.Cells[indexRow, 1].Value = (index + 1);
                    workSheet.Cells[indexRow, 2].Value = employee.EmployeeCode;
                    workSheet.Cells[indexRow, 3].Value = employee.Fullname;
                    workSheet.Cells[indexRow, 4].Value = employee.GenderName;
                    workSheet.Cells[indexRow, 5].Value = String.Format("{0:dd/MM/yyyy}", employee.DateOfBirth);
                    workSheet.Cells[indexRow, 6].Value = employee.PositionName;
                    workSheet.Cells[indexRow, 7].Value = employee.DepartmentName;
                    workSheet.Cells[indexRow, 8].Value = employee.AccountNumber;
                    workSheet.Cells[indexRow, 9].Value = employee.BankName;
                    //Căn chỉnh từng ô
                    workSheet.Cells[indexRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    workSheet.Cells[indexRow, 5].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    // Định dạng từng dòng
                    using (ExcelRange Rng = workSheet.Cells["A" + indexRow.ToString() + ":I" + indexRow.ToString()])
                    {
                        // Chỉnh viền 
                        Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        // Chỉnh chữ
                        Rng.Style.Font.Name = "Times New Roman";
                        Rng.Style.Font.Size = 11;
                        // Căn giữa theo chiều dọc
                        Rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    }
                    index++;
                    indexRow++;
                }

                // Lưu các giá trị đã gán
                package.Save();
            }
            stream.Position = 0;
            // Đặt tên cho bảng tính
            return stream;
        }

        public void SetStyleHeaderExcel(ref ExcelWorksheet workSheet)
        {
            // Định dạng 3 dòng tiêu đề đầu
            // Dòng 1
            // Căn chỉnh định dạng
            using (ExcelRange Rng = workSheet.Cells["A1:I1"])
            {
                // Căn giữa
                Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                // Chỉnh chữ
                Rng.Value = "DANH SÁCH NHÂN VIÊN";
                Rng.Merge = true;
                Rng.Style.Font.Size = 16;
                Rng.Style.Font.Bold = true;
                Rng.Style.Font.Name = "Arial";

            }
            // Dòng 2
            // Căn chỉnh định dạng
            using (ExcelRange Rng = workSheet.Cells["A2:I2"])
            {
                // Căn chỉnh định dạng
                Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                // Chỉnh chữ
                Rng.Value = "";
                Rng.Merge = true;
                Rng.Style.Font.Size = 16;
            }
            // Dòng 3
            // Căn chỉnh định dạng
            using (ExcelRange Rng = workSheet.Cells["A3:I3"])
            {
                // Căn chỉnh định dạng
                // Căn giữa 
                Rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                Rng.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                // Chỉnh chữ 
                Rng.Style.Font.Size = 10;
                Rng.Style.Font.Bold = true;
                Rng.Style.Font.Name = "Arial";
                // Chỉnh viền ngoài
                Rng.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                Rng.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                // Chỉnh màu
                Rng.Style.Fill.PatternType = ExcelFillStyle.Solid;
                Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#D8d8d8");
                Rng.Style.Fill.BackgroundColor.SetColor(colFromHex);
            }
            // Giá trị cột tiêu đề là dòng 3
            workSheet.Cells[3, 1].Value = "STT";
            workSheet.Cells[3, 2].Value = "Mã nhân viên";
            workSheet.Cells[3, 3].Value = "Tên nhân viên";
            workSheet.Cells[3, 4].Value = "Giới tính";
            workSheet.Cells[3, 5].Value = "Ngày sinh";
            workSheet.Cells[3, 6].Value = "Chức danh";
            workSheet.Cells[3, 7].Value = "Tên đơn vị";
            workSheet.Cells[3, 8].Value = "Số tài khoản";
            workSheet.Cells[3, 9].Value = "Tên ngân hàng";
        }

        public async Task<ResultPading> GetEmployeesPaging(int pageIndex, int pageSize)
        {
            //Lấy ra toàn bộ danh sách nhân viên
            Employee[] employees = (await _employeeRespository.GetAll()).Cast<Employee>().ToArray();

            //Số lượng nhân viên 
            var lengthEmployees = employees.Length;

            //Lấy số lượng phần tử của page hiện tại
            //Đặt mặc định là số lượng pageSize
            var sumIndex = pageSize;
            //Nếu số phần hiện tại không đủ một trang pageSize
            if (pageIndex * pageSize > lengthEmployees)
            {
                sumIndex = lengthEmployees - pageSize * (pageIndex - 1);
            }

            //Lấy thứ tự index đầu tiên của mảng 
            var currPageIndex = (pageIndex - 1) * pageSize;

            //Mảng phần chỉ có trong phân trang
            Employee[] employeesPaging = new Employee[sumIndex];

            //Tiến hàng Copy 1 phần mảng toàn bộ danh sách nhân viên sang mảng chỉ chứa các phần tử trong một trang
            Array.Copy(employees, currPageIndex, employeesPaging, 0, sumIndex);

            //Lưu kết quả vào một biến chứa 2 trường: Data và tổng số lượng phần tử
            var result = new ResultPading(employeesPaging, employees.Count());

            return result;

        }

        public async Task<ResultPading> GetSearchPaging(string keyWord, int pageIndex, int pageSize)
        {
            //Lấy ra toàn bộ danh sách nhân viên được tìm kiếm theo từ khóa
            Employee[] employees = (await _employeeRespository.GetSearch(keyWord)).Cast<Employee>().ToArray();
            
            //Số lượng nhân viên được tìm kiếm
            var lengthEmployees = employees.Length;

            //Lấy số lượng phần tử của page hiện tại
            //Đặt mặc định là số lượng pageSize
            var sumIndex = pageSize;
            //Nếu số phần hiện tại không đủ một trang pageSize
            if (pageIndex * pageSize > lengthEmployees)
            {
                sumIndex = lengthEmployees - pageSize * (pageIndex - 1);
            }

            //Lấy thứ tự index đầu tiên của mảng 
            var currPageIndex = (pageIndex - 1) * pageSize;

            //Mảng phần chỉ có trong phân trang
            Employee[] employeesPaging = new Employee[sumIndex];

            //Tiến hàng Copy 1 phần mảng toàn bộ danh sách nhân viên sang mảng chỉ chứa các phần tử trong một trang
            Array.Copy(employees, currPageIndex, employeesPaging, 0, sumIndex);

            //Lưu kết quả vào một biến chứa 2 trường: Data và tổng số lượng phần tử
            var result = new ResultPading(employeesPaging, employees.Count());

            return result;
        }
        #endregion
    }
}
