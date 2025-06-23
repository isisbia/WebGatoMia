using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebGatoMia.Data;
using WebGatoMia.Models;
using WebGatoMia.Services.Interfaces;

namespace WebGatoMia.Services.Implementations
{
    public class UserTypeService : IUserTypeService
    {
        private readonly GatoMiaDbContext _context;

        public UserTypeService(GatoMiaDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserType>> GetAllUserTypesAsync()
        {
            return await _context.UserTypes.ToListAsync();
        }

        public async Task<UserType?> GetUserTypeByIdAsync(int id)
        {
            return await _context.UserTypes.FindAsync(id);
        }
    }
}
