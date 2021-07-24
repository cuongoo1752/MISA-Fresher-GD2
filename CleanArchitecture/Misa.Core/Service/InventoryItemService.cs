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
        IDetailComboRepository _detailComboRepository;
        #endregion

        #region CONSTRUCTOR

        public InventoryItemService(IInventoryItemRepository inventoryItemRespository, IDetailComboRepository detailComboRepository) : base(inventoryItemRespository)
        {
            _inventoryItemRespository = inventoryItemRespository;
            _detailComboRepository = detailComboRepository;
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
                    querySQL.Append(" AND ");
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
                    queryWhere += $"= {filter.value}";
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
        public async Task<string> GetCodeMax(string tableName, string codeName)
        {
            // Lấy giá trị code lớn nhất
            long codeMax = await _inventoryItemRespository.GetCodeMax(tableName, codeName);
            // Tăng một đơn vị
            codeMax++;
            // Thêm tiền tố
            string prefix = "";
            if(codeName == Properties.Resources.SKUCode)
            {
                prefix = Properties.Resources.Prefix_SKUCode;
            }
            else if (codeName == Properties.Resources.BarCode)
            {
                prefix = Properties.Resources.Prefix_BarCode;
            }

            return prefix + codeMax.ToString();

        }

        public async Task<int> InsertCodeMax(string tableName, string codeName, string prefix, string value)
        {
            int rowAffect = 0;
            if(value.Length > 0)
            {
                // Kiểm tra tiền tố
                if(codeName == Properties.Resources.SKUCode && value.Length > 1) {
                    string prefixTemp = value.Substring(0, 2);
                    if (prefixTemp != Properties.Resources.Prefix_SKUCode)
                    {
                        return rowAffect;
                    }
                }

                // Thêm một đơn vị cho mã lớn nhất
                StringBuilder stringNumberCode = new StringBuilder("");

                //Tiến hàng lọc số trong chuỗi
                for (int i = 0; i < value.Length; i++)
                {
                    if (Char.IsDigit(value[i]))
                            stringNumberCode.Append(value[i]);
                }

                long numberCode = 0;

                //Chuyển được lọc vào biến mới kiểu int: numberCode
                if (stringNumberCode.Length > 0 && stringNumberCode.Length <= 20)
                {
                    numberCode = long.Parse(stringNumberCode.ToString());

                    rowAffect = await _inventoryItemRespository.InsertCodeMax(tableName, codeName, prefix, numberCode);
                }
                    
            }

            return rowAffect;
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
                
            }

            return new DetailItem()
            {
                inventoryItem =  inventoryItem,
                inventoryItemsColor = inventoryItemsColor,
                colors = colors,
            };
        }

        public async Task<DetailItem> GetComboByID(Guid ComboID)
        {
            // Lấy dữ liệu phần tử combo
            InventoryItem inventoryItem = await _inventoryItemRespository.GetByid(ComboID);

            // Lấy dữ liệu phần tử chi tiết trong Combo
            List<InventoryItem> inventoryItemsColor = (await _inventoryItemRespository.GetInventoryItemByComboID(ComboID)).Cast<InventoryItem>().ToList();

            return new DetailItem()
            {
                inventoryItem = inventoryItem,
                inventoryItemsColor = inventoryItemsColor,
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
                // Sinh Code cho đối tượng nếu chưa có
                if (string.IsNullOrWhiteSpace(item.SKUCode))
                {
                    item.SKUCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode);
                }
                 if (string.IsNullOrWhiteSpace(item.BarCode))
                {
                    item.BarCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode);
                }


                // Kiểm tra dữ liệu
                item.EntityState = EntityState.Add;
                if (await VadidateMerchandise(item))
                {
                    isValid = true;
                    break;
                }

                // Sinh Id cho đối tượng
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

                // Thêm mới mã CodeMax
                await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode, null, item.SKUCode);
                await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode, null, item.BarCode);

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

        public async Task<int> InsertCombos(DetailItem detailItem)
        {
            int rowAffect = 0;
            bool isValid = false;

            //Tạo mã Guid
            detailItem.inventoryItem.InventoryItemID = Guid.NewGuid();

            // Sinh Code cho đối tượng nếu chưa có
            if (string.IsNullOrWhiteSpace(detailItem.inventoryItem.SKUCode))
            {
                detailItem.inventoryItem.SKUCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode);
            }
             if (string.IsNullOrWhiteSpace(detailItem.inventoryItem.BarCode))
            {
                detailItem.inventoryItem.BarCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode);
            }

            // Vadidate
            detailItem.inventoryItem.EntityState = EntityState.Add;
            if(await VadidateMerchandise(detailItem.inventoryItem))
            {
                isValid = true;
            }

            if(isValid == false)
            {
                // Chuyển List inventoryitem sang detailcombo
                List<DetailCombo> detailCombos = ConvertListInventoryItemToListDetailCombo(detailItem.inventoryItem.InventoryItemID, detailItem.inventoryItemsColor);

                // Thêm mới mã CodeMax
                await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode, null, detailItem.inventoryItem.SKUCode);
                await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode, null, detailItem.inventoryItem.BarCode);

                // Thêm Combo chính
                rowAffect += await _inventoryItemRespository.Insert(detailItem.inventoryItem) ?? 0;

                // Thực hiện thêm dữ liệu DetailCombo
                rowAffect += await _detailComboRepository.DeleteInsertDetailCombo(new Guid(), detailCombos);
            }

            return rowAffect;
        }


        public List<DetailCombo> ConvertListInventoryItemToListDetailCombo(Guid ComboID, List<InventoryItem> inventoryItems)
        {
            List<DetailCombo> detailCombos = new List<DetailCombo>();

            foreach (var item in inventoryItems)
            {
                detailCombos.Add(new DetailCombo()
                {
                    ComboID = ComboID,
                    InventoryItemID = item.InventoryItemID,
                    Part = item.Part,
                    IsUse = item.IsUse,
                    Quantity = item.Quantity,
                }) ;
            }

            return detailCombos;
        }
        #endregion

        #region PUT
        public async Task<int> UpdateMerchandises(DetailItem detailItem)
        {
            bool isValid = false;
            int rowAffect = 0;

            // Gộp vào chung một mảng
            List<InventoryItem> inventoryItems = new List<InventoryItem>();
            inventoryItems.Add(detailItem.inventoryItem);
            inventoryItems.AddRange(detailItem.inventoryItemsColor);

            //Lọc dữ liệu
            // Tạo dữ liệu
            List<InventoryItem> updateItems = new List<InventoryItem>();
            // Lấy mảng hàng hóa Child cũ
            List<InventoryItem> itemsChild = (await _inventoryItemRespository.GetInventoryItemByParentID(inventoryItems[0].InventoryItemID)).Cast<InventoryItem>().ToList();
            for (int i = 1; i < inventoryItems.Count(); i++)
            {

                for (int a = 0; a < itemsChild.Count(); a++)
                {
                    if (inventoryItems[i].InventoryItemID == itemsChild[a].InventoryItemID)
                    {
                        updateItems.Add(inventoryItems[i]);
                        inventoryItems.RemoveAt(i);
                        itemsChild.RemoveAt(a);
                        i--;
                        a--;
                        break;
                    }
                }
            }

            Guid ParentID = inventoryItems[0].InventoryItemID;

            // Chuyển đối tượng chính sang mảng Update
            updateItems.Add(inventoryItems[0]);
            inventoryItems.RemoveAt(0);

            //Vadidate dữ liệu
            // Mảng thêm mới
            foreach (var item in inventoryItems)
            {
                // Sinh Code cho đối tượng nếu chưa có
                if (string.IsNullOrWhiteSpace(item.SKUCode))
                {
                    item.SKUCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode);
                }
                 if (string.IsNullOrWhiteSpace(item.BarCode))
                {
                    item.BarCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode);
                }

                item.EntityState = EntityState.Add;
                if(await VadidateMerchandise(item))
                {
                    isValid = true;
                    break;
                }

                // Thêm mới mã CodeMax
                await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode, null, item.SKUCode);
                await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode, null, item.BarCode);

                // Gán mã mới
                item.InventoryItemID = Guid.NewGuid();
                item.ParentID = ParentID;
            }
            // Mảng chỉnh sửa
            if(isValid == false)
            {

                foreach (var item in itemsChild)
                {
                    // Sinh Code cho đối tượng nếu chưa có
                    if (string.IsNullOrWhiteSpace(item.SKUCode))
                    {
                        item.SKUCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode);
                    }
                     if (string.IsNullOrWhiteSpace(item.BarCode))
                    {
                        item.BarCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode);
                    }

                    item.EntityState = EntityState.Update;
                    if (await VadidateMerchandise(item, item.InventoryItemID))
                    {
                        isValid = true;
                        break;
                    }

                    // Thêm mới mã CodeMax
                    await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode, null, item.SKUCode);
                    await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode, null, item.BarCode);

                }

                // Thêm, Sửa, Xóa các phần tử
                rowAffect = await _inventoryItemRespository.InsertUpdateDeleteMerchandise(inventoryItems, updateItems, itemsChild);
            }
           
            
            return rowAffect;
        }

        public async Task<int> UpdateCombos(DetailItem detailItem)
        {
            int rowAffect = 0;
            bool isValid = false;
            // Sinh Code cho đối tượng nếu chưa có
            if (string.IsNullOrWhiteSpace(detailItem.inventoryItem.SKUCode))
            {
                detailItem.inventoryItem.SKUCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode);
            }
             if (string.IsNullOrWhiteSpace(detailItem.inventoryItem.BarCode))
            {
                detailItem.inventoryItem.BarCode = await GetCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode);
            }

            // Vadidate
            detailItem.inventoryItem.EntityState = EntityState.Update;
            if (await VadidateMerchandise(detailItem.inventoryItem, detailItem.inventoryItem.InventoryItemID))
            {
                isValid = true;
            }

            
            if (isValid == false)
            {
                // Thêm mới mã CodeMax
                await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.SKUCode, null, detailItem.inventoryItem.SKUCode);
                await InsertCodeMax(Properties.Resources.InventoryItemTable, Properties.Resources.BarCode, null, detailItem.inventoryItem.BarCode);

                // Chuyển List inventoryitem sang detailcombo
                List<DetailCombo> detailCombos = ConvertListInventoryItemToListDetailCombo(detailItem.inventoryItem.InventoryItemID, detailItem.inventoryItemsColor);

                // Thêm đối tượng chính vào cơ sở dữ liệu
                rowAffect += await _inventoryItemRespository.Update(detailItem.inventoryItem.InventoryItemID ,detailItem.inventoryItem) ?? 0;

                // Thực hiện xóa, thêm dữ liệu
                rowAffect += await _detailComboRepository.DeleteInsertDetailCombo(detailItem.inventoryItem.InventoryItemID, detailCombos);
            }

            return rowAffect;
        }
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


        #endregion
    }
}
