using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using HappyDog.SL.Data;
using HappyDog.SL;
using HappyDog.SL.EventArguments;

namespace HappyDog.SL.Controls
{
    public partial class Toolbar : UserControl
    {
        public bool isMoreBtnClicked = false;
        
        public Toolbar()
        {
            InitializeComponent();
        }
        public UIElementCollection ButtonGroup
        {
            get
            {
                return ButtonStack.Children;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            int buttonCount = ButtonStack.Children.Count;
        }

        public void onMoreBtnClick(object sender, MouseEventArgs e)
        {

            //StackPanel moreStackPanel = ButtonStack.Children[6] as StackPanel;
            //Border panelContainer = ButtonStack.Children[6] as Border;

            if (!isMoreBtnClicked)
            {
                Canvas panelCanvas = ButtonStack.Children[6] as Canvas; // TODO: fix Hard coded index
                TranslateTransform trans = new TranslateTransform();
                trans.X = -100;
                trans.Y = 40;
                panelCanvas.RenderTransform = trans;
                panelCanvas.Visibility = Visibility.Visible;
                isMoreBtnClicked = true;
            }
            else
            {
                Canvas panelCanvas = ButtonStack.Children[6] as Canvas;
                panelCanvas.Visibility = Visibility.Collapsed;
                isMoreBtnClicked = false;
            }
        }

        public void onMoreBtnLeave(object sender, MouseEventArgs e)
        {
            //StackPanel moreStackPanel = ButtonStack.Children[6] as StackPanel;
            //Border panelContainer = ButtonStack.Children[6] as Border;

            //TranslateTransform trans = new TranslateTransform();
            //trans.X = -150;
            //trans.Y = 30;
            //moreStackPanel.Visibility = Visibility.Collapsed;

            //Canvas panelCanvas = ButtonStack.Children[6] as Canvas;
            //panelCanvas.Visibility = Visibility.Collapsed;
        }


    }
}
