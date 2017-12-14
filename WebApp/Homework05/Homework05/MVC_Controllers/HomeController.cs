using Homework05;
using Homework05.Models;
using Homework05.Providers;
using Homework05.Results;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Homework_04.Controllers
{
    public class HomeController : Controller
    {
        private IAuthenticationManager AuthenticationManager { get { return HttpContext.GetOwinContext().Authentication; } }

        public HomeController()
        {
        }

        public ActionResult Index()
        {
            if(Session["accessToken"] != null)
            {
                return RedirectToAction("Dashboard", "Admin");
            }
            if (Request.IsAuthenticated)
            {
                
            }
            ViewBag.Title = "Home Page";

            return View();
        }

        public async Task<ActionResult> LoginAsync(LoginViewModel Vm)
        {
            if (Vm.UserName.Equals("Admin"))
            {
                using (var client = new HttpClient())
                {
                    Vm.GrantType = "password";

                    try
                    {
                        HttpResponseMessage result = await client.PostAsync("http://caremesurvey-nc.azurewebsites.net/oauth2/token", new FormUrlEncodedContent(Vm.ToDict()));

                        if (result.IsSuccessStatusCode)
                        {
                            string jsonResult = await result.Content.ReadAsStringAsync();
                            var resultObject = JsonConvert.DeserializeObject<TokenModel>(jsonResult);
                            Session["accessToken"] = resultObject.Access_Token;
                            FormsAuthentication.SetAuthCookie(resultObject.Access_Token, false);
                            return Json(new { success = true, responseText = "Login successful",resultObject });
                           // return RedirectToAction("Dashboard", "Admin");
                        }
                        else
                        {
                            ModelState.AddModelError("LoginError", "Wrong credentials!");
                            return Json(new { success = false, responseText = "Wrong credentials!" });
                        }
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("LoginError", e.Message);
                        return Json(new { success = false, responseText = e.Message });
                    }
                }
            }
            else
            {
                ModelState.AddModelError("UnauthorizedError", "Only Admin can login!");
                return Json(new { success = false, responseText = "Only Admin can login!" });
            }
        }

        public async Task<ActionResult> LogOffAsync()
        {
            using(var client = new HttpClient())
            {
                try
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Session["accessToken"]);
                    HttpResponseMessage result = await client.PostAsync("http://caremesurvey-nc.azurewebsites.net/api/Account/Logout", null);
                    if (result.IsSuccessStatusCode)
                    {
                        Session.Clear();
                        Session.Abandon();
                        return Json(new { success = true, responseText = "LogOff successful" });
                    }
                    else
                    {                       
                        return Json(new { success = false, responseText = result.ReasonPhrase });
                    }
                }
                catch(Exception e)
                {
                    return Json(new { success = false, responseText = e.Message });
                }
            }
           // AuthenticationManager.SignOut();
            
        }
    }
}
