using DiscountManagement.Domain.ColleagueDiscountAgg;
using DiscountManagement.Domain.CustomerDiscountAgg;
using DiscountManagement.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DiscountManagement.Infrastructure.EFCore
{
    public class DiscountContext :DbContext
    {
        public DbSet<CustomerDiscount> CustomerDiscounts { get; set; }
        public DbSet<ColleagueDiscount> ColleagueDiscounts { get; set; }


        public DiscountContext(DbContextOptions<DiscountContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(CustomerDiscountMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer();
        //}

        public class DatabaseDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DiscountContext>
        {
            public DiscountContext CreateDbContext(string[] args)
            {
                var builder = new DbContextOptionsBuilder<DiscountContext>();
                var connecton =
                    "Data Source=.;Initial Catalog=Lampshade;Integrated Security=true;TrustServerCertificate=True;";
                builder.UseSqlServer(connecton);
                return new DiscountContext(builder.Options);
            }
        }



    }
}
