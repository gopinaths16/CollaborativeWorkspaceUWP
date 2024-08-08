using CollaborativeWorkspaceUWP.Models;
using CollaborativeWorkspaceUWP.ViewModels;
using CollaborativeWorkspaceUWP.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CollaborativeWorkspaceUWP.CustomControls.UserControls
{
    public sealed partial class UserOnboardControl : UserControl
    {
        UserOnboardViewModel userOnboardViewModel;

        public UserOnboardControl()
        {
            userOnboardViewModel = new UserOnboardViewModel();
            this.InitializeComponent();
        }

        private void LoginCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ResetFields();
            userOnboardViewModel.IsLoginContext = true;
        }

        private void SignupCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ResetFields();
            userOnboardViewModel.IsLoginContext = false;
        }

        private void UserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (UserName.Text != "" && Password.Password != "")
            {
                if (userOnboardViewModel.IsLoginContext || (!userOnboardViewModel.IsLoginContext && ConfirmPassword.Password != "" && DisplayName.Text != ""))
                {
                    SetEnableStatusForButton(true);
                }
                else
                {
                    SetEnableStatusForButton(false);
                }
            }
            else
            {
                SetEnableStatusForButton(false);
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (UserName.Text != "" && Password.Password != "")
            {
                if(userOnboardViewModel.IsLoginContext || (!userOnboardViewModel.IsLoginContext && ConfirmPassword.Password != "" && DisplayName.Text != ""))
                {
                    SetEnableStatusForButton(true);
                }
                else 
                { 
                    SetEnableStatusForButton(false);
                }
            }
            else
            {
                SetEnableStatusForButton(false);
            }
        }

        public void SetEnableStatusForButton(bool status)
        {
            if (userOnboardViewModel.IsLoginContext)
            {
                LoginButton.IsEnabled = status;
            }
            else
            {
                SignupButton.IsEnabled = status;
            }
        }

        public void ResetFields()
        {
            if(UserName != null)
            {
                UserName.Text = string.Empty;
            }
            if(Password != null)
            {
                Password.Password = string.Empty;
            }
            if(ConfirmPassword != null)
            {
                ConfirmPassword.Password = string.Empty;
            }
            if(DisplayName != null)
            {
                DisplayName.Text = string.Empty;
            }
            if(ErrorMessage != null)
            {
                ErrorMessage.IsOpen = false;
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.Username = UserName.Text;
            user.Password = Password.Password;
            user.DisplayName = DisplayName.Text;
            bool result = userOnboardViewModel.Login(user);
            if(result)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainView), null);
            }
            else
            {
                ErrorMessage.Message = "User does not exist";
                ErrorMessage.IsOpen = true;
            }
        }

        private void SignupButton_Click(object sender, RoutedEventArgs e)
        {
            User user = new User();
            user.Username = UserName.Text;
            user.Password = Password.Password;
            user.DisplayName = DisplayName.Text;
            bool result = userOnboardViewModel.Signup(user);
            if (result)
            {
                Frame rootFrame = Window.Current.Content as Frame;
                rootFrame.Navigate(typeof(MainView), null);
            }
            else
            {
                ErrorMessage.Message = "User already exists";
                ErrorMessage.IsOpen = true;
            }
        }
    }
}
