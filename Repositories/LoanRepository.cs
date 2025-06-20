using LibraryManagementSystem.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Repositories
{
    public class LoanRepository : MongoRepository<Loan>
    {
        private readonly IMongoCollection<Loan> _loanCollection;

        public LoanRepository(IConfiguration configuration)
            : base(configuration, "Loans")
        {
            var client = new MongoDB.Driver.MongoClient(configuration["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(configuration["MongoDbSettings:DatabaseName"]);
            _loanCollection = database.GetCollection<Loan>("Loans");
        }

        public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
        {
            var filter = Builders<Loan>.Filter.Eq(l => l.ReturnDate, null);
            return await _loanCollection.Find(filter).ToListAsync();
        }
    }
} 