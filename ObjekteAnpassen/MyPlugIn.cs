using System;
using System.AddIn;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interop;
using System.Windows.Threading;
using Aucotec.EngineeringBase.Client.Runtime;

namespace ObjekteAnpassen
{
    /// <summary>
    /// Implements Wizard ObjekteAnpassen
    /// </summary>
    [AddIn("ObjekteAnpassen", Description = "Entwicklungstool", Publisher = "StolzlechnerR")]
    public class MyPlugIn : PlugInWizard
    {
        /// <summary>
        /// Runs the wizard.
        /// </summary>
        /// <param name="myApplication">Application object instance</param>	
        public override void Run(Application myApplication)
        {
            MainWindow frm = new MainWindow();
            frm.DataContext = myApplication.RootObject;

            WindowInteropHelper wih = new WindowInteropHelper(frm);
            wih.Owner = myApplication.ActiveWindow.Handle;
            frm.ShowDialog();

            // Make a synchronously shutdown
            if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
                Dispatcher.CurrentDispatcher.InvokeShutdown();
        }
    }
}

