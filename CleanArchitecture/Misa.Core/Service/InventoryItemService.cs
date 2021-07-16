using Misa.Core.Entities;
using Misa.Core.Entities.Category;
using Misa.Core.Entities.Category.ColorAndSize;
using Misa.Core.Entities.DataController;
using Misa.Core.Entities.Page;
using Misa.Core.Enum;
using Misa.Core.Exceptions;
using Misa.Core.Interfaces.Repository;
using Misa.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Misa.Core.Service
{
    public class InventoryItemService : EntityService<InventoryItem>, IInventoryItemService
    {
        #region DECLARE
        IInventoryItemRepository _inventoryItemRespository;
        #endregion

        #region CONSTRUCTOR

        public InventoryItemService(IInventoryItemRepository inventoryItemRespository) : base(inventoryItemRespository)
        {
            _inventoryItemRespository = inventoryItemRespository;
        }


        #endregion

        #region METHOD

        #region GET
        public async Task<ActionServiceResult> GetOptionPage(OptionPage inputPage)
        {
            #region Xử lý câu lệnh Query
            // Xử lý và trả về câu lệnh Query SQL

            StringBuilder querySQL = new StringBuilder(" ");
            string abbrTableInventoryItem = "ii";
            string abbrTableUnit = "u";
            string abbrTableItemCategory = "ic";


            // Câu lệnh SELECT
            querySQL.Append($"SELECT {abbrTableInventoryItem}.*");
            querySQL.Append(" ");

            // Câu lệnh FROM
            querySQL.Append($"FROM InventoryItem {abbrTableInventoryItem}");
            foreach (var filter in inputPage.filter)
            {
                if (!String.IsNullOrEmpty(filter.tableReference))
                {
                    if (filter.tableReference == "ItemCategory")
                    {
                        querySQL.Append($" INNER JOIN {filter.tableReference} {abbrTableItemCategory} " +
                            $"ON {abbrTableInventoryItem}.{filter.tableReference}Id = {abbrTableItemCategory}.{filter.tableReference}Id");
                    }
                    else if (filter.tableReference == "Unit")
                    {
                        querySQL.Append($" INNER JOIN {filter.tableReference} {abbrTableUnit} " +
                            $"ON {abbrTableInventoryItem}.{filter.tableReference}Id = {abbrTableUnit}.{filter.tableReference}Id");
                    }

                }
            }


            querySQL.Append(" ");

            // Câu lệnh WHERE
            if (inputPage.filter.Count > 0)
            {
                querySQL.Append("WHERE ");
                querySQL.Append($"{abbrTableInventoryItem}.ParentID IS NULL AND ");
            }
            else
            {
                querySQL.Append("WHERE ");
                querySQL.Append($"{abbrTableInventoryItem}.ParentID IS NULL ");
            }
            int indexFilter = 0;
            foreach (var filter in inputPage.filter)
            {
                if (String.IsNullOrEmpty(filter.tableReference))
                {
                    querySQL.Append(GenerateQuerySQLWhereStateFilter(abbrTableInventoryItem, filter));
                }
                else
                {
                    if (filter.tableReference == "ItemCategory")
                    {
                        querySQL.Append(GenerateQuerySQLWhereStateFilter(abbrTableItemCategory, filter));
                    }
                    else if (filter.tableReference == "Unit")
                    {
                        querySQL.Append(GenerateQuerySQLWhereStateFilter(abbrTableUnit, filter));
                    }

                }


                if (indexFilter != inputPage.filter.Count - 1)
                {
                    querySQL.Append("AND ");
                }
                else
                {
                    querySQL.Append(" ");
                }
                indexFilter++;
            }

            // Câu lệnh GROUP BY

            // Câu lệnh HAVING

            // Câu lệnh ORDER BY
            if (inputPage.sort.Count > 0)
            {
                querySQL.Append("ORDER BY ");
            }
            int indexSort = 0;
            foreach (var sort in inputPage.sort)
            {
                //Thêm giá trị cần sắp xếp
                querySQL.Append($"{abbrTableInventoryItem}.{ sort.property} { sort.direction}");

                //Nếu là không phải lần thêm cuối cùng thì thêm dấu phẩy ở cuối
                if (indexSort != inputPage.sort.Count - 1)
                {
                    querySQL.Append(", ");
                }
                //Nếu là lần thêm cuối cùng thì xuống dòng
                else
                {
                    querySQL.Append(" ");
                }
                indexSort++;
            }
            #endregion

            var inventoryItems = (await _inventoryItemRespository.GetInventoryItemsByQuery(querySQL.ToString())).Cast<InventoryItem>().ToArray();
            var totalRow = inventoryItems.Length;
            if(totalRow > 0)
            {
                ModifiedPagging(inputPage.page, inputPage.limit, ref inventoryItems);
            }

            if(totalRow >= 0)
            {
                return new ActionServiceResult()
                {
                    Success = true,
                    Data = inventoryItems,
                    Total = totalRow,
                    Messenge = " ",
                    Code = Enum.MISACode.Success
                };
            }
            else
            {
                return new ActionServiceResult()
                {
                    Success = false,
                    Data = inventoryItems,
                    Total = totalRow,
                    Messenge = " ",
                    Code = Enum.MISACode.Exception
                };
            }

            
        }

        /// <summary>
        /// Hàm phân trang theo PageIndex và PageSize
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại</param>
        /// <param name="pageSize">Giới hạn trang hiện tại</param>
        /// <param name="inventoryItems">Mảng cần phân trang</param>
        /// Created By: LMCUONG(12/07/2021)
        public void ModifiedPagging(int pageIndex,int pageSize,ref InventoryItem[] inventoryItems)
        {

            //Số lượng nhân viên 
            var lengthInventoryItems = inventoryItems.Length;

            //Lấy số lượng phần tử của page hiện tại
            //Đặt mặc định là số lượng pageSize
            var sumIndex = pageSize;
            //Nếu số phần hiện tại không đủ một trang pageSize
            if (pageIndex * pageSize > lengthInventoryItems)
            {
                sumIndex = lengthInventoryItems - pageSize * (pageIndex - 1);
            }

            //Lấy thứ tự index đầu tiên của mảng 
            var currPageIndex = (pageIndex - 1) * pageSize;

            //Mảng phần chỉ có trong phân trang
            InventoryItem[] inventoryItemsPaging = new InventoryItem[sumIndex];

            //Tiến hàng Copy 1 phần mảng toàn bộ danh sách nhân viên sang mảng chỉ chứa các phần tử trong một trang
            Array.Copy(inventoryItems, currPageIndex, inventoryItemsPaging, 0, sumIndex);

            inventoryItems = inventoryItemsPaging;
        }

        // <summary>
        /// Xử lý câu lệnh trong Where khi Filter
        /// </summary>
        /// <param name="filter">Một biến có đầy đủ các giá trị cần Filter</param>
        /// <returns></returns>
        public string GenerateQuerySQLWhereStateFilter(string tableName, Filter filter)
        {
            string queryWhere = $"{tableName}.{ filter.property} ";
            switch (filter.type)
            {
                //Nếu là String
                case 1:
                    {
                        //Tiến hàng so sánh chuỗi
                        switch (filter.stateFilter)
                        {
                            case 1:
                                queryWhere += $"LIKE '%{filter.value}%'";
                                break;
                            case 2:
                                queryWhere += $"LIKE '{filter.value}'";
                                break;
                            case 3:
                                queryWhere += $"LIKE '{filter.value}%'";
                                break;
                            case 4:
                                queryWhere += $"LIKE '%{filter.value}'";
                                break;
                            case 5:
                                queryWhere += $"NOT LIKE '%{filter.value}%'";
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                // Nếu là số
                case 2:
                    {
                        //Tiến hàng so sánh số
                        switch (filter.stateFilter)
                        {
                            case 1:
                                queryWhere += $"= {filter.value}";
                                break;
                            case 2:
                                queryWhere += $"< {filter.value}";
                                break;
                            case 3:
                                queryWhere += $"<= {filter.value}";
                                break;
                            case 4:
                                queryWhere += $"> {filter.value}";
                                break;
                            case 5:
                                queryWhere += $">= {filter.value}";
                                break;
                            default:
                                break;
                        }
                        break;
                    }

                default:
                    break;
            }
            return queryWhere;
        }

        /// <summary>
        /// Bỏ dấu các chuỗi tiêng việt
        /// </summary>
        /// <param name="text">Chuỗi cần bỏ dấu</param>
        /// <returns>Chuỗi không có dấu, viết thường</returns>
        public string RemoveVietnameseTone(string text)
        {
            string result = text.ToLower();
            result = Regex.Replace(result, "à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ|/g", "a");
            result = Regex.Replace(result, "è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ|/g", "e");
            result = Regex.Replace(result, "ì|í|ị|ỉ|ĩ|/g", "i");
            result = Regex.Replace(result, "ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ|/g", "o");
            result = Regex.Replace(result, "ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ|/g", "u");
            result = Regex.Replace(result, "ỳ|ý|ỵ|ỷ|ỹ|/g", "y");
            result = Regex.Replace(result, "đ", "d");
            return result;
        }
        public async Task<string> GetSKUCodeMax(string SKUCodeInput)
        {
            if (string.IsNullOrEmpty(SKUCodeInput))
            {
                return "";
            }

            // Xử lý chuỗi lấy ra các chữ cái đâu tiền
            string[] words = RemoveVietnameseTone(SKUCodeInput).Split(' ');
            StringBuilder prefixSKUCode = new StringBuilder("");

            string tempWord = "";
            foreach (var word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    tempWord = word.ToUpper();
                    prefixSKUCode.Append(tempWord[0]);
                }

            }

            // Lấy mã lớn nhất từ Server
            string SKUCodeMax = await _inventoryItemRespository.GetSKUCodeMax(prefixSKUCode.ToString());
            string biggestSKUCodeNew =  "";
            if (string.IsNullOrWhiteSpace(SKUCodeMax))
            {
                biggestSKUCodeNew = prefixSKUCode + "01";
            }
            else
            {
                // Thêm một đơn vị cho mã lớn nhất
                string stringNumberCode = string.Empty;

                //Tiến hàng lọc số trong chuỗi
                for (int i = 0; i < SKUCodeMax.Length; i++)
                {
                    if (Char.IsDigit(SKUCodeMax[i]))
                        stringNumberCode += SKUCodeMax[i];
                }

                int? numberCode = null;

                //Chuyển được lọc vào biến mới kiểu int: numberCode
                if (stringNumberCode.Length > 0)
                    numberCode = int.Parse(stringNumberCode);

                //Tăng số đã lọc lên, để không bị trùng mã cũ
                numberCode++;
                if(numberCode < 10)
                {
                    //Thêm tiền tố cho mã nhân viên
                    biggestSKUCodeNew = prefixSKUCode.ToString() + "0" + numberCode.ToString();
                }
                else
                {
                    //Thêm tiền tố cho mã nhân viên
                    biggestSKUCodeNew = prefixSKUCode.ToString() + numberCode.ToString();
                }

                
            }
            

            return biggestSKUCodeNew;

        }

        public async Task<DetailItem> GetMerchandiseByID(Guid InventoryItemID)
        {
            // Lấy dữ liệu phần tử gốc
            InventoryItem inventoryItem = await _inventoryItemRespository.GetByid(InventoryItemID);

            // Lấy dữ liệu phần tử tham chiếu có size và color
            List<InventoryItem> inventoryItemsColor = (await _inventoryItemRespository.GetInventoryItemByParentID(InventoryItemID)).Cast<InventoryItem>().ToList();



            // Tạo ra tổ hợp màu sắc
            List<ColorItem> colors = new List<ColorItem>();
            List<SizeItem> sizes = new List<SizeItem>();

            foreach (var item in inventoryItemsColor)
            {
                // Thêm màu sắc
                
                if(colors.Exists(x => x.ColorCode == item.ColorCode) == false)
                {
                   colors.Add(new ColorItem()
                   {
                        Color = item.Color,
                        ColorCode = item.ColorCode,
                        EditMode = item.EditMode,
                    });
                }
                // Thêm Size
                if (sizes.Exists(x => x.SizeCode == item.SizeCode) == false)
                {
                    sizes.Add(new SizeItem()
                    {
                        Size = item.Size,
                        SizeCode = item.SizeCode,
                        EditMode = item.EditMode,
                    });
                }
            }

            return new DetailItem()
            {
                inventoryItem =  inventoryItem,
                inventoryItemsColor = inventoryItemsColor,
                colors = colors,
            };
        }

        #endregion

        #region POST
        public async Task<int> InsertMerchandises(DetailItem detailItem)
        {
            int rowAffect = 0;

            // Gộp vào chung một mảng
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            inventoryItems.Add(detailItem.inventoryItem);
            inventoryItems.AddRange(detailItem.inventoryItemsColor);
            // Vadidate dữ liệu
            bool isValid = false;
            int index = 0;
            Guid newGuid = Guid.NewGuid();
            foreach (var item in inventoryItems)
            {
                if (await VadidateMerchandise(item))
                {
                    isValid = true;
                    break;
                }

                if(index == 0)
                {
                    item.InventoryItemID = newGuid;
                    item.ParentID = null;
                }
                else
                {
                    item.InventoryItemID = Guid.NewGuid();
                    item.ParentID = newGuid;
                }
                index++;
            }

            // Cất dữ liệu
            if (isValid == false)
            {
                // Insert Dữ liệu
                List<InventoryItem> updateItems = new List<InventoryItem>();
                List<InventoryItem> deleteItems = new List<InventoryItem>();
                rowAffect = await _inventoryItemRespository.InsertUpdateDeleteMerchandise(inventoryItems, updateItems, deleteItems);

            }


            return rowAffect;
        }
        #endregion


        #region PUT
        public async Task<int> UpdateMerchandises(DetailItem detailItem)
        {
            // Gộp vào chung một mảng
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            inventoryItems.Add(detailItem.inventoryItem);
            inventoryItems.AddRange(detailItem.inventoryItemsColor);

            // Vadidate dữ liệu và gán ID dữ liệu
            bool isValid = false;
            Guid parentID = inventoryItems[0].InventoryItemID;
            int index = 0;
            foreach (var item in inventoryItems)
            {
          
                if(item.InventoryItemID != Guid.Empty && await VadidateMerchandise(item, item.InventoryItemID))
                {
                    isValid = true;
                    break;
                }
                if(item.InventoryItemID == Guid.Empty && index > 0)
                {
                    item.InventoryItemID = Guid.NewGuid();
                    item.ParentID = parentID;
                }

                index++;
            }



            //Lọc dữ liệu và cất dữ liệu
            if (isValid == false)
            {
                //Lọc dữ liệu
                  // Tạo dữ liệu
                List<InventoryItem> insertItems = new List<InventoryItem>();
                List<InventoryItem> updateItems = new List<InventoryItem>();
                List<InventoryItem> deleteItems = new List<InventoryItem>();
                  // Lấy mảng hàng hóa Child cũ
                List<InventoryItem> itemsChild = (await _inventoryItemRespository.GetInventoryItemByParentID(inventoryItems[0].InventoryItemID)).Cast<InventoryItem>().ToList();
                
                
                // Cất dữ liệu
                //->List dữ liệu thêm
                //->List dữ liệu sửa
                //->List dữ liệu xóa
            }

            



            throw new NotImplementedException();
        }
        #endregion

        /// <summary>
        /// Kiểm tra Mã SKUCode và mã vạch trùng trong hệ thống 
        /// </summary>
        /// <param name="inventoryItem">Đối tượng cần kiểm tra</param>
        /// <param name="inventoryItemId">Id đối tượng đối với update</param>
        /// <returns>false - không trùng, true - có trùng</returns>
        public async Task<bool> VadidateMerchandise(InventoryItem inventoryItem, Guid? inventoryItemId = null)
        {
            //Kiểm tra mã nhân viên có trùng không
            var isDuplicateCustomerCode = false;

            //Nếu đang thêm nhân viên mới
            if (inventoryItem.EntityState == EntityState.Add)
            {
                //Kiểm tra mã với trường hợp thêm nhân viên mới
                isDuplicateCustomerCode = await _inventoryItemRespository.CheckSKUCodeExist(inventoryItem.SKUCode);
            }
            //Nếu đang sửa nhân viên
            else
            {
                //Kiểm tra mã với trường hợp sửa nhân viên
                isDuplicateCustomerCode = await _inventoryItemRespository.CheckSKUCodeExist(inventoryItem.SKUCode, inventoryItemId);
            }

            //Nếu mã bị trùng 
            if (isDuplicateCustomerCode == true)
            {
                //Trả lại ngoại lệ mã đã bị trùng
                throw new VadidateException(string.Format(Properties.Resources.VadidateMsg_SKUCodeExits, inventoryItem.SKUCode));
            }



            //Kiểm tra xem số điện thoại cho trùng không
            var isDuplicatePhoneNumeber = false;

            //Nếu đang thêm nhân viên mới
            if (inventoryItem.EntityState == EntityState.Add && inventoryItem.BarCode != null)
            {
                //Kiểm tra số điện thoại với trường thêm nhân viên
                isDuplicatePhoneNumeber = await _inventoryItemRespository.CheckBarCodeExist(inventoryItem.BarCode);
            }
            //Nếu đang sửa nhân viên mới
            else
            {
                //Kiểm tra số điện thoại với trường hợp sửa nhân viên
                isDuplicatePhoneNumeber = await _inventoryItemRespository.CheckBarCodeExist(inventoryItem.BarCode, inventoryItemId);
            }

            //Nếu số điện thoại bị trùng
            if (isDuplicatePhoneNumeber == true)
            {
                //Trả lại ngoại lệ số điện thoại đã bị trùng trong hệ thống
                throw new VadidateException(string.Format(Properties.Resources.VadidateMsg_BarCodeExits, inventoryItem.BarCode));
            }

            return isDuplicateCustomerCode;
        }

        #endregion
    }
}
