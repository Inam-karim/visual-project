using LibraryManagementSystem.Models;
using Microsoft.Extensions.Configuration;

namespace LibraryManagementSystem.Repositories
{
    public class BookRepository : MongoRepository<Book>
    {
        public BookRepository(IConfiguration configuration)
            : base(configuration, "Books")
        {
        }
    }
} 