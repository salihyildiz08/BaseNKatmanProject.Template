using FluentValidation;

namespace BaseNKatmanProject.Application.DTOs.Auth;
public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Geçerli bir e-posta giriniz.");
        RuleFor(x => x.Password).NotEmpty()
            .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı.")
            .Matches("[A-Z]").WithMessage("Şifre en az 1 büyük harf içermeli.")
            .Matches("[a-z]").WithMessage("Şifre en az 1 küçük harf içermeli.")
            .Matches("[0-9]").WithMessage("Şifre en az 1 rakam içermeli.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az 1 özel karakter içermeli.");
        RuleFor(x => x.Ad).NotEmpty().WithMessage("Ad alanı boş bırakılamaz.");
        RuleFor(x => x.Soyad).NotEmpty().WithMessage("Soyad alanı boş bırakılamaz.");
    }
}
