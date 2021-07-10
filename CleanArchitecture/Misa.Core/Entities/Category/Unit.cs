using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities.Category
{
    public class Unit : Entity
    {
        public Guid UnitID { get; set; }
        public string UnitName { get; set; }
        public string Description { get; set; }
        public int EditMode { get; set; }
    }
}
