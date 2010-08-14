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
    public partial class NewAdminWindow : ChildWindow
    {
        public NewAdminData NewAdmin { get; private set; }

        public NewAdminWindow()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            string password1 = this.Password1.Password;
            string password2 = this.Password2.Password;

            if (password1 != password2)
            {
                ShopproHelper.ShowMessageWindow(this, "Error", "The passwords you entered did not match.", false);
            }
            else
            {
                string userName = this.Name.Text;
                string userEmail = this.Email.Text;
                this.NewAdmin = new NewAdminData(userName, userEmail, password1);
                this.DialogResult = true;            
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }

    public class NewAdminData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public NewAdminData(string name, string email, string password)
        {
            this.Name = name;
            this.Email = email;
            this.Password = password;
        }
    }
}

