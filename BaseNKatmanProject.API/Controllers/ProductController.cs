using AutoMapper;
using BaseNKatmanProject.Application.DTOs.Product;
using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Core.Entities;
using BaseNKatmanProject.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BaseNKatmanProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, IMapper mapper, ILogger<ProductController> logger)
        {
            _productService = productService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseMessage<IEnumerable<ProductDto>>>> GetAll([FromQuery] PageParameters pageParameters)
        {
            _logger.LogInformation("Ürün listesi getiriliyor. Sayfa: {PageNumber}, Sayfa Boyutu: {PageSize}", pageParameters.PageNumber, pageParameters.PageSize);

            pageParameters.Validate();

            var result = await _productService.GetAllAsync(pageParameters);
            if (!result.Success)
            {
                _logger.LogWarning("Ürün listesi getirilemedi: {Reason}", result.Message);
                return BadRequest(result);
            }

            var dtoList = _mapper.Map<IEnumerable<ProductDto>>(result.Data);
            _logger.LogInformation("{Count} ürün getirildi.", dtoList.Count());
            return Ok(ResponseMessage<IEnumerable<ProductDto>>.SuccessResult(dtoList));
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<ProductDto>>> GetById(Guid id)
        {
            _logger.LogInformation("Ürün detay isteği. Id: {Id}", id);

            var result = await _productService.GetByIdAsync(id);
            if (!result.Success)
            {
                _logger.LogWarning("Ürün bulunamadı. Id: {Id}", id);
                return NotFound(result);
            }

            var dto = _mapper.Map<ProductDto>(result.Data);
            _logger.LogInformation("Ürün bulundu. Id: {Id}", id);

            return Ok(ResponseMessage<ProductDto>.SuccessResult(dto));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage<ProductDto>>> Create([FromBody] ProductCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Ürün oluşturma başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<ProductDto>.Failure(errors));
            }

            var entity = _mapper.Map<Product>(model);

            var result = await _productService.AddAsync(entity);
            if (!result.Success)
            {
                _logger.LogError("Ürün oluşturma başarısız. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            var dto = _mapper.Map<ProductDto>(result.Data);
            _logger.LogInformation("Ürün oluşturuldu. Id: {Id}, Ad: {Name}", dto.Id, dto.Name);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, ResponseMessage<ProductDto>.SuccessResult(dto));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<ProductDto>>> Update(Guid id, [FromBody] ProductUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Ürün güncelleme başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<ProductDto>.Failure(errors));
            }

            var existingResult = await _productService.GetByIdAsync(id);
            if (!existingResult.Success)
            {
                _logger.LogWarning("Güncellenecek ürün bulunamadı. Id: {Id}", id);
                return NotFound(existingResult);
            }

            var entity = existingResult.Data;
            _mapper.Map(model, entity);

            var updateResult = await _productService.UpdateAsync(entity);
            if (!updateResult.Success)
            {
                _logger.LogError("Ürün güncelleme başarısız. Sebep: {Message}", updateResult.Message);
                return BadRequest(updateResult);
            }

            var updatedDto = _mapper.Map<ProductDto>(updateResult.Data);
            _logger.LogInformation("Ürün güncellendi. Id: {Id}", id);

            return Ok(ResponseMessage<ProductDto>.SuccessResult(updatedDto));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<ProductDto>>> Delete(Guid id)
        {
            _logger.LogInformation("Ürün silme isteği. Id: {Id}", id);

            var existingResult = await _productService.GetByIdAsync(id);
            if (!existingResult.Success)
            {
                _logger.LogWarning("Silinecek ürün bulunamadı. Id: {Id}", id);
                return NotFound(existingResult);
            }

            var deleteResult = await _productService.RemoveAsync(existingResult.Data);
            if (!deleteResult.Success)
            {
                _logger.LogError("Ürün silme başarısız. Sebep: {Message}", deleteResult.Message);
                return BadRequest(deleteResult);
            }

            var deletedDto = _mapper.Map<ProductDto>(deleteResult.Data);
            _logger.LogInformation("Ürün silindi. Id: {Id}", id);

            return Ok(ResponseMessage<ProductDto>.SuccessResult(deletedDto));
        }
    }
}
