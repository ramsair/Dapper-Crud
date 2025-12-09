global using Dapper;
using System.Data;

namespace Dapper_Crud.Models
{
    public class CountryRepository : ICountryRepo
    {
        private readonly IDbConnection _db;

        public CountryRepository(IDbConnection db) //Constructor dependancy injection
        {
            _db = db;
        }

        public async Task<int> CreateAsync(Country country)//, int? t = null optional parameter
        {
            //var query = @"INSERT INTO dbo.Country (CountryName)
            //              VALUES (@CountryName);
            //              SELECT CAST(SCOPE_IDENTITY() as int) ";
            //return await _db.ExecuteScalarAsync<int>(query, country);
            //var resp =  await _db.ExecuteScalarAsync<int>(query, country);
            //return (int)resp;
            int insertedId = 0;
            //2. Transactions
            using (IDbTransaction trans = _db.BeginTransaction())  // disposing 2nd way 
            {
                try
                {
                    var query = @"INSERT INTO dbo.Country (CountryName)
                                  VALUES (@CountryName);
                                  SELECT CAST(SCOPE_IDENTITY() as int) ";
                    insertedId = await _db.ExecuteScalarAsync<int>(query, 
                                            country,
                                            trans, commandType: CommandType.Text);
                    trans.Commit();
                }
                catch (Exception)
                {
                    trans.Rollback();
                }
                finally
                {
                     trans.Dispose();//1 disposing object way
                }
            }
            return insertedId;
            //ghg
        }

        public async Task<IEnumerable<Country>> GetAllAsync()
        {
            var procedure = "dbo.usp_GetCountries";
            return await _db.QueryAsync<Country>(
                procedure,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<Country?> GetByIdAsync(int id)
        {
            var query = "SELECT * FROM dbo.Country WHERE CountryId = @Id";
            return await _db.QueryFirstOrDefaultAsync<Country>(query, new { Id = id });
        }

        public async Task<bool> UpdateAsync(Country country)
        {
            var query = @"UPDATE dbo.Country 
                          SET CountryName = @CountryName, ModifiedAt = SYSUTCDATETIME()
                          WHERE CountryId = @CountryId";
            var rows = await _db.ExecuteAsync(query, country);
            return rows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
             //var query = $"DELETE FROM dbo.Country WHERE CountryId = {id}";//1 way but not gonna use

             var query = "DELETE FROM dbo.Country WHERE CountryId = @Id";
            // var rows = await _db.ExecuteAsync(query, new { Id = id }); //Way 2

            //Way 2 using dynamic parameters
            var prams = new DynamicParameters();
            prams.Add("@Id", id, DbType.Int32, ParameterDirection.Input);
            var rows = await _db.ExecuteAsync(query, prams);
            return rows > 0;
        }
    }
}
