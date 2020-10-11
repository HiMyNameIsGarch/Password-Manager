using Microsoft.AspNet.Identity;
using PassManager_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Extensions.Configuration;
using System.Web.UI.WebControls;
using PassManager_WebApi.ViewModels;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/UserAccount")]
    public class UserAccountController : ApiController
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();
        /// <summary>
        /// Get all the accounts of the current user(no filters)
        /// </summary>
        /// <returns>a list of preview accounts</returns>
        //GET api/UserAccount
        public IHttpActionResult Get()
        {
            IEnumerable<AccountPreviewVM> accounts = GetCurrentUser().Accounts.Select(acc => new AccountPreviewVM(acc)).ToList();
            return Ok(accounts);
        }
        /// <summary>
        /// Checks if the id is valid, then verifies if it belongs to the user then return it
        /// </summary>
        /// <param name="id"> Identifier for the account</param>
        /// <returns> Account of the user</returns>
        //GET api/UserAccount/id
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) return BadRequest("Id is invalid");
            AccountVM account = GetCurrentUser().Accounts.Select(a => new AccountVM(a)).FirstOrDefault(a => a.Id == id);
            if (account is null) return BadRequest("Account does not exist");
            return Ok(account);
        }
        /// <summary>
        /// Get the data about the account from the user and store it in the database
        /// </summary>
        /// <param name="account">The account that user sends</param>
        /// <returns>Status code(message if errors)</returns>
        //POST api/UserAccount
        public IHttpActionResult Post([FromBody]AccountVM account)
        {
            if (account is null) return BadRequest("Account does not exist");
            GetCurrentUser().Accounts.Add(new Account(account));
            db.SaveChanges();
            return Ok();
        }
        /// <summary>
        /// Takes the account from the user, checks if something went wrong and update it to the database
        /// </summary>
        /// <param name="id">Identifier for the account</param>
        /// <param name="account">An account that user sends</param>
        /// <returns>Status code(with message if errors)</returns>
        //PUT api/UserAccount/id
        public IHttpActionResult Put(int id, [FromBody] AccountVM account)
        {
            if (account is null) return BadRequest("Account does not exist!");
            if (id != account.Id || id <= 0) return BadRequest("Id and account does not match!");
            Account currentAccount = GetCurrentAccount(id, GetCurrentUser());
            if (currentAccount is null) return BadRequest("Account does not exist!");
            currentAccount.AdaptTo(account);
            db.SaveChanges();
            return Ok();
        }
        /// <summary>
        /// Takes the id from the user, verifies if errors then delete the account of the user based on the id
        /// </summary>
        /// <param name="id">Identifier for the account</param>
        /// <returns>Status code(with message if errors)</returns>
        //DELETE api/UserAccount/id
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Id is invalid!");
            Account accountToBeDeleted = GetCurrentAccount(id, GetCurrentUser());
            if (accountToBeDeleted is null) return BadRequest("Account does not exist!");
            db.Accounts.Remove(accountToBeDeleted);
            db.SaveChanges();
            return Ok();
        }
        private AspNetUser GetCurrentUser()
        {
            return db.AspNetUsers.Find(User.Identity.GetUserId());
        }
        private Account GetCurrentAccount(int id, AspNetUser user)
        {
            return user.Accounts.FirstOrDefault(a => a.Id == id);
        }
    }
}
