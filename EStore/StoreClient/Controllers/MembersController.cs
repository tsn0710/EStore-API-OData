using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StoreClient.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace StoreClient.Controllers
{
    public class MembersController : Controller
    {
        private readonly HttpClient client = null;
        private string MemberApiUrl = "";
        public MembersController()
        {
            client = new HttpClient();
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "https://localhost:7222/odata/Members";
        }
        public async Task<IActionResult> IndexAsync()
        {
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            var listMemberj = data["value"];
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Member> listMembers = System.Text.Json.JsonSerializer.Deserialize<List<Member>>(listMemberj.ToString(), options);
            return View(listMembers);
        }

        public async Task<IActionResult> CreateAsync()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Member member)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Member>(member, options);

            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("https://localhost:7222/odata/Members", httpContent);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Members");

            }
            return RedirectToAction("Create", "Members");
        }

        public async Task<IActionResult> DeleteAsync(int memberid)
        {
            if (HttpContext.Session.GetInt32("MemberId") == null)
            {
                return RedirectToAction("Login", "Home");
            }
            HttpResponseMessage response = await client.DeleteAsync(MemberApiUrl + "(" + memberid + ")");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");

            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DetailAsync(int memberid)
        {
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl + "(" + memberid + ")");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            Member member = System.Text.Json.JsonSerializer.Deserialize<Member>(data.ToString(), options);
            return View(member);
        }
        public async Task<IActionResult> UpdateAsync(int memberid)
        {
      

            HttpResponseMessage response = await client.GetAsync(MemberApiUrl + "(" + memberid + ")");
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            Member member = System.Text.Json.JsonSerializer.Deserialize<Member>(data.ToString(), options);
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAsync(Member member)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = false,
            };
            string json = System.Text.Json.JsonSerializer.Serialize<Member>(member, options);

            StringContent httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync($"https://localhost:7222/odata/Members/{member.MemberId}", httpContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Members");

            }
            return RedirectToAction("Update", "Members");
        }

    }
}
