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
            get { return SetIfDeleteUpdate(_id); }// then return the value of the id
            set { _id = SetIfDeleteUpdate(value); }//the set the value
        }
        private int SetIfDeleteUpdate(int value)
        {
            return UpdateType == TypeOfUpdates.Delete ? value : -1;
        }
    }
}
