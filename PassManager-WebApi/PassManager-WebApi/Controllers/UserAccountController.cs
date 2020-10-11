using Microsoft.AspNet.Identity;
using PassManager_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Extensions.Configuration;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/UserAccount")]
    public class UserAccountController : ApiController
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();
        //GET api/UserAccount
        public IHttpActionResult Get()
        {
            var accounts = GetCurentUser().Accounts.ToList();
            return Ok(accounts);
        }
        private AspNetUser GetCurentUser()
        {
            return db.AspNetUsers.Find(User.Identity.GetUserId());
        }
    }
}
