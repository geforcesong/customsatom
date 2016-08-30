using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProTemplate.Web.DataCrawler.Data
{
    public class LandingNetInfo
    {
        // input
        public string Conveyance { get; set; }
        public string VoyageNumber { get; set; }
        public string BillNumber { get; set; }
        //output
        public string ConveyanceOnline { get; set; }
        public string VoyageNumberOnline { get; set; }
        public decimal GrossWeightOnline { get; set; }
        public decimal PackageAmountOnline { get; set; }
        public int OnlineContainerCount { get; set; }
        public string OnlineContainerNumber { get; set; }

        public LandingNetInfo()
        {
            OnlineContainerCount = 0;
            OnlineContainerNumber = "";
        }
    }
}