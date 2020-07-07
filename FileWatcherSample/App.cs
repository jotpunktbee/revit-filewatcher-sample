using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using FileWatcherSample.Services;

namespace FileWatcherSample
{
    public class App : IExternalApplication
    {
        public static App Instance = null;
        public static IDialogService DialogService = new DialogService();
        public static RequestHandler RequestHandler = null;
        public static ExternalEvent ExternalEvent = null;

        private static readonly string _path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public Result OnStartup(UIControlledApplication a)
        {
            App.Instance = this;
            App.RequestHandler = new RequestHandler();
            App.ExternalEvent = ExternalEvent.Create(RequestHandler);

            try
            {
                RibbonPanel ribbonPanel = a.CreateRibbonPanel("FileWatcherSample");
                PushButton pushButton = ribbonPanel.AddItem(
                    new PushButtonData(
                        "ActivateFileWatcherSample",
                        "Not Watching",
                        Path.Combine(_path, "FileWatcherSample.dll"),
                        "FileWatcherSample.MainCommand")) as PushButton;

                if (pushButton != null)
                {
                    pushButton.Image = new BitmapImage(Utils.CreateImagePackUri("Bell-Off.16x16.png"));
                    pushButton.LargeImage = new BitmapImage(Utils.CreateImagePackUri("Bell-Off.32x32.png"));

                    MainCommand.PushButton = pushButton;
                }
            }
            catch (Exception ex)
            {
                DialogService.ShowErrorMessage("Failed to initialize plugin!" + System.Environment.NewLine + ex.Message);
            }

            return Result.Succeeded;
        }

        public Result OnShutdown(UIControlledApplication a)
        {
            App.Instance = null;

            return Result.Succeeded;
        }
    }
}
