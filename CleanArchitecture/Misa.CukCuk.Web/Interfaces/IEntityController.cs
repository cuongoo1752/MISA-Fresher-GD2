using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Misa.CukCuk.Web.Interfaces
{
    public interface IEntityController<TEntity>
    {
        [HttpGet]
        public Task<IActionResult> GetAll();
        [HttpGet("{Id}")]
        public Task<IActionResult> GetById(Guid Id);
        [HttpPost]
        public Task<IActionResult> Insert([FromBody] TEntity entity);
        [HttpPut("{Id}")]
        public Task<IActionResult> Update(Guid Id, [FromBody] TEntity entity);
        [HttpDelete("{Id}")]
        public Task<IActionResult> Delete(Guid Id);
        
    }
}
