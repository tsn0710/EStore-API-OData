using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;
using StoreClient.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace StoreClient.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HttpClient client = null;
        private string ProductApiUrl = "";
        private string CategoriesApiUrl = "";
        public ProductsController()
        {
            client = new HttpClient();
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ProductApiUrl = "https://localhost:7222/odata/Products";
            CategoriesApiUrl = "https://localhost:7222/odata/Categories";
        }
        public async Task<IActionResult> IndexAsync(int pageNumber, string productName, int UnitpriceFrom, int UnitpriceTo)
        {
            if (productName == null) { productName = string.Empty; }
            if (pageNumber == 0)
            {
                pageNumber = 1;
            }
            if (UnitpriceFrom == 0 && UnitpriceTo == 0)
            {
                UnitpriceFrom = 0;
                UnitpriceTo = 999999;
            }
            if (UnitpriceFrom > UnitpriceTo)
            {
                UnitpriceFrom = 0;
                UnitpriceTo = 999999;
            }
            if (pageNumber == -10)
            {
                pageNumber = 1; productName = ""; UnitpriceFrom = 0;
                UnitpriceTo = 999999;
            }
            //lay list Product tu DataBase
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            var listProductsj = data["value"];
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Product> allProducts = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(listProductsj.ToString(), options);
            //loc list dua vao Name va UnitPrice
            List<Product> allProductsFound = allProducts
                .Where(p => p.ProductName.Contains(productName)
                && p.UnitPrice <= UnitpriceTo
                && p.UnitPrice >= UnitpriceFrom)
                .ToList();
            //lay ra 5 product //may be 3 may be 2 or 1 or 0  becareful
            PageList<Product> fiveProduct = PageList<Product>.Create(allProductsFound.AsQueryable(), pageNumber, 3);
            ViewData["fiveProduct"] = fiveProduct;
            ViewBag.productName = productName;
            ViewBag.UnitpriceFrom = UnitpriceFrom;
            ViewBag.UnitpriceTo = UnitpriceTo;
            ViewBag.pageCount = fiveProduct.TotalPages;
            ViewBag.pageNumber = pageNumber;
            //fix loi dac biet: ko tim thay
            if (fiveProduct.Count() == 0)
            {
                ViewBag.pageNumber = 0;
            }
            return View();
        }
        public async Task<IActionResult> CreateAsync()
        {
            HttpResponseMessage response = await client.GetAsync(CategoriesApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            var listCategoriesj = data["value"];
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Category> listCategories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(listCategoriesj.ToString(), options);
            ViewData["categories"] = listCategories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Product product)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Product>(product, options);
      
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7222/odata/Products", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Products");

            }
            return RedirectToAction("Create", "Products");
        }

        public async Task<IActionResult> DetailAsync(int productid)
        {
            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + "(" + productid + ")");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            Product product = System.Text.Json.JsonSerializer.Deserialize<Product>(data.ToString(), options);
            return View(product);
        }

        public async Task<IActionResult> DeleteAsync(int productid)
        {
            if (HttpContext.Session.GetInt32("MemberId") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpResponseMessage response = await client.DeleteAsync(ProductApiUrl + "(" + productid + ")");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> UpdateAsync(int productid)
        {
            HttpResponseMessage responseC = await client.GetAsync(CategoriesApiUrl);
            string strDataC = await responseC.Content.ReadAsStringAsync();
            var dataC = JObject.Parse(strDataC);
            var listCategoriesj = dataC["value"];
            var optionsC = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Category> listCategories = System.Text.Json.JsonSerializer.Deserialize<List<Category>>(listCategoriesj.ToString(), optionsC);
            ViewData["categories"] = listCategories;

            HttpResponseMessage response = await client.GetAsync(ProductApiUrl + "(" + productid + ")");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            Product product = System.Text.Json.JsonSerializer.Deserialize<Product>(data.ToString(), options);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(Product product)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Product>(product, options);

            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"https://localhost:7222/odata/Products/{product.ProductId}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Products");

            }
            return RedirectToAction("Update", "Products");
        }
    }
}
