using System;
using System.Threading.Tasks;
using PassManager.Views.Popups;

namespace PassManager.Models
{
    public static class TaskExtensions
    {
        /// <summary>
        /// you can await a task that don't return anything in a constructor or in a setter(to get data) and catch exceptions if needed
        /// </summary>
        /// <param name="task">task that will be awaited</param>
        /// <param name="errorCallBack">if that task throw an exception, this method will be invoked</param>
        public async static void AwaitWithPopup(this Task task, Action<Exception> errorCallBack, bool animate = true)
        {
            await PageService.PopAllAsync();
            await PageService.PushPopupAsync(new WaitForActionView(), animate);
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                errorCallBack?.Invoke(ex);
            }
            await PageService.PopPopupAsync(animate);
        }
        //awaits a task(in a constructor or in a setter)
        public async static void Await(this Task task)
        {
            await task;
        }
    }
}
