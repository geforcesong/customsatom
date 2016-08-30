using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;

namespace ProTemplate.UserControls.RadWindows
{
    public partial class VerifyDetailInfoWindow : RadWindow
    {
        public VerifyDetailInfoWindow(string rawMessage)
        {
            InitializeComponent();
            gdResult.ItemsSource = GetResultList(rawMessage);
        }

        List<MessageItem> GetResultList(string rawMessage)
        {
            List<MessageItem> result = new List<MessageItem>();
            string [] items = rawMessage.Split('。');
            foreach (var item in items)
            {
                MessageItem mi = new MessageItem();
                if (item.Contains("预录Item不存在") || item.Contains("预录的值"))
                {
                    mi.Msg1 = item;
                }
                else if (item.Contains("打单Item不存在")|| item.Contains("打单中的值"))
                {
                    mi.Msg2 = item;
                }
                else
                {
                    try
                    {
                        string[] tmp = item.Split('!');
                        mi.Title = tmp[0];
                        mi.Msg1 = tmp[1].Split('，')[0];
                        mi.Msg2 = tmp[1].Split('，')[1];
                    }
                    catch { }
                }
                result.Add(mi);
            }
            return result;
        }
    }

    public class MessageItem
    {
        public string Title { get; set; }
        public string Msg1 { get; set; }
        public string Msg2 { get; set; }
    }
}
