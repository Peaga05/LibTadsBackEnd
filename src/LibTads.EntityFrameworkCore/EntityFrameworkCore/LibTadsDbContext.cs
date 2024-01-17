using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using LibTads.Authorization.Roles;
using LibTads.Authorization.Users;
using LibTads.MultiTenancy;
using LibTads.Domain;

namespace LibTads.EntityFrameworkCore
{
    public class LibTadsDbContext : AbpZeroDbContext<Tenant, Role, User, LibTadsDbContext>
    {
        
        public DbSet<Autor> Autores { get; set; }
        public LibTadsDbContext(DbContextOptions<LibTadsDbContext> options)
            : base(options)
        {
        }
    }
}
