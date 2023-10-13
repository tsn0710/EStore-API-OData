using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StoreClient.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace StoreClient.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly HttpClient client = null;
        private string MemberApiUrl = "";
        public UserProfileController()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "https://localhost:7222/odata/Members";
        }
        public async Task<IActionResult> EditProfileAsync()
        {
            if (HttpContext.Session.GetInt32("MemberId") == null)
            {
                return RedirectToAction("Login","Home");
            }
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl + "(" + HttpContext.Session.GetInt32("MemberId")+")");
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            Member member = System.Text.Json.JsonSerializer.Deserialize<Member>(data.ToString(), options);
            ViewData["user"] = member;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> EditAsync(Member member)
        {
            if (member.MemberId != HttpContext.Session.GetInt32("MemberId"))
            {
                return RedirectToAction("EditProfile", "UserProfile");
            }
            string json = JsonSerializer.Serialize<Member>(member);
            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(MemberApiUrl + "(" + member.MemberId + ")", httpContent);
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.Remove("MemberId");
                HttpContext.Session.Remove("Email");
                HttpContext.Session.SetInt32("MemberId", member.MemberId);
                HttpContext.Session.SetString("Email", member.Email);
                return RedirectToAction("Index", "Order");
            }
            return RedirectToAction("EditProfile", "UserProfile");
        }
    }
}
