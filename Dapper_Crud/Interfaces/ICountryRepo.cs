namespace Dapper_Crud.Models
{
    public interface ICountryRepo
    {
        Task<IEnumerable<Country>> GetAllAsync();
        Task<Country?> GetByIdAsync(int id);
        Task<int> CreateAsync(Country country);
        Task<bool> UpdateAsync(Country country);
        Task<bool> DeleteAsync(int id);     

    }       
}
