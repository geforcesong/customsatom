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
using System.ServiceModel.DomainServices.Client;

namespace ProTemplate.UserControls.CustomControl
{
    public partial class DeclarationFilter : UserControl
    {
        public event EventHandler ExcuteQueryClick;
        public event EventHandler ResetClick;
        public event EventHandler DuplicatedClick;

        public DeclarationFilter()
        {
            InitializeComponent();

            InitialFilterItem();
        }

        private void InitialFilterItem()
        {
            dfi1.InitialFilterCondition();
            dfi2.InitialFilterCondition();
            dfi3.InitialFilterCondition();
            dfi4.InitialFilterCondition();

            dfi1.SetDefault("ReceivedDate");
            dfi2.SetDefault("");
            dfi3.SetDefault("");
            dfi4.SetDefault("");

        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            if (ExcuteQueryClick != null)
            {
                ExcuteQueryClick(sender, e);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            InitialFilterItem();
            if (ResetClick != null)
            {
                ResetClick(sender, e);
            }
        }

        private void btnQueryDuplicate_Click(object sender, RoutedEventArgs e)
        {
            if (DuplicatedClick != null)
            {
                DuplicatedClick(sender, e);
            }
        }
        public string ExcuteQuery()
        {
            string strConditions = "1 = 1";
            List<string> conditionList = new List<string>();
            conditionList.Add(dfi1.Query());
            conditionList.Add(dfi2.Query());
            conditionList.Add(dfi3.Query());
            conditionList.Add(dfi4.Query());

            foreach (string condition in conditionList)
            {
                if (!string.IsNullOrEmpty(condition))
                {
                    strConditions += condition;
                }
            }

            return strConditions;
        }
    }
}
