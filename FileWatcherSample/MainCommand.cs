using System;
using System.Collections.Generic;
using System.IO;
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
        private static string _lastFile = string.Empty;
        private static string _lastFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);

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
                string selectedFolder = App.DialogService.OpenFolderDialog(_lastFolder, Utils.GetRevitWindow(uiapp));
                if (selectedFolder == null)
                {
                    App.DialogService.ShowErrorMessage("No folder selected!");
                    return Result.Failed;
                }

                _lastFolder = selectedFolder;
                _fileSystemWatcher = new System.IO.FileSystemWatcher(_lastFolder, "*.csv");
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

            PushButton.ItemText = "Not Watching";
            _subscribed = false;
        }

        private void OnChanged(object sender, System.IO.FileSystemEventArgs e)
        {
            MainCommand._lastFile = e.FullPath;
            App.RequestHandler.RequestService.Make(Services.RequestId.MoveElements);
            App.ExternalEvent.Raise();
        }

        private void OnCreated(object sender, System.IO.FileSystemEventArgs e)
        {
            MainCommand._lastFile = e.FullPath;
            App.RequestHandler.RequestService.Make(Services.RequestId.MoveElements);
            App.ExternalEvent.Raise();
        }

        private void RegisterEvents()
        {
            if (_fileSystemWatcher == null)
                throw new ArgumentNullException("_fileSystemWatcher");

            _fileSystemWatcher.Created += OnCreated;
            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.EnableRaisingEvents = true;
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

        public static void MoveByXyz(UIApplication uiapp)
        {
            if (string.IsNullOrEmpty(_lastFile))
                return;

            Dictionary<Element, XYZ> elements = new Dictionary<Element, XYZ>();

            Document doc = uiapp.ActiveUIDocument.Document;
            Utils.ParseCsv(_lastFile, fields =>
            {
                elements.Add(
                    doc.GetElement(fields[0]),
                    new XYZ(double.Parse(fields[1]), double.Parse(fields[2]), int.Parse(fields[3])));
            });

            using (Transaction t = new Transaction(doc))
            {
                t.Start("Move Elements");

                foreach (KeyValuePair<Element, XYZ> element in elements)
                {
                    if (element.Value == null)
                        continue;
                    
                    Utils.MoveElementByTwoPoints((element.Key.Location as LocationPoint).Point, element.Value, element.Key);
                }

                t.Commit();
            }
        }
    }
}
