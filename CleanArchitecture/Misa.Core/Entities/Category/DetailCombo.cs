using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities.Category
{
    public class DetailCombo: Entity
    {
        public Guid ComboID { get; set; }
        public Guid InventoryItemID { get; set; }
        public int Quantity { get; set; }
        public int Part { get; set; }
        public bool IsUse { get; set; }
    }
}
