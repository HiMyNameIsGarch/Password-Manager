using System;
using System.Collections.Generic;
using System.Text;

namespace PassManager.Models
{
    public class TaskStatus
    {
        public TaskStatus(bool error, string errorMsg = "")
        {
            IsError = error;
            Message = errorMsg;
        }
        public bool IsError { get; set; }
        public string Message { get; set; }
    }
}
