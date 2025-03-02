using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Catalog;
using Nop.Data;

namespace Nop.Web.Controllers
{
    [Route("api/mixproduct")]
    [ApiController]
    public class MixProductApiController : ControllerBase
    {
        private readonly IRepository<MixProduct> _mixProductRepository;

        public MixProductApiController(IRepository<MixProduct> mixProductRepository)
        {
            _mixProductRepository = mixProductRepository;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var mixProducts = _mixProductRepository.Table.ToList();
            return Ok(mixProducts);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var mixProduct = _mixProductRepository.GetById(id);
            if (mixProduct == null)
                return NotFound();

            return Ok(mixProduct);
        }

        [HttpPost]
        public IActionResult Create([FromBody] MixProduct model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _mixProductRepository.Insert(model);
            return CreatedAtAction(nameof(GetById), new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MixProduct model)
        {
            var mixProduct = _mixProductRepository.GetById(id);
            if (mixProduct == null)
                return NotFound();

            mixProduct.ProductIds = model.ProductIds;
            mixProduct.Deleted = model.Deleted;

            _mixProductRepository.Update(mixProduct);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var mixProduct = _mixProductRepository.GetById(id);
            if (mixProduct == null)
                return NotFound();

            _mixProductRepository.Delete(mixProduct);
            return NoContent();
        }

    }
}
