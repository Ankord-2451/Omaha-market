using Omaha_market.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Omaha_market.Core
{
    public class SessionWorker
    {
        private HttpContext Context;
        public SessionWorker(HttpContext context)
        {
            Context = context;
        }
        public void SaveToken(string token)
        {
            Context.Session.SetString("token", $"Bearer {token}");
        }

        public void SaveUserModel(AuthorizedUser user)
        {
            Context.Session.SetString("user", JsonSerializer.Serialize<AuthorizedUser>(user));
        }

        public int GetUserId()
        {
            var user = Context.Session.GetString("user");
            if(user!=null)
            { 
            return JsonSerializer.Deserialize<AuthorizedUser>(user).ID;
            }
            return -1;
        }
        public string GetUserName()
        {
            var user = Context.Session.GetString("user");
            if (user != null)
            {
                return JsonSerializer.Deserialize<AuthorizedUser>(user).name;
            }
            return "";
        }
        public bool IsAdmin()
        {
            var user = Context.Session.GetString("user");
            if (user != null)
            {
                return (JsonSerializer.Deserialize<AuthorizedUser>(user).role == role.Admin);
            }
            return false;
        }

        public bool IsCustomer()
        {
            var user = Context.Session.GetString("user");
            if (user != null)
            {
                return (JsonSerializer.Deserialize<AuthorizedUser>(user).role == role.Customer);
            }
            return false;
        }

        public bool IsAuthorized()
        {
            var user = Context.Session.GetString("user");

            return (user != null);
        }
        public void Clear()
        {
          Context.Session.Clear();
        }

        public void SetLanguage(string lang)
        {
            Context.Session.SetString("Lang", lang);
        }

        public bool IsRu()
        {
            return (Context.Session.GetString("Lang") == "Ru");
        }

        public string GetLanguage()
        {
            if (Context.Session.GetString("Lang") != null)
                return Context.Session.GetString("Lang");
            else
            {
                SetLanguage("Ro");
                return "Ro";
            }
        }

        public IDictionary<string,string> GetLangDic()
        {
            var dic = Context.Session.GetString("LangDic");
            if (dic != null)
            {
                return (JsonSerializer.Deserialize<IDictionary<string,string>>(dic));
            }
            return null;
        }

        public void SetLangDic(Dictionary<string, string> dic)
        {
            Context.Session.SetString("LangDic", JsonSerializer.Serialize<IDictionary<string, string>>(dic));
        }
    }
}
