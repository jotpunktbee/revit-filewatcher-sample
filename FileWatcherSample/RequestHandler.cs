using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.UI;
using FileWatcherSample.Services;

namespace FileWatcherSample
{
    public class RequestHandler : IExternalEventHandler
    {
        #region Field
        private RequestService _requestService = new RequestService();
        #endregion

        #region Property
        public RequestService RequestService
        {
            get
            {
                return _requestService;
            }
        }
        #endregion

        #region Constructor
        #endregion

        #region Method
        public void Execute(UIApplication uiapp)
        {
            try
            {
                switch (RequestService.Take())
                {
                    case RequestId.None:
                        break;
                    case RequestId.MoveElements:
                        MainCommand.MoveByXyz(uiapp);
                        break;
                }
            }
            catch (Exception ex)
            {
                App.DialogService.ShowErrorMessage(ex.Message);
            }
        }

        public string GetName()
        {
            return "FileWatcherSampleEventHandler";
        }
        #endregion
    }
}
