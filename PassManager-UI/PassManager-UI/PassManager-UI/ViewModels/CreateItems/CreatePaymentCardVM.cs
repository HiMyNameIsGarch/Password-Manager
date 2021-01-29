using PassManager.Enums;
using PassManager.Models;
using PassManager.Models.Api;
using PassManager.Models.Api.Processors;
using PassManager.Models.Interfaces;
using PassManager.Models.Items;
using PassManager.ViewModels.Bases;
using PassManager.Views.Popups;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace PassManager.ViewModels.CreateItems
{
    public class CreatePaymentCardVM : BaseItemVM, IBackButtonBehavior
    {
        public CreatePaymentCardVM() : base(TypeOfItems.PaymentCard)
        {
            _goBack = new Command(GoBackButton);
            _paymentCard = new PaymentCard();
        }
        //variables
        private PaymentCard _paymentCard;
        private PaymentCard _tempPaymentCard;
        private ICommand _goBack;
        public PaymentCard PaymentCard
        {
            get { return _paymentCard; }
            set { _paymentCard = value; NotifyPropertyChanged(); }
        }
        public ICommand GoBack
        {
            get { return _goBack; }
        }
        public async void GoBackButton()
        {
            if (PaymentCard.IsChanged(_tempPaymentCard))
            {
                bool wantsToLeave = await PageService.DisplayAlert("Wait!", "Are you sure you want to leave?", "Yes", "No");
                if (wantsToLeave)
                    await Shell.Current.Navigation.PopToRootAsync();
            }
            else
                await Shell.Current.Navigation.PopToRootAsync();
        }
        private protected override async Task GetDataAsync(int id)
        {
            PaymentCard card = await PaymentCardProcessor.GetCard(ApiHelper.ApiClient, id);
            if (card != null)
            {
                var decryptedCard = (PaymentCard)DecryptItem(card);
                PaymentCard = decryptedCard;
                _tempPaymentCard = (PaymentCard)decryptedCard.Clone();//store a temp card for future verifications
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and we couldn't get your payment card, try again!"));
        }
        private protected override async Task Create()
        {
            var encryptedCard = (PaymentCard)EncryptItem(PaymentCard);
            bool isSuccess = await PaymentCardProcessor.CreateCard(ApiHelper.ApiClient, encryptedCard);
            if (isSuccess)
            {
                var latestCreatedItem = await EntireItemsProcessor.GetLatestCreated(ApiHelper.ApiClient, TypeOfItems.PaymentCard);
                if (latestCreatedItem is null)
                {
                    await PageService.PushPopupAsync(new ErrorView("Something went wrong and your payment card has not been created, try again!"));
                }
                else
                {
                    UpdateModel model = new UpdateModel(TypeOfUpdates.Create, latestCreatedItem);
                    await GoTo("PaymentCard", model);
                }
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your payment card has not been created, try again!"));
        }
        private protected override async Task Modify(int id)
        {
            if (!PaymentCard.IsChanged(_tempPaymentCard)) await Shell.Current.Navigation.PopToRootAsync();
            var encryptedCard = (PaymentCard)EncryptItem(PaymentCard);
            bool isSuccess = await PaymentCardProcessor.Modify(ApiHelper.ApiClient, id, encryptedCard);
            if (isSuccess)
            {
                if (_tempPaymentCard.Name != PaymentCard.Name)//if some props from itempreviews changed, then update the item
                {
                    UpdateModel model = new UpdateModel(TypeOfUpdates.Modify, new ItemPreview(PaymentCard.Id, PaymentCard.Name, TypeOfItems.PaymentCard.ToSampleString(), TypeOfItems.PaymentCard));
                    await GoTo("PaymentCard", model);
                }
                else
                    await GoTo("PaymentCard");
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your payment card has not been modified, try again!"));
        }
        private protected override async Task Delete()
        {
            bool isSuccess = await PaymentCardProcessor.Delete(ApiHelper.ApiClient, PaymentCard.Id);
            if (isSuccess)
            {
                UpdateModel model = new UpdateModel(TypeOfUpdates.Delete, new ItemPreview() { Id = PaymentCard.Id, ItemType = TypeOfItems.PaymentCard });
                await GoTo("PaymentCard", model);
            }
            else
                await PageService.PushPopupAsync(new ErrorView("Something went wrong and your payment card has not been deleted, try again!"));
        }
        private protected override Models.TaskStatus IsModelValid()
        {
            string msgToDisplay = string.Empty;
            if (string.IsNullOrEmpty(PaymentCard.Name))
                msgToDisplay = "You need to complete at least Name in order to save!";
            if (PaymentCard.NameOnCard?.Length > 64)
                msgToDisplay = "Your name on card must be maximum 64 characters!";
            if (PaymentCard.CardType?.Length > 32)
                msgToDisplay = "Your Card Type must be maximum 32 characters!";
            if (PaymentCard.CardNumber?.Length > 19)
                msgToDisplay = "Your Card Number must be maximum 19 characters!";
            if (PaymentCard.SecurityCode?.Length > 3)
                msgToDisplay = "Your Security Code must be maximum 3 characters!";
            if (string.IsNullOrEmpty(msgToDisplay))
                return new Models.TaskStatus(false, string.Empty);
            else
                return new Models.TaskStatus(true, msgToDisplay);
        }
        private protected override object EncryptItem(object obj)
        {
            var cardToEncrypt = (PaymentCard)obj;
            cardToEncrypt.NameOnCard = VaultManager.EncryptString(cardToEncrypt.NameOnCard);
            cardToEncrypt.CardType = VaultManager.EncryptString(cardToEncrypt.CardType);
            cardToEncrypt.CardNumber = VaultManager.EncryptString(cardToEncrypt.CardNumber);
            cardToEncrypt.SecurityCode = VaultManager.EncryptString(cardToEncrypt.SecurityCode);
            cardToEncrypt.Notes = VaultManager.EncryptString(cardToEncrypt.Notes);
            return cardToEncrypt;
        }
        private protected override object DecryptItem(object obj)
        {
            var cardToDecrypt = (PaymentCard)obj;
            cardToDecrypt.NameOnCard = VaultManager.DecryptString(cardToDecrypt.NameOnCard);
            cardToDecrypt.CardType = VaultManager.DecryptString(cardToDecrypt.CardType);
            cardToDecrypt.CardNumber = VaultManager.DecryptString(cardToDecrypt.CardNumber);
            cardToDecrypt.SecurityCode = VaultManager.DecryptString(cardToDecrypt.SecurityCode);
            cardToDecrypt.Notes = VaultManager.DecryptString(cardToDecrypt.Notes);
            return cardToDecrypt;
        }
    }
}
