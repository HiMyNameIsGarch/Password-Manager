using PassManager.Enums;
using PassManager.Models;
using PassManager.Models.Api;
using PassManager.Models.Api.Processors;
using PassManager.Models.CallStatus;
using PassManager.Models.Items;
using PassManager.ViewModels.Bases;
using PassManager.Views.Popups;
using System.Threading.Tasks;

namespace PassManager.ViewModels.CreateItems
{
    public class CreateNoteVM : BaseItemVM
    {
        public CreateNoteVM() : base(TypeOfItems.Note)
        {
            _note = new Note();
        }
        //variables
        private Note _note;
        private Note _tempNote;
        //props
        public Note Note 
        {
            get { return _note; }
            set { _note = value; NotifyPropertyChanged(); }
        }
        //override functions
        private protected override bool IsItemChanged()
        {
            return Note.IsChanged(_tempNote);
        }
        private protected override async Task GetDataAsync(int id)
        {
            Note note = await NoteProcessor.GetNote(ApiHelper.ApiClient, id);
            if (note != null)
            {
                var decryptedNote = (Note)DecryptItem(note);
                Note = decryptedNote;
                _tempNote = (Note)decryptedNote.Clone();//store a temp password for future verifications
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and we couldn't get your note, try again!"));
        }
        private protected override async Task<bool> CreateAsync()
        {
            var encryptedNote = (Note)EncryptItem(Note);
            bool isSuccess = await NoteProcessor.CreateNote(ApiHelper.ApiClient, encryptedNote);
            return isSuccess;
        }
        private protected override async Task<ModifyCallStatus> ModifyAsync(int id)
        {
            var encryptedNote = (Note)EncryptItem(Note);
            bool isSuccess = await NoteProcessor.Modify(ApiHelper.ApiClient, id, encryptedNote);
            return new ModifyCallStatus(isSuccess, _tempNote.Name != Note.Name, new ItemPreview(Note.Id, Note.Name, TypeOfItems.Note.ToSampleString(), TypeOfItems.Note));
        }
        private protected override async Task<DeleteCallStatus> DeleteAsync()
        {
            bool isSuccess = await NoteProcessor.Delete(ApiHelper.ApiClient, Note.Id);
            return new DeleteCallStatus(isSuccess, Note.Id);
        }
        private protected override Models.TaskStatus IsModelValid()
        {
            string msgToDisplay = string.Empty;
            if (string.IsNullOrEmpty(Note.Name))
                msgToDisplay = "You need to complete at least Name in order to save!";
            else if (Note.Name.Length > 64)
                msgToDisplay = "Your Name must be max 64 characters long!";
            return Models.TaskStatus.Status(msgToDisplay);
        }
        private protected override object EncryptItem(object obj)
        {
            var noteToEncrypt = (Note)obj;
            noteToEncrypt.Notes = VaultManager.EncryptString(noteToEncrypt.Notes);
            return noteToEncrypt;
        }
        private protected override object DecryptItem(object obj)
        {
            var noteToDecrypt = (Note)obj;
            noteToDecrypt.Notes = VaultManager.DecryptString(noteToDecrypt.Notes);
            return noteToDecrypt;
        }
    }
}
