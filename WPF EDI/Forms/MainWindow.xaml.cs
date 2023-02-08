using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPF_EDI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Button classes
        private void ShowExitConfirmationMessageBox_Click(object sender, RoutedEventArgs e)
        {
            //Configure Message Box information
            string message = "Are you sure you want to exit?";
            string caption = "Exit";
            MessageBoxButton buttons = MessageBoxButton.OKCancel;

            //Show message box
            MessageBoxResult result = MessageBox.Show(message, caption, buttons);
            if (result == MessageBoxResult.OK)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else
            {
                e.Handled = true;
            }
        }

        private void ShowNextTab_Click(object sender, RoutedEventArgs e)
        {
            tabItems.SelectedIndex++;
        }

        private void ShowPreviousTab_Click(object sender, RoutedEventArgs e)
        {
            tabItems.SelectedIndex--;
        }

        private void AddNewServiceLine_Click(object sender, RoutedEventArgs e)
        {

        }
        #endregion

        #region File Menu Classes

        private void optionsMenu_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as MenuItem).IsSubmenuOpen = true;
        }

        private void optionsMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as MenuItem).IsSubmenuOpen = false;
        }

        private void preferencesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Preferences();
            dialog.ShowDialog();
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        #endregion

        #region Cancel Buttons
        private void cancelBtnStart_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnFacTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnBPTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnProviderTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnPatientTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnPayerTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnClaimTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnSLTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnVerifyTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnGenerateTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        #endregion

        #region Done Buttons
        private void doneBtnFinishedTab_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        #endregion

        #region Next Buttons
        private void nextBtnStart_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnFacTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnBPTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnProviderTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnPatientTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnPayerTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnClaimTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnSLTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnVerifyTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnGenerateTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        #endregion

        #region Back Buttons
        private void backBtnEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnFacTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnBPTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnProviderTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnPatientTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnPayerTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnClaimTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnSLTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnVerifyTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnGenerateTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }
        #endregion

        #region Browse
        private void browseBtnGenerateTab_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            // dlg.FileName =            //set file name to be [payerid]"."[currentdate]
            if (ediOptions.SelectedItem == item837)
            {
                dlg.DefaultExt = "837";
            }
            else if (ediOptions.SelectedItem == item835)
            {
                dlg.DefaultExt = "835";
            }
            else if (ediOptions.SelectedItem == item820)
            {
                dlg.DefaultExt = "820";
            }
            else if (ediOptions.SelectedItem == item270)
            {
                dlg.DefaultExt = "270";
            }
            else if (ediOptions.SelectedItem == item271)
            {
                dlg.DefaultExt = "271";
            }
            else if (ediOptions.SelectedItem == item276)
            {
                dlg.DefaultExt = "276";
            }
            else if (ediOptions.SelectedItem == item277)
            {
                dlg.DefaultExt = "277";
            }
            else
            {
                dlg.DefaultExt = "txt";
            }

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                fileName.Text = dlg.FileName;
            }
        }
        #endregion
    }

}
