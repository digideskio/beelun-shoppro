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

using System.Threading;

namespace HappyDog.SL.Content
{
    #region Double click support
    /// <summary>
    /// Double Event handler definition
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DblClickEvent(object sender, MouseEventArgs e);

    /// <summary>
    /// A derived class to support double click
    /// </summary>
    public class AgDataGrid : DevExpress.Windows.Controls.AgDataGrid
    {
        public new void Refresh()
        {
            base.DataController.DoRefresh();
        }

        private static class MouseHelper
        {
            static int Timeout = 500;
            static bool clicked = false;
            static Point position;
            public static bool IsDoubleClick(MouseButtonEventArgs e)
            {
                if (clicked)
                {
                    clicked = false;
                    return position.Equals(e.GetPosition(null));
                }
                clicked = true;
                position = e.GetPosition(null);
                ParameterizedThreadStart threadStart = new ParameterizedThreadStart(ResetThread);
                Thread thread = new Thread(threadStart);
                thread.Start();
                return false;
            }
            private static void ResetThread(object state)
            {
                Thread.Sleep(Timeout);
                clicked = false;
            }
        }

        public event DblClickEvent DblClick;

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.Surface.MouseLeftButtonUp += new MouseButtonEventHandler(Surface_MouseLeftButtonUp);
        }

        void Surface_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseHelper.IsDoubleClick(e))
            {
                if (DblClick != null)
                {
                    DblClick(sender, e);
                }
                e.Handled = true;
            }
        }
    }
    #endregion
}
