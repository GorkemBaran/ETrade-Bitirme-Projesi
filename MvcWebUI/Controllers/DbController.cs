using DataAccess.Contexts;
using DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace MvcWebUI.Areas.Database.Controllers
{
    public class DbController : Controller // ~/Db/Seed
    {
        private readonly ETradeContext _db;

        public DbController(ETradeContext db)
        {
            _db = db;
        }

        public IActionResult Seed()
        {
            var productStores = _db.ProductStores.ToList();
            _db.ProductStores.RemoveRange(productStores);

            var stores = _db.Stores.ToList();
            _db.Stores.RemoveRange(stores);

            var products = _db.Products.ToList();
            _db.Products.RemoveRange(products);

            var categories = _db.Categories.ToList();
            _db.Categories.RemoveRange(categories);

            var users = _db.Users.ToList();
            _db.Users.RemoveRange(users);

            var roles = _db.Roles.ToList();
            _db.Roles.RemoveRange(roles);

            if (roles.Count > 0)
            {
                _db.Database.ExecuteSqlRaw("dbcc CHECKIDENT ('Roles', RESEED, 0)");
            }

            _db.Stores.Add(new Store()
            {
                Name = "Hepsiburada",
                IsVirtual = true
            });
            _db.Stores.Add(new Store()
            {
                Name = "Vatan",
                IsVirtual = false
            });

            _db.SaveChanges();

            _db.Categories.Add(new Category()
            {
                Name = "Computer",
                Description = "Laptops, desktops and computer peripherals",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Laptop",
                        UnitPrice = 3000.5,
                        ExpirationDate = new DateTime(2032, 1, 27),
                        StockAmount = 10,
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Mouse",
                        UnitPrice = 20.5,
                        StockAmount = 50,
                        Description = "Computer peripheral",
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            },
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Keyboard",
                        UnitPrice = 40,
                        StockAmount = 45,
                        Description = "Computer peripheral",
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            },
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Monitor",
                        UnitPrice = 2500,
                        ExpirationDate = DateTime.Parse("05/19/2027"),
                        StockAmount = 20,
                        Description = "Computer peripheral",
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    }
                }
            });

            _db.Categories.Add(new Category()
            {
                Name = "Home Theater System",
                Products = new List<Product>()
                {
                    new Product()
                    {
                        Name = "Speaker",
                        UnitPrice = 2500,
                        StockAmount = 70
                    },
                    new Product()
                    {
                        Name = "Receiver",
                        UnitPrice = 5000,
                        StockAmount = 30,
                        Description = "Home theater system component",
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    },
                    new Product()
                    {
                        Name = "Equalizer",
                        UnitPrice = 1000,
                        StockAmount = 40,
                        ProductStores = new List<ProductStore>()
                        {
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Hepsiburada").Id
                            },
                            new ProductStore()
                            {
                                StoreId = _db.Stores.SingleOrDefault(s => s.Name == "Vatan").Id
                            }
                        }
                    }
                }
            });

            _db.Roles.Add(new Role()
            {
                Name = "Admin",
                Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Password = "gorkem",
                        UserName = "gorkem"
                    }
                }
            });
            _db.Roles.Add(new Role()
            {
                Name = "User",
                Users = new List<User>()
                {
                    new User()
                    {
                        IsActive = true,
                        Password = "baran",
                        UserName = "baran"
                    }
                }
            });

            _db.SaveChanges();

            return Content("<label style=\"color:red\";><b>Database seed successful.</b></label>", "text/html", Encoding.UTF8);
        }
    }
}
