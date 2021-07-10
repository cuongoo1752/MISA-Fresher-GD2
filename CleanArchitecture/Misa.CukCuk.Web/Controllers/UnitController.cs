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
    public class UnitController : EntityController<Unit>
    {
        #region DECLARE
        IUnitRepository _unitRepository;
        IUnitService _unitServices;
        #endregion

        #region CONSTRUCTOR
        public UnitController(IUnitRepository unitRepository, IUnitService unitServices) : base(unitRepository, unitServices)
        {
            _unitRepository = unitRepository;
            _unitServices = unitServices;
        }
        #endregion
    }
}
