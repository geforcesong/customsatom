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
    public class HSCodeDictionaryDataModel
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string ManagementName { get; set; }
        public string DeclarationFactor { get; set; }
        public string FirstUnitName { get; set; }
        public string SecondUnitName { get; set; }
        public string ExportRate { get; set; }
        public string DrawbackRate { get; set; }
    }
}
