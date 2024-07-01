using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.Product;
using ShopManagement.Domain.ProductAgg;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ShopManagement.Application
{
    public class ProductApplication:IProductApplication
    {
        private readonly IProductRepository _productRepository;

        public ProductApplication(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public OperationResult Create(CreateProduct command)
        {
            var Operation=new OperationResult();
            if (_productRepository.Exist(x => x.Name == command.Name))
                return Operation.Failed(ApplicationMessages.DuplicatedRecord);
            var slug = command.Slug.Slugify();
            var product = new Product(command.Name, command.Code, command.ShortDescription,
                command.Description, command.Picture, command.PictureAlt, command.PictureTitle, command.CategoryId,
                slug, command.Keywords, command.MetaDescription);

            _productRepository.Create(product);
            _productRepository.SaveChanges();
            return Operation.Succedded();
        }

        public OperationResult Edit(EditProduct command)
        {
           var operation=new OperationResult();
           var Product = _productRepository.Get(command.Id);
           if (Product == null)
           {
               return operation.Failed(ApplicationMessages.RecordNotFound);
           }

           if (_productRepository.Exist(x => x.Name == command.Name && x.Id != command.Id))
               return operation.Failed(ApplicationMessages.DuplicatedRecord);
           
           var slug = command.Slug.Slugify();
           Product.Edit(command.Name, command.Code, command.ShortDescription,
               command.Description, command.Picture, command.PictureAlt, command.PictureTitle, command.CategoryId,
               slug, command.Keywords, command.MetaDescription);
           _productRepository.SaveChanges();
           return operation.Succedded();

        }
        public EditProduct GetDetails(long id)
        {
           return _productRepository.GetDetails(id);
        }

        public List<ProductViewModel> Search(ProductSearchModel command)
        {
            return _productRepository.Search(command);
        }

        public List<ProductViewModel> GetProducts()
        {
            return _productRepository.GetProducts();
        }
    }
}
