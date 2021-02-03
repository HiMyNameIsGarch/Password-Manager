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
            string iconUrl = IconHelper.GetImageUrl(TypeOfItems.Password);
            return db.Passwords
                .Where(item => item.UserId == userId)
                .OrderByDescending(p => p.NumOfVisits)
                .ThenBy(p => p.Name)
                .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = item.Username, ItemType = TypeOfItems.Password, IconUrl = iconUrl });
        }
        internal static IQueryable<ItemPreview> GetAllWifis(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            string iconUrl = IconHelper.GetImageUrl(TypeOfItems.Wifi);
            string subTitle = TypeOfItems.Wifi.ToSampleString();
            return db.Wifis
            .Where(item => item.UserId == userId)
            .OrderByDescending(p => p.NumOfVisits)
            .ThenBy(p => p.Name)
            .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = subTitle, ItemType = TypeOfItems.Wifi, IconUrl = iconUrl });
        }
        internal static IQueryable<ItemPreview> GetAllNotes(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            string iconUrl = IconHelper.GetImageUrl(TypeOfItems.Note);
            return db.Notes
            .Where(item => item.UserId == userId)
            .OrderByDescending(p => p.NumOfVisits)
            .ThenBy(p => p.Name)
            .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = TypeOfItems.Note.ToString(), ItemType = TypeOfItems.Note, IconUrl = iconUrl });
        }
        internal static IQueryable<ItemPreview> GetAllPaymentCards(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            string iconUrl = IconHelper.GetImageUrl(TypeOfItems.PaymentCard);
            string subTitle = TypeOfItems.PaymentCard.ToSampleString();
            return db.PaymentCards
            .Where(note => note.UserId == userId)
            .OrderByDescending(note => note.NumOfVisits)
            .ThenBy(note => note.Name)
            .Select(note => new ItemPreview() { Id = note.Id, Title = note.Name, SubTitle = subTitle, ItemType = TypeOfItems.PaymentCard, IconUrl = iconUrl });
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

            if (previews is null) return BadRequest(ErrorMsg.InvalidSearchString);

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
