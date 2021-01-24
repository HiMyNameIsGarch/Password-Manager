namespace PassManager_WebApi.Models.Interfaces
{
    interface IModelValid
    {
        /// <summary>
        /// checks if the model of the object is valid or not
        /// </summary>
        /// <returns>if it is valid, it will return an empty string, if not a message with the error</returns>
        string IsModelValid();
    }
}
