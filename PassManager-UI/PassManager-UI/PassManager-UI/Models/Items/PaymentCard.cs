using System;
using System.Collections.Generic;
using System.Text;

namespace PassManager.Models.Items
{
    public class PaymentCard : ICloneable
    {
        public PaymentCard()
        {
            Name = NameOnCard = CardType = CardNumber = SecurityCode = Notes = string.Empty;
            StartDate = ExpirationDate = DateTime.MinValue;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string NameOnCard { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public string SecurityCode { get; set; }
        public string Notes { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        internal bool IsChanged(PaymentCard card)
        {
            if(card is null)
            {
                return Name.Length > 0 || !StartDate.Equals(DateTime.MinValue) || !ExpirationDate.Equals(DateTime.MinValue)
                    || NameOnCard.Length > 0 || CardType.Length > 0 || CardNumber.Length > 0 || SecurityCode.Length > 0 || Notes.Length > 0;
            }
            return card.Name != Name || card.StartDate != StartDate || card.ExpirationDate != ExpirationDate || card.NameOnCard != NameOnCard
                || card.CardType != CardType || card.CardNumber != CardNumber || card.SecurityCode != SecurityCode || card.Notes != Notes;
        }
    }
}
