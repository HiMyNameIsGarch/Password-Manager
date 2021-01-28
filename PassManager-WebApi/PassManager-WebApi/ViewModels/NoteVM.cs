using PassManager_WebApi.Models;
using PassManager_WebApi.Models.Interfaces;

namespace PassManager_WebApi.ViewModels
{
    public class NoteVM : IModelValid
    {
        public NoteVM() { }
        public NoteVM(Note note)
        {
            if (note is null) return;
            Id = note.Id;
            Name = note.Name;
            Notes = note.Notes;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public string IsModelValid()
        {
            if (string.IsNullOrEmpty(Name))
                return "You need to complete at least \"name\" in order to save!";
            return string.Empty;
        }
    }
}