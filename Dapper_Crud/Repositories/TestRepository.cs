using Dapper_Crud.Models;

namespace Dapper_Crud.Repositories
{
    public class TestRepository : ICountryRepo
    {
        public Task<int> CreateAsync(Country country)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Country>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Country?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Country country)
        {
            throw new NotImplementedException();
        }
    }

}
