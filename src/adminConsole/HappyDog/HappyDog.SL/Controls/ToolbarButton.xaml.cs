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
using System.Windows.Media.Imaging;

namespace HappyDog.SL.Controls
{
    public partial class ToolbarButton : UserControl
    {
        public bool isSelected = false;

        /// <summary>
        /// ImageSource dependency prop
        /// </summary>
        public string ImageSource
        {
            get { return (string)this.GetValue(ImageSourceProperty); }

            set
            {
                base.SetValue(ImageSourceProperty, (string)value);
                BackgroundImage.SetValue(Image.SourceProperty, new BitmapImage(new Uri((string)value, UriKind.Relative))); // Relative
            }
        }
        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(string), typeof(ToolbarButton), new PropertyMetadata(new PropertyChangedCallback(OnImageSourceValueChanged)));
        public ImageSource ImageSourceObj
        {
            set { BackgroundImage.Source = value; }
            get { return BackgroundImage.Source; }
        }
        public static void OnImageSourceValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Add code here
        }

        /// <summary>
        /// Text dependency prop
        /// </summary>
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ToolbarButton), new PropertyMetadata(new PropertyChangedCallback(OnTextValueChanged)));
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }

            set
            {
                base.SetValue(TextProperty, (string)value);
                ButtonTextBlock.Text = (string)value;
            }
        }
        public static void OnTextValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ToolbarButton btn = d as ToolbarButton;
            btn.ButtonTextBlock.Text = btn.Text;
        }

        /// <summary>
        /// Tag dependency prop
        /// </summary>
        //public static readonly DependencyProperty TagProperty = DependencyProperty.Register("Tag", typeof(string), typeof(ToolbarButton), new PropertyMetadata(new PropertyChangedCallback(OnTagValueChanged)));
        //public string Tag
        //{
        //    get { return (string)this.GetValue(TagProperty); }

        //    set
        //    {
        //        base.SetValue(TagProperty, (string)value);
        //    }
        //}
        //public static void OnTagValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    // Todo
        //}

        #region Constructor
        public ToolbarButton()
        {
            InitializeComponent();
            VerticalAlignment = VerticalAlignment.Top;
            Storyboard.SetTarget(Activate, this);
            Storyboard.SetTarget(Deactivate, this);

            MouseEnter += new MouseEventHandler(ToolbarButton_MouseEnter);
            MouseLeave += new MouseEventHandler(ToolbarButton_MouseLeave);
        }
        #endregion

        void SetToolbarZIndex(int nValue)
        {
            StackPanel panel = this.Parent as StackPanel;
            Toolbar toolbar = panel.Parent as Toolbar;
            toolbar.SetValue(Canvas.ZIndexProperty, nValue);
        }

        void ToolbarButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!isSelected)
            {
                Deactivate.Begin();
                buttonBorder.BorderThickness = new Thickness(0);
                buttonBorderRight.BorderThickness = new Thickness(0);
                buttonBorder.Background = new SolidColorBrush();
                ButtonTextBlock.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        void ToolbarButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!isSelected)
            {
                Activate.Begin();
                buttonBorder.BorderThickness = new Thickness(1, 0, 1, 1);
                buttonBorder.Background = new SolidColorBrush(Colors.Orange);
                buttonBorderRight.BorderThickness = new Thickness(0, 1, 0, 0);
                ButtonTextBlock.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        /// <summary>
        /// Select this button
        /// </summary>
        public void SelectButton()
        {
            this.isSelected = true;
            Activate.Begin();
            buttonBorder.BorderThickness = new Thickness(2, 0, 2, 2);
            buttonBorder.Background = new SolidColorBrush(Colors.Magenta);
            buttonBorder.BorderBrush = new SolidColorBrush(Colors.White);
            buttonBorderRight.BorderThickness = new Thickness(0, 2, 0, 0);
            buttonBorderRight.BorderBrush = new SolidColorBrush(Colors.Gray);
            ButtonTextBlock.Foreground = new SolidColorBrush(Colors.White);
        }

        /// <summary>
        /// Unselect this button
        /// </summary>
        public void Unselect()
        {
            this.isSelected = false;
            Deactivate.Begin();
            buttonBorder.BorderThickness = new Thickness(0);
            buttonBorderRight.BorderThickness = new Thickness(0);
            buttonBorder.Background = new SolidColorBrush();
            ButtonTextBlock.Foreground = new SolidColorBrush(Colors.Black);
        }

        public void Flash()
        {
            this.flash.Begin();
        }
    }
}
