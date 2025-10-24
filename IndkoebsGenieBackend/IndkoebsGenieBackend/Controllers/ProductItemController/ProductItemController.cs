using IndkoebsGenieBackend.DTO.ProductItemDTO;
using IndkoebsGenieBackend.Interfaces.IProductItem;
using Microsoft.AspNetCore.Mvc;

namespace IndkoebsGenieBackend.Controllers.ProductItemController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        private readonly IProductItemService _service;
        private readonly IProductItemRepository _repository;

        public ProductItemController(IProductItemService service, IProductItemRepository repository)
        {
            _service = service;
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductItemResponse>>> GetAllAsync()
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductItemResponse>> GetByIdAsync([FromRoute] int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item is null)
                return NotFound(new { message = $"ProductItem with id {id} not found." });

            return Ok(item);
        }


        [HttpPost]
        public async Task<ActionResult<ProductItemResponse>> CreateAsync([FromBody] ProductItemRequest request)
        {
            try
            {
                var created = await _service.CreateAsync(request);

                return CreatedAtAction(nameof(GetByIdAsync), new { id = created.Id }, created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{id:int}/update")]
        public async Task<ActionResult<ProductItemResponse>> UpdateAsync([FromRoute] int id, [FromBody] ProductItemRequest request)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, request);
                if (updated is null)
                    return NotFound(new { message = $"ProductItem with id {id} not found." });

                return Ok(new { message = "Product updated successfully", product = updated });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted)
                return NotFound(new { message = $"ProductItem with id {id} not found." });

            return Ok(new { message = "Product deleted successfully" });
        }


        [HttpPatch("{id:int}/toggle")]
        public async Task<ActionResult<ProductItemResponse>> ToggleAsync([FromRoute] int id, [FromQuery] bool completed = true)
        {

            var current = await _service.GetByIdAsync(id);
            if (current is null)
                return NotFound(new { message = $"ProductItem with id {id} not found." });

            var request = new ProductItemRequest
            {
                Name = current.Name,
                Quantity = current.Quantity,
                Notes = current.Notes,
                IsCompleted = completed,
                Category = current.Category,
                GroceryListId = current.GroceryListId
            };

            var updated = await _service.UpdateAsync(id, request);
            return Ok(updated);
        }
    }
}
