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
    public class CreatePaymentCardVM : BaseItemVM
    {
        public CreatePaymentCardVM() : base(TypeOfItems.PaymentCard)
        {
            _paymentCard = new PaymentCard();
        }
        //variables
        private PaymentCard _paymentCard;
        private PaymentCard _tempPaymentCard;
        //props
        public PaymentCard PaymentCard
        {
            get { return _paymentCard; }
            set { _paymentCard = value; NotifyPropertyChanged(); }
        }
        //functions
        private protected override bool IsItemChanged()
        {
            return PaymentCard.IsChanged(_tempPaymentCard);
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
                await PageService.PushPopupAsync(new ErrorView(ErrorMsg.CouldNotGetItem(ItemType)));
        }
        private protected override async Task<bool> CreateAsync()
        {
            var encryptedCard = (PaymentCard)EncryptItem(PaymentCard);
            bool isSuccess = await PaymentCardProcessor.CreateCard(ApiHelper.ApiClient, encryptedCard);
            return isSuccess;
        }
        private protected override async Task<ModifyCallStatus> ModifyAsync(int id)
        {
            var encryptedCard = (PaymentCard)EncryptItem(PaymentCard);
            bool isSuccess = await PaymentCardProcessor.Modify(ApiHelper.ApiClient, id, encryptedCard);
            return new ModifyCallStatus(isSuccess, _tempPaymentCard.Name != PaymentCard.Name, new ItemPreview(PaymentCard.Id, PaymentCard.Name, TypeOfItems.PaymentCard.ToSampleString(), TypeOfItems.PaymentCard));
        }
        private protected override async Task<DeleteCallStatus> DeleteAsync()
        {
            bool isSuccess = await PaymentCardProcessor.Delete(ApiHelper.ApiClient, PaymentCard.Id);
            return new DeleteCallStatus(isSuccess, PaymentCard.Id);
        }
        private protected override Models.TaskStatus IsModelValid()
        {
            string msgToDisplay = string.Empty;
            if (string.IsNullOrEmpty(PaymentCard.Name))
                msgToDisplay = ErrorMsg.CompleteFields("Name");
            if (PaymentCard.NameOnCard?.Length > 64)
                msgToDisplay = ErrorMsg.FieldMaxCharLong("Name On Card", 64);
            if (PaymentCard.CardType?.Length > 32)
                msgToDisplay = ErrorMsg.FieldMaxCharLong("Card Type", 32);
            if (PaymentCard.CardNumber?.Length > 19)
                msgToDisplay = ErrorMsg.FieldMaxCharLong("Card Number", 19);
            if (PaymentCard.SecurityCode?.Length > 3)
                msgToDisplay = ErrorMsg.FieldMaxCharLong("Security Code", 3);

            return Models.TaskStatus.Status(msgToDisplay);
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
