using System.ComponentModel.DataAnnotations;

namespace WebGatoMia.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome não pode exceder {1} caracteres.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "O nome não deve conter números ou caracteres especiais.")]
        [Display(Name = "Nome")] // Nome a ser exibido no formulário HTML
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [DataType(DataType.Password)] // Indica que este campo deve ser tratado como senha
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$",
            ErrorMessage = "A senha deve conter entre 8 à 20 caracteres, incluindo letras maiúsculas e minúsculas, números e caracteres especiais (@$!%*?&).")]
        [Display(Name = "Senha")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "A confirmação da senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "A senha e a confirmação de senha não coincidem.")]
        [Display(Name = "Confirmar Senha")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Formato de telefone inválido.")]
        [RegularExpression(@"^[0-9]{10,15}$", ErrorMessage = "O telefone deve conter apenas números e ter entre 10 e 15 dígitos.")]
        [Display(Name = "Telefone")]
        public string? Phone { get; set; } // O '?' indica que o campo é opcional (nullable)

    }
}