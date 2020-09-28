using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Client.Components
{
    /// <summary>
    /// V 1.0.0 - 2020-09-23 13:59:56
    /// 首次创建
    /// </summary>
    public class TextBoxAdv : TextBox
    {
        public TextBlock mPlaceHolderTextBlock { get; set; }

        public TextBoxAdv()
        {
            var p = new System.Windows.Media.VisualBrush();
            p.AlignmentX = System.Windows.Media.AlignmentX.Left; // 控制 PlaceHolder 位置
            p.AlignmentY = System.Windows.Media.AlignmentY.Center; // 控制 PlaceHolder 位置
            p.Stretch = System.Windows.Media.Stretch.None;
            p.TileMode = System.Windows.Media.TileMode.None;

            mPlaceHolderTextBlock = new TextBlock();
            mPlaceHolderTextBlock.FontStyle = System.Windows.FontStyles.Italic;
            mPlaceHolderTextBlock.Foreground = System.Windows.Media.Brushes.Silver;

            p.Visual = mPlaceHolderTextBlock;

            this.Resources.Add("txtPlaceholder", p);

            var style = new System.Windows.Style();
            style.TargetType = typeof(TextBoxAdv);

            #region PlaceHolder 触发器 -- 当 NULL 或 string.Empty 时显示 PlaceHolder 内容

            var tiggerWhenNull = new System.Windows.Trigger();
            tiggerWhenNull.Property = TextBoxAdv.TextProperty;
            tiggerWhenNull.Value = null;

            tiggerWhenNull.Setters.Add(new System.Windows.Setter(TextBoxAdv.BackgroundProperty, p));

            style.Triggers.Add(tiggerWhenNull);

            var tiggerWhenStringEmpty = new System.Windows.Trigger();
            tiggerWhenStringEmpty.Property = TextBoxAdv.TextProperty;
            tiggerWhenStringEmpty.Value = "";

            tiggerWhenStringEmpty.Setters.Add(new System.Windows.Setter(TextBoxAdv.BackgroundProperty, p));

            style.Triggers.Add(tiggerWhenStringEmpty);

            #endregion

            this.Style = style;
        }

        #region Placeholder

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register
        (
            name: "Placeholder",
            propertyType: typeof(string),
            ownerType: typeof(TextBoxAdv),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onPlaceholder_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); }
        }

        public static void onPlaceholder_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is TextBoxAdv) == false) { return; }
            var target = d as TextBoxAdv;
            target.mPlaceHolderTextBlock.Text = e.NewValue?.ToString();
        }

        #endregion

        #region PlaceholderColor

        public static readonly DependencyProperty PlaceholderColorProperty = DependencyProperty.Register
        (
            name: "PlaceholderColor",
            propertyType: typeof(System.Windows.Media.Brush),
            ownerType: typeof(TextBoxAdv),
            validateValueCallback: null, //new ValidateValueCallback((toValidate) => { return toValidate is System.Windows.Media.Brush; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onPlaceholderColor_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.Brush PlaceholderColor
        {
            get { return (System.Windows.Media.Brush)GetValue(PlaceholderColorProperty); }
            set { SetValue(PlaceholderColorProperty, value); }
        }

        public static void onPlaceholderColor_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is TextBoxAdv) == false) { return; }
            var target = d as TextBoxAdv;
            target.mPlaceHolderTextBlock.Foreground = (System.Windows.Media.Brush)e.NewValue;
        }

        #endregion
    }

    //// TextBoxAdv 改编自下面的 XAML 代码
    //
    //<UserControl.Resources>
    //    <VisualBrush
    //        x:Key="TextBoxPlaceholder_VisualBrush"
    //        AlignmentX="Left"
    //        Stretch="None"
    //        TileMode="None">
    //        <VisualBrush.Visual>
    //            <TextBlock
    //                FontStyle = "Italic"
    //                Foreground="Red"
    //                Text="不加引号Index(用逗号分隔)" />
    //        </VisualBrush.Visual>
    //    </VisualBrush>

    //    <Style
    //        x:Key="TextBoxPlaceholder_Style"
    //        TargetType="TextBox">
    //        <Style.Triggers>
    //            <Trigger Property = "Text" Value="{x:Null}">
    //                <Setter Property = "Background" Value="{StaticResource TextBoxPlaceholder_VisualBrush}" />
    //            </Trigger>
    //            <Trigger Property = "Text" Value="">
    //                <Setter Property = "Background" Value="{StaticResource TextBoxPlaceholder_VisualBrush}" />
    //            </Trigger>
    //        </Style.Triggers>
    //    </Style>
    //</UserControl.Resources>

    //<TextBox Style = "{StaticResource TextBoxPlaceholder_Style}" />
}
