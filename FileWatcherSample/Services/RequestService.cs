using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatcherSample.Services
{
    /// <summary>
    /// Collection of all requests which are support by this plugin
    /// </summary>
    public enum RequestId : int
    {
        None = 0,
        ShowDialog = 1,
    }

    public class RequestService
    {
        #region Field
        private int _request;
        #endregion

        #region Method
        /// <summary>
        /// Set new RequestId to be executed
        /// </summary>
        /// <param name="requestId"></param>
        public void Make(RequestId requestId)
        {
            Interlocked.Exchange(ref _request, (int)requestId);
        }

        /// <summary>
        /// Get active RequestId and reset it to 0
        /// </summary>
        /// <returns></returns>
        public RequestId Take()
        {
            return (RequestId)Interlocked.Exchange(ref _request, 0);
        }
        #endregion
    }
}
