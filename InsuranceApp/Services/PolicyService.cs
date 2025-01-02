using InsuranceApp.Models;
using System.Data;
using Dapper;

namespace InsuranceApp.Services
{
    public class PolicyService
    {
        private readonly IDbConnection _dbConnection;

        public PolicyService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // Get all policies
        public async Task<IEnumerable<Policy>> GetAllPoliciesAsync()
        {
            try
            {
                var query = "SELECT * FROM Policy";
                return await _dbConnection.QueryAsync<Policy>(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all policies: {ex.Message}");
                throw; 
            }
        }

        // Get policies for a specific partner
        public async Task<IEnumerable<Policy>> GetPoliciesByPartnerIdAsync(int partnerId)
        {
            try
            {
                var query = "SELECT * FROM Policy WHERE PartnerId = @PartnerId";
                return await _dbConnection.QueryAsync<Policy>(query, new { PartnerId = partnerId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching policies for partner ID {partnerId}: {ex.Message}");
                throw; 
            }
        }

        // Get a policy by PolicyNumber to check for duplicates
        public async Task<Policy?> GetPolicyByNumberAsync(string policyNumber)
        {
            try
            {
                var query = "SELECT * FROM Policy WHERE PolicyNumber = @PolicyNumber";
                // It returns a Policy or null if it does not exist.
                return await _dbConnection.QueryFirstOrDefaultAsync<Policy>(query, new { PolicyNumber = policyNumber });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking policy by number: {ex.Message}");
                throw;
            }
        }

        // Add a new policy
        public async Task AddPolicyAsync(Policy policy)
        {
            try
            {
                // Check if the policy number already exists
                var existingPolicy = await GetPolicyByNumberAsync(policy.PolicyNumber);
                if (existingPolicy != null)
                {
                    throw new InvalidOperationException("Policy number already exists.");
                }

                var query = @"
                    INSERT INTO Policy (PolicyNumber, PolicyAmount, PartnerId)
                    VALUES (@PolicyNumber, @PolicyAmount, @PartnerId)";
                await _dbConnection.ExecuteAsync(query, policy);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding new policy: {ex.Message}");
                throw;
            }
        }
    }
}
