using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace ChromeChecker
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public static string GetChromeUrl(Process process)
        {
            if (process == null)
                throw new ArgumentNullException("process");

            if (process.MainWindowHandle == IntPtr.Zero)
                return null;

            AutomationElement element = AutomationElement.FromHandle(process.MainWindowHandle);
            if (element == null)
                return null;

            AutomationElementCollection edits5 = element.FindAll(TreeScope.Subtree, new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.Edit));
            AutomationElement edit = edits5[0];
            string url = ((ValuePattern)edit.GetCurrentPattern(ValuePattern.Pattern)).Current.Value as string;
            return url;
        }

        private void checker_Tick(object sender, EventArgs e)
        {
            try
            {
                foreach (Process process in Process.GetProcessesByName("chrome"))
                {
                    string url = GetChromeUrl(process);
                    if (url == null)
                        continue;
                    if (url == "")
                        label1.Text = "Your open page in the browser: " + "none";
                    else
                        label1.Text = "Your open page in the browser: " + url;
                }
            }
            catch
            {
                label1.Text = "Your open page in the browser: " + "none";
            }

        }
    }
}
