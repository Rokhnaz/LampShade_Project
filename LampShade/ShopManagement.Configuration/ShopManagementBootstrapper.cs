using Microsoft.Extensions.DependencyInjection;
using ShopManagement.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Application.Contracts.Slide;
using ShopManagement.Domain.ProductAgg;
using ShopManagement.Domain.ProductCategoryAgg;
using ShopManagement.Domain.ProductPictureAgg;
using ShopManagement.Domain.SlideAgg;
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

            servicec.AddTransient<IProductApplication, ProductApplication>();
            servicec.AddTransient<IProductRepository, ProductRepository>();

            servicec.AddTransient<IProductPictureApplication, ProductPictureApplication>();
            servicec.AddTransient<IProductPictureRepository, ProductPictureRepository>();

            servicec.AddTransient<ISlideApplication, SlideApplication>();
            servicec.AddTransient<ISlideRepository, SlideRepository>();

            servicec.AddDbContext<ShopContext>(x => x.UseSqlServer(connectionString));
        }
    }
}
