using InsuranceApp.Models;
using System.Data;
using Dapper;

namespace InsuranceApp.Services
{
    public class PartnerService
    {
        private readonly IDbConnection _dbConnection;

        public PartnerService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Get all partners
        public async Task<IEnumerable<Partner>> GetAllPartnersAsync()
        {
            try
            {
                var query = "SELECT * FROM Partner";
                var partners = await _dbConnection.QueryAsync<Partner>(query);
                return partners.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all partners: {ex.Message}");
                throw; 
            }
        }

        // Get partner by ID
        public async Task<Partner?> GetPartnerByIdAsync(int id)
        {
            try
            {
                var query = "SELECT * FROM Partner WHERE PartnerId = @Id";
                return await _dbConnection.QuerySingleOrDefaultAsync<Partner>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching partner by ID {id}: {ex.Message}");
                throw; 
            }
        }

        // Add a new partner
        public async Task AddPartnerAsync(Partner partner)
        {
            try
            {
                // If CreatedAtUtc is not set, the default DateTime.UtcNow will be used
                if (partner.CreatedAtUtc == default)
                {
                    partner.CreatedAtUtc = DateTime.UtcNow;
                }

                var query = @"
                    INSERT INTO Partner (FirstName, LastName, Address, PartnerNumber, CroatianPIN, PartnerTypeId, CreatedAtUtc, CreateByUser, IsForeign, ExternalCode, Gender, CityId)
                    VALUES (@FirstName, @LastName, @Address, @PartnerNumber, @CroatianPIN, @PartnerTypeId, @CreatedAtUtc, @CreateByUser, @IsForeign, @ExternalCode, @Gender, @CityId)";
                
                await _dbConnection.ExecuteAsync(query, partner);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom dodavanja partnera: {ex.Message}");
                throw new ApplicationException("Došlo je do greške prilikom dodavanja partnera.", ex);
            }
        }

    }
}
