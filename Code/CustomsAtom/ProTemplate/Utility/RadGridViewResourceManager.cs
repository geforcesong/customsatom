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
using Telerik.Windows.Controls;

namespace ProTemplate.Utility
{
    public class RadGridViewResourceManager : LocalizationManager
    {
        public override string GetStringOverride(string key)
        {
            switch (key)
            {
                //---------------------- RadGridView Group Panel text:
                case "GridViewGroupPanelText":
                    return "拖动列头到这里可以对该列进行分组.";

                //---------------------- RadGridView Filter Dropdown items texts:

                case "GridViewClearFilter":
                    return "清空过滤条件";


                case "GridViewFilterSelectAll":
                    return "选择所有";


                case "GridViewFilterContains":
                    return "包含";


                case "GridViewFilterEndsWith":
                    return "以**结尾";


                case "GridViewFilterIsContainedIn":
                    return "被包含...";


                case "GridViewFilterIsEqualTo":
                    return "等于";


                case "GridViewFilterIsGreaterThan":
                    return "大于";


                case "GridViewFilterIsGreaterThanOrEqualTo":
                    return "大于等于";


                case "GridViewFilterIsLessThan":
                    return "小于";


                case "GridViewFilterIsLessThanOrEqualTo":
                    return "小于等于";


                case "GridViewFilterIsNotEqualTo":
                    return "不等于";


                case "GridViewFilterStartsWith":
                    return "以**开头";


                case "GridViewFilterAnd":
                    return "并且";


                case "GridViewFilter":
                    return "进行过滤";


                case "GridViewFilterShowRowsWithValueThat":
                    return "显示行值";

                case "GridViewFilterIsNotContainedIn":
                    return "不被包含...";
                case "GridViewFilterDoesNotContain":
                    return "不包含";
                case "GridViewFilterOr":
                    return "或";
                    
            }

            return base.GetStringOverride(key);
        }
    }
}
