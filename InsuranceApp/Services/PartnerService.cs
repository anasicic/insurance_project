using Dapper;
using InsuranceApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

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

        // Get city name by CityId
        public async Task<string> GetCityNameByIdAsync(int cityId)
        {
            try
            {
                var query = "SELECT CityName FROM City WHERE CityId = @CityId";
                var cityName = await _dbConnection.QuerySingleOrDefaultAsync<string>(query, new { CityId = cityId });
                return cityName ?? "Unknown City";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching city name for ID {cityId}: {ex.Message}");
                return "Unknown City";
            }
        }

        // Get all cities
        public async Task<IEnumerable<City>> GetAllCitiesAsync()
        {
            const string query = "SELECT CityId, CityName FROM City ORDER BY CityName";
            return await _dbConnection.QueryAsync<City>(query);
        }

        // Add a new partner
        public async Task AddPartnerAsync(Partner partner)
        {
            try
            {
                // Check if a partner with the same CroatianPIN already exists
                var existingPartnerByPIN = await _dbConnection.QueryFirstOrDefaultAsync<Partner>(
                    "SELECT * FROM Partner WHERE CroatianPIN = @CroatianPIN", new { CroatianPIN = partner.CroatianPIN });

                if (existingPartnerByPIN != null)
                {
                    throw new ApplicationException("OIB (Croatian PIN) already exists.");
                }

                // Check if a partner with the same ExternalCode already exists
                var existingPartnerByExternalCode = await _dbConnection.QueryFirstOrDefaultAsync<Partner>(
                    "SELECT * FROM Partner WHERE ExternalCode = @ExternalCode", new { ExternalCode = partner.ExternalCode });

                if (existingPartnerByExternalCode != null)
                {
                    throw new ApplicationException("External Code already exists.");
                }

                // Set CreatedAtUtc if not set
                if (partner.CreatedAtUtc == default)
                {
                    partner.CreatedAtUtc = DateTime.UtcNow;
                }

                // SQL query to insert partner
                var query = @"
                    INSERT INTO Partner (FirstName, LastName, Address, PartnerNumber, CroatianPIN, PartnerTypeId, CreatedAtUtc, CreateByUser, IsForeign, ExternalCode, Gender, CityId)
                    VALUES (@FirstName, @LastName, @Address, @PartnerNumber, @CroatianPIN, @PartnerTypeId, @CreatedAtUtc, @CreateByUser, @IsForeign, @ExternalCode, @Gender, @CityId);
                    SELECT last_insert_rowid();";  // SQLite specific query to get the last inserted ID
                
                var partnerId = await _dbConnection.QuerySingleAsync<int>(query, partner);
                partner.PartnerId = partnerId;

                Console.WriteLine($"Partner ID: {partnerId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding partner: {ex.Message}");
                throw new ApplicationException("An error occurred while adding the partner.", ex);
            }
        }

        // Get policy count for a partner
        public async Task<int> GetPolicyCountByPartnerIdAsync(int partnerId)
        {
            const string query = "SELECT COUNT(*) FROM Policy WHERE PartnerId = @PartnerId";
            return await _dbConnection.QuerySingleAsync<int>(query, new { PartnerId = partnerId });
        }

        // Get total policy amount for a partner
        public async Task<decimal> GetPolicyTotalAmountByPartnerIdAsync(int partnerId)
        {
            const string query = "SELECT COALESCE(SUM(PolicyAmount), 0) FROM Policy WHERE PartnerId = @PartnerId";
            return await _dbConnection.QuerySingleAsync<decimal>(query, new { PartnerId = partnerId });
        }

        // Get partner details with policy information
        public async Task<(Partner, int, decimal)> GetPartnerDetailsWithPoliciesAsync(int partnerId)
        {
            try
            {
                var partner = await GetPartnerByIdAsync(partnerId);

                if (partner == null)
                {
                    throw new KeyNotFoundException($"Partner with ID {partnerId} not found.");
                }

                var policyCount = await GetPolicyCountByPartnerIdAsync(partnerId);
                var totalPolicyAmount = await GetPolicyTotalAmountByPartnerIdAsync(partnerId);

                return (partner, policyCount, totalPolicyAmount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching partner details for ID {partnerId}: {ex.Message}");
                throw;
            }
        }
    }
}
