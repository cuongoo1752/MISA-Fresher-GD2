using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities.Page
{
    public class Sort
    {
        public Sort(string Property, string Direction)
        {
            property = Property;
            direction = Direction;
        }
        /// <summary>
        /// Tên thuộc tính sắp xếp
        /// </summary>
        public string property { get; set; }
        /// <summary>
        /// Loại sắp xếp
        /// desc - sắp xếp giảm dần
        /// asc - sắp xếp tăng dần
        /// </summary>
        public string direction { get; set; }
    }
}
