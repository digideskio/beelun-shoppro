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

using HappyDog.SL.Common;

namespace HappyDog.SL.Content
{
    public partial class ResetPasswordWindow : ChildWindow
    {
        public string NewPassword { get; private set; }

        public ResetPasswordWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string text1 = this.Password1.Password;
            string text2 = this.Password2.Password;
            if (text1 == text2)
            {
                this.NewPassword = text1;
                this.DialogResult = true;
            }
            else
            {
                ShopproHelper.ShowMessageWindow(this, "Error", "The passwords you entered did not match.", false);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

