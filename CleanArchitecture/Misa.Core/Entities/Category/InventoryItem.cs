using Misa.Core.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities.Category
{
    public class InventoryItem : Entity
    {
        public int MyProperty { get; set; }
        public Guid InventoryItemID { get; set; }
        public string BarCode { get; set; }
        public string SKUCode { get; set; }
        public string InventoryItemName { get; set; }
        public ItemType InventoryItemType { get; set; }
        public Guid ItemCategoryID { get; set; }
        public double BuyPrice { get; set; }
        public double CostPrice { get; set; }
        public Guid UnitID { get; set; }
        public double InitStock { get; set; }
        public double MinimumStock { get; set; }
        public double MaximumStock { get; set; }
        public bool ShowInMenu { get; set; }
        public string ShowLocation { get; set; }
        public string StockLocation { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public string Size { get; set; }
        public string SizeCode { get; set; }
        public string Description { get; set; }
        public string PictureDesciption { get; set; }
        public string PictureLocation { get; set; }
        public string PictureName { get; set; }
        public Guid? PictureID { get; set; }
        public int EditMode { get; set; }
        public double Weight { get; set; }
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public Guid? ParentID { get; set; }
    }   
}
