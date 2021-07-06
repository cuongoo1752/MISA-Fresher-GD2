using Dapper;
using Microsoft.Extensions.Configuration;
using Misa.Core.Entities;
using Misa.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Infrastructure
{
    public class EmployeeRepository : BaseEntityRepository<Employee>, IEmployeeRepository
    {
        #region DECLARE
        string _connectionString;
        IConfiguration _configuration;
        DynamicParameters Parameters;
        #endregion

        #region CONSTRUCTOR

        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            Parameters = new DynamicParameters();
            InitConnection();
        }

        #endregion


        #region METHOD

        public async Task<string> GetBiggestEmployeeCode()
        {
            //Truy cập vào cở sở dữ liệu lấy ra mã lớn nhất trong hệ thống
            var BigEmployeeCode = await dbConnection.QueryFirstOrDefaultAsync<string>($"Proc_GetBiggestEmployeeCode", param: Parameters, commandType: CommandType.StoredProcedure);
            return BigEmployeeCode;
        }

        public async Task<IEnumerable<Employee>> GetEmployeeByDepartment(Guid DepartmentId)
        {
            //Thêm thông tin phòng ban
            Parameters.Add($"@m_DepartmentId", DepartmentId);

            //Truy cập vào cở sở dữ liệu lấy ra tất cả thông tin nhân viên có cùng phòng ban với dữ liệu gửi vào
            var employees = await dbConnection.QueryAsync<Employee>($"Proc_GetEmployeeByDepartmentId", param: Parameters, commandType: CommandType.StoredProcedure);
            return employees;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesPaging(int pageIndex, int pageSize)
        {
            //Thêm các dữ liệu phân trang
            Parameters.Add($"@m_PageIndex", pageIndex);
            Parameters.Add($"@m_PageSize", pageSize);

            //Truy cập vào cở sở dữ liệu lấy thông tin nhân viên theo phân trang
            var employees = await dbConnection.QueryAsync<Employee>($"Proc_GetEmployeePaging", param: Parameters, commandType: CommandType.StoredProcedure);
            return employees;
        }


        
        public async Task<IEnumerable<Employee>> GetSearchPaging(string keyWord, int pageIndex, int pageSize)
        {
            //Thêm các từ khóa cần tìm kiếm
            Parameters.Add($"@m_keyword", keyWord);

            //Thêm các dữ liệu phân trang
            Parameters.Add($"@m_PageIndex", pageIndex);
            Parameters.Add($"@m_PageSize", pageSize);

            //Truy cập vào cở sở dữ liệu lấy ra nhân viên có từ khóa tìm kiếm, và lọc theo phân trang
            var employees = await dbConnection.QueryAsync<Employee>($"Proc_SearchEmployeePaging", param: Parameters, commandType: CommandType.StoredProcedure);
            return employees;
        }
        async Task<IEnumerable<Employee>> IEmployeeRepository.GetSearch(string keyWord)
        {
            //Thêm từ khóa cần tìm kiếm
            Parameters.Add($"@m_keyword", keyWord);

            //Truy cập vào cở sở dữ liệu lấy ra danh sách nhân viên theo từ khóa tìm kiếm
            var employees = await dbConnection.QueryAsync<Employee>($"Proc_SearchEmployee", param: Parameters, commandType: CommandType.StoredProcedure);
            return employees;
        }



        #endregion
    }
}
