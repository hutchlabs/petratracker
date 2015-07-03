using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

using petratracker.Models;

namespace petratracker.UserControls
{
    public partial class PaymentsSubscriptions : UserControl
    {
        #region Private Members 

        private Microsoft.Win32.OpenFileDialog _dlg = new Microsoft.Win32.OpenFileDialog();

        #endregion

        #region Constructor

        public PaymentsSubscriptions()
        {
            InitializeComponent();

            try
            {
                viewSubscriptionsJobs.ItemsSource = TrackerJobs.GetJobs();
            }
            catch (Exception jobsErr)
            {
                MessageBox.Show(jobsErr.Message);
            }
        }

        #endregion

        #region Event Handlers

        #region View Schedules Events
        private void viewJobs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Job selVal = (Job) viewSubscriptionsJobs.SelectedItem;
            //Code.ActiveScript.job_id = selVal.id;
            //subscriptions openSubs = new subscriptions();
            //this.NavigationService.Navigate(new Uri("pages/subscriptions.xaml", UriKind.Relative));
        }


        private void viewJobs_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string[] hiddenHeaders = { "id", "updated_at", "job_type", "approved_by", "modified_by", "created_at", "User", "owner" };

            if (hiddenHeaders.Contains(e.Column.Header.ToString()))
            {
                e.Cancel = true;
            }

            if (e.Column.Header.ToString().Equals("job_description"))
            {
                e.Column.Header = "Description";
            }
        }
        #endregion

        #region Add Schedules Events
        
        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Set filter for file extension and default file extension 
            _dlg.DefaultExt = ".xls";
            _dlg.Filter = "Text documents (.xls)|*.xls";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = _dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                txtfileLocation.Text = _dlg.SafeFileName;
            }
        }

        private void btnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            Models.TrackerPayment newUpload = new Models.TrackerPayment();
            newUpload.read_microgen_data(_dlg.FileName, cmbDealType.Text, txtDealDescription.Text);
            if (newUpload.isUploaded)
            {
                MessageBox.Show("File upload Successfully", "Upload Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("File upload was not successful, please use a valid file format.", "Upload Failure", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        
        #endregion

        #endregion
    }
}
