using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities.Page
{
    public class Filter
    {
        public Filter(bool IsFilterRow, string Value, int StateFilter, string Property, int Type, string TableReference = null)
        {
            isFilterRow = IsFilterRow;
            value = Value;
            stateFilter = StateFilter;
            property = Property;
            type = Type;
            tableReference = TableReference;
        }
        /// <summary>
        /// Trạng thái có filter hay không?
        /// </summary>
        public bool isFilterRow { get; set; }
        /// <summary>
        /// Giá trị trị Filter
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// Trạng thái Filter nếu type là String
        /// 1 - Chứa
        /// 2 - Bằng
        /// 3 - Bắt đầu bằng
        /// 4 - Kết thúc bằng
        /// 5 - Không chứa
        /// Trạng thái Filter nếu type là Int
        /// 1 - Bằng
        /// 2 - Nhỏ hơn
        /// 3 - Nhỏ hơn hoặc bằng
        /// 4 - Lớn hơn
        /// 5 - Lớn hơn hoặc bằng
        /// </summary>
        public int stateFilter { get; set; }
        /// <summary>
        /// Tên thuộc tính được Filter
        /// </summary>
        public string property { get; set; }
        /// <summary>
        /// Kiểu dữ liệu
        /// 1 - String
        /// 2 - Int
        /// </summary>
        public int type { get; set; }
        public string tableReference { get; set; }
    }
}
