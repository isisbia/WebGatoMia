using WebGatoMia.Models; // Certifique-se de que o namespace dos seus modelos está correto
using WebGatoMia.Models.ViewModels;

namespace WebGatoMia.Services.Interfaces
{
    public interface IUserService
    {
        // CREATE
        Task<User> CreateUserAsync(User user);

        // READ
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User?> GetUserByEmailAsync(string email);

        // UPDATE
        Task<bool> UpdateUserAsync(User user);

        // DELETE (ou inativação)
        Task<bool> DeactivateUserAsync(Guid id); // Preferível a exclusão física
        Task<bool> DeleteUserAsync(Guid id); // Se a exclusão física for realmente necessária
        // --- NOVOS MÉTODOS PARA REGISTRO E AUTENTICAÇÃO ---
        Task<bool> RegisterUserAsync(RegisterViewModel model); // ADICIONE ESTA LINHA
        Task<User?> AuthenticateUserAsync(string email, string password); // ADICIONE ESTA LINHA
    }
}