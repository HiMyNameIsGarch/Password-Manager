using PassManager.Enums;

namespace PassManager.Models
{
    public static class ErrorMsg
    {
        public const string CompleteAllFieldsError = "You need to complete all fields in order to register!";
        public const string ServerError = "Oops... We couldn't connect to server, try again later!";
        public const string BasicError = "Oops... Something went wrong, try again!";
        public static string FieldMaxCharLong(string field, int maxLength)
        {
            return $"Your {field} must be maximum {maxLength} characters long!";
        }
        public static string CompleteFields(string field)
        {
            return $"You need to complete at least {field} in order to save!";
        }
        public static string CompleteFields(string field1, string field2)
        {
            return $"You need to complete at least {field1} and {field2} in order to save!";
        }
        public static string CompleteFields(string field1, string field2, string field3)
        {
            return $"You need to complete at least {field1}, {field2} and {field3} in order to save!";
        }
        public static string CouldNotGetItem(TypeOfItems item)
        {
            return $"Something went wrong and we couldn't get your {item.ToSampleString()}, try again!";
        }
        public static string ItemNotCreated(TypeOfItems item)
        {
            return $"Something went wrong and your {item.ToSampleString()} has not been created, try again!";
        }
        public static string ItemNotDeleted(TypeOfItems item)
        {
            return $"Something went wrong and your {item.ToSampleString()} has not been deleted, try again!";
        }
        public static string ItemNotModified(TypeOfItems item)
        {
            return $"Something went wrong and your {item.ToSampleString()} has not been modified, try again!";
        }
        public static string NoItems(TypeOfItems item)
        {
            return $"You have no {item.ToPluralString()} yet, click on button below to add a new one!";
        }
    }
}
