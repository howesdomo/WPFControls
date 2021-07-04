using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Client.Components.PrinterPanel
{
    public class ZebraPrinter : INotifyPropertyChanged
    {
        private Printer _Printer;
        public Printer Printer
        {
            get { return _Printer; }
            set
            {
                _Printer = value;
                this.OnPropertyChanged(nameof(Printer));
            }
        }            

        private string _Speed;
        public string Speed
        {
            get { return _Speed; }
            set
            {
                _Speed = value;
                this.OnPropertyChanged(nameof(Speed));
            }
        }

        private string _Darkness;
        public string Darkness
        {
            get { return _Darkness; }
            set
            {
                _Darkness = value;
                this.OnPropertyChanged(nameof(Darkness));
            }
        }


        private string _AlignTop;
        public string AlignTop
        {
            get { return _AlignTop; }
            set
            {
                _AlignTop = value;
                this.OnPropertyChanged(nameof(AlignTop));
            }
        }


        private string _AlignLeft;
        public string AlignLeft
        {
            get { return _AlignLeft; }
            set
            {
                _AlignLeft = value;
                this.OnPropertyChanged(nameof(AlignLeft));
            }
        }


        #region INotifyPropertyChanged成员

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
