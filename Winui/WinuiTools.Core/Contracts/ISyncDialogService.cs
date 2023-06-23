using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WinuiTools.Core.Contracts
{
    public interface ISyncDialogService
    {
        public void ShowMessage(string message);
        public void ShowErrorMessage(string message);
    }
}
