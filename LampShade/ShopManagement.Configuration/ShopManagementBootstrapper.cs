using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Infrastructure.EFCore;
using ShopManagement.Infrastructure.EFCore.Repository;

namespace ShopManagement.Configuration
{
    public class ShopManagementBootstrapper
    {
        public static void Configure(IServiceCollection servicec,string connectionString)
        {
            servicec.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();
            servicec.AddTransient<IProductCategoryRepository, ProductCategoryRepository>();

            servicec.AddTransient<IProductCategoryApplication, ProductCategoryApplication>();

            servicec.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
