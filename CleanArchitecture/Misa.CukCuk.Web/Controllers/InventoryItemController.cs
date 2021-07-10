using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Core.Entities.Category;
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
    public class InventoryItemController : EntityController<InventoryItem>
    {
        #region DECLARE
        IInventoryItemRepository _inventoryItemRepository;
        IInventoryItemService _inventoryItemServices;
        #endregion

        #region CONSTRUCTOR
        public InventoryItemController(IInventoryItemRepository inventoryItemRepository, IInventoryItemService inventoryItemServices) : base(inventoryItemRepository, inventoryItemServices)
        {
            _inventoryItemRepository = inventoryItemRepository;
            _inventoryItemServices = inventoryItemServices;
        }
        #endregion
    }
}
