using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FileWatcherSample.Services
{
    public interface IDialogService
    {
        void ShowMessage(string title, string message);
        void ShowErrorMessage(string message);
        void ShowSucessMessage(string message);
        string OpenFolderDialog(string defaultDirectory, Window owner);
    }
}
