﻿using System;
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
    public class TransactionDataModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string PinYin { get; set; }
    }
}
