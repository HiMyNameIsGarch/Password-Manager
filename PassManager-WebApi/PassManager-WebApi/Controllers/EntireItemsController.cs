using Microsoft.AspNet.Identity;
using PassManager_WebApi.Enums;
using PassManager_WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/EntireItems")]
    public class EntireItemsController : ApiController
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();
        internal static IQueryable<ItemPreview> GetAllPasswords(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            return db.Passwords
                .Where(item => item.UserId == userId)
                .OrderByDescending(p => p.NumOfVisits)
                .ThenBy(p => p.Name)
                .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = item.Username, ItemType = TypeOfItems.Password });
        }
        internal static IQueryable<ItemPreview> GetAllWifis(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            return db.Wifis
            .Where(item => item.UserId == userId)
            .OrderByDescending(p => p.NumOfVisits)
            .ThenBy(p => p.Name)
            .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = "Wi-Fi", ItemType = TypeOfItems.Wifi });
        }
        internal static IQueryable<ItemPreview> GetAllNotes(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            return db.Notes
            .Where(item => item.UserId == userId)
            .OrderByDescending(p => p.NumOfVisits)
            .ThenBy(p => p.Name)
            .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = TypeOfItems.Note.ToString(), ItemType = TypeOfItems.Note });
        }
        internal static IQueryable<ItemPreview> GetAllPaymentCards(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            return db.PaymentCards
            .Where(note => note.UserId == userId)
            .OrderByDescending(note => note.NumOfVisits)
            .ThenBy(note => note.Name)
            .Select(note => new ItemPreview() { Id = note.Id, Title = note.Name, SubTitle = "Payment card", ItemType = TypeOfItems.PaymentCard });
        }
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

            if (previews is null) return BadRequest("Your search string could not be empty!");

            return Ok(previews.Reverse());//reverse list to order better items type
        }
        private IEnumerable<ItemPreview> GetAllItems()
        {
            string userId = User.Identity.GetUserId();
            //take passwords
            var items = GetAllPasswords(db, userId)
            .Union
            (GetAllWifis(db, userId))
            .Union
            (GetAllNotes(db, userId))
            .Union
            (GetAllPaymentCards(db, userId));
            //return them
            return items;
        }
        private IEnumerable<ItemPreview> SearchItems(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return null;
            //get current user id
            string userId = User.Identity.GetUserId();
            //get items from db
            var items = GetAllPasswords(db, userId)
                .Where(s => s.Title.Contains(searchString))
            .Union//take wifis
            (GetAllWifis(db, userId))
            .Where(s => s.Title.Contains(searchString))
            .Union
            (GetAllNotes(db, userId))
            .Where(s => s.Title.Contains(searchString))
            .Union
            (GetAllPaymentCards(db, userId))
            .Where(s => s.Title.Contains(searchString));
            //return them
            return items;
        }
    }
}
