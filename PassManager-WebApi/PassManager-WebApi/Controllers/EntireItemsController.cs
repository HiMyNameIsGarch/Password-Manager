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
        //GET api/EntireItems
        public IHttpActionResult Get()//get latest passwords preview
        {
            var previews = GetAllItems();
            
            return Ok(previews.Reverse().GroupItems());//reverse list to order better items type
        }
        //GET API/EntireItems?searchString=a
        public IHttpActionResult Get(string searchString)
        {
            var previews = SearchItems(searchString);

            if (previews is null) return BadRequest(ErrorMsg.InvalidSearchString);

            return Ok(previews.Reverse().GroupItems());//reverse list to order better items type
        }
        private IEnumerable<ItemPreview> GetAllItems()
        {
            string userId = User.Identity.GetUserId();
            //take passwords
            var items = EntireItems.GetAllPasswords(db, userId)
            .Union
            (EntireItems.GetAllWifis(db, userId))
            .Union
            (EntireItems.GetAllNotes(db, userId))
            .Union
            (EntireItems.GetAllPaymentCards(db, userId));
            //return them
            return items;
        }
        private IEnumerable<ItemPreview> SearchItems(string searchString)
        {
            if (string.IsNullOrEmpty(searchString)) return null;
            //get current user id
            string userId = User.Identity.GetUserId();
            //get items from db
            var items = EntireItems.GetAllPasswords(db, userId)
                .Where(s => s.Title.Contains(searchString))
            .Union//take wifis
            (EntireItems.GetAllWifis(db, userId))
            .Where(s => s.Title.Contains(searchString))
            .Union
            (EntireItems.GetAllNotes(db, userId))
            .Where(s => s.Title.Contains(searchString))
            .Union
            (EntireItems.GetAllPaymentCards(db, userId))
            .Where(s => s.Title.Contains(searchString));
            //return them
            return items;
        }
    }
}
