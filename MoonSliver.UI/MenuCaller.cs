using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Native.XQ.SDK;
using Native.XQ.SDK.Event.EventArgs;
using Native.XQ.SDK.Interfaces;

namespace MoonSliver.UI
{
    
    public class MenuCaller : IXQCallMenu
    {
        public static XQAPI api;

        [STAThread]
        public void CallMenu(object sender, XQEventArgs e)
        {
            api = e.XQAPI;
            MainWindow main = new MainWindow();
            main.Show();
        }
    }
}
