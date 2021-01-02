namespace PassManager.Models
{
    public class TaskStatus
    {
        public TaskStatus(bool error, string errorMsg = "")
        {
            IsError = error;
            Message = errorMsg;
        }
        public bool IsError { get; }
        public string Message { get; }
    }
}
