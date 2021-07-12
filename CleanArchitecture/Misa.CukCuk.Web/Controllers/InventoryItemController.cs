using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Core.Entities;
using Misa.Core.Entities.Category;
using Misa.Core.Entities.Page;
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

            if(result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
