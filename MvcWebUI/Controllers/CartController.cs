using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        public IActionResult AddToCart(int productId)
        {
            List<CartItemModel> cart = GetFromSession();



            ProductModel product = _productService.Query().SingleOrDefault(p => p.Id == productId);
            if (product is null)
                return View("_Error", "Product not found!");
            if (product.StockAmount == 0)
            {
                TempData["Message"] = "The product added to cart is not available in stock!";
                return RedirectToAction("Index", "Products");
            }
            int userId = Convert.ToInt32(User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Sid).Value);
            CartItemModel cartItem = new CartItemModel()
            {
                ProductId= productId,
                ProductName = product.Name,
                UnitPrice = product.UnitPrice ?? 0,
                UserId = userId
            };
            cart.Add(cartItem);
            // sepetteki eklenen ÜRÜN sayısı
            int cartProductCount = cart.Count(c => c.ProductId == productId);
            if (cartProductCount > product.StockAmount)
            {
                TempData["Message"] = "The product added to cart is not available in stock!";
            }
            else
            {
                string cartJson = JsonConvert.SerializeObject(cart); // C# -> JSON: Serialize
                HttpContext.Session.SetString("cart", cartJson);
                TempData["Message"] = product.Name + " " + " with " + cartProductCount + " amount added to cart.";
            }
            return RedirectToAction("Index", "Products");
        }

        private List<CartItemModel> GetFromSession()
        {
            List<CartItemModel> cart = new List<CartItemModel>();
            string? cartJson = HttpContext.Session.GetString("cart");
            if (!string.IsNullOrWhiteSpace(cartJson))
                cart = JsonConvert.DeserializeObject<List<CartItemModel>>(cartJson); // JSON -> C#: Deserialize
            return cart;
        }

        public IActionResult GetCart()
        {
            List<CartItemModel> cart = GetFromSession();
            var groupByCart = from c in cart
                              group c by new { c.ProductId, c.UserId, c.ProductName }
                              into cGroupBy
                              select new CartItemGroupByModel()
                              {
                                  ProductId = cGroupBy.Key.ProductId, // gruplananlar key ile ulaşılır
                                  UserId = cGroupBy.Key.UserId,
                                  ProductName = cGroupBy.Key.ProductName,
                                  TotalUnitPrice = cGroupBy.Sum(cgb => cgb.UnitPrice),
                                  TotalUnitPriceDisplay = cGroupBy.Sum(cgb => cgb.UnitPrice).ToString("C2"),
                                  TotalCount = cGroupBy.Count()
                              };

            groupByCart = groupByCart.OrderBy(gbc => gbc.ProductName).ToList();

            //return View("Cart", cart);
            return View("GroupByCart", groupByCart);
        }

        public IActionResult Delete(int productId, int userId)
        {
            List<CartItemModel> cart = GetFromSession();
            CartItemModel cartItem = cart.FirstOrDefault(c => c.ProductId == productId && c.UserId == userId);
            if (cartItem is not null)
            {
                cart.Remove(cartItem);
                HttpContext.Session.SetString("cart", JsonConvert.SerializeObject(cart));
                TempData["Message"] = "Product removed from cart.";
            }
            return RedirectToAction(nameof(GetCart));
        }

        public IActionResult ClearCart()
        {
            //HttpContext.Session.Clear();
            //HttpContext.Session.Remove("cart");
            var cart = GetFromSession();
            var userCart = cart.Where(c => c.UserId == Convert.ToInt32(User.Claims.SingleOrDefault(cl => cl.Type == ClaimTypes.Sid).Value)).ToList();
            foreach (var userCartItem in userCart)
            {
                cart.Remove(userCartItem);
            }
            var cartJson = JsonConvert.SerializeObject(cart);
            HttpContext.Session.SetString("cart", cartJson);
            TempData["Message"] = "Cart cleared.";
            return RedirectToAction(nameof(GetCart));
        }
    }
}
