using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Domain;
using _0_Framework.Infrastructure;
using DiscountManagement.Application.Contract.CustomerDiscount;
using DiscountManagement.Domain.CustomerDiscountAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;

namespace DiscountManagement.Infrastructure.EFCore.Repository
{
    public class CustomerDiscountRepository:RepositoryBase<long,CustomerDiscount>,ICustomerDiscountRepository

    {
        private readonly DiscountContext _discountContext;
        private readonly ShopContext _shopContext;

        public CustomerDiscountRepository(ShopContext shopContext, DiscountContext discountContext) : base(discountContext)
        {
            _discountContext = discountContext;
            _shopContext = shopContext;
        }


        public EditCustomerDiscount GetDetails(long id)
        {
            return _discountContext.CustomerDiscounts.Select(x => new EditCustomerDiscount()
            {
                Id = id,
                ProductId = x.ProductId,
                DiscountRate = x.DiscountRate,
                StartDate = x.StartDate.ToString(),
                EndDate = x.EndDate.ToString(),
                Reason = x.Reason,
                

            }).FirstOrDefault(x=>x.Id==id);
        }

        public List<CustomerDiscountViewModel> Search(CustomerDiscountSearchModel searchModel)
        {
            var Products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();
            var query = _discountContext.CustomerDiscounts.Select(x => new CustomerDiscountViewModel()
            {
                Id = x.Id,
                DiscountRate = x.DiscountRate,
                ProductId = x.ProductId,
                StartDate = x.StartDate.ToFarsi(),
                EndDate = x.EndDate.ToFarsi(),
                Reason = x.Reason,
                StartDateGr = x.StartDate,
                EndDateGr = x.EndDate,
                CreationDate = x.CreationDate.ToFarsi(),


            });
            if (searchModel.ProductId>0)
            {
                query = query.Where(x => x.ProductId == searchModel.ProductId);
            }

            if (!string.IsNullOrWhiteSpace(searchModel.StartDate))
            {
                query = query.Where(x => x.StartDateGr > searchModel.StartDate.ToGeorgianDateTime());

            }
            if (!string.IsNullOrWhiteSpace(searchModel.EndDate))
            {
                query = query.Where(x => x.EndDateGr > searchModel.EndDate.ToGeorgianDateTime());

            }

            var discounts = query.OrderByDescending(x => x.Id).ToList();
            discounts.ForEach(discount=>discount.Product=Products.FirstOrDefault(x=>x.Id==discount.ProductId)?.Name);
            return discounts;
        }
    }
}
