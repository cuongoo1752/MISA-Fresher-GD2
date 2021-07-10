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
    public class ItemCategoryService : EntityService<ItemCategory>, IItemCategoryService
    {
        #region DECLARE
        IItemCategoryRepository _itemCategoryRespository;
        #endregion

        #region CONSTRUCTOR
        public ItemCategoryService(IItemCategoryRepository itemCategoryRespository) : base(itemCategoryRespository)
        {
            _itemCategoryRespository = itemCategoryRespository;
        }
        #endregion
    }
}
