using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace CustomsAtomMobileSite.Models
{
    public class ModelBase
    {
        public static string ConnectionString = "";
        static ModelBase()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["CustomsAtomSQLServer"].ConnectionString;
        }
    }
}