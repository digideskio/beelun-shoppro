using System;
using HappyDog.SL.Content;


namespace HappyDog.SL.EventArguments
{
    public class NavigateRequestArgs : EventArgs
    {
        public PageEnum TargetPage { get; set; }
        public object Parameters { get; set; }
        public bool IsIphoneMode { get; set; }
        public bool IsBackToPage { get; set; }

        public NavigateRequestArgs(PageEnum targetPage, object parameters, bool isIphoneMode, bool isBackToPage)
        {
            this.TargetPage = targetPage;
            this.Parameters = parameters;
            this.IsIphoneMode = isIphoneMode;
            this.IsBackToPage = isBackToPage;
        }

        public NavigateRequestArgs(PageEnum targetPage, object parameters)
        {
            this.TargetPage = targetPage;
            this.Parameters = parameters;
            this.IsIphoneMode = false;
            this.IsBackToPage = false;
        }

        public NavigateRequestArgs(object parameters)
        {
            this.TargetPage = PageEnum.NONE;
            this.Parameters = parameters;
            this.IsIphoneMode = false;
            this.IsBackToPage = false;
        }
    }
}
