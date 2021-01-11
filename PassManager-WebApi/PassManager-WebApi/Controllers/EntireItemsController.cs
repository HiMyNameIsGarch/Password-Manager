using Microsoft.AspNet.Identity;
using PassManager_WebApi.Enums;
using PassManager_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Diagnostics;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/EntireItems")]
    public class EntireItemsController : ApiController
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();
        //GET api/EntireItems
        public IHttpActionResult Get()//get latest passwords preview
        {
            IEnumerable<ItemPreview> previews = GetAllItems(GetCurrentUser());

            return Ok(previews);
        }
        public IHttpActionResult Get(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return BadRequest("Your item is null or empty!");
            }
            IEnumerable<ItemPreview> previews = GetAllItems(GetCurrentUser(), searchString);
            return Ok(previews);
        }
        public IHttpActionResult Get(TypeOfUpdates updateType)
        {
            var user = GetCurrentUser();
            if (updateType != TypeOfUpdates.Create && updateType != TypeOfUpdates.Modify)
            {
                return BadRequest("UpdateType is invalid!");
            }
            //take passwords
            var passwords = user.Passwords
            .OrderByDescending(s => updateType == TypeOfUpdates.Create ? s.CreateDate : updateType == TypeOfUpdates.Modify ? s.LastModified : s.LastVisited)
            .Select(item => new ItemPreview(item.Id, item.Name, item.Username, TypeOfItems.Password))
            .Union(user.Wifis
                .OrderByDescending(s => updateType == TypeOfUpdates.Create ? s.CreateDate : updateType == TypeOfUpdates.Modify ? s.LastModified : s.LastVisited)
                .Select(item => new ItemPreview(item.Id, item.Name, TypeOfItems.Wifi.ToString(), TypeOfItems.Wifi))).FirstOrDefault();
            if (passwords is null)
            {
                return BadRequest("No item were updated!");
            }
            return Ok(passwords);
        }
        private IEnumerable<ItemPreview> GetAllItems(AspNetUser user, string searchString = "")
        {
            if(user != null)
            {
                //base list, where all the items will be stored
                IEnumerable<ItemPreview> items = Enumerable.Empty<ItemPreview>();

                //take passwords
                var passwords = user.Passwords
                .OrderByDescending(p => p.LastVisited)
                .Select(item => new ItemPreview(item.Id, item.Name, item.Username, TypeOfItems.Password));
                //Take wifis
                var wifis = user.Wifis
                    .OrderByDescending(p => p.LastVisited)
                    .Select(item => new ItemPreview(item.Id, item.Name, TypeOfItems.Wifi.ToString(), TypeOfItems.Wifi));
                items = ConcatLists(items, new IEnumerable<ItemPreview>[] { passwords, wifis});

                if (!string.IsNullOrEmpty(searchString))
                {
                    items = items.Where(i => i.Title.Contains(searchString));
                }

                return items;
            }
            return null;
        }
        private IEnumerable<ItemPreview> ConcatLists(IEnumerable<ItemPreview> baseList, IEnumerable<ItemPreview>[] lists)
        {
            foreach (var list in lists)
            {
                baseList = baseList.Concat(list);
            }
            return baseList;
        }
        private AspNetUser GetCurrentUser()
        {
            return db.AspNetUsers.Find(User.Identity.GetUserId());
        }
    }
}
