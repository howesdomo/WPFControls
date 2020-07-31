using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Client.Test
{
    /// <summary>
    /// Interaction logic for FrmTestMarqueeLabel.xaml
    /// </summary>
    public partial class FrmTestMarqueeLabel : Window
    {
        public FrmTestMarqueeLabel()
        {
            InitializeComponent();           
        }
    }

    public class FrmTestMarqueeLabel_ViewModel : BaseViewModel
    {
        private string _Text = "自经丧乱少睡眠，长夜沾湿何由彻！安得广厦千万间，大庇天下寒士俱欢颜。风雨不动安如山！呜呼，何时眼前突兀见此屋，吾庐独破受冻死亦足！";

        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                this.OnPropertyChanged();
            }
        }

        private Brush _BackgroundColor = Brushes.Yellow;

        public Brush BackgroundColor
        {
            get { return _BackgroundColor; }
            set
            {
                _BackgroundColor = value;
                this.OnPropertyChanged();
            }
        }

        private Thickness _Margin = new Thickness(20, 15, 10, 5);

        public Thickness Margin
        {
            get { return _Margin; }
            set { _Margin = value; }
        }


        private Thickness _Padding = new Thickness(5, 10, 15, 20);

        public Thickness Padding
        {
            get { return _Padding; }
            set
            {
                _Padding = value;
                this.OnPropertyChanged();

            }
        }

        private Brush _TextColor = Brushes.Orange;

        public Brush TextColor
        {
            get { return _TextColor; }
            set
            {
                _TextColor = value;
                this.OnPropertyChanged();
            }
        }

        private int _WordsPerSecond = 7;

        public int WordsPerSecond
        {
            get { return _WordsPerSecond; }
            set
            {
                _WordsPerSecond = value;
                this.OnPropertyChanged();
            }
        }

        private double _StartBreakSecond = 1;

        public double StartBreakSecond
        {
            get { return _StartBreakSecond; }
            set
            {
                _StartBreakSecond = value;
                this.OnPropertyChanged();
            }
        }

        private double _EndBreakSecond = 1;

        public double EndBreakSecond
        {
            get { return _EndBreakSecond; }
            set
            {
                _EndBreakSecond = value;
                this.OnPropertyChanged();
            }
        }

        private double _ResetSecond = 1;

        public double ResetSecond
        {
            get { return _ResetSecond; }
            set
            {
                _ResetSecond = value;
                this.OnPropertyChanged();
            }
        }

        private double _FontSize = 30;
        public double FontSize
        {
            get { return _FontSize; }
            set
            {
                _FontSize = value;
                this.OnPropertyChanged();
            }
        }
    }
}
