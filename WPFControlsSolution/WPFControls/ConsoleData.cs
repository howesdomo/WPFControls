using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Util.Model
{
    public enum ConsoleMsgType
    {
        /// <summary>
        /// 作为 ComboBox 搜索条件空值使用
        /// </summary>
        NONE = -1,
        DEFAULT = 0,
        DEBUG = 1,
        INFO = 2,
        WARNING = 4,
        ERROR = 11,
        BUSINESSERROR = 12,
    }

    public class ConsoleData
    {
        public ConsoleData(string content)
        {
            init
            (
                content: content,
                consoleMsgType: 0,
                entryTime: DateTime.Now
            );
        }

        public ConsoleData(string content, ConsoleMsgType consoleMsgType)
        {
            init
            (
                content: content,
                consoleMsgType: consoleMsgType,
                entryTime: DateTime.Now
            );
        }

        public ConsoleData(string content, DateTime entryTime)
        {
            init
            (
                content: content,
                consoleMsgType: 0,
                entryTime: entryTime
            );
        }

        public ConsoleData(string content, ConsoleMsgType consoleMsgType, DateTime entryTime)
        {
            init
            (
                content: content,
                consoleMsgType: consoleMsgType,
                entryTime: entryTime
            );
        }

        private void init(string content, ConsoleMsgType consoleMsgType, DateTime entryTime)
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
                case ConsoleMsgType.DEFAULT:
                    Foreground = "Black";
                    break;
                case ConsoleMsgType.DEBUG:
                    Foreground = "Silver";
                    break;
                case ConsoleMsgType.INFO:
                    Foreground = "Green";
                    break;
                case ConsoleMsgType.WARNING:
                    Foreground = "Orange";
                    break;
                case ConsoleMsgType.ERROR:
                case ConsoleMsgType.BUSINESSERROR:
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
