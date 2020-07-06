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
        #endregion
    }
}
