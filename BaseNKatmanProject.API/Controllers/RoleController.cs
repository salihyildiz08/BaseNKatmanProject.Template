using AutoMapper;
using BaseNKatmanProject.Application.DTOs.Role;
using BaseNKatmanProject.Application.Interfaces.Services.Role;
using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaseNKatmanProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;

        public RoleController(IRoleService roleService, IMapper mapper, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseMessage<IEnumerable<RoleDto>>>> GetAll()
        {
            _logger.LogInformation("Rol listeleme isteği alındı.");

            var result = await _roleService.GetAllAsync();
            if (!result.Success)
            {
                _logger.LogWarning("Rol listeleme başarısız. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            var dtoList = _mapper.Map<IEnumerable<RoleDto>>(result.Data);

            _logger.LogInformation("Rol listeleme başarılı. Toplam: {Count}", dtoList.Count());

            return Ok(ResponseMessage<IEnumerable<RoleDto>>.SuccessResult(dtoList));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<RoleDto>>> GetById(Guid id)
        {
            _logger.LogInformation("Rol detay isteği. Id: {Id}", id);

            var result = await _roleService.GetByIdAsync(id);
            if (!result.Success)
            {
                _logger.LogWarning("Rol bulunamadı. Id: {Id}", id);
                return NotFound(result);
            }

            var dto = _mapper.Map<RoleDto>(result.Data);
            _logger.LogInformation("Rol bulundu. Id: {Id}", id);

            return Ok(ResponseMessage<RoleDto>.SuccessResult(dto));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage<RoleDto>>> Create([FromBody] RoleCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                _logger.LogWarning("Rol oluşturma başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<RoleDto>.Failure(errors));
            }

            var roleEntity = _mapper.Map<ApplicationRole>(model);
            roleEntity.NormalizedName = model.Name.ToUpperInvariant();

            var result = await _roleService.AddAsync(roleEntity);
            if (!result.Success)
            {
                _logger.LogError("Rol oluşturma başarısız. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            var dto = _mapper.Map<RoleDto>(result.Data);
            _logger.LogInformation("Rol oluşturuldu. Id: {Id}, Adı: {Name}", dto.Id, dto.Name);

            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, ResponseMessage<RoleDto>.SuccessResult(dto));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<RoleDto>>> Update(Guid id, [FromBody] RoleUpdateDto model)
        {
            _logger.LogInformation("Rol güncelleme isteği. Id: {Id}", id);

            var existingRole = await _roleService.GetByIdAsync(id);
            if (!existingRole.Success)
            {
                _logger.LogWarning("Güncellenecek rol bulunamadı. Id: {Id}", id);
                return NotFound(existingRole);
            }

            var roleEntity = existingRole.Data;
            _mapper.Map(model, roleEntity);

            var updateResult = await _roleService.UpdateAsync(roleEntity);
            if (!updateResult.Success)
            {
                _logger.LogError("Rol güncelleme başarısız. Sebep: {Message}", updateResult.Message);
                return BadRequest(updateResult);
            }

            var dto = _mapper.Map<RoleDto>(updateResult.Data);
            _logger.LogInformation("Rol güncellendi. Id: {Id}", id);

            return Ok(ResponseMessage<RoleDto>.SuccessResult(dto));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<RoleDto>>> Delete(Guid id)
        {
            _logger.LogInformation("Rol silme isteği. Id: {Id}", id);

            var existingRoleResult = await _roleService.GetByIdAsync(id);
            if (!existingRoleResult.Success)
            {
                _logger.LogWarning("Silinecek rol bulunamadı. Id: {Id}", id);
                return NotFound(existingRoleResult);
            }

            var roleEntity = existingRoleResult.Data;
            roleEntity.SilindiMi = true;

            var updateResult = await _roleService.UpdateAsync(roleEntity);
            if (!updateResult.Success)
            {
                _logger.LogError("Rol silme başarısız. Sebep: {Message}", updateResult.Message);
                return BadRequest(updateResult);
            }

            var dto = _mapper.Map<RoleDto>(updateResult.Data);
            _logger.LogInformation("Rol soft delete yapıldı. Id: {Id}", id);

            return Ok(ResponseMessage<RoleDto>.SuccessResult(dto));
        }
    }
}
