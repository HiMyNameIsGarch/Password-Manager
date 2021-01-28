using Microsoft.AspNet.Identity;
using PassManager_WebApi.Enums;
using PassManager_WebApi.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Http;

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
            var previews = GetAllItems();
            
            return Ok(previews.Reverse());//reverse list to order better items type
        }
        //GET API/EntireItems?searchString=a
        public IHttpActionResult Get(string searchString)
        {
            var previews = SearchItems(searchString);

            if(previews is null)
                return BadRequest("Your search string can't be empty!");

            return Ok(previews.Reverse());//reverse list to order better items type
        }
        private IEnumerable<ItemPreview> GetAllItems()
        {
            string userId = User.Identity.GetUserId();
            //take passwords
            var items = db.Passwords
            .Where(item => item.UserId == userId)
            .OrderByDescending(p => p.NumOfVisits)
            .ThenBy(p => p.Name)
            .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = item.Username, ItemType = TypeOfItems.Password })
            .Union//take wifis
            (db.Wifis
            .Where(item => item.UserId == userId)
            .OrderByDescending(p => p.NumOfVisits)
            .ThenBy(p => p.Name)
            .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = "Wi-Fi", ItemType = TypeOfItems.Wifi }))
            .Union
            (db.Notes//take notes
            .Where(item => item.UserId == userId)
            .OrderByDescending(p => p.NumOfVisits)
            .ThenBy(p => p.Name)
            .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = TypeOfItems.Note.ToString(), ItemType = TypeOfItems.Note }));
            //return them
            return items;
        }
        private IEnumerable<ItemPreview> SearchItems(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return null;
            //get current user id
            string userId = User.Identity.GetUserId();
            //get items from db
            var items = db.Passwords//take passwords
                .Where(item => item.UserId == userId)
                .OrderByDescending(p => p.NumOfVisits)
                .ThenBy(p => p.Name)
                .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = item.Username, ItemType = TypeOfItems.Password })
                .Where(item => item.Title.Contains(searchString))
                .Union
                (db.Wifis//take wifis
                .Where(item => item.UserId == userId)
                .OrderByDescending(p => p.NumOfVisits)
                .ThenBy(p => p.Name)
                .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = "Wi-Fi", ItemType = TypeOfItems.Wifi }))
                .Where(item => item.Title.Contains(searchString))
                .Union
                (db.Notes//take notes
                .Where(item => item.UserId == userId)
                .OrderByDescending(p => p.NumOfVisits)
                .ThenBy(p => p.Name)
                .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = TypeOfItems.Note.ToString(), ItemType = TypeOfItems.Note })
                .Where(item => item.Title.Contains(searchString)));
            //return them
            return items;
        }
    }
}
