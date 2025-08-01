using AutoMapper;
using BaseNKatmanProject.Application.DTOs.User;
using BaseNKatmanProject.Application.Interfaces.Services.User;
using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BaseNKatmanProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseMessage<IEnumerable<UserDto>>>> GetAll()
        {
            _logger.LogInformation("Kullanıcı listesi getiriliyor.");

            var result = await _userService.GetAllAsync();
            if (!result.Success)
            {
                _logger.LogWarning("Kullanıcı listesi getirilemedi: {Reason}", result.Message);
                return BadRequest(result);
            }

            var dtoList = _mapper.Map<IEnumerable<UserDto>>(result.Data);
            _logger.LogInformation("{Count} kullanıcı getirildi.", dtoList.Count());
            return Ok(ResponseMessage<IEnumerable<UserDto>>.SuccessResult(dtoList));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<UserDto>>> GetById(Guid id)
        {
            _logger.LogInformation("Kullanıcı detay isteği. Id: {Id}", id);

            var result = await _userService.GetByIdAsync(id);
            if (!result.Success)
            {
                _logger.LogWarning("Kullanıcı bulunamadı. Id: {Id}", id);
                return NotFound(result);
            }

            var dto = _mapper.Map<UserDto>(result.Data);
            _logger.LogInformation("Kullanıcı bulundu. Id: {Id}", id);

            return Ok(ResponseMessage<UserDto>.SuccessResult(dto));
        }

        [HttpPost]
        public async Task<ActionResult<ResponseMessage<UserDto>>> Create([FromBody] UserCreateDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                _logger.LogWarning("Kullanıcı oluşturma başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<UserDto>.Failure(errors));
            }

            var userEntity = _mapper.Map<ApplicationUser>(model);
            userEntity.UserName = model.Email;

            var result = await _userService.AddAsync(userEntity);

            if (!result.Success)
            {
                _logger.LogError("Kullanıcı oluşturma başarısız. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            var userDto = _mapper.Map<UserDto>(result.Data);
            _logger.LogInformation("Kullanıcı oluşturuldu. Id: {Id}, Email: {Email}", userDto.Id, userDto.Email);

            return CreatedAtAction(nameof(GetById), new { id = result.Data.Id }, ResponseMessage<UserDto>.SuccessResult(userDto));
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<UserDto>>> Update(Guid id, [FromBody] UserUpdateDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                _logger.LogWarning("Kullanıcı güncelleme başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<UserDto>.Failure(errors));
            }

            var existingUserResult = await _userService.GetByIdAsync(id);
            if (!existingUserResult.Success)
            {
                _logger.LogWarning("Güncellenecek kullanıcı bulunamadı. Id: {Id}", id);
                return NotFound(existingUserResult);
            }

            var userEntity = existingUserResult.Data;
            _mapper.Map(model, userEntity);

            var updateResult = await _userService.UpdateAsync(userEntity);
            if (!updateResult.Success)
            {
                _logger.LogError("Kullanıcı güncelleme başarısız. Sebep: {Message}", updateResult.Message);
                return BadRequest(updateResult);
            }

            var updatedDto = _mapper.Map<UserDto>(updateResult.Data);
            _logger.LogInformation("Kullanıcı güncellendi. Id: {Id}", id);

            return Ok(ResponseMessage<UserDto>.SuccessResult(updatedDto));
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ResponseMessage<UserDto>>> Delete(Guid id)
        {
            _logger.LogInformation("Kullanıcı silme isteği. Id: {Id}", id);

            var existingUserResult = await _userService.GetByIdAsync(id);
            if (!existingUserResult.Success)
            {
                _logger.LogWarning("Silinecek kullanıcı bulunamadı. Id: {Id}", id);
                return NotFound(existingUserResult);
            }

            var result = await _userService.RemoveAsync(existingUserResult.Data);
            if (!result.Success)
            {
                _logger.LogError("Kullanıcı silme başarısız. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            var deletedDto = _mapper.Map<UserDto>(result.Data);
            _logger.LogInformation("Kullanıcı silindi. Id: {Id}", id);

            return Ok(ResponseMessage<UserDto>.SuccessResult(deletedDto));
        }

        [HttpPut("{id:guid}/changepassword")]
        public async Task<ActionResult<ResponseMessage<bool>>> ChangePassword(Guid id, [FromBody] ChangePasswordDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Parola değiştirme başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<bool>.Failure(errors));
            }

            var result = await _userService.ChangePasswordAsync(id, model.NewPassword);

            if (!result.Success)
            {
                _logger.LogError("Parola değiştirme başarısız. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Parola değiştirildi. Kullanıcı Id: {Id}", id);
            return Ok(result);
        }

        [HttpPost("{id:guid}/roles")]
        public async Task<ActionResult<ResponseMessage<bool>>> AddRole(Guid id, [FromBody] CreateUserRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Rol atama başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<bool>.Failure(errors));
            }

            var result = await _userService.AddToRoleAsync(id, model.Name);

            if (!result.Success)
            {
                _logger.LogError("Rol atama başarısız. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Rol atandı. Kullanıcı Id: {UserId}, Rol: {RoleName}", id, model.Name);
            return Ok(result);
        }

        [HttpDelete("{id:guid}/roles")]
        public async Task<ActionResult<ResponseMessage<bool>>> RemoveRole(Guid id, [FromBody] CreateUserRoleDto model)
        {
            if (!ModelState.IsValid)
            {
                var errors = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                _logger.LogWarning("Rol kaldırma başarısız. Model doğrulama hatası: {Errors}", errors);
                return BadRequest(ResponseMessage<bool>.Failure(errors));
            }

            var result = await _userService.RemoveFromRoleAsync(id, model.Name);

            if (!result.Success)
            {
                _logger.LogError("Rol kaldırma başarısız. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Rol kaldırıldı. Kullanıcı Id: {UserId}, Rol: {RoleName}", id, model.Name);
            return Ok(result);
        }

        [HttpGet("{id:guid}/roles")]
        public async Task<ActionResult<ResponseMessage<IList<string>>>> GetRoles(Guid id)
        {
            _logger.LogInformation("Kullanıcı rolleri sorgulandı. Kullanıcı Id: {UserId}", id);

            var result = await _userService.GetRolesAsync(id);

            if (!result.Success)
            {
                _logger.LogWarning("Kullanıcı rolleri alınamadı. Sebep: {Message}", result.Message);
                return BadRequest(result);
            }

            _logger.LogInformation("Kullanıcı rolleri getirildi. Kullanıcı Id: {UserId}, Rol sayısı: {Count}", id, result.Data.Count);
            return Ok(result);
        }
    }
}
