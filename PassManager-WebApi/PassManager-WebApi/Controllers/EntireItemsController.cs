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
    public class EntireItemsController : ApiController
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();
        //GET api/EntireItems
        public IHttpActionResult Get()//get latest passwords preview
        {
            AspNetUser user = GetCurrentUser();
            //base list, where all the items will be stored
            IEnumerable<ItemPreview> unGroupedItems = new List<ItemPreview>();

            //take passwords
            IEnumerable<ItemPreview> passwords = user.Passwords
            .OrderBy(p => p.LastVisited)
            .Select(item => new ItemPreview(item.Id, item.Name, item.Username, TypeOfItems.Password));

            //Take wifis
            IEnumerable<ItemPreview> wifis = user.Wifis
                .OrderBy(p => p.LastVisited)
                .Select(item => new ItemPreview(item.Id, item.Name, TypeOfItems.Wifi.ToString(), TypeOfItems.Wifi));

            //concat all items
            unGroupedItems = ConcatLists(unGroupedItems, new IEnumerable<ItemPreview>[] { passwords, wifis});

            return Ok(unGroupedItems);
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
