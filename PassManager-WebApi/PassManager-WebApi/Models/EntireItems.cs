using PassManager_WebApi.Enums;
using System.Collections.Generic;
using System.Linq;

namespace PassManager_WebApi.Models
{
    internal static class EntireItems
    {
        internal static IEnumerable<Grouping<string, ItemPreview>> GroupItems(this IEnumerable<ItemPreview> listToGroup) 
        {
            if (listToGroup is null) return null;
            return listToGroup.GroupBy(item => item.ItemType)
                              .Select(item => new Grouping<string, ItemPreview>(item.Key.ToPluralString(), item));
        }
        internal static IQueryable<ItemPreview> GetAllPasswords(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            string iconUrl = IconHelper.GetImageUrl(TypeOfItems.Password);
            return db.Passwords
                .Where(item => item.UserId == userId)
                .OrderByDescending(p => p.NumOfVisits)
                .ThenBy(p => p.Name)
                .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = item.Username, ItemType = TypeOfItems.Password, IconUrl = iconUrl });
        }
        internal static IQueryable<ItemPreview> GetAllWifis(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            string iconUrl = IconHelper.GetImageUrl(TypeOfItems.Wifi);
            string subTitle = TypeOfItems.Wifi.ToSampleString();
            return db.Wifis
            .Where(item => item.UserId == userId)
            .OrderByDescending(p => p.NumOfVisits)
            .ThenBy(p => p.Name)
            .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = subTitle, ItemType = TypeOfItems.Wifi, IconUrl = iconUrl });
        }
        internal static IQueryable<ItemPreview> GetAllNotes(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            string iconUrl = IconHelper.GetImageUrl(TypeOfItems.Note);
            return db.Notes
            .Where(item => item.UserId == userId)
            .OrderByDescending(p => p.NumOfVisits)
            .ThenBy(p => p.Name)
            .Select(item => new ItemPreview() { Id = item.Id, Title = item.Name, SubTitle = TypeOfItems.Note.ToString(), ItemType = TypeOfItems.Note, IconUrl = iconUrl });
        }
        internal static IQueryable<ItemPreview> GetAllPaymentCards(PasswordManagerEntities db, string userId)
        {
            if (db is null || string.IsNullOrEmpty(userId)) return null;
            string iconUrl = IconHelper.GetImageUrl(TypeOfItems.PaymentCard);
            string subTitle = TypeOfItems.PaymentCard.ToSampleString();
            return db.PaymentCards
            .Where(note => note.UserId == userId)
            .OrderByDescending(note => note.NumOfVisits)
            .ThenBy(note => note.Name)
            .Select(note => new ItemPreview() { Id = note.Id, Title = note.Name, SubTitle = subTitle, ItemType = TypeOfItems.PaymentCard, IconUrl = iconUrl });
        }
    }
}