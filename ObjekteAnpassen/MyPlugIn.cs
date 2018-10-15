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
            try
            {
                MainWindow frm = new MainWindow(myApplication.Selection.FirstOrDefault(), myApplication);
                frm.DataContext = myApplication.RootObject;

                WindowInteropHelper wih = new WindowInteropHelper(frm);
                wih.Owner = myApplication.ActiveWindow.Handle;
                frm.ShowDialog();
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show(e.ToString(), "Fehler");
            }
            finally
            {
                GC.Collect();

                myApplication.Utils.UnloadCache();

                // Make a synchronously shutdown
                if (!AppDomain.CurrentDomain.IsDefaultAppDomain())
                    Dispatcher.CurrentDispatcher.InvokeShutdown();
            }
        }
    }
}

