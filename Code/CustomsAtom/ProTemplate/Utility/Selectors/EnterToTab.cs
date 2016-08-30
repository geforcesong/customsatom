using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Windows.Threading;

namespace ProTemplate.Utility
{
    public class EnterToTab
    {
        private static List<Type> _ctrlTypes = new List<Type>()  
        {  
            typeof(DatePicker),  
            typeof(TextBox),  
            typeof(PasswordBox),  
            typeof(CheckBox),  
            typeof(ComboBox),  
            typeof(AutoCompleteBox)
        };
        public static void RegisterType(Type type)
        {
            if (!_ctrlTypes.Contains(type))
            {
                _ctrlTypes.Add(type);
            }
        }
        public static IEnumerable<T> FindChildren<T>(DependencyObject parent) where T : class
        {
            var count = VisualTreeHelper.GetChildrenCount(VisualTreeHelper.GetChild(parent, 0));
            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    var t = child as T;
                    if (t != null)
                        yield return t;
                    var children = FindChildren<T>(child);
                    foreach (var item in children)
                        yield return item;
                }
            }
        }
        private UIElement _parent;
        private List<Control> _controls = new List<Control>();
        public EnterToTab(UIElement parent)
        {
            _parent = parent;
            // 如果控件还没有加载完就调用 Associate() 方法,则不能查找到子控件,  
            // 所以用定时器  
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromTicks(100);
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Start();
        }
        void Timer_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            Associate();
        }
        private bool _isAssociating = false;
        private void Associate()
        {
            if (_isAssociating) return;
            _isAssociating = true;
            try
            {
                // 清除原来关联过的  
                foreach (var c in _controls)
                {
                    c.KeyUp -= new KeyEventHandler(Control_KeyUp);
                }
                _controls.Clear();
                // 获取可以 Tab 的控件并加入到列表中  
                IEnumerable<Control> originals = EnterToTab.FindChildren<Control>(_parent);
                foreach (var c in originals)
                {
                    if (c.IsTabStop && c.Visibility == Visibility.Visible && c.IsEnabled)
                    {
                        var t1 = c.GetType();
                        foreach (var t2 in _ctrlTypes)
                        {
                            if (t1.IsAssignableFrom(t2))
                            {
                                c.KeyUp += new KeyEventHandler(Control_KeyUp);
                                _controls.Add(c);
                                break;
                            }
                        }
                    }
                }
                // 根据 TabIndex 的原始值排序  
                _controls.Sort(new TabIndexComparer());
            }
            finally
            {
                _isAssociating = false;
            }
        }
        void Control_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter || e.Key == Key.Down) ||
                (e.Key == Key.Up))
            {
                Control source = (sender as Control);
                int index = _controls.IndexOf(source);
                if (e.Key == Key.Enter || e.Key == Key.Down)
                {
                    if (index < _controls.Count - 1)
                    {
                        _controls[index + 1].Focus();
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        _controls[index - 1].Focus();
                    }
                }
            }
        }
        private class TabIndexComparer : IComparer<Control>
        {
            public int Compare(Control x, Control y)
            {
                if (x == y)
                {
                    return 0;
                }
                if (x.TabIndex <= y.TabIndex)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }  
}
