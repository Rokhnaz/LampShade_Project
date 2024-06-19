using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Infrastructure;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Infrastructure.EFCore.Repository
{
    public class ProductPictureRepository:RepositoryBase<long,ProductPicture>,IProductPictureRepository
    {
        private readonly ShopContext _Context;
        public ProductPictureRepository(ShopContext Context):base(Context)
        {
            _Context = Context;
        }

        public EditProductPicture GetDetails(long id)
        {
            return _Context.ProductPictures.Select(x => new EditProductPicture()
            {
                Id = x.Id,
                Picture = x.Picture,
                PictureAlt = x.PictureAlt,
                PictureTitle = x.PictureTitle,
                productId = x.productId
            }).FirstOrDefault(x => x.Id == id);
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            var query = _Context.ProductPictures.Include(x => x.Product).Select(x => new ProductPictureViewModel()
            {
                Id = x.Id,
                Picture = x.Picture,
                CreationDate = x.CreationDate.ToString(),
                Product = x.Product.Name,
                ProductId=x.productId,
                IsRemoved = x.IsRemoved
            });

            if(searchModel.ProductId!=0)
                query= query.Where(x => x.Id == searchModel.ProductId);
            return query.OrderByDescending(x => x.Id).ToList();
        }
    }
}
