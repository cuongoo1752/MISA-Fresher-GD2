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
    public class DetailComboService : EntityService<DetailCombo>, IDetailComboService
    {
        #region DECLARE
        IDetailComboRepository _detailComboRespository;
        #endregion

        #region CONSTRUCTOR
        public DetailComboService(IDetailComboRepository detailComboRespository) : base(detailComboRespository)
        {
            _detailComboRespository = detailComboRespository;
        }
        #endregion
    }
}
