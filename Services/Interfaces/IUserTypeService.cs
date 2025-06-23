using System.Collections.Generic;
using System.Threading.Tasks;
using WebGatoMia.Models;

namespace WebGatoMia.Services.Interfaces
{
    public interface IUserTypeService
    {
        Task<IEnumerable<UserType>> GetAllUserTypesAsync();
        Task<UserType?> GetUserTypeByIdAsync(int id);
    }
}
