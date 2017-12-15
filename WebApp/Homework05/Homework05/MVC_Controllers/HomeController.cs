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

namespace Homework05.MVC_Controllers
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

                        UserInfoViewModel userinfo = await GetUserInfo(resultObject.Access_Token);

                        if (userinfo.IsAdmin() || userinfo.IsStudyCoordinator())
                        {
                            resultObject.UserRole = userinfo.IsAdmin() ? "Admin" : "StudyCoordinator";
                            resultObject.UserId = userinfo.Id;

                            Session["accessToken"] = resultObject.Access_Token;
                            Session["userRole"] = resultObject.UserRole;
                            Session["userName"] = userinfo.UserName;
                            Session["userId"] = userinfo.Id;
                            FormsAuthentication.SetAuthCookie(resultObject.Access_Token, false);
                            return Json(new { success = true, responseText = "Login successful", resultObject });
                        }
                        else
                        {
                            ModelState.AddModelError("UnauthorizedError", "Only Admin or Coordinator can login!");
                            return Json(new { success = false, responseText = "Only Admin or Coordinator can login!" });
                        }
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

        private async Task<UserInfoViewModel> GetUserInfo(string token)
        {
            using (var client = new HttpClient())
            {
                HttpResponseMessage userInfoJson = await client.GetAsync("http://caremesurvey-nc.azurewebsites.net/api/Account/UserInfo?token=" + token);
                if (userInfoJson.IsSuccessStatusCode)
                {
                    string userInfoJsonResult = await userInfoJson.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<UserInfoViewModel>(userInfoJsonResult);
                }
            }
            return null;
        }
    }
}
