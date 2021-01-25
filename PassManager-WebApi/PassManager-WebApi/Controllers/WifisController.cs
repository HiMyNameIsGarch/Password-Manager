using Microsoft.AspNet.Identity;
using PassManager_WebApi.Enums;
using PassManager_WebApi.Models;
using PassManager_WebApi.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Wifis")]
    public class WifisController : ApiController
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();

        //GET api/Wifis
        public IHttpActionResult Get()
        {
            string userId = User.Identity.GetUserId();
            //take all ordered wifis
            IEnumerable<ItemPreview> wifis = db.Wifis
                .Where(item => item.UserId == userId)
                .OrderByDescending(item => item.NumOfVisits)
                .ThenByDescending(item => item.Name)
                .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = "Wi-Fi", ItemType = TypeOfItems.Wifi });
            
            return Ok(wifis);
        }
        //GET api/Wifis/5
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) return BadRequest("Id is invalid");
            string userId = User.Identity.GetUserId();
            //get current wifi based on userId
            Wifi wifi = db.Wifis.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (wifi is null) return BadRequest("Wifi does not exist!");
            wifi.NumOfVisits++;
            db.SaveChanges();
            return Ok(new WifiVM(wifi));
        }
        //POST api/Wifi
        public IHttpActionResult Post([FromBody] WifiVM wifi)
        {
            if (wifi is null) return BadRequest("Wifi does not exist!");
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
            if (wifi is null) return BadRequest("Wifi does not exist!");
            if (id != wifi.Id) return BadRequest("Id does not match with the wifi");
            if (id <= 0) return BadRequest("Id is invalid");
            //check if wifi is valid
            var isModelValid = wifi.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            //get current user id and modify wifi
            string userId = User.Identity.GetUserId();
            Wifi wifiToBeModified = db.Wifis.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (wifiToBeModified is null) return BadRequest("Wifi not found");
            wifiToBeModified.ModifyTo(wifi);
            db.SaveChanges();
            return Ok();
        }
        //DELETE api/Wifi/5
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Id is invalid");
            string userId = User.Identity.GetUserId();
            Wifi wifiToBeDeleted = db.Wifis.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (wifiToBeDeleted is null) return BadRequest("Wifi not found");
            db.Wifis.Remove(wifiToBeDeleted);
            db.SaveChanges();
            return Ok();
        }
    }
}
