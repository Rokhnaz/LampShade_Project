﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using InventoryManagement.Application.Contract.Inventory;
using InventoryManagement.Domain.InventoryAgg;
using Microsoft.EntityFrameworkCore;
using ShopManagement.Infrastructure.EFCore;

namespace InvnentoryManagement.Infrastructure.EFCore.Repository
{
    public class InventoryRepository:RepositoryBase<long,Inventory>,IInventoryRepository
    {

        private readonly InventoryContext _inventoryContext;
        private readonly ShopContext _shopContext;
        public InventoryRepository(InventoryContext context,ShopContext shopContext) : base(context)
        {
            _inventoryContext = context;
            _shopContext = shopContext;
        }

        public EditInventory GetDetails(long id)
        {
           return _inventoryContext.Inventory.Select(x => new EditInventory()
           {
               Id = x.Id,
               ProductId = x.ProductId,
               UnitPrice = x.UnitPrice,

           }).FirstOrDefault(x => x.Id == id);
        }

        public Inventory GetBy(long ProductId)
        {
            return _inventoryContext.Inventory.FirstOrDefault(x => x.ProductId == ProductId);
        }

        public List<InventoryViewModel> Search(InventorySearchModel searchModel)
        {
            var products = _shopContext.Products.Select(x => new { x.Id, x.Name }).ToList();
            var query = _inventoryContext.Inventory.Select(x => new InventoryViewModel
            {
                Id = x.Id,
                UnitPrice = x.UnitPrice,
                InStock = x.InStock,
                ProductId = x.ProductId,
                CurrentCount = x.CalculateCurrentCount(),
                CreationDate = x.CreationDate.ToFarsi()
            });

            if (searchModel.ProductId > 0)
                query = query.Where(x => x.ProductId == searchModel.ProductId);

            if (searchModel.InStock)
                query = query.Where(x => !x.InStock);

            var inventory = query.OrderByDescending(x => x.Id).ToList();

            inventory.ForEach(item =>
                item.Product = products.FirstOrDefault(x => x.Id == item.ProductId)?.Name);

            return inventory;
        }

        public List<InventoryOperationViewModel> GetOperationLog(long InventoryId)
        {
            var inventory = _inventoryContext.Inventory.FirstOrDefault(x => x.Id == InventoryId);
            return inventory.Operations.Select(x => new InventoryOperationViewModel()
            {
                Id = x.Id,
                Count = x.Count,
                CurrentCount = x.CurrentCount,
                Description = x.Description,
                Operation = x.Operation,
                OperationDate = x.OperationDate.ToFarsi(),
                Operator = "مدیر سیستم",
                OperatorId = x.OperatorId,
                OrderId = x.OrderId
            }).OrderByDescending(x=>x.Id).ToList();
        }
    }
}
