using System.Web.UI;
using System.Web.UI.WebControls;

namespace Utility
{
    public static class Clear
    {
        public static  void ClearDependents(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ClearDependents(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = string.Empty;
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            {
                                var srcctrl = (DropDownList)c;
                                if (srcctrl.Items.Count>0)
                                ((DropDownList)c).SelectedIndex = 0;
                            }
                            break;

                    }
                }
            }
        }
    }
}