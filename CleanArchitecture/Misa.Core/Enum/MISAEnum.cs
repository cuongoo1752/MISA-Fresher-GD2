using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Enum
{
    /// <summary>
    /// Trạng thái của object
    /// </summary>
    /// CreatedBy LMCUONG (23/05/2021)
    public enum EntityState
    {
        Add = 1,
        Update = 2,
        Delete = 3
    }

    public enum MISACode
    {
        Success = 200,
        /// <summary>
        /// Lỗi validate dữ liệu chung
        /// </summary>
        Validate = 400,

        /// <summary>
        /// Lỗi validate dữ liệu không hợp lệ
        /// </summary>
        ValidateEntity = 401,

        /// <summary>
        /// Lỗi validate dữ liệu do không đúng nghiệp vụ
        /// </summary>
        ValidateBussiness = 402,

        /// <summary>
        /// Lỗi Exception
        /// </summary>
        Exception = 500,

        /// <summary>
        /// Lỗi File không đúng định dạng
        /// </summary>
        FileFormat = 600,
    }

    public enum ItemType
    {
        Merchandise = 1,
        Combo = 2,
        Service = 3
    }



}
