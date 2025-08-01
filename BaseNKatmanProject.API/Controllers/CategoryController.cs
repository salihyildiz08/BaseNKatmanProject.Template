using AutoMapper;
using BaseNKatmanProject.Application.DTOs.Category;
using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Core.Entities;
using BaseNKatmanProject.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BaseNKatmanProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, IMapper mapper, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseMessage<IEnumerable<CategoryDto>>>> GetAll([FromQuery] PageParameters pageParameters)
        {
            _logger.LogInformation("Kategori listesi getiriliyor. Sayfa: {PageNumber}, Sayfa Boyutu: {PageSize}", pageParameters.PageNumber, pageParameters.PageSize);

            // PageParameters içindeki Validate() methodunu çağırarak gelen değerleri kontrol edelim
            pageParameters.Validate();

            var result = await _categoryService.GetAllAsync(pageParameters);
            if (!result.Success)
            {
                _logger.LogWarning("Kategori listesi getirilemedi: {Reason}", result.Message);
                return BadRequest(result);
            }

            var dtoList = _mapper.Map<IEnumerable<CategoryDto>>(result.Data);
            _logger.LogInformation("{Count} kategori getirildi.", dtoList.Count());
            return Ok(ResponseMessage<IEnumerable<CategoryDto>>.SuccessResult(dtoList));
        }


        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<CategoryDto>>> GetById(Guid id)
        {
            _logger.LogInformation("Kategori detay isteği. Id: {Id}", id);

            var result = await _categoryService.GetByIdAsync(id);
            if (!result.Success)
            {
                _logger.LogWarning("Kategori bulunamadı. Id: {Id}", id);
                return NotFound(result);
            }

            var dto = _mapper.Map<CategoryDto>(result.Data);
            _logger.LogInformation("Kategori bulundu. Id: {Id}", id);

            return Ok(ResponseMessage<CategoryDto>.SuccessResult(dto));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage<CategoryDto>>> Create([FromBody] CategoryCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Kategori oluşturma başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<CategoryDto>.Failure(errors));
            }

            var entity = _mapper.Map<Category>(model);

            var result = await _categoryService.AddAsync(entity);
            if (!result.Success)
            {
                _logger.LogError("Kategori oluşturma başarısız. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            var dto = _mapper.Map<CategoryDto>(result.Data);
            _logger.LogInformation("Kategori oluşturuldu. Id: {Id}, Ad: {Name}", dto.Id, dto.Name);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, ResponseMessage<CategoryDto>.SuccessResult(dto));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<CategoryDto>>> Update(Guid id, [FromBody] CategoryUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Kategori güncelleme başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<CategoryDto>.Failure(errors));
            }

            var existingResult = await _categoryService.GetByIdAsync(id);
            if (!existingResult.Success)
            {
                _logger.LogWarning("Güncellenecek kategori bulunamadı. Id: {Id}", id);
                return NotFound(existingResult);
            }

            var entity = existingResult.Data;
            _mapper.Map(model, entity);

            var updateResult = await _categoryService.UpdateAsync(entity);
            if (!updateResult.Success)
            {
                _logger.LogError("Kategori güncelleme başarısız. Sebep: {Message}", updateResult.Message);
                return BadRequest(updateResult);
            }

            var updatedDto = _mapper.Map<CategoryDto>(updateResult.Data);
            _logger.LogInformation("Kategori güncellendi. Id: {Id}", id);

            return Ok(ResponseMessage<CategoryDto>.SuccessResult(updatedDto));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<CategoryDto>>> Delete(Guid id)
        {
            _logger.LogInformation("Kategori silme isteği. Id: {Id}", id);

            var existingResult = await _categoryService.GetByIdAsync(id);
            if (!existingResult.Success)
            {
                _logger.LogWarning("Silinecek kategori bulunamadı. Id: {Id}", id);
                return NotFound(existingResult);
            }

            var deleteResult = await _categoryService.RemoveAsync(existingResult.Data);
            if (!deleteResult.Success)
            {
                _logger.LogError("Kategori silme başarısız. Sebep: {Message}", deleteResult.Message);
                return BadRequest(deleteResult);
            }

            var deletedDto = _mapper.Map<CategoryDto>(deleteResult.Data);
            _logger.LogInformation("Kategori silindi. Id: {Id}", id);

            return Ok(ResponseMessage<CategoryDto>.SuccessResult(deletedDto));
        }
    }
}
