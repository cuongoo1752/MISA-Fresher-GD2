using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.Core.Entities;
using Misa.Core.Enum;
using Misa.Core.Interfaces.Repository;
using Misa.Core.Interfaces.Services;
using Misa.CukCuk.Web.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Misa.CukCuk.Web.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class EntityController<TEntity> : ControllerBase, IEntityController<TEntity> where TEntity: Entity
    {
        #region DECLARE
        IEntityRespository<TEntity> _entityRepository;
        IEntityService<TEntity> _entityServices;
        #endregion

        #region CONSTRUCTOR
        public EntityController(IEntityRespository<TEntity> entityRepository, IEntityService<TEntity> entityServices)
        {
            _entityRepository = entityRepository;
            _entityServices = entityServices;
        }
        #endregion

        #region METHOD
        /// <summary>
        /// Lấy ra toàn hộ danh sách đối tượng
        /// </summary>
        /// <returns> Thành công: Danh sách đối tượng, thất bại trả về lỗi</returns>
        /// Created By: LMCUONG(10/06/2021)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var entities = await _entityRepository.GetAll();
            if (entities.Count() > 0)
            {
                return Ok(entities);
            }
            return NoContent();
        }
        /// <summary>
        /// Lấy một đối tượng theo Id đối tượng
        /// </summary>
        /// <param name="Id">Id đối tượng</param>
        /// <returns>Thành công: Một đối tượng, thất bại trả về lỗi</returns>
        /// Created By: LMCUONG(10/06/2021)
        [HttpGet("{Id:Guid}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            var entity = await _entityRepository.GetByid(Id);
            if (entity != null)
            {
                return Ok(entity);
            }
            return NoContent();
        }
        /// <summary>
        /// Thêm mới một đối tượng
        /// </summary>
        /// <param name="entity">Thông tin đối tượng thêm mới</param>
        /// <returns>Thành công: Mã thành công, thất bại: trả về lỗi</returns>
        /// Created By: LMCUONG(10/06/2021)
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] TEntity entity)
        {
            entity.EntityState = EntityState.Add;
            int? rowAffects = await _entityServices.Insert(entity);
            if (rowAffects > 0)
            {
                return Ok(rowAffects);
            }
            return NoContent();
        }
        /// <summary>
        /// Sửa một đối tượng có trong hệ thống
        /// </summary>
        /// <param name="Id">Id của đối tượng</param>
        /// <param name="entity">Thông tin đối tượng đã sửa</param>
        /// <returns>Thành công: Mã thành công, thất bại: trả về lỗi</returns>
        /// Created By: LMCUONG(10/06/2021)
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] TEntity entity)
        {
            entity.EntityState = EntityState.Update;
            int? rowAffects = await _entityServices.Update(Id, entity);
            if (rowAffects > 0)
            {
                return Ok(rowAffects);
            }
            return NoContent();
        }
        /// <summary>
        /// Xóa một đối tượng trong hệ thống
        /// </summary>
        /// <param name="Id">Id của đối tượng cần xóa</param>
        /// <returns>Thành công: Mã thành công, thất bại: trả về lỗi</returns>
        /// Created By: LMCUONG(10/06/2021)
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            int rowAffects = await _entityRepository.Delete(Id);
            if (rowAffects > 0)
            {
                return Ok(rowAffects);
            }
            return NoContent();
        }

        #endregion
    }
}
