using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManagement.Domain.InventoryAgg;
using InvnentoryManagement.Infrastructure.EFCore.Mappings;
using Microsoft.EntityFrameworkCore;

namespace InvnentoryManagement.Infrastructure.EFCore
{
    public class InventoryContext:DbContext
    {
        public InventoryContext(DbContextOptions<InventoryContext> options):base(options)
        {
            
        }

        public DbSet<Inventory> Inventory;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(InventoryMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
