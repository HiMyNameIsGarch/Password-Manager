using PassManager.Enums;

namespace PassManager.Models
{
    public class UpdateModel
    {
        public UpdateModel(TypeOfUpdates updateType, int id = -1)
        {
            UpdateType = updateType;
            _id = id;
        }
        private int _id;
        public TypeOfUpdates UpdateType { get; set; }
        public int IdToDelete //if the updatetype is delete
        {
            get { return UpdateType == TypeOfUpdates.Delete ? _id : -1; }// then return the value of the id
            set { _id = UpdateType == TypeOfUpdates.Delete ? value : -1; }//the set the value
        }
    }
}
