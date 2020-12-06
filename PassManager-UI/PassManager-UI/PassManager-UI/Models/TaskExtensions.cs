using System;

namespace PassManager.Models
{
    public static class TaskExtensions
    {
        public async static void Await(this System.Threading.Tasks.Task task, Action<Exception> errorCallBack)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                errorCallBack?.Invoke(ex);
            }
        }
        public async static void Await(this System.Threading.Tasks.Task task)
        {
            await task;
        }
    }
}
