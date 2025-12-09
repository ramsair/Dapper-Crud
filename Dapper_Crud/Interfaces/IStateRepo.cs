namespace Dapper_Crud.Models
{
    public interface IStateRepo
    {
        Task<IEnumerable<State>> GetAllAsync();
        Task<State?> GetByIdAsync(int id);
        Task<int> CreateAsync(State state);
        Task<bool> UpdateAsync(State state);
        Task<bool> DeleteAsync(int id);
    }
}
