using Dapper;
using System.Data;

namespace Dapper_Crud.Models
{
    public class StateRepository : IStateRepo
    {
        private readonly IDbConnection _db;

        public StateRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<State>> GetAllAsync()
        {
            var query = "SELECT * FROM dbo.State WHERE IsActive = 1";
            return await _db.QueryAsync<State>(query);
        }

        public async Task<State?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM dbo.State WHERE StateId = @Id";
            return await _db.QueryFirstOrDefaultAsync<State>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(State state)
        {
            var query = @"INSERT INTO dbo.State (StateName, CountryId)
                          VALUES (@StateName, @CountryId);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _db.ExecuteScalarAsync<int>(query, state);
        }

        public async Task<bool> UpdateAsync(State state)
        {
            var query = @"UPDATE dbo.State 
                          SET StateName = @StateName, CountryId = @CountryId, ModifiedAt = SYSUTCDATETIME()
                          WHERE StateId = @StateId";
            var rows = await _db.ExecuteAsync(query, state);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM dbo.State WHERE StateId = @Id";
            var rows = await _db.ExecuteAsync(query, new { Id = id });
            return rows > 0;
        }
    }
}
