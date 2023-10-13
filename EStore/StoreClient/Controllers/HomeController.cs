using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using StoreClient.Models;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;

namespace StoreClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient client = null;
        private string MemberApiUrl = "";
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            MemberApiUrl = "https://localhost:7222/odata/Members";
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LoginAsync(Member member)
        {
            if (member.Password == null || member.Password.Length == 0 || member.Email == null || member.Email.Length == 0)
                return View();
            //get list of member
            HttpResponseMessage response = await client.GetAsync(MemberApiUrl);
            string strData = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(strData);
            var listMemberj = data["value"];
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            List<Member> listMembers = JsonSerializer.Deserialize<List<Member>>(listMemberj.ToString(), options);
            //is list contain this member ?
            Member member1 = null;
            foreach (Member m in listMembers)
            {
                if (m.Email.Equals(member.Email.Trim()) && m.Password.Equals(member.Password.Trim()))
                {
                    member1 = new Member();
                    member1.MemberId = m.MemberId;
                    member1.Email = m.Email;
                    break;
                }
            }
            if (member1 == null)
                return View();
            else
            {
                HttpContext.Session.SetInt32("MemberId", member1.MemberId);
                HttpContext.Session.SetString("Email", member1.Email);
                return RedirectToAction("Index", "Order");
            }
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("MemberId");
            HttpContext.Session.Remove("Email");
            return RedirectToAction("Index", "Home");
        }
    }
}