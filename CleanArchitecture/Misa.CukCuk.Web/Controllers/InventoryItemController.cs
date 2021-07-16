using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Core.Entities;
using Misa.Core.Entities.Category;
using Misa.Core.Entities.DataController;
using Misa.Core.Entities.Page;
using Misa.Core.Enum;
using Misa.Core.Interfaces.Repository;
using Misa.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Misa.CukCuk.Web.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class InventoryItemController : ControllerBase
    {
        #region DECLARE
        IInventoryItemRepository _inventoryItemRepository;
        IInventoryItemService _inventoryItemServices;
        #endregion

        #region CONSTRUCTOR
        public InventoryItemController(IInventoryItemRepository inventoryItemRepository, IInventoryItemService inventoryItemServices)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryItemServices = inventoryItemServices;
        }
        #endregion
        /// <summary>
        /// Trả về danh sách hàng hóa theo phân trang, tìm kiếm, sắp xếp
        /// </summary>
        /// <param name="page">Dữ liệu chứa đầy đủ các thông tin phân trang tìm kiếm, sắp xếp</param>
        /// <returns>Danh sách hàng hóa</returns>
        /// Created By: LMCUONG(12/07/2021)
        [HttpPost]
        public async Task<IActionResult> GetOptiopPage([FromBody] OptionPage page)
        {

            ActionServiceResult result = await _inventoryItemServices.GetOptionPage(page);

            if (result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        /// <summary>
        /// Xử lý tên hàng hóa từ client và trả về mã SKUCode tương ứng
        /// </summary>
        /// <param name="InventoryItemName">Tên hàng hóa gửi lên</param>
        /// <returns>Kết quả chưa mã SKUCode tương ứng</returns>
        [HttpPost("CodeMax")]
        public async Task<IActionResult> GetSKUCode(string InventoryItemName)
        {
            string SKUCodeNew = await _inventoryItemServices.GetSKUCodeMax(InventoryItemName);
            if (string.IsNullOrEmpty(SKUCodeNew))
            {
                return BadRequest(new ActionServiceResult()
                {
                    Data = SKUCodeNew,
                    Success = false,
                    Code = Core.Enum.MISACode.Validate,
                    Messenge = "Mã không hợp lệ!"
                });
            }
            else
            {
                return Ok(new ActionServiceResult()
                {
                    Data = SKUCodeNew,
                    Success = true,
                    Code = Core.Enum.MISACode.Success
                });
            }
        }


        [HttpPost("{Id}")]
        public async Task<IActionResult> GetInventoryItemById(Guid Id, ItemType type)
        {
            DetailItem result = new DetailItem();
            switch (type)
            {
                case ItemType.Merchandise:
                    result = await _inventoryItemServices.GetMerchandiseByID(Id);

                    if(result.inventoryItem != null)
                    {
                        return Ok(new ActionServiceResult()
                        {
                            Success = true,
                            Code = MISACode.Success,
                            Data = result,
                            Messenge = "Lấy dữ liệu thành công"
                        });
                    }
                    else
                    {
                        return BadRequest(new ActionServiceResult()
                        {
                            Success = false,
                            Code = MISACode.Exception,
                            Data = result,
                            Messenge = "Lấy dữ liệu không thành công!"
                        });
                    }

                case ItemType.Combo:
                    return Ok();
                case ItemType.Service:

                    result = await _inventoryItemServices.GetMerchandiseByID(Id);

                    if (result.inventoryItem != null)
                    {
                        return Ok(new ActionServiceResult()
                        {
                            Success = true,
                            Code = MISACode.Success,
                            Data = result,
                            Messenge = "Lấy dữ liệu thành công"
                        });
                    }
                    else
                    {
                        return BadRequest(new ActionServiceResult()
                        {
                            Success = false,
                            Code = MISACode.Exception,
                            Data = result,
                            Messenge = "Lấy dữ liệu không thành công!"
                        });
                    }
                default:
                    break;
            }

            return Ok();

        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteInventoryItemByListID([FromBody] List<Guid> listID)
        {
            int rowAffect = await _inventoryItemRepository.DeleteInventoryItemByListID(listID);

            return Ok(new ActionServiceResult()
            {
                Success = true,
                Data = new
                {
                    rowAffect = rowAffect
                },
                    Code = MISACode.Success,
                Messenge = "Xóa thành công!"
            });
        }

        [HttpPost("InsertMerchandise")]
        public async Task<IActionResult> InsertInventoryItem([FromBody] DetailItem detailItem)
        {
            int rowAffect = await _inventoryItemServices.InsertMerchandises(detailItem);

            return Ok(new ActionServiceResult() {
                Success = true,
                Data = new
                {
                    rowAffect = rowAffect
                },
                Code = MISACode.Success,
                Messenge = "Thêm mới thành công!"
            });
        }
    }
}
