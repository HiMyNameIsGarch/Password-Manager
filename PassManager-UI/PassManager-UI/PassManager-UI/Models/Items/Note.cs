using System;

namespace PassManager.Models.Items
{
    public class Note : ICloneable
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
        internal bool IsChanged(Note note)
        {
            if (note is null)
            {
                return Name.Length > 0 || Notes.Length > 0;
            }
            return note.Name != Name || note.Notes != Notes;
        }
    }
}
