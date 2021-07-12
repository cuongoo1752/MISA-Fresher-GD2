using Misa.Core.Entities;
using Misa.Core.Entities.Category;
using Misa.Core.Entities.Page;
using Misa.Core.Interfaces.Repository;
using Misa.Core.Interfaces.Services;
using System;
using System.Linq;
using System.Text;
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
        #endregion 
    }
}
