using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dapper_Crud.Models
{
    public interface IDistrictRepo
    {
        Task<IEnumerable<District>> GetAllAsync();
        Task<IEnumerable<District>> GetByStateIdAsync(int stateId);
        Task<District?> GetByIdAsync(int id);
        Task<int> CreateAsync(District district);
        Task<bool> UpdateAsync(District district);
        Task<bool> DeleteAsync(int id);
    }
}
