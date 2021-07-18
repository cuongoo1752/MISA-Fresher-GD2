using Misa.Core.Entities.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Interfaces.Repository
{
    public interface IDetailComboRepository : IEntityRespository<DetailCombo>
    {
        /// <summary>
        /// Xóa và thêm mới các đối tượng DetailCombo
        /// </summary>
        /// <param name="deleteComboId">Id Combo các đối tượng cần xóa</param>
        /// <param name="insertDetailCombos">List DetailCombo cần thêm mới</param>
        /// <returns>Số bản ghi bị ảnh hưởng</returns>
        /// Created By: LMCUONG(16/07/2021 )
        public Task<int> DeleteInsertDetailCombo(Guid deleteComboId, List<DetailCombo> insertDetailCombos);
    }
}
