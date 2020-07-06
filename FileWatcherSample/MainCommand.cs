using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace FileWatcherSample
{
    [Transaction(TransactionMode.Manual)]
    public class MainCommand : IExternalCommand
    {
        private static bool _subscribed = false;
        private static System.IO.FileSystemWatcher _fileSystemWatcher = null;

        public static PushButton PushButton { get; set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (PushButton == null)
            {
                App.DialogService.ShowErrorMessage("PushButton is not defined!");
                return Result.Failed;
            }

            if (!_subscribed)
            {
                _fileSystemWatcher = new System.IO.FileSystemWatcher(@"C:\Temp");
                try
                {
                    RegisterEvents();
                }
                catch (Exception ex)
                {
                    App.DialogService.ShowErrorMessage(ex.Message);
                }

                Subscribe();
            }
            else
            {
                try
                {
                    UnregisterEvents();
                }
                catch (Exception ex)
                {
                    App.DialogService.ShowErrorMessage(ex.Message);
                }

                Unsubscribe();
            }

            return Result.Succeeded;
        }

        private static void Subscribe()
        {
            PushButton.Image = new BitmapImage(Utils.CreateImagePackUri("Bell.16x16.png"));
            PushButton.LargeImage = new BitmapImage(Utils.CreateImagePackUri("Bell.32x32.png"));

            PushButton.ItemText = "Watching";
            _subscribed = true;
        }

        private static void Unsubscribe()
        {
            PushButton.Image = new BitmapImage(Utils.CreateImagePackUri("Bell-Off.16x16.png"));
            PushButton.LargeImage = new BitmapImage(Utils.CreateImagePackUri("Bell-Off.32x32.png"));

            PushButton.ItemText = "Not Watching!";
            _subscribed = false;
        }

        private void OnChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            // TODO: Add external event
        }

        private void OnCreated(object sender, System.IO.FileSystemEventArgs e)
        {
            // TODO: Add external event
        }

        private void RegisterEvents()
        {
            if (_fileSystemWatcher == null)
                throw new ArgumentNullException("_fileSystemWatcher");

            _fileSystemWatcher.Created += OnCreated;
            _fileSystemWatcher.Changed += OnChanged;
        }

        private void UnregisterEvents()
        {
            if (_fileSystemWatcher == null)
                return;

            _fileSystemWatcher.Created -= OnCreated;
            _fileSystemWatcher.Changed -= OnChanged;

            _fileSystemWatcher.Dispose();
            _fileSystemWatcher = null;
        }
    }
}
