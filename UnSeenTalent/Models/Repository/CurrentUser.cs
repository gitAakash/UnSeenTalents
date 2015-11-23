using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnseentalentsApp.Models.Db;

namespace UnseentalentsApp.Models.Repository
{

    public class CurrentUserModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool isEmailVerify { get; set; }
        public bool isToken { get; set; }
        public Int64 userid { get; set; }
        public string numberOfToken { get; set; }
        public string ProfilePic { get; set; }
        public List<RolesModel> userrole { get; set; }
    }
    public class CurrentUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool isEmailVerify { get; set; }
        public bool isToken { get; set; }
        public Int64 userid { get; set; }
        public string numberOfToken { get; set; }
        public string ProfilePic { get; set; }
        public List<RolesModel> userrole { get; set; }

        private void ReSet()
        {
            this.FirstName = String.Empty;
            this.LastName = String.Empty;
            this.Email = String.Empty;
            this.isEmailVerify = false;
            this.isToken = false;
            this.userid = 0;
            this.userrole = null;
            this.ProfilePic = "";
            this.numberOfToken = "0";
        }

        public CurrentUser()
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string json = HttpContext.Current.User.Identity.Name;
                var user = JsonConvert.DeserializeObject<CurrentUserModel>(json);
                if (user != null)
                {
                    this.FirstName = user.FirstName;
                    this.LastName = user.LastName;
                    this.Email = user.Email;
                    this.isEmailVerify = user.isEmailVerify;
                    this.isToken = user.isToken;
                    this.userid = user.userid;
                    this.userrole = user.userrole;
                    this.ProfilePic = user.ProfilePic;
                    this.numberOfToken = user.numberOfToken;

                }
                else
                {
                    this.ReSet();
                }
            }
            else
            {
                this.ReSet();
            }

        }

    }
    public class userrole
    {
        public int roleid { get; set; }
    }
}