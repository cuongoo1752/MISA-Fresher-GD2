using Misa.Core.Entities.Category;
using Misa.Core.Interfaces.Repository;
using Misa.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
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
    }
}
