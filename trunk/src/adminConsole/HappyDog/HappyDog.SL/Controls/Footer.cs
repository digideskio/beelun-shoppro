using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using HappyDog.SL.EventArguments;

namespace HappyDog.SL.Controls
{
    [TemplateVisualState(Name = StateNormal, GroupName = "CommonStates")]
    [TemplateVisualState(Name = StateRetrievingData, GroupName = "CommonStates")]
    public class Footer : Control
    {
        #region Constants
        private const string StateNormal = "Normal";
        private const string StateRetrievingData = "RetrievingData";
        #endregion

        #region Constructor
        public Footer()
        {
            DefaultStyleKey = typeof(Footer);
        }
        #endregion

        #region Public Functions
        public void ContentPageStateChanged(PageStatusChangeArgs.eStatuses status)
        {
            // TODO: be able to show runtime error/info/warning message
            switch (status)
            {
                case PageStatusChangeArgs.eStatuses.Loading:
                    VisualStateManager.GoToState(this, StateNormal, true);
                    break;
                case PageStatusChangeArgs.eStatuses.Active:
                    VisualStateManager.GoToState(this, StateNormal, true);
                    break;
                case PageStatusChangeArgs.eStatuses.RetrievingData:
                    VisualStateManager.GoToState(this, StateRetrievingData, true);
                    break;
                case PageStatusChangeArgs.eStatuses.Closed:
                    VisualStateManager.GoToState(this, StateNormal, true);
                    break;
                default:
                    VisualStateManager.GoToState(this, StateNormal, true);
                    break;
            }
        }
        #endregion
    }
}
