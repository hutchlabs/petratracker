using MahApps.Metro.Controls;
using petratracker.Models;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace petratracker.Pages
{
    public partial class AddSubscription : UserControl
    {
        #region Private Members

        private bool _loadedInFlyout = false;
        private Microsoft.Win32.OpenFileDialog _dlg = new Microsoft.Win32.OpenFileDialog();

        #endregion

        #region Constructor

        public AddSubscription(bool inFlyout=false)
        {
            InitializeComponent();
            _loadedInFlyout = inFlyout;
        }

        #endregion

        #region Add Subscriptions Events
        
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_loadedInFlyout)
                close_flyout();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            _dlg.DefaultExt = ".xls";
            _dlg.Filter = "Excel files |*.xlsx;*.xls";
            _dlg.Title = "Please select the payments file to process";

            Nullable<bool> result = _dlg.ShowDialog();

            if (result == true)
            {
                txtfileLocation.Text = _dlg.SafeFileName;
            }
        }

        private void btnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            if (TrackerPayment.read_microgen_data(_dlg.FileName, cmbDealType.Text, txtDealDescription.Text,cmb_tier.Text))
             {
                 MessageBox.Show("File upload Successfully", "Upload Complete", MessageBoxButton.OK, MessageBoxImage.Information);
                 
                if (_loadedInFlyout)
                     close_flyout();
             }
             else
             {
                 MessageBox.Show("File upload was not successful, please use a valid file format.", "Upload Failure", MessageBoxButton.OK, MessageBoxImage.Exclamation);
             }
        }
        #endregion

        #region Private Helpers
        
        private void close_flyout()
        {
            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.Content = null;
            flyout.IsOpen = !flyout.IsOpen;
        }

        #endregion
    }
}
