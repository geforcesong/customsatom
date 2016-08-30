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

namespace ProTemplate.Models
{
    public class HSInputModel
    {
        public int ID { get; set; }
        public int DoubleCheckDeclarationId { get; set; }
        public string ControlNumber { get; set; }
        public string HSCode { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }
        public string FirstUnitName { get; set; }
        public string FirstQuantity { get; set; }
        public string SecondUnitName { get; set; }
        public string SecondQuantity { get; set; }
        public string DeclaredUnitName { get; set; }
        public string DeclaredQuantity { get; set; }
        public string TotalAmount { get; set; }
        public string CurrencyName { get; set; }
    }
}
