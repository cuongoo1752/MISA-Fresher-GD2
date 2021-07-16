using Misa.Core.Entities.Category;
using Misa.Core.Entities.Category.ColorAndSize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities.DataController
{
    public class DetailItem
    {
        public InventoryItem inventoryItem { get; set; }
        public List<InventoryItem> inventoryItemsColor { get; set; }
        public List<ColorItem>? colors { get; set; }
    }
}
