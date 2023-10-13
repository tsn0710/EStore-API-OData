using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public OrderController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            OrderApiUrl = "https://localhost:7222/odata/Orders";
            MemberApiUrl = "https://localhost:7222/odata/Members";
            ProductApiUrl = "https://localhost:7222/odata/Products";
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
            ViewData["requireddate"] = DateTime.Now;
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
            return View(ov);
        }
    }
}
