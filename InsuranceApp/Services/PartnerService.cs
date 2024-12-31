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

        // Dohvat imena grada prema CityId
        public async Task<string> GetCityNameByIdAsync(int cityId)
        {
            try
            {
                // SQL upit za dohvat imena grada prema CityId
                var query = "SELECT CityName FROM City WHERE CityId = @CityId";
                
                // Izvrši upit i dohvatimo ime grada
                var cityName = await _dbConnection.QuerySingleOrDefaultAsync<string>(query, new { CityId = cityId });

                // Ako grad nije pronađen, vraćamo "Unknown City"
                return cityName ?? "Unknown City";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom dohvaćanja grada za ID {cityId}: {ex.Message}");
                return "Unknown City"; // U slučaju greške
            }
        }

        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            const string query = "SELECT CityId, CityName FROM City ORDER BY CityName";

            var cities = await _dbConnection.QueryAsync<City>(query);
            return cities;
        }

        // Add a new partner
        public async Task AddPartnerAsync(Partner partner)
        {
            
            try
            {
                // Provjera postoji li partner s istim CroatianPIN (OIB)
                var existingPartnerByPIN = await _dbConnection.QueryFirstOrDefaultAsync<Partner>(
                    "SELECT * FROM Partner WHERE CroatianPIN = @CroatianPIN", new { CroatianPIN = partner.CroatianPIN });

                if (existingPartnerByPIN != null)
                {
                    throw new ApplicationException("OIB (Croatian PIN) already exists.");
                }

                // Provjera postoji li partner s istim ExternalCode
                var existingPartnerByExternalCode = await _dbConnection.QueryFirstOrDefaultAsync<Partner>(
                    "SELECT * FROM Partner WHERE ExternalCode = @ExternalCode", new { ExternalCode = partner.ExternalCode });

                if (existingPartnerByExternalCode != null)
                {
                    throw new ApplicationException("External Code already exists.");
                }

                Console.WriteLine($"FirstName: {partner.FirstName}");
                Console.WriteLine($"LastName: {partner.LastName}");
                Console.WriteLine($"CityId: {partner.CityId}");
                Console.WriteLine($"PartnerNumber: {partner.PartnerNumber}");

                // Ako CreatedAtUtc nije postavljen, postavlja se trenutni UTC datum
                if (partner.CreatedAtUtc == default)
                {
                    partner.CreatedAtUtc = DateTime.UtcNow;
                }

                // SQL upit za unos partnera u bazu
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
