using Misa.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.Core.Interfaces.Services
{
    public interface IEntityService<TEntity> where TEntity : Entity
    {   
        /// <summary>
        /// Kiểm tra dữ liệu nhận vào
        /// Nếu dữ liệu hợp lệ trả lại kết quả đã thêm đối tượng thành công hay thất bại
        /// </summary>
        /// <param name="entity">Đối tượng cần thêm</param>
        /// <returns>Số dòng bị ảnh hưởng trong database: 1 là thêm thành công, 0 là thêm thất bại</returns>
        /// Created By: LMCUONG(22/06/2021)
        Task<int?> Insert(TEntity entity);
        /// <summary>
        /// Kiểm tra dữ liệu nhận vào
        /// Nếu dữ liệu hợp lệ trả lại kết quả đã sửa đối tượng thành công hay thất bại
        /// </summary>
        /// <param name="entityId">Mã của đối tượng cần sửa</param>
        /// <param name="entity">Đối tượng cần thêm</param>
        /// <returns>Số dòng bị ảnh hưởng trong database: 1 là sửa thành công, 0 là sửa thất bại</returns>
        /// Created By: LMCUONG(22/06/2021)
        Task<int?> Update(Guid entityId, TEntity entity);
    }
}
