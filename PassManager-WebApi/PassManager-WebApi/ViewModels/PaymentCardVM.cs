using PassManager_WebApi.Models;
using PassManager_WebApi.Models.Interfaces;
using System;

namespace PassManager_WebApi.ViewModels
{
    public class PaymentCardVM : IModelValid
    {
        public PaymentCardVM() { }
        public PaymentCardVM(PaymentCard paymentCard)
        {
            if(paymentCard != null)
            {
                Id = paymentCard.Id;
                Name = paymentCard.Name;
                StartDate = paymentCard.StartDate;
                ExpirationDate = paymentCard.ExpirationDate;
                NameOnCard = paymentCard.NameOnCard;
                CardType = paymentCard.CardType;
                CardNumber = paymentCard.CardNumber;
                SecurityCode = paymentCard.SecurityCode;
                Notes = paymentCard.Notes;
            }
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> ExpirationDate { get; set; }
        public string NameOnCard { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
        public string Notes { get; set; }

        public string IsModelValid()
        {
            if (string.IsNullOrEmpty(Name))
                return "You need to complete at least Name in order to save!";
            if (NameOnCard?.Length > 108)
                return "Your Username must be maximum 64 characters!";
            if (CardType?.Length > 64)
                return "Your CardType must be maximum 32 characters!";
            if (CardNumber?.Length > 44)
                return "Your CardNumber must be maximum 19 characters!";
            if (SecurityCode?.Length > 24)
                return "Your CardNumber must be maximum 3 characters!";
            return string.Empty;
        }
    }
}