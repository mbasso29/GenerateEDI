using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        #region Click classes
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

        #endregion

        #region File Menu Classes
        private void preferencesMenuItem_Click(object sender, RoutedEventArgs e)
        {

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

        private void cancelBtnClmInfoTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnSrvLineTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnAddtlInfoTab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnVerifyEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        private void cancelBtnGenerateEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowExitConfirmationMessageBox_Click(sender, e);
        }

        #endregion

        #region Done Button
        private void doneBtn_Click(object sender, RoutedEventArgs e)
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

        private void nextBtnClaimInfoTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnSrvLineTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnAddtlInfoTab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnVerifyEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        private void nextBtnGenerateEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowNextTab_Click(sender, e);
        }

        #endregion

        #region Back Buttons
        private void backBtnEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnClaimInfoTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnSrvLineTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnAddtlInfoTab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnVerifyEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        private void backBtnGenerateEDITab_Click(object sender, RoutedEventArgs e)
        {
            ShowPreviousTab_Click(sender, e);
        }

        #endregion

        #region Browse

        public void browseBtn_Click(object sender, RoutedEventArgs e)
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
            else if (ediOptions.SelectedItem == item834)
            {
                dlg.DefaultExt = "834";
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
            else if (ediOptions.SelectedItem == item278)
            {
                dlg.DefaultExt = "278";
            }
            else if (ediOptions.SelectedItem == item997)
            {
                dlg.DefaultExt = "997";
            }
            else
            {
                dlg.DefaultExt = "txt";
            }

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filenameTextBox.Text = dlg.FileName;
            }
        }

        #endregion

        #region Restrict to numeric code

        private void clmAmtTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void pmtAmtTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void trsfrAmtTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void pmtAmtTxtBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void adjAmtTxtBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void trsfrAmtTxtBox1_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        #endregion
    }
}
