using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace LibTads.EntityFrameworkCore
{
    public static class LibTadsDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<LibTadsDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<LibTadsDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
