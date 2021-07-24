using Misa.Core.Enum;
using Misa.Core.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Entities.Category
{
    public class InventoryItem : Entity
    {
        
        public Guid InventoryItemID { get; set; }
        public string BarCode { get; set; }
        public string SKUCode { get; set; }
        public string InventoryItemName { get; set; }
        public ItemType InventoryItemType { get; set; }
        public Guid? ItemCategoryID { get; set; }
        public int BuyPrice { get; set; }
        public int CostPrice { get; set; }
        public Guid? UnitID { get; set; }
        public int InitStock { get; set; }
        public int MinimumStock { get; set; }
        public int MaximumStock { get; set; }
        public bool ShowInMenu { get; set; }
        public string ShowLocation { get; set; }
        public string StockLocation { get; set; }
        public string Color { get; set; }
        public string ColorCode { get; set; }
        public string Size { get; set; }
        public string SizeCode { get; set; }
        public string Description { get; set; }
        public string PictureType { get; set; }
        public string PictureBase64 { get; set; }
        public string PictureLocation { get; set; }
        public string PictureName { get; set; }
        public Guid? PictureID { get; set; }
        public int EditMode { get; set; }
        public int Weight { get; set; }
        public int Length { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Guid? ParentID { get; set; }
        public int State { get; set; }
        public int ManageType { get; set; }

        public int Quantity { get; set; }
        public int Part { get; set; }
        public bool IsUse { get; set; }


        public string InventoryItemTypeName
        {
            get {

                switch (InventoryItemType)
                {
                    case ItemType.Merchandise:
                        return "Hàng hóa";
                    case ItemType.Combo:
                        return "Combo";
                    case ItemType.Service:
                        return "Dịch vụ";
                    default:
                        return "Tất cả";
                }
            }
        }



        public string StateName
        {
            get {
                switch (State)
                {
                    case 1:
                        return "Tất cả";
                    case 2:
                        return "Đang kinh doanh";
                    case 3:
                        return "Ngừng kinh doanh";
                    default:
                        return "Tất cả";
                }
            }
        }


        public string ManageTypeName
        {
            get {
                switch (ManageType)
                {
                    case 1:
                        return "Tất cả";
                    case 2:
                        return "Lô/Hạn sử dụng";
                    case 3:
                        return "Serial IMEI";
                    case 4:
                        return "Khác";
                    default:
                        return "Khác";
                }
            }
        }


        public string  ShowInMenuName
        {
            get {
                if (ShowInMenu)
                {
                    return "Có";
                }
                else
                {
                    return "Không";
                }
            }
        }


    }
}
