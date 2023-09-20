using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PBRmatsWeb.Data
{
    public class PBRmatsContext : IdentityDbContext
    {
        public PBRmatsContext(DbContextOptions<PBRmatsContext> options)
            : base(options)
        {
        }
    }
}