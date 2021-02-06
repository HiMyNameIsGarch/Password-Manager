
namespace PassManager.Models.Items
{
    public class Note
    {
        public Note()
        {
            Name = Notes = string.Empty;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Notes { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public bool IsChanged(Note note)
        {
            if (note is null)
            {
                return Name.Length > 0 || Notes.Length > 0;
            }
            return note.Name != Name || note.Notes != Notes;
        }
    }
}
