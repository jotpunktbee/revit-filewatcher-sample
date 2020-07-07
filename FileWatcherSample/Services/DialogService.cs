using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace FileWatcherSample.Services
{
    public class DialogService : IDialogService
    {
        #region Method
        public void ShowErrorMessage(string message)
        {
            ShowMessage("Error", message);
        }

        public void ShowMessage(string title, string message)
        {
            Autodesk.Revit.UI.TaskDialog.Show(title, message);
        }

        public void ShowSucessMessage(string message)
        {
            Autodesk.Revit.UI.TaskDialog.Show("Success", message);
        }

        public string OpenFolderDialog(string defaultDirectory, Window owner)
        {
            if (defaultDirectory == null)
                defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

            CommonOpenFileDialog commonOpenFileDialog = new CommonOpenFileDialog()
            {
                InitialDirectory = defaultDirectory,
                IsFolderPicker = true,
            };

            if (commonOpenFileDialog.ShowDialog(owner ?? null) == CommonFileDialogResult.Ok)
                return commonOpenFileDialog.FileName;

            return null;
        }
        #endregion
    }
}
