using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProTemplate.Web.Utility;

namespace ProTemplate.Web
{
    public partial class Default : System.Web.UI.Page
    {
        public string InitParams = "";
        public int ScreenWidth;
        public int ScreenHeight;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadInitParams();
            }
        }

        protected void LoadInitParams()
        {
            InitParams = string.Format("ClientIP={0}", IPMan.GetClientIP(Request));
        }
    }
}