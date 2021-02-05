using PassManager_WebApi.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace PassManager_WebApi.App_Start
{
    public class Logging
    {
        private const string BasePath = @"D:\Curs\My code\Password-Manager\PassManager-WebApi\PassManager-WebApi";
        private string LogsPath = string.Empty;
        const string FolderName = "\\Logs";
        private string FullName = string.Empty;
        private const string DateFormat = "yyyy-dd-MM";
        private const string FileExtension = ".log";
        public Logging()
        {
            //var workingDirectory = Environment.CurrentDirectory;
            //LogsPath = Path.Combine(Directory.GetParent(workingDirectory).Parent.Parent.FullName, FolderName);
            LogsPath = BasePath + FolderName;
            SetCurrentLogFile();
        }
        public void Start()
        {
            SetCurrentLogFile();
            using (StreamWriter writer = File.AppendText(FullName))
            {
                writer.Write("\rPassword Manager start at: ");
                writer.WriteLine($"{DateTime.Now.ToLongTimeString()} {DateTime.Now.ToLongDateString()}\n");
            }
        }
        private void SetCurrentLogFile()
        {
            var currentDate = DateTime.Now.ToString(DateFormat);
            var currentFileName = currentDate + FileExtension;
            var fullPath = Path.Combine(LogsPath, currentFileName);
            //get latest created file
            var lastCreatedLog = Directory.GetFiles(LogsPath, "*" + FileExtension).LastOrDefault();
            if (!string.IsNullOrEmpty(lastCreatedLog))
            {
                if (lastCreatedLog.EndsWith(currentFileName))
                {
                    FullName = fullPath;
                    return;
                }
                FullName = fullPath;
                CreateEmptyFile(fullPath);
                return;
            }
            FullName = fullPath;
            CreateEmptyFile(fullPath);
        }
        private void CreateEmptyFile(string fullPath)
        {
            FileStream fs = File.Create(fullPath);
            fs.Close();
        }
        //basic log types actions
        public void Error(string message, Exception exception = null)
        {
            Log(MessageType.Error, message, exception);
        }
        public void Warning(string message)
        {
            Log(MessageType.Warning, message, null);
        }
        public void Info(string message)
        {
            Log(MessageType.Info, message, null);
        }
        public void Debug(string message)
        {
            Log(MessageType.Debug, message, null);
        }
        public void Fatal(string message, Exception exception)
        {
            Log(MessageType.Fatal, message, exception);
        }
        //base log
        private void Log(MessageType messageType, string message, Exception exception = null)
        {
            //add date time when the log occurs
            string toLog = DateTime.Now.ToString("HH:mm:ss FFF");
            //add prefix
            switch (messageType)
            {
                case MessageType.Debug:
                    toLog += " DEBUG: ";
                    break;
                case MessageType.Info:
                    toLog += " INFO: ";
                    break;
                case MessageType.Warning:
                    toLog += " WARNING: ";
                    break;
                case MessageType.Error:
                    toLog += " ERROR: ";
                    break;
                case MessageType.Fatal:
                    toLog += " FATAL: ";
                    break;
            }
            //add msg
            toLog += message;
            //add exception if it is
            if (exception != null)
            {
                toLog += Environment.NewLine;
                toLog += "Exception Message: " + exception.Message;
                toLog += Environment.NewLine;
                toLog += "Exception StackTrace: " + exception.StackTrace;
            }
            //write to file
            using (StreamWriter streamWriter = File.AppendText(FullName))
            {
                streamWriter.WriteLine(toLog);
            }
        }
    }
}