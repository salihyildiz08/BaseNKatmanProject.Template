using BaseNKatmanProject.Core.Commons;
using BaseNKatmanProject.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseNKatmanProject.Application.Interfaces.Services.User
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ResponseMessage<ApplicationUser>> GetByIdAsync(Guid id, params Expression<Func<ApplicationUser, object>>[] includes)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id && !u.SilindiMi);
            if (user == null)
                return ResponseMessage<ApplicationUser>.Failure("Kullanıcı bulunamadı.");

            return ResponseMessage<ApplicationUser>.SuccessResult(user);
        }

        public async Task<ResponseMessage<IEnumerable<ApplicationUser>>> GetAllAsync(
            PageParameters pageParameters = null,
            Expression<Func<ApplicationUser, bool>> filter = null,
            Func<IQueryable<ApplicationUser>, IOrderedQueryable<ApplicationUser>> orderBy = null,
            params Expression<Func<ApplicationUser, object>>[] includes)
        {
            IQueryable<ApplicationUser> query = _userManager.Users.Where(u => !u.SilindiMi);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            // Pagination uygulama
            if (pageParameters != null)
            {
                pageParameters.Validate();
                query = query.Skip((pageParameters.PageNumber - 1) * pageParameters.PageSize)
                             .Take(pageParameters.PageSize);
            }

            var list = await query.ToListAsync();
            return ResponseMessage<IEnumerable<ApplicationUser>>.SuccessResult(list);
        }

        public async Task<ResponseMessage<IEnumerable<ApplicationUser>>> FindAsync(
            Expression<Func<ApplicationUser, bool>> filter,
            params Expression<Func<ApplicationUser, object>>[] includes)
        {
            if (filter == null)
                return ResponseMessage<IEnumerable<ApplicationUser>>.Failure("Filtre belirtilmeli.");

            var users = await _userManager.Users.Where(filter).Where(u => !u.SilindiMi).ToListAsync();
            return ResponseMessage<IEnumerable<ApplicationUser>>.SuccessResult(users);
        }

        public async Task<ResponseMessage<ApplicationUser>> AddAsync(ApplicationUser entity)
        {
            var result = await _userManager.CreateAsync(entity);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseMessage<ApplicationUser>.Failure(errors);
            }
            return ResponseMessage<ApplicationUser>.SuccessResult(entity, "Kullanıcı başarıyla eklendi.");
        }

        public async Task<ResponseMessage<ApplicationUser>> UpdateAsync(ApplicationUser entity)
        {
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == entity.Id && !u.SilindiMi);
            if (existingUser == null)
                return ResponseMessage<ApplicationUser>.Failure("Kullanıcı bulunamadı.");

            existingUser.Ad = entity.Ad;
            existingUser.Soyad = entity.Soyad;
            existingUser.Email = entity.Email;
            existingUser.UserName = entity.UserName;
            existingUser.PhoneNumber = entity.PhoneNumber;
            // İstersen diğer güncellenmesi gereken alanlar buraya eklenebilir

            var result = await _userManager.UpdateAsync(existingUser);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseMessage<ApplicationUser>.Failure(errors);
            }

            return ResponseMessage<ApplicationUser>.SuccessResult(existingUser, "Kullanıcı başarıyla güncellendi.");
        }

        public async Task<ResponseMessage<ApplicationUser>> RemoveAsync(ApplicationUser entity)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == entity.Id && !u.SilindiMi);
            if (user == null)
                return ResponseMessage<ApplicationUser>.Failure("Kullanıcı bulunamadı.");

            // Soft delete işlemi
            user.SilindiMi = true;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseMessage<ApplicationUser>.Failure(errors);
            }

            return ResponseMessage<ApplicationUser>.SuccessResult(user, "Kullanıcı başarıyla silindi.");
        }

        public async Task<int> SaveChangesAsync()
        {
            // UserManager Identity tarafından otomatik kaydedilir.
            return await Task.FromResult(1);
        }

        public async Task<ResponseMessage<bool>> ChangePasswordAsync(Guid id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return ResponseMessage<bool>.Failure("Kullanıcı bulunamadı.");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseMessage<bool>.Failure(errors);
            }

            return ResponseMessage<bool>.SuccessResult(true, "Şifre başarıyla değiştirildi.");
        }

        public async Task<ResponseMessage<IList<string>>> GetRolesAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ResponseMessage<IList<string>>.Failure("Kullanıcı bulunamadı.");

            var roles = await _userManager.GetRolesAsync(user);
            return ResponseMessage<IList<string>>.SuccessResult(roles, "Roller başarıyla getirildi.");
        }

        public async Task<ResponseMessage<bool>> AddToRoleAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ResponseMessage<bool>.Failure("Kullanıcı bulunamadı.");

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
                return ResponseMessage<bool>.Failure("Rol bulunamadı.");

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseMessage<bool>.Failure(errors);
            }

            return ResponseMessage<bool>.SuccessResult(true, "Rol başarıyla eklendi.");
        }

        public async Task<ResponseMessage<bool>> RemoveFromRoleAsync(Guid userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return ResponseMessage<bool>.Failure("Kullanıcı bulunamadı.");

            var result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return ResponseMessage<bool>.Failure(errors);
            }

            return ResponseMessage<bool>.SuccessResult(true, "Rol başarıyla kaldırıldı.");
        }
    }
}
