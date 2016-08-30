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

namespace ProTemplate.Utility.PageObjects
{
    public class SearchConditionObject
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SearchType SearchType { get; set; }
        public string DeclarationCodes { get; set; }

        public SearchConditionObject()
        {
            StartDate = DateTime.Now.Date;
            EndDate = DateTime.Now.Date.AddDays(2).AddSeconds(-1);
            SearchType = Utility.SearchType.SearchByDate;
            DeclarationCodes = "";
        }
    }
}
