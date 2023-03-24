using AppCore.Business.Services.Bases;
using AppCore.Results;
using AppCore.Results.Bases;
using Business.Models;
using DataAccess.Entities;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

namespace Business.Services
{
    public interface IProductService : IService<ProductModel>
    {
        Result DeleteImage(int id);
    }

    public class ProductService : IProductService
    {
        private readonly ProductRepoBase _productRepo;

        public ProductService(ProductRepoBase productRepo)
        {
            _productRepo = productRepo;
        }

        public Result Add(ProductModel model)
        {
            //var dbEntity = _productRepo.Query().SingleOrDefault(p => p.Name.ToUpper() == model.Name.ToUpper().Trim());
            //var dbEntity = _productRepo.Query(p => p.Name.ToUpper() == model.Name.ToUpper().Trim()).SingleOrDefault();
            //if (dbEntity is not null)
            //    return new ErrorResult("Product with same name exists!");
            if (_productRepo.Query().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim()))
                return new ErrorResult("Product with same name exists!"); // All

            var entity = new Product()
            {
                //CategoryId = model.CategoryId.HasValue ? model.CategoryId.Value : 0,
                //CategoryId = model.CategoryId ?? 0,
                CategoryId = model.CategoryId.Value,
                //Description = (model.Description ?? "").Trim()
                Description = model.Description?.Trim(),
                ExpirationDate = model.ExpirationDate,
                Name = model.Name.Trim(),
                StockAmount = model.StockAmount.Value,
                UnitPrice = model.UnitPrice.Value,
                ProductStores = model.StoreIds?.Select(sId => new ProductStore()
                {
                    StoreId = sId
                }).ToList(),
                Image = model.Image,
                ImgExtension = model.ImgExtension?.ToLower()
            };
            _productRepo.Add(entity);
            model.Id = entity.Id;
            return new SuccessResult();
        }

        public Result Delete(int id)
        {
			var productStoreEntites = _productRepo.DbContext.Set<ProductStore>().Where(ps => ps.ProductId == id).ToList();
			_productRepo.DbContext.Set<ProductStore>().RemoveRange(productStoreEntites);
			_productRepo.DbContext.SaveChanges();

			//_productRepo.Delete(p => p.Id == id);
			_productRepo.Delete(id);
            return new SuccessResult();
        }

        public void Dispose()
        {
            _productRepo.Dispose();
        }

        public IQueryable<ProductModel> Query()
        {
            // AutoMapper
            return _productRepo.Query(p => p.Category, p => p.ProductStores).Select(p => new ProductModel()
            {
                CategoryId = p.CategoryId,
                Description = p.Description,
                ExpirationDate = p.ExpirationDate,
                Guid = p.Guid,
                Id = p.Id,
                Name = p.Name,
                StockAmount = p.StockAmount,
                UnitPrice = p.UnitPrice,

                UnitPriceDisplay = p.UnitPrice.ToString("C2"),
                ExpirationDateDisplay = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToString("yyyy/MM/dd") : "",
                CategoryDisplay = p.Category.Name,
                StoresDisplay = string.Join("<br />", p.ProductStores.Select(ps => ps.Store.Name)),
                StoreIds = p.ProductStores.Select(ps => ps.StoreId).ToList(),

                Image = p.Image,
                ImgSrcDisplay = p.Image != null ? (p.ImgExtension == ".jpg" || p.ImgExtension == ".jpeg" ? "data:image/jpeg;base64," : "data:image/png;base64,") + Convert.ToBase64String(p.Image) : null
            });
        }

        public Result Update(ProductModel model)
        {
            if (_productRepo.Query().Any(p => p.Name.ToUpper() == model.Name.ToUpper().Trim() && p.Id != model.Id))
                return new ErrorResult("Product with same name exists!");

            var productStoreEntites = _productRepo.DbContext.Set<ProductStore>().Where(ps => ps.ProductId == model.Id).ToList();
            _productRepo.DbContext.Set<ProductStore>().RemoveRange(productStoreEntites);
            _productRepo.DbContext.SaveChanges();

            var entity = _productRepo.Query().SingleOrDefault(p => p.Id == model.Id);
            
            entity.CategoryId = model.CategoryId.Value;
            entity.Description = model.Description?.Trim();
            entity.ExpirationDate = model.ExpirationDate;
            entity.Name = model.Name.Trim();
            entity.StockAmount = model.StockAmount.Value;
            entity.UnitPrice = model.UnitPrice.Value;
            
			entity.ProductStores = model.StoreIds?.Select(sId => new ProductStore()
			{
				StoreId = sId
			}).ToList();

            if (model.Image != null)
            {
                entity.Image = model.Image;
                entity.ImgExtension = model.ImgExtension?.ToLower();
			}

            _productRepo.Update(entity);
            return new SuccessResult();
        }

		public Result DeleteImage(int id)
        {
            var product = _productRepo.Query(p => p.Id == id).SingleOrDefault();
            product.Image = null;
            product.ImgExtension = null;
            _productRepo.Update(product);
            return new SuccessResult();
        }

	}
}
