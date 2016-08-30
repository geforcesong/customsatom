using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProcessorUtilities
{
    public class Information
    {
        public static bool IsDate(string dateTimeData)
        {
            bool bRet = false;
            try
            {
                DateTime.Parse(dateTimeData);
                bRet = true;
            }
            catch
            {
                bRet = false;
            }
            return bRet;
        }

        public static bool IsNumeric(string number)
        {
            bool bRet = false;
            try
            {
                double.Parse(number);
                bRet = true;
            }
            catch
            {
                bRet = false;
            }
            return bRet;
        }
    }
}
