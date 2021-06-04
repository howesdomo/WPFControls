using System.Collections.Generic;
using System.Windows;

namespace System.Windows.Controls
{
    /// <summary>
    /// <para>WPF Controls 工具类</para>
    /// <para></para>
    /// <para>
    /// V 1.0.0 - 2021-06-01 11:35:13
    /// 首次创建
    /// </para>
    /// </summary>
    public class WPFControlsUtils
    {
        /// <summary>
        /// 通过名称查找子控件，并返回一个List集合
        /// </summary>
        /// <typeparam name="T">需要查找的控件类型</typeparam>
        /// <param name="root">源头控件</param>
        /// <param name="name">(选填)FrameworkElement.Name 或 在XAML中定义控件的 Name</param>
        /// <returns></returns>
        public static List<T> FindChilrenOfType<T>(DependencyObject root, string name = null) where T : FrameworkElement
        {
            if (root == null) { return null; }

            List<T> r = new List<T>();

            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(root); i++)
            {
                DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(root, i);

                if (child is T toAdd)
                {
                    if (toAdd.Name == name | string.IsNullOrEmpty(name))
                    {
                        r.Add((T)child);
                    }
                }

                r.AddRange(FindChilrenOfType<T>(child, name));
            }

            return r;
        }

        /// <summary>
        /// 通过名称查找某子控件
        /// </summary>
        /// <typeparam name="T">需要查找的控件类型</typeparam>
        /// <param name="root">源头控件</param>
        /// <param name="name">(选填)FrameworkElement.Name 或 在XAML中定义控件的 Name</param>
        /// <returns></returns>
        public static T FindChildOfType<T>(DependencyObject root, string name = null) where T : FrameworkElement
        {
            if (root == null) { return null; }

            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(root); i++)
            {
                DependencyObject child = System.Windows.Media.VisualTreeHelper.GetChild(root, i);

                if (child is T toAdd && (toAdd.Name == name | string.IsNullOrEmpty(name)))
                {
                    return toAdd;
                }
                else
                {
                    T grandChild = FindChildOfType<T>(child, name);
                    if (grandChild != null)
                    {
                        return grandChild;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 通过名称查找父控件
        /// </summary>
        /// <typeparam name="T">需要查找的控件类型</typeparam>
        /// <param name="obj"></param>
        /// <param name="parentName">(选填)父节点在 FrameworkElement.Name 或 在XAML中定义控件的 Name</param>
        /// <returns></returns>
        public static T FindParentOfType<T>(DependencyObject obj, string parentName = null) where T : FrameworkElement
        {
            if (obj == null) { return null; }

            DependencyObject parent = System.Windows.Media.VisualTreeHelper.GetParent(obj);

            while (parent != null)
            {
                if (parent is T toAdd && (toAdd.Name == parentName | string.IsNullOrEmpty(parentName)))
                {
                    return toAdd;
                }

                parent = System.Windows.Media.VisualTreeHelper.GetParent(parent);
            }

            return null;
        }
    }
}
