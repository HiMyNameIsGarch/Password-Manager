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
    [RoutePrefix("api/Notes")]
    public class NotesController : ApiController, IBaseItemController<NoteVM>
    {
        private PasswordManagerEntities db = new PasswordManagerEntities();
        //GET api/Notes
        public IHttpActionResult Get()
        {
            string userId = User.Identity.GetUserId();
            //bring from db just the item preview for notes
            IEnumerable<ItemPreview> notes = EntireItems.GetAllNotes(db, userId);

            return Ok(notes.GroupItems());
        }
        //GET api/Notes?lastCreated=true
        public IHttpActionResult Get(bool lastCreated)
        {
            if (lastCreated)
            {
                var userId = User.Identity.GetUserId();
                string iconUrl = IconHelper.GetImageUrl(TypeOfItems.Note);
                var lastestPass = db.Notes
                    .Where(w => w.UserId == userId)
                    .OrderByDescending(s => s.CreateDate)
                    .Take(1)
                    .Select(item => new ItemPreview() { 
                        Id = item.Id, 
                        Title = item.Name, 
                        SubTitle = TypeOfItems.Note.ToString(),
                        ItemType = TypeOfItems.Note,
                        IconUrl = iconUrl
                    });
                return Ok(lastestPass.FirstOrDefault());
            }
            return BadRequest();
        }
        //GET api/Notes/5
        public IHttpActionResult Get(int id)
        {
            if (id <= 0) return BadRequest(ErrorMsg.InvalidId);
            string userId = User.Identity.GetUserId();
            Note note = db.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note is null) return BadRequest(ErrorMsg.ItemDoesNotExist(TypeOfItems.Note));
            note.NumOfVisits++;
            db.SaveChanges();
            return Ok(new NoteVM(note));
        }
        //POST api/Notes
        public IHttpActionResult Post([FromBody] NoteVM note)
        {
            if (note is null) return BadRequest(ErrorMsg.ItemDoesNotExist(TypeOfItems.Note));
            var isModelValid = note.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            string userId = User.Identity.GetUserId();
            db.Notes.Add(new Note(note,userId));
            db.SaveChanges();
            return Ok();
        }
        //PUT api/Notes/5
        public IHttpActionResult Put(int id, [FromBody] NoteVM note)
        {
            if (note is null) return BadRequest(ErrorMsg.ItemDoesNotExist(TypeOfItems.Note));
            if (id != note.Id) return BadRequest(ErrorMsg.InvalidIdMatchingWith(TypeOfItems.Note));
            if (id <= 0) return BadRequest(ErrorMsg.InvalidId);
            var isModelValid = note.IsModelValid();
            if (!string.IsNullOrEmpty(isModelValid)) return BadRequest(isModelValid);
            string userId = User.Identity.GetUserId();
            Note noteToBeModified = db.Notes.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (noteToBeModified is null) return BadRequest(ErrorMsg.ItemNotFound(TypeOfItems.Note));
            noteToBeModified.ModifyTo(note);
            db.SaveChanges();
            return Ok();
        }
        //DELETE api/Notes/5
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest(ErrorMsg.InvalidId);
            string userId = User.Identity.GetUserId();
            Note noteToBeDeleted = db.Notes.FirstOrDefault(w => w.Id == id && w.UserId == userId);
            if (noteToBeDeleted is null) return BadRequest(ErrorMsg.ItemNotFound(TypeOfItems.Note));
            db.Notes.Remove(noteToBeDeleted);
            db.SaveChanges();
            return Ok();
        }
    }
}
