namespace PassManager.Models.CallStatus
{
    public class DeleteCallStatus
    {
        public DeleteCallStatus(bool isSucces, int id)
        {
            IsCallSucces = isSucces;
            Id = id;
        }
        public bool IsCallSucces { get; set; }
        public int Id { get; set; }
    }
}
