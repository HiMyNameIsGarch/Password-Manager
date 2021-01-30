using PassManager_WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using PassManager_WebApi.Models.Interfaces;
using PassManager_WebApi.ViewModels;
using Microsoft.AspNet.Identity;
using PassManager_WebApi.Enums;

namespace PassManager_WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/PaymentCards")]
    public class PaymentCardsController : ApiController, IBaseItemController<PaymentCardVM>
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();
        public IHttpActionResult Get()
        {
            string userId = User.Identity.GetUserId();
            //bring from db just the item preview for payment card
            IEnumerable<ItemPreview> paymentCards = EntireItemsController.GetAllPaymentCards(db, userId);
            
            return Ok(paymentCards);
        }
        public IHttpActionResult Get(bool lastCreated)
        {
            if(!lastCreated) return BadRequest();
            string userId = User.Identity.GetUserId();
            string subTitle = TypeOfItems.PaymentCard.ToSampleString();
            //bring from db just the item preview for payment card
            var latestCards = db.PaymentCards
                .Where(note => note.UserId == userId)
                .OrderByDescending(note => note.CreateDate)
                .Take(1)
                .Select(note => new ItemPreview() { Id = note.Id, Title = note.Name, SubTitle = subTitle, ItemType = TypeOfItems.PaymentCard });
            
            return Ok(latestCards.FirstOrDefault());
        }
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) return BadRequest("Id is invalid!");
            string userId = User.Identity.GetUserId();
            PaymentCard paymentCard = db.PaymentCards.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (paymentCard is null) return BadRequest("Payment card does not exist!");
            paymentCard.NumOfVisits++;
            db.SaveChanges();
            return Ok(new PaymentCardVM(paymentCard));
        }
        public IHttpActionResult Post([FromBody] PaymentCardVM item)
        {
            return BadRequest();
            if (item is null) return BadRequest("Payment card does not exist!");
            var isModelValid = item.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            string userId = User.Identity.GetUserId();
            db.PaymentCards.Add(new PaymentCard(item, userId));
            db.SaveChanges();
            return Ok();
        }
        public IHttpActionResult Put(int id, [FromBody] PaymentCardVM item)
        {
            if (item is null) return BadRequest("Payment card does not exist!");
            if (id != item.Id) return BadRequest("Id does not match with the payment card");
            if (id <= 0) return BadRequest("Id is invalid!");
            var isModelValid = item.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            string userId = User.Identity.GetUserId();
            PaymentCard cardToBeModified = db.PaymentCards.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (cardToBeModified is null) return BadRequest("Payment card not found!");
            cardToBeModified.ModifyTo(item);
            db.SaveChanges();
            return Ok();
        }
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Id is invalid!");
            string userId = User.Identity.GetUserId();
            PaymentCard cardToBeDeleted = db.PaymentCards.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (cardToBeDeleted is null) return BadRequest("Payment card not found");
            db.PaymentCards.Remove(cardToBeDeleted);
            db.SaveChanges();
            return Ok();
        }
    }
}
