using Microsoft.EntityFrameworkCore;
using WebGatoMia.Data;
using WebGatoMia.Models;
using WebGatoMia.Models.ViewModels;
using WebGatoMia.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace WebGatoMia.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly GatoMiaDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserService(GatoMiaDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // CREATE
        public async Task<User> CreateUserAsync(User user)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                return user; // Ou lançar exceção, dependendo do fluxo desejado
            }

            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                DateRegistration = DateTime.UtcNow,
                IsActive = true,
                UserTypeId = 3 // Tipo padrão
            };

            // Hash da senha (assume que user.PasswordHash tem senha em texto puro aqui)
            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, user.PasswordHash);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        // READ - Todos usuários com UserType incluído
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.UserType).ToListAsync();
        }

        // READ - Por Id
        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _context.Users.Include(u => u.UserType).FirstOrDefaultAsync(u => u.Id == id);
        }

        // READ - Por Email
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.UserType).FirstOrDefaultAsync(u => u.Email == email);
        }

        // UPDATE
        public async Task<bool> UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                return false;
            }

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Phone = user.Phone;
            existingUser.DateRegistration = user.DateRegistration;
            existingUser.IsActive = user.IsActive;
            existingUser.UserTypeId = user.UserTypeId;

            // Atualização da senha:
            // Se user.PasswordHash não for nulo ou vazio E for diferente do hash atual,
            // isso significa que a senha foi enviada em texto puro para ser atualizada.
            if (!string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                var passwordVerification = _passwordHasher.VerifyHashedPassword(existingUser, existingUser.PasswordHash, user.PasswordHash);
                if (passwordVerification == PasswordVerificationResult.Failed)
                {
                    // Senha nova (texto puro) - hash e atualiza
                    existingUser.PasswordHash = _passwordHasher.HashPassword(existingUser, user.PasswordHash);
                }
                // Se for Success ou SuccessRehashNeeded, não altera a senha aqui
            }

            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
            return true;
        }

        // INATIVA USUÁRIO
        public async Task<bool> DeactivateUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            user.IsActive = false;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // REMOVE USUÁRIO
        public async Task<bool> DeleteUserAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        // REGISTRO DE USUÁRIO VIA VIEWMODEL
        public async Task<bool> RegisterUserAsync(RegisterViewModel model)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (existingUser != null)
            {
                return false; // Email já existe
            }

            var newUser = new User
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                DateRegistration = DateTime.UtcNow,
                IsActive = true,
                UserTypeId = 3 // Tipo padrão
            };

            newUser.PasswordHash = _passwordHasher.HashPassword(newUser, model.Password);

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return true;
        }

        // AUTENTICAÇÃO
        public async Task<User?> AuthenticateUserAsync(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null || !user.IsActive)
            {
                return null;
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);

            if (verificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            if (verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, password);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            return user;
        }
    }
}
