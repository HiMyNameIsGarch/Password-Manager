using Microsoft.AspNet.Identity;
using PassManager_WebApi.Enums;
using PassManager_WebApi.Models;
using PassManager_WebApi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PassManager_WebApi.Models.Interfaces;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Wifis")]
    public class WifisController : ApiController, IBaseItemController<WifiVM>
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();

        //GET api/Wifis
        public IHttpActionResult Get()
        {
            string userId = User.Identity.GetUserId();
            //take all ordered wifis
            IEnumerable<ItemPreview> wifis = EntireItems.GetAllWifis(db, userId);
            
            return Ok(wifis.GroupItems());
        }
        //GET api/Wifi?lastCreated=true
        public IHttpActionResult Get(bool lastCreated)
        {
            if (lastCreated)
            {
                var userId = User.Identity.GetUserId();
                string wifiName = TypeOfItems.Wifi.ToSampleString();
                string iconUrl = IconHelper.GetImageUrl(TypeOfItems.Wifi);
                var lastestWifi = db.Wifis
                    .Where(w => w.UserId == userId)
                    .OrderByDescending(s => s.CreateDate)
                    .Take(1)
                    .Select(item => new ItemPreview() {
                        Id = item.Id,
                        Title = item.Name, 
                        SubTitle = wifiName, 
                        ItemType = TypeOfItems.Wifi, 
                        IconUrl = iconUrl
                        });
                return Ok(lastestWifi.FirstOrDefault());
            }
            return BadRequest();
        }
        //GET api/Wifis/5
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) return BadRequest(ErrorMsg.InvalidId);
            string userId = User.Identity.GetUserId();
            //get current wifi based on userId
            Wifi wifi = db.Wifis.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (wifi is null) return BadRequest(ErrorMsg.ItemDoesNotExist(TypeOfItems.Wifi));
            wifi.NumOfVisits++;
            db.SaveChanges();
            return Ok(new WifiVM(wifi));
        }
        //POST api/Wifi
        public IHttpActionResult Post([FromBody] WifiVM wifi)
        {
            if (wifi is null) return BadRequest(ErrorMsg.ItemDoesNotExist(TypeOfItems.Wifi));
            var isModelValid = wifi.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            string userId = User.Identity.GetUserId();
            db.Wifis.Add(new Wifi(wifi, userId));
            db.SaveChanges();
            return Ok();
        }
        //PUT api/Wifi/5
        public IHttpActionResult Put(int id, [FromBody] WifiVM wifi)
        {
            //check if the wifi and the id match
            if (wifi is null) return BadRequest(ErrorMsg.ItemDoesNotExist(TypeOfItems.Wifi));
            if (id != wifi.Id) return BadRequest(ErrorMsg.InvalidIdMatchingWith(TypeOfItems.Wifi));
            if (id <= 0) return BadRequest(ErrorMsg.InvalidId);
            //check if wifi is valid
            var isModelValid = wifi.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            //get current user id and modify wifi
            string userId = User.Identity.GetUserId();
            Wifi wifiToBeModified = db.Wifis.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (wifiToBeModified is null) return BadRequest(ErrorMsg.ItemNotFound(TypeOfItems.Wifi));
            wifiToBeModified.ModifyTo(wifi);
            db.SaveChanges();
            return Ok();
        }
        //DELETE api/Wifi/5
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest(ErrorMsg.InvalidId);
            string userId = User.Identity.GetUserId();
            Wifi wifiToBeDeleted = db.Wifis.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (wifiToBeDeleted is null) return BadRequest(ErrorMsg.ItemNotFound(TypeOfItems.Wifi));
            db.Wifis.Remove(wifiToBeDeleted);
            db.SaveChanges();
            return Ok();
        }
    }
}
