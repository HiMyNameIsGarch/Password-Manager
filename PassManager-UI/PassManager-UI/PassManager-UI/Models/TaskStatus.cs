namespace PassManager.Models
{
    public class TaskStatus
    {
        public TaskStatus(bool error, string errorMsg = "")
        {
            IsError = error;
            Message = errorMsg;
        }
        public static TaskStatus Status(string msg)
        {
            if (string.IsNullOrEmpty(msg))
                return new TaskStatus(false, string.Empty);
            else
                return new TaskStatus(true, msg);
        }
        public bool IsError { get; }
        public string Message { get; }
    }
}
