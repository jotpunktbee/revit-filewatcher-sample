using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

using Microsoft.VisualBasic.FileIO;

namespace FileWatcherSample
{
    public class Utils
    {
        #region Method
        public static Uri CreateImagePackUri(string fileName)
        {
            return new Uri("pack://application:,,,/FileWatcherSample;component/Resources/" + fileName);
        }

        public static void ParseCsv(string file, Action<string[]> action)
        {
            using (TextFieldParser parser = new TextFieldParser(file))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    action(fields);
                }
            }
        }

        public static void MoveElementByTwoPoints(XYZ point1, XYZ point2, Element element)
        {
            if (!(element.Location is LocationPoint locationPoint))
                return;

            XYZ translation = point2 - point1;
            locationPoint.Move(translation);
        }

        public static Window GetRevitWindow(UIApplication uiapp)
        {
            HwndSource hwndSource = HwndSource.FromHwnd(uiapp.MainWindowHandle);
            return hwndSource.RootVisual as Window;
        }
        #endregion
    }
}
