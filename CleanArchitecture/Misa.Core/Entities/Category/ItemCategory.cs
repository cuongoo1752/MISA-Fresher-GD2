using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities.Category
{
    public class ItemCategory : Entity
    {
        public Guid ItemCategoryId { get; set; }
        public string ItemCategoryCode { get; set; }
        public string ItemCategoryName { get; set; }
        public string Description { get; set; }
        public int EditMode { get; set; }
    }
}
