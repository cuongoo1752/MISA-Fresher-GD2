using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Core.Entities;
using Misa.Core.Entities.Category;
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
        [HttpGet]
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
        [HttpGet("CodeMax")]
        public async Task<IActionResult> GetSKUCode(string InventoryItemName)
        {
            string SKUCodeNew = await _inventoryItemServices.CreateSKUCodeMax(InventoryItemName);
            if (string.IsNullOrEmpty(SKUCodeNew))
            {
                return BadRequest(new ActionServiceResult()
                {
                    Data = SKUCodeNew,
                    Success = false,
                    Code = Core.Enum.MISACode.Validate,
                    Messenge = "Mã không hợp lệ!"
                }) ;
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


        [HttpGet("{Id}")]
        public async Task<IActionResult> GetInventoryItemById(Guid Id, ItemType type)
        {
            switch (type)
            {
                case ItemType.Merchandise:
                    break;
                case ItemType.Combo:
                    break;
                case ItemType.Service:
                    break;
                default:
                    break;
            }


            return Ok();
        }
    }
}
