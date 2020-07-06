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
        public static PushButton PushButton { get; set; }
        public static bool Subscribed { get; private set; } = false;
        public static System.IO.FileSystemWatcher FileSystemWatcher { get; private set; }

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            if (PushButton == null)
            {
                TaskDialog.Show("Error", "PushButton is not defined!");
                return Result.Failed;
            }

            if (!Subscribed)
            {
                PushButton.Image = new BitmapImage(Utils.CreateImagePackUri("Bell.16x16.png"));
                PushButton.LargeImage = new BitmapImage(Utils.CreateImagePackUri("Bell.32x32.png"));

                PushButton.ItemText = "Watching";
                Subscribed = true;
            }
            else
            {
                PushButton.Image = new BitmapImage(Utils.CreateImagePackUri("Bell-Off.16x16.png"));
                PushButton.LargeImage = new BitmapImage(Utils.CreateImagePackUri("Bell-Off.32x32.png"));

                PushButton.ItemText = "Not Watching!";
                Subscribed = false;
            }

            return Result.Succeeded;
        }
    }
}
