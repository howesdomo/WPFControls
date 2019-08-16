using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Util.Model
{
    public enum ConsoleMsgType
    {
        Info = 0,
        Question = 1,
        Warning = 2,
        Error = 3
    }

    public class ConsoleData
    {
        public ConsoleData(string content)
        {
            this.Content = content;
            this.ConsoleMsgType = 0;
            this.EntryTime = DateTime.Now;
        }

        public ConsoleData(string content, ConsoleMsgType consoleMsgType)
        {
            this.Content = content;
            this.ConsoleMsgType = consoleMsgType;
            this.EntryTime = DateTime.Now;
        }

        public ConsoleData(string content, DateTime entryTime)
        {
            this.Content = content;
            this.ConsoleMsgType = 0;
            this.EntryTime = entryTime;
        }

        public ConsoleData(string content, ConsoleMsgType consoleMsgType, DateTime entryTime)
        {
            this.Content = content;
            this.ConsoleMsgType = consoleMsgType;
            this.EntryTime = entryTime;
            getForeground();
        }

        private void getForeground()
        {
            switch (ConsoleMsgType)
            {
                case ConsoleMsgType.Info:
                    // Foreground = Colors.Green;
                    Foreground = "Green";
                    break;
                case ConsoleMsgType.Question:
                    // Foreground = Colors.Purple;
                    Foreground = "Purple";
                    break;
                case ConsoleMsgType.Warning:
                    // Foreground = Colors.Orange;
                    Foreground = "Orange";
                    break;
                case ConsoleMsgType.Error:
                    // Foreground = Colors.Red;
                    Foreground = "Red";
                    break;
            }
        }

        public ConsoleMsgType ConsoleMsgType { get; set; }

        public string Content { get; set; }

        public DateTime EntryTime { get; set; }

        public string Foreground { get; set; }
    }
}
