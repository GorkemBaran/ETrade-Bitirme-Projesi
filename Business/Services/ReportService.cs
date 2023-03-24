using Business.Models;
using DataAccess.Entities;
using DataAccess.Repositories;
using System.Security.Cryptography.X509Certificates;

namespace Business.Services
{
    public interface IReportService
    {
        List<ReportModel> GetListInnerJoin(ReportFilterModel filter);
        List<ReportModel> GetListLeftOuterJoin(ReportFilterModel filter);
    }

    /*
    select p.Name [Product Name],
    p.UnitPrice [Unit Price],
    cast(p.StockAmount as varchar)  + ' adet' [Stock Amount],
    p.expirationdate [Expiration Date],
    c.name [Category Name],
    s.Name [Store Name],
    case when IsVirtual = 1 then 'Virtual' else 'Real' end as Virtual
    from products p inner join categories c
    on p.CategoryId = c.Id
    inner join ProductStores ps
    on ps.ProductId = p.Id
    inner join Stores s
    on ps.StoreId = s.Id
    order by c.Name, p.Name
    */
    public class ReportService : IReportService
    {
        private readonly ProductRepoBase _productRepo;

        public ReportService(ProductRepoBase productRepo)
        {
            _productRepo = productRepo;
        }

        public List<ReportModel> GetListInnerJoin(ReportFilterModel filter)
        {
            var productQuery = _productRepo.Query();
            var categoryQuery = _productRepo.Query<Category>();
            var storeQuery = _productRepo.Query<Store>();
            var productStoreQuery = _productRepo.Query<ProductStore>();
            var query = from product in productQuery
                        join category in categoryQuery
                        on product.CategoryId equals category.Id
                        join productStore in productStoreQuery
                        on product.Id equals productStore.ProductId
                        join store in storeQuery
                        on productStore.StoreId equals store.Id
                        //where product.Name == "HP"
                        //orderby category.Name
                        select new ReportModel()
                        {
                            // Display
                            CategoryName = category.Name,
                            ExpirationDate = product.ExpirationDate.HasValue ? product.ExpirationDate.Value.ToString("MM/dd/yyyy") : "",
                            ProductName = product.Name,
                            StockAmount = product.StockAmount + " units",
                            StoreName = store.Name,
                            UnitPrice = product.UnitPrice.ToString("C2"),
                            Virtual = store.IsVirtual ? "Yes" : "No",

                            // Filter
                            CategoryId = category.Id,
                            UnitPriceValue = product.UnitPrice,
                            ExpirationDateValue = product.ExpirationDate,
                            StoreId = store.Id
                        };
            query = query.OrderBy(q => q.CategoryName).ThenBy(q => q.ProductName);
            if (filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(filter.ProductName))
                {
                    query = query.Where(q => q.ProductName.ToUpper().Contains(filter.ProductName.ToUpper().Trim()));
                }
                if (filter.CategoryId.HasValue)
                {
                    query = query.Where(q => q.CategoryId == filter.CategoryId.Value);
                }
                if (filter.UnitPriceBegin.HasValue)
                {
                    query = query.Where(q => q.UnitPriceValue >= filter.UnitPriceBegin);
                }
                if (filter.UnitPriceEnd.HasValue)
                {
                    query = query.Where(q => q.UnitPriceValue <= filter.UnitPriceEnd);
                }
                if (filter.ExpirationDateBegin.HasValue)
                {
                    query = query.Where(q => q.ExpirationDateValue >= filter.ExpirationDateBegin);
                }
                if (filter.ExpirationDateEnd.HasValue)
                {
                    query = query.Where(q => q.ExpirationDateValue <= filter.ExpirationDateEnd);
                }
                if (filter.StoreIds is not null && filter.StoreIds.Count > 0)
                {
                    query = query.Where(q => filter.StoreIds.Contains(q.StoreId ?? 0));
                }
            }
            return query.ToList();
        }

        public List<ReportModel> GetListLeftOuterJoin(ReportFilterModel filter)
        {
            var productQuery = _productRepo.Query();
            var categoryQuery = _productRepo.Query<Category>();
            var storeQuery = _productRepo.Query<Store>();
            var productStoreQuery = _productRepo.Query<ProductStore>();

            var query = from p in productQuery
                        join c in categoryQuery
                        on p.CategoryId equals c.Id into productcategories
                        from productcategory in productcategories.DefaultIfEmpty()
                        join ps in productStoreQuery
                        on p.Id equals ps.ProductId into productproductstores
                        from productproductstore in productproductstores.DefaultIfEmpty()
                        join s in storeQuery
                        on productproductstore.StoreId equals s.Id into storesproductstores
                        from storesproductstore in storesproductstores.DefaultIfEmpty()
                        select new ReportModel()
                        {
                            // Display
                            CategoryName = productcategory.Name,
                            ExpirationDate = (p.ExpirationDate ?? new DateTime(2000, 1, 1)).ToString("MM/dd/yyyy"),
                            ProductName = p.Name,
                            StockAmount = $"{p.StockAmount} units",
                            StoreName = storesproductstore.Name,
                            UnitPrice = p.UnitPrice.ToString("C2"),
                            Virtual = storesproductstore.IsVirtual ? "Yes" : "No",

                            //Filter
                            CategoryId = productcategory.Id,
                            UnitPriceValue = p.UnitPrice,
                            ExpirationDateValue = p.ExpirationDate,
                            StoreId = storesproductstore.Id
                        };

            // Display
            //CategoryName = category.Name,
            //                ExpirationDate = product.ExpirationDate.HasValue ? product.ExpirationDate.Value.ToString("MM/dd/yyyy") : "",
            //                ProductName = product.Name,
            //                StockAmount = product.StockAmount + " units",
            //                StoreName = store.Name,
            //                UnitPrice = product.UnitPrice.ToString("C2"),
            //                Virtual = store.IsVirtual ? "Yes" : "No",

            //                // Filter
            //                CategoryId = category.Id,
            //                UnitPriceValue = product.UnitPrice,
            //                ExpirationDateValue = product.ExpirationDate,
            //                StoreId = store.Id

            query = query.OrderBy(q => q.CategoryName).ThenBy(q => q.ProductName);
            if (filter is not null)
            {
                if (!string.IsNullOrWhiteSpace(filter.ProductName))
                {
                    query = query.Where(q => q.ProductName.ToUpper().Contains(filter.ProductName.ToUpper().Trim()));
                }
                if (filter.CategoryId.HasValue)
                {
                    query = query.Where(q => q.CategoryId == filter.CategoryId.Value);
                }
                if (filter.UnitPriceBegin.HasValue)
                {
                    query = query.Where(q => q.UnitPriceValue >= filter.UnitPriceBegin);
                }
                if (filter.UnitPriceEnd.HasValue)
                {
                    query = query.Where(q => q.UnitPriceValue <= filter.UnitPriceEnd);
                }
                if (filter.ExpirationDateBegin.HasValue)
                {
                    query = query.Where(q => q.ExpirationDateValue >= filter.ExpirationDateBegin);
                }
                if (filter.ExpirationDateEnd.HasValue)
                {
                    query = query.Where(q => q.ExpirationDateValue <= filter.ExpirationDateEnd);
                }
                if (filter.StoreIds is not null && filter.StoreIds.Count > 0)
                {
                    query = query.Where(q => filter.StoreIds.Contains(q.StoreId ?? 0));
                }
            }
            return query.ToList();
        }

       
    }
}
