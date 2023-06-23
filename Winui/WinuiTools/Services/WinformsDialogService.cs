using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinuiTools.Core.Contracts;

namespace WinuiTools.Services
{
    /// <summary>
    /// This is a rather brutal solution to overcome winui3's inability to show basic modal dialogs boxes, especially at startup while MainWindow still not loaded
    /// For this to work, we are referencing a 64bit version of System.Windows.Forms from C:\Program Files\dotnet\shared\Microsoft.WindowsDesktop.App\7.0.5\System.Windows.Forms.dll
    /// </summary>
    public class WinformsDialogService:ISyncDialogService
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
