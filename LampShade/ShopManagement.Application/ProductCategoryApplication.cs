using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductCategory;
using ShopManagement.Domain.ProductCategoryAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagement.Application
{
    public class ProductCategoryApplication : IProductCategoryApplication
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryApplication(IProductCategoryRepository productCategoryRepositoryRepository)
        {
            _productCategoryRepository = productCategoryRepositoryRepository; 
        }

        public OperationResult Create(CreateProductCategory command)
        {
            var OperationResult= new OperationResult();
            if (_productCategoryRepository.Exist(x=>x.Name==command.Name))
            {
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);
            }

            var slug = command.Slug.Slugify();

            var ProductCategory = new ProductCategory(command.Name, command.Description, command.Picture,
                command.PictureAlt, command.PictureTitle, command.Keywords, command.MetaDescription, slug);
            _productCategoryRepository.Create(ProductCategory);

            _productCategoryRepository.SaveChanges();

            return OperationResult.Succedded();

        }

        public OperationResult Edit(EditProductCategory command)
        {
            var OperationResult=new OperationResult();

            var productCategory = _productCategoryRepository.Get(command.Id);

            if (productCategory == null)
                return OperationResult.Failed(ApplicationMessages.RecordNotFound);


            if (_productCategoryRepository.Exist(x => x.Name == command.Name && x.Id != command.Id))
                return OperationResult.Failed(ApplicationMessages.DuplicatedRecord);

            var slug= command.Slug.Slugify();
            productCategory.Edit(command.Name, command.Description, command.Picture, command.PictureAlt,
                command.PictureTitle, command.Keywords, command.MetaDescription, slug);
            _productCategoryRepository.SaveChanges();
            return OperationResult.Succedded();
        }

        public EditProductCategory GetDetails(long id)
        {
           return _productCategoryRepository.GetDetails(id);
        }

        public List<ProductCategoryViewModel> GetProductCategories()
        {
            return _productCategoryRepository.GetProductCategories();
        }

        public List<ProductCategoryViewModel> Search(ProductCategorySearchModel command)
        {
            return _productCategoryRepository.Search(command);
        }
    }
}
