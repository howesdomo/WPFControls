﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace WPFControls
{
    public class BusyIndicator : UserControl
    {
        public double mMaxOpacity = 1d;
        public double mMinOpacity = 0.2d;

        List<System.Windows.Shapes.Path> mPathList;

        public BusyIndicator()
        {
            Grid mRoot = new Grid();
            this.Content = mRoot;

            mPathList = new List<System.Windows.Shapes.Path>();

            for (int i = 0; i < 12; i++)
            {
                var p = new System.Windows.Shapes.Path();
                mPathList.Add(p);
                mRoot.Children.Add(p);


                // p.Data = System.Windows.Media.Geometry.Parse("M 0,0 L -10,0 L -10,-60 L 0,-70 L 10,-60 L 10,0 Z");
                p.Data = System.Windows.Media.Geometry.Parse("M 0,0 L -10,0 L -10,-60 L 10,-60 L 10,0 Z");
                p.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                p.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Gray);

                p.Opacity = mMinOpacity;

                var tg = new System.Windows.Media.TransformGroup();
                // TransformGroup 需要进行 2个步骤
                // 1 向上位移 50 ==> Y = -50;
                // 2 旋转一定角度, 让 12 个 Path 组成一个圆 ==> Angle = i * 30;

                var tt = new System.Windows.Media.TranslateTransform();
                tt.Y = -50;
                tg.Children.Add(tt);

                var rt = new System.Windows.Media.RotateTransform();
                rt.Angle = i * 30; // 360 / 12 = 30
                tg.Children.Add(rt);

                p.RenderTransform = tg;

                var ani = new System.Windows.Media.Animation.DoubleAnimation();
                ani.From = mMaxOpacity;
                ani.To = mMinOpacity;
                ani.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(1));

                // 为了实现依次闪烁, 错开每个动画的开始时间
                ani.BeginTime = TimeSpan.FromMilliseconds(i * 1000 / 12);
                ani.RepeatBehavior = System.Windows.Media.Animation.RepeatBehavior.Forever;

                p.BeginAnimation(System.Windows.Shapes.Path.OpacityProperty, ani);
            }
        }

        #region [DP] PathData - 可以修改等待图案的样式

        public static readonly DependencyProperty PathDataProperty = DependencyProperty.Register
        (
            name: "PathData",
            propertyType: typeof(string),
            ownerType: typeof(BusyIndicator),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onPathData_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public string PathData
        {
            get { return (string)GetValue(PathDataProperty); }
            set { SetValue(PathDataProperty, value); }
        }

        public static void onPathData_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is BusyIndicator) == false) { return; }
            var target = d as BusyIndicator;

            foreach (var path in target.mPathList)
            {
                try
                {
                    var pathData = System.Windows.Media.Geometry.Parse(e.NewValue.ToString());
                    path.Data = pathData;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    System.Diagnostics.Debugger.Break();
                }
            }
        }

        #endregion

        #region [DP] PathStroke - 可以修改等待图案的边框颜色

        public static readonly DependencyProperty PathStrokeProperty = DependencyProperty.Register
        (
            name: "PathStroke",
            propertyType: typeof(System.Windows.Media.SolidColorBrush),
            ownerType: typeof(BusyIndicator),
            validateValueCallback: null,
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onPathStroke_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.SolidColorBrush PathStroke
        {
            get { return (System.Windows.Media.SolidColorBrush)GetValue(PathStrokeProperty); }
            set { SetValue(PathStrokeProperty, value); }
        }

        public static void onPathStroke_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is BusyIndicator) == false) { return; }
            var target = d as BusyIndicator;

            foreach (var path in target.mPathList)
            {
                path.Stroke = (System.Windows.Media.SolidColorBrush)e.NewValue;
            }
        }

        #endregion

        #region [DP] PathFill - 可以修改等待图案的填充颜色

        public static readonly DependencyProperty PathFillProperty = DependencyProperty.Register
        (
            name: "PathFill",
            propertyType: typeof(System.Windows.Media.SolidColorBrush),
            ownerType: typeof(BusyIndicator),
            validateValueCallback: null, // new ValidateValueCallback((toValidate) => { return true; }),
            typeMetadata: new PropertyMetadata
            (
                defaultValue: null,
                propertyChangedCallback: onPathFill_PropertyChangedCallback,
                coerceValueCallback: null
            )
        );

        public System.Windows.Media.SolidColorBrush PathFill
        {
            get { return (System.Windows.Media.SolidColorBrush)GetValue(PathFillProperty); }
            set { SetValue(PathFillProperty, value); }
        }

        public static void onPathFill_PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if ((d is BusyIndicator) == false) { return; }
            var target = d as BusyIndicator;


            foreach (var path in target.mPathList)
            {
                path.Fill = (System.Windows.Media.SolidColorBrush)e.NewValue;
            }
        }

#endregion New Region
    }
}
