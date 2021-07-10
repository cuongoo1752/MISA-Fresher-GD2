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
    public  class UnitService : EntityService<Unit>, IUnitService
    {
        #region DECLARE
        IUnitRepository _unitRespository;
        #endregion

        #region CONSTRUCTOR
        public UnitService(IUnitRepository unitRespository) : base(unitRespository)
        {
            _unitRespository = unitRespository;
        }
        #endregion
    }
}
