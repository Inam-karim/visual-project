using LibraryManagementSystem.Models;
using Microsoft.Extensions.Configuration;

namespace LibraryManagementSystem.Repositories
{
    public class MemberRepository : MongoRepository<Member>
    {
        public MemberRepository(IConfiguration configuration)
            : base(configuration, "Members")
        {
        }
    }
} 