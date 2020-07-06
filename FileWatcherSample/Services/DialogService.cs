using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWatcherSample.Services
{
    public class DialogService : IDialogService
    {
        #region Field
        #endregion

        #region Property
        #endregion

        #region Constructor
        #endregion

        #region Method
        public void ShowErrorMessage(string message)
        {
            ShowMessage("Error", message);
        }

        public void ShowMessage(string title, string message)
        {
            TaskDialog.Show(title, message);
        }

        public void ShowSucessMessage(string message)
        {
            TaskDialog.Show("Success", message);
        }
        #endregion
    }
}
