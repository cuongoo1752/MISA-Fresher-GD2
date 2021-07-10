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
    public class DetailComboController : EntityController<DetailCombo>
    {
        #region DECLARE
        IDetailComboRepository _detailComboRepository;
        IDetailComboService _detailComboServices;
        #endregion

        #region CONSTRUCTOR
        public DetailComboController(IDetailComboRepository detailComboRepository, IDetailComboService detailComboServices) : base(detailComboRepository, detailComboServices)
        {
            _detailComboRepository = detailComboRepository;
            _detailComboServices = detailComboServices;
        }
        #endregion
    }
}
