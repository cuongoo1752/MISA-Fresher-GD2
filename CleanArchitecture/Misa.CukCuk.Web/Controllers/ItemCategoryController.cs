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
    public class ItemCategoryController : EntityController<ItemCategory>
    {
        #region DECLARE
        IItemCategoryRepository _itemCategoryRepository;
        IItemCategoryService _itemCategoryServices;
        #endregion

        #region CONSTRUCTOR
        public ItemCategoryController(IItemCategoryRepository itemCategoryRepository, IItemCategoryService itemCategoryServices) : base(itemCategoryRepository, itemCategoryServices)
        {
            _itemCategoryRepository = itemCategoryRepository;
            _itemCategoryServices = itemCategoryServices;
        }
        #endregion
    }
}
