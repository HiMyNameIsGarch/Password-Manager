using PassManager_WebApi.Enums;

namespace PassManager_WebApi.Models
{
    public static class ErrorMsg
    {
        public const string InvalidSearchString = "Your search string could not be empty!";
        public const string InvalidId = "Id is invalid";
        public static string ItemNotFound(TypeOfItems itemType)
        {
            return $"{itemType.ToSampleString()} not found!";
        }
        public static string ItemDoesNotExist(TypeOfItems itemType)
        {
            return $"{itemType.ToSampleString()} does not exist!";
        }
        public static string InvalidIdMatchingWith(TypeOfItems itemType)
        {
            return $"Id does not match with the {itemType.ToSampleString()} id!";
        }
    }
}