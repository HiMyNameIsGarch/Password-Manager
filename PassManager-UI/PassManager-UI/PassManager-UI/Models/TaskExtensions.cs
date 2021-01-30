﻿using System;
using System.Threading.Tasks;
using PassManager.Views.Popups;
using Rg.Plugins.Popup.Services;

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
        //loops thru all the pages in the stack and if finds the page that match up with the parameter returns true
        private static bool CheckIfPageExists(string popupName)
        {
            foreach (var pageInStack in PopupNavigation.Instance.PopupStack)
            {
                if (pageInStack.ToString() == popupName) return true;
            }
            return false;
        }
    }
}
