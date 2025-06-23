using System.ComponentModel.DataAnnotations;

namespace WebGatoMia.Models.ViewModels
{
    public class UserCreateViewModel
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "As senhas não coincidem.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        public int UserTypeId { get; set; } = 3;

        public bool IsActive { get; set; } = true;
    }
}
