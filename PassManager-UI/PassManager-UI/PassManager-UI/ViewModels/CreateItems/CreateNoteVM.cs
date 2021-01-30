using PassManager.Enums;
using PassManager.Models;
using PassManager.Models.Api;
using PassManager.Models.Api.Processors;
using PassManager.Models.Items;
using PassManager.ViewModels.Bases;
using PassManager.Views.Popups;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
        //implementation for commands
        public override async void GoBackButton()
        {
            if (Note.IsChanged(_tempNote))
            {
                bool wantsToLeave = await PageService.DisplayAlert("Wait!", "Are you sure you want to leave?", "Yes", "No");
                if (wantsToLeave)
                    await Shell.Current.Navigation.PopToRootAsync();
            }
            else
                await Shell.Current.Navigation.PopToRootAsync();
        }
        //override functions
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
        private protected override async Task Create()
        {
            var encryptedNote = (Note)EncryptItem(Note);
            bool isSuccess = await NoteProcessor.CreateNote(ApiHelper.ApiClient, encryptedNote);
            if (isSuccess)
            {
                var latestCreatedItem = await EntireItemsProcessor.GetLatestCreated(ApiHelper.ApiClient, TypeOfItems.Note);
                if (latestCreatedItem is null)
                {
                    await PageService.PushPopupAsync(new ErrorView("Something went wrong and your note has not been created, try again!"));
                }
                else
                {
                    UpdateModel model = new UpdateModel(TypeOfUpdates.Create, latestCreatedItem);
                    await GoTo("Note", model);
                }
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your note has not been created, try again!"));
        }
        private protected override async Task Modify(int id)
        {
            if (!Note.IsChanged(_tempNote)) await Shell.Current.Navigation.PopToRootAsync();
            var encryptedNote = (Note)EncryptItem(Note);
            bool isSuccess = await NoteProcessor.Modify(ApiHelper.ApiClient, id, encryptedNote);
            if (isSuccess)
            {
                if (_tempNote.Name != Note.Name)//if some props from item preview changed, then update the item
                {
                    UpdateModel model = new UpdateModel(TypeOfUpdates.Modify, new ItemPreview(Note.Id, Note.Name, TypeOfItems.Note.ToSampleString(), TypeOfItems.Note));
                    await GoTo("Note", model);
                }
                else
                    await GoTo("Note");
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your note has not been modified, try again!"));
        }
        private protected override async Task Delete()
        {
            bool isSuccess = await NoteProcessor.Delete(ApiHelper.ApiClient, Note.Id);
            if (isSuccess)
            {
                UpdateModel model = new UpdateModel(TypeOfUpdates.Delete, new ItemPreview() { Id = Note.Id, ItemType = TypeOfItems.Note });
                await GoTo("Note", model);
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your note has not been deleted, try again!"));
        }
        private protected override Models.TaskStatus IsModelValid()
        {
            string msgToDisplay = string.Empty;
            if (string.IsNullOrEmpty(Note.Name))
                msgToDisplay = "You need to complete at least \"Name\" in order to save!";
            else if (Note.Name.Length > 64)
                msgToDisplay = "Your Name must be max 64 characters long!";

            if (string.IsNullOrEmpty(msgToDisplay))
                return new Models.TaskStatus(false, string.Empty);
            else
                return new Models.TaskStatus(true, msgToDisplay);
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
