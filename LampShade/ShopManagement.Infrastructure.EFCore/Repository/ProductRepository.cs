using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Domain;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductRepository:RepositoryBase<long,Product>,IProductRepository
    {
        private readonly ShopContext _Context;
        public ProductRepository(ShopContext context) : base(context)
        {
            _Context = context;
        }

        public EditProduct GetDetails(long id)
        {
            return _Context.Products.Select(x => new EditProduct()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Slug = x.Slug,
                CategoryId = x.CategoryId,
                Description = x.Description,
                Keywords = x.Keywords,
                MetaDescription = x.Description,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                ShortDescription = x.ShortDescription,
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<ProductViewModel> GetProducts()
        {
            return _Context.Products.Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
        }

        public List<ProductViewModel> Search(ProductSearchModel searchModel)
        {
            var query = _Context.Products.Include(x => x.Category).Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Code = x.Code,
                Category = x.Category.Name,
                Picture = x.Picture,
                CategoryId= x.CategoryId,
                CreationDate = x.CreationDate.ToFarsi(),

            });
            if (!string.IsNullOrWhiteSpace(searchModel.Name))
                query = query.Where(x => x.Name.Contains(searchModel.Name));

            if (!string.IsNullOrWhiteSpace(searchModel.Code))
                query = query.Where(x => x.Code.Contains(searchModel.Code));

            if (searchModel.CategoryId!=0)
                query = query.Where(x => x.CategoryId==searchModel.CategoryId);

            return query.OrderByDescending(x => x.Id).ToList();
        }
    }

}
