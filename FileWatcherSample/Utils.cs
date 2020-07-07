using Autodesk.Revit.DB;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        #endregion
    }
}
