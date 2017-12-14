using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace edu.uncc.homework4.Models
{
    public class UserInfoModel
    {
        public string Username { get; set; }
        public string Fullname { get; set; }

        public string Id { get; set; }
        public int RegionId { get; set; }

        public string Email { get; set; }
        public string DeviceId { get; set; }
    }
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string GrantType { get; set; }
        public Dictionary<string, string> ToDict()
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("username", this.UserName);
            parameters.Add("password", this.Password);
            parameters.Add("grant_type", this.GrantType);           

            return parameters;
        }
    }

    public class DeviceIdModel
    {
        public string DeviceId { get; set; }
        public string UserId { get; set; }

        public Dictionary<string, string> ToDict()
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("DeviceId", this.DeviceId);
            parameters.Add("UserId", this.UserId);
            

            return parameters;
        }
    }
    public class TokenModel
    {
        public string Access_Token { get; set; }
    }

    public class RegisterUserModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }

        public Dictionary<string, string> ToDict()
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("UserName", this.UserName);
            parameters.Add("Password", this.Password);
            parameters.Add("ConfirmPassword", this.ConfirmPassword);
            parameters.Add("Email", this.Email);
            parameters.Add("Name", this.FullName);

            return parameters;
        }
    }
}