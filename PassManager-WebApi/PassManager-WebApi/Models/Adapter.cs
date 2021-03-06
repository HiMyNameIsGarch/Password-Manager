﻿using System;
using PassManager_WebApi.ViewModels;

namespace PassManager_WebApi.Models
{
    public partial class Password
    {
        public Password() { }
        public Password(PasswordVM passwordVM, string userId)
        {
            ModifyTo(passwordVM);
            CreateDate = DateTime.Now;
            NumOfVisits = 1;
            UserId = userId;
        }

        public void ModifyTo(PasswordVM passwordVM)
        {
            Name = passwordVM.Name;
            Username = passwordVM.Username;
            PasswordEncrypted = passwordVM.PasswordEncrypted;
            Url = passwordVM.Url;
            Notes = passwordVM.Notes;
            LastModified = DateTime.Now;
        }
    }
    public partial class Wifi
    {
        public Wifi() { }
        public Wifi(WifiVM wifiVM, string userId)
        {
            ModifyTo(wifiVM);
            UserId = userId;
            NumOfVisits = 1;
            CreateDate = DateTime.Now;
        }
        public void ModifyTo(WifiVM wifiVM)
        {
            Name = wifiVM.Name;
            PasswordEncrypted = wifiVM.PasswordEncrypted;
            SSID = wifiVM.SSID;
            SettingsPassword = wifiVM.SettingsPassword;
            ConnectionType = wifiVM.ConnectionType;
            Notes = wifiVM.Notes;
            LastModified = DateTime.Now;
        }
    }
    public partial class Note
    {
        public Note(){ }
        public Note(NoteVM note, string userId)
        {
            ModifyTo(note);
            UserId = userId;
            NumOfVisits = 1;
            CreateDate = DateTime.Now;
        }
        public void ModifyTo(NoteVM note)
        {
            Name = note.Name;
            Notes = note.Notes;
            LastModified = DateTime.Now;
        }
    }
    public partial class PaymentCard
    {
        public PaymentCard() { }
        public PaymentCard(PaymentCardVM paymentCard, string userId)
        {
            ModifyTo(paymentCard);
            UserId = userId;
            NumOfVisits = 1;
            CreateDate = DateTime.Now;
        }
        public void ModifyTo(PaymentCardVM paymentCard)
        {
            Name = paymentCard.Name;
            StartDate = paymentCard.StartDate;
            ExpirationDate = paymentCard.ExpirationDate;
            NameOnCard = paymentCard.NameOnCard;
            CardType = paymentCard.CardType;
            CardNumber = paymentCard.CardNumber;
            SecurityCode = paymentCard.SecurityCode;
            Notes = paymentCard.Notes;
            LastModified = DateTime.Now;
        }
    }
}