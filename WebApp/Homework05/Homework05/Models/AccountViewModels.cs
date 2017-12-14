using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Homework05.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }

        public string UserName { get; set; }

        public int SurveyGroupId { get; set; }
        public string Id { get; set; }

        public string DeviceId { get; set; }
        public bool HasRegistered { get; set; }

        public IList<string> Roles { get; set; }

        public string LoginProvider { get; set; }
    }
    public class DeviceIdModel
    {
        [Key]
        public string DeviceId { get; set; }
        public string UserId { get; set; }
    }
    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }

    public class LoginViewModel
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

    public class TokenModel
    {
        public string Access_Token { get; set; }
    }
}
