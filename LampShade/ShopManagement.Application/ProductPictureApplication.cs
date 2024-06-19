using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using ShopManagement.Application.Contracts.ProductPicture;
using ShopManagement.Domain.ProductPictureAgg;

namespace ShopManagement.Application
{
    public class ProductPictureApplication:IProductPictureApplication
    {
        private  readonly IProductPictureRepository _productPictureRepository;

        public ProductPictureApplication(IProductPictureRepository productPictureRepository)
        {
            _productPictureRepository = productPictureRepository;
        }
        public OperationResult Create(CreateProductPicture command)
        {
            var Operation=new OperationResult();
            if (_productPictureRepository.Exist(x => x.Picture == command.Picture && x.productId == command.productId))
                return Operation.Failed(ApplicationMessages.DuplicatedRecord);
            var productPicture = new ProductPicture(command.productId, command.Picture, command.PictureAlt,
                command.PictureTitle);
            _productPictureRepository.Create(productPicture);
            _productPictureRepository.SaveChanges();
            return Operation.Succedded();

        }

        public OperationResult Edit(EditProductPicture command)
        {
            var Operation=new OperationResult();
            var productPicture = _productPictureRepository.Get(command.Id);
            if (productPicture == null)
                return Operation.Failed(ApplicationMessages.RecordNotFound);
            if (_productPictureRepository.Exist(x =>
                    x.Picture == command.Picture && x.productId == command.productId && x.Id != command.Id))
                return Operation.Failed(ApplicationMessages.DuplicatedRecord);

            productPicture.Edit(command.productId, command.Picture, command.PictureAlt, command.PictureTitle);
            _productPictureRepository.SaveChanges();
            return Operation.Succedded();
        }

        public OperationResult Remove(long id)
        {
            var Operation = new OperationResult();
            var productPicture= _productPictureRepository.Get(id);
            if (productPicture == null)
                return Operation.Failed(ApplicationMessages.RecordNotFound);
            productPicture.Remove();
            _productPictureRepository.SaveChanges();
            return Operation.Succedded();
        }

        public OperationResult Restore(long id)
        {
            var Operation = new OperationResult();
            var productPicture = _productPictureRepository.Get(id);
            if (productPicture == null)
                return Operation.Failed(ApplicationMessages.RecordNotFound);
            productPicture.Restore();
            _productPictureRepository.SaveChanges();
            return Operation.Succedded();
        }

        public EditProductPicture GetDetails(long id)
        {
            return _productPictureRepository.GetDetails(id);
        }

        public List<ProductPictureViewModel> Search(ProductPictureSearchModel searchModel)
        {
            return _productPictureRepository.Search(searchModel);
        }
    }
}
