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
using System.Windows.Shapes;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for uploadDeal.xaml
    /// </summary>
    public partial class uploadDeal : Page
    {
        public uploadDeal()
        {
            InitializeComponent();
        }

        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            

            // Set filter for file extension and default file extension 

            dlg.DefaultExt = ".xls";
            dlg.Filter = "Text documents (.xls)|*.xls";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                txtfileLocation.Text = dlg.SafeFileName;
            }


        }

        private void btnUploadFile_Click(object sender, RoutedEventArgs e)
        {
            Models.TrackerPayment newUpload = new Models.TrackerPayment();
            newUpload.read_microgen_data(dlg.FileName, cmbDealType.Text, txtDealDescription.Text);
            if (newUpload.isUploaded)
            {
                MessageBox.Show("File upload Successfully", "Upload Complete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("File upload was not successful, please use a valid file format.", "Upload Failure", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cmbDealType.SelectedIndex = 0;
        }




    }
}
