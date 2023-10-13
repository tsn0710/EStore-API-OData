using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StoreClient.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace StoreClient.Controllers
{
    public class OrderController : Controller
    {
        private readonly HttpClient client = null;
        private string OrderApiUrl = "";
        private string MemberApiUrl="";
        private string ProductApiUrl = "";
        private string OrderDetailApiUrl = "";
        public OrderController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:7222/odata/Orders";
            MemberApiUrl = "https://localhost:7222/odata/Members";
            ProductApiUrl = "https://localhost:7222/odata/Products";
            OrderDetailApiUrl = "https://localhost:7222/odata/OrderDetails";
        }
        public async Task<IActionResult> IndexAsync()
        {
            //lay list Product tu DataBase
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            var listOrderj = data["value"];
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Order> listOrders = System.Text.Json.JsonSerializer.Deserialize<List<Order>>(listOrderj.ToString(), options);
            return View(listOrders);
        }

        public async Task<IActionResult> CreateAsync()
        {
            //lay list Member tu DataBase
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            var listMemberj = data["value"];
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Member> members = System.Text.Json.JsonSerializer.Deserialize<List<Member>>(listMemberj.ToString(), options);
            ViewData["members"] = members;
            ViewData["orderdate"] = DateTime.Now;
            ViewData["shippeddate"] = DateTime.Now;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromForm] Order order)
        {
            //Order a = new Order()
            //{
            //    MemberId = order.MemberId,
            //    OrderDate = order.OrderDate,
            //    RequiredDate = order.RequiredDate,
            //    ShippedDate = order.ShippedDate,
            //    Freight = order.Freight
            //};
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Order>(order, options);
            //string one = "{\"@odata.type\" : \"Collection(StoreAPI.Models.Order)\",";
            //json=json.Remove(0, 1);
            //string jon1 = one + json ;
            //json = "{\"OrderId\": 0,\"MemberId\": 999,\"OrderDate\": \"2023-09-16T00:00:00+07:00\",\"RequiredDate\": \"2023-09-20T00:00:00+07:00\",\"ShippedDate\": \"2023-09-19T00:00:00+07:00\",\"Freight\": 2}";
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7222/odata/Orders", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Order");

            }
            return RedirectToAction("Create", "Order");

            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7222/odata/Orders");
            //var content = new MultipartFormDataContent();
            //content.Add(new StringContent("0"), "OrderId");
            //content.Add(new StringContent("6"), "MemberId");
            //content.Add(new StringContent("2023-10-12T00:00:00"), "OrderDate");
            //content.Add(new StringContent("2023-10-12T00:00:00"), "RequiredDate");
            //content.Add(new StringContent("2023-10-22T00:00:00"), "ShippedDate");
            //content.Add(new StringContent(""), "OrderDetails");
            //request.Content = content;
            //var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            //return RedirectToAction("Create", "Order");
        }

        public async Task<IActionResult> UpdateOrderAsync(int orderid)
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl+"("+ orderid + ")?$expand=OrderDetails");
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Order order = System.Text.Json.JsonSerializer.Deserialize<Order>(data.ToString(), options);
            List<OrderDetailView> odvs = new List<OrderDetailView>();
            foreach(OrderDetail od in order.OrderDetails)
            {
                OrderDetailView odv = new OrderDetailView()
                {
                    OrderDetailId=od.OrderDetailId,
                    ProductId= od.ProductId,
                    UnitPrice= od.UnitPrice,
                    Quantity= od.Quantity,
                    Discount= od.Discount,
                    isDeleted="f"
                };
                odvs.Add(odv);
            }
            OrderView ov = new OrderView()
            {
                OrderId= orderid,
                MemberId=order.MemberId,
                OrderDate=order.OrderDate,
                RequiredDate=order.RequiredDate,
                ShippedDate=order.ShippedDate,
                Freight=order.Freight,
                MaxNo=odvs.Count,
                OrderDetailsView=odvs
            };
            //ViewData["order"] = ov;
            //lay list Product tu DataBase
            HttpResponseMessage response1 = await client.GetAsync(MemberApiUrl);
            string strData1 = await response1.Content.ReadAsStringAsync();
            var data1 = JObject.Parse(strData1);
            var listMemberj1 = data1["value"];
            var options1 = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Member> members = System.Text.Json.JsonSerializer.Deserialize<List<Member>>(listMemberj1.ToString(), options1);
            ViewData["members"] = members;
            //lay list Product tu DataBase
            HttpResponseMessage response2 = await client.GetAsync(ProductApiUrl);
            string strData2 = await response2.Content.ReadAsStringAsync();
            var data2 = JObject.Parse(strData2);
            var listProductj = data2["value"];
            List<Product> products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(listProductj.ToString(), options);
            ViewData["products"] = products;
            return View(ov);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateOrderPostAsync(IFormCollection collection)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            //lay gia tri max
            //lap voi moi OrderDetail
            //neu la delete thi phai co OrderDetailID de xoa
            //neu la old thi next
            //neu la new thi add (can OrderID)
            int max = Int32.Parse(collection["MaxNo2"]);
            int orderId = Int32.Parse(collection["orderID"]);
            bool result = true;
            for(int i = 1; i <= max; i++)
            {
                
                if ((collection["isDelete_" + i].Equals("d")) && (collection["orderDetailId_" + i].ToString().Length!=0))
                {
                    result = await deleteOrderDetailAsync(collection["orderDetailId_" + i]);
                }
                else if(collection["isDelete_" + i].Equals("o"))
                {
                }
                else if (collection["isDelete_" + i].Equals("n"))
                {
                    result = await addOrderDetailAsync(orderId,collection["productid_" + i], collection["unitprice_" + i], collection["quantity_" + i], collection["discount_" + i]);
                }
                else
                {
                }
                if (result == false)
                {
                    return RedirectToAction("UpdateOrder", "Order", new { orderid =orderId});
                }
            }
            if (result == true)
            {
                return RedirectToAction("Index", "Order");
            }
            else
            {
                return RedirectToAction("Create", "Order");
            }
        }


        private async Task<bool> addOrderDetailAsync(int orderId, StringValues productid_5, StringValues unitprice_5, StringValues quantity_5, StringValues discount_5)
        {
            OrderDetail od = new OrderDetail()
            {
                OrderDetailId = 0,
                ProductId = Int32.Parse(productid_5),
                OrderId = orderId,
                UnitPrice = Int32.Parse(unitprice_5),
                Quantity = Int32.Parse(quantity_5),
                Discount = Int32.Parse(discount_5)
            };
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<OrderDetail>(od, options);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(OrderDetailApiUrl, httpContent);
            if (response.IsSuccessStatusCode)
            {
                return true;

            }
            return false;
        }

        private async Task<bool> deleteOrderDetailAsync(StringValues orderDetailId_1)
        {
            HttpResponseMessage response = await client.DeleteAsync(OrderDetailApiUrl + "(" + Int32.Parse(orderDetailId_1) + ")");
            if (response.IsSuccessStatusCode)
            {
                return true;

            }
            return false;
        }

        public async Task<IActionResult> DetailAsync(int orderid)
        {
            HttpResponseMessage response = await client.GetAsync(OrderApiUrl + "(" + orderid + ")?$expand=OrderDetails");
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Order order = System.Text.Json.JsonSerializer.Deserialize<Order>(data.ToString(), options);
            List<OrderDetailView> odvs = new List<OrderDetailView>();
            foreach (OrderDetail od in order.OrderDetails)
            {
                OrderDetailView odv = new OrderDetailView()
                {
                    ProductId = od.ProductId,
                    UnitPrice = od.UnitPrice,
                    Quantity = od.Quantity,
                    Discount = od.Discount,
                    isDeleted = "f"
                };
                odvs.Add(odv);
            }
            OrderView ov = new OrderView()
            {
                OrderId = orderid,
                MemberId = order.MemberId,
                OrderDate = order.OrderDate,
                RequiredDate = order.RequiredDate,
                ShippedDate = order.ShippedDate,
                Freight = order.Freight,
                MaxNo = odvs.Count,
                OrderDetailsView = odvs
            };
            //lay list Product tu DataBase
            HttpResponseMessage response2 = await client.GetAsync(ProductApiUrl);
            string strData2 = await response2.Content.ReadAsStringAsync();
            var data2 = JObject.Parse(strData2);
            var listProductj = data2["value"];
            List<Product> products = System.Text.Json.JsonSerializer.Deserialize<List<Product>>(listProductj.ToString(), options);
            ViewData["products"] = products;
            return View(ov);
        }
    }
}
