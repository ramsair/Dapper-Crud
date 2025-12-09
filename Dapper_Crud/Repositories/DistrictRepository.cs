using Dapper;
using System.Data;

namespace Dapper_Crud.Models
{
    public class DistrictRepository : IDistrictRepo
    {
        private readonly IDbConnection _db;

        public DistrictRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<IEnumerable<District>> GetAllAsync()
        {
            var query = "SELECT * FROM dbo.District WHERE IsActive = 1";
            return await _db.QueryAsync<District>(query);
        }

        public async Task<IEnumerable<District>> GetByStateIdAsync(int stateId)
        {
            var query = "SELECT * FROM dbo.District WHERE StateId = @StateId AND IsActive = 1";
            return await _db.QueryAsync<District>(query, new { StateId = stateId });
        }

        public async Task<District?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM dbo.District WHERE DistrictId = @Id";
            return await _db.QueryFirstOrDefaultAsync<District>(query, new { Id = id });
        }

        public async Task<int> CreateAsync(District district)
        {
            var query = @"INSERT INTO dbo.District (DistrictName, StateId, IsActive)
                          VALUES (@DistrictName, @StateId, 1);
                          SELECT CAST(SCOPE_IDENTITY() as int)";
            return await _db.ExecuteScalarAsync<int>(query, district);
        }

        public async Task<bool> UpdateAsync(District district)
        {
            var query = @"UPDATE dbo.District 
                          SET DistrictName = @DistrictName, 
                              StateId = @StateId,
                              ModifiedAt = SYSUTCDATETIME()
                          WHERE DistrictId = @DistrictId";
            var rows = await _db.ExecuteAsync(query, district);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = "DELETE FROM dbo.District WHERE DistrictId = @Id";
            var rows = await _db.ExecuteAsync(query, new { Id = id });
            return rows > 0;
        }
    }
}
