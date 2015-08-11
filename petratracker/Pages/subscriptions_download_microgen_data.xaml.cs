using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using MahApps.Metro.Controls;
using petratracker.Models;



namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for subscriptions_download_microgen_data.xaml
    /// </summary>
    public partial class subscriptions_download_microgen_data : UserControl
    {

        #region Constructor
        public subscriptions_download_microgen_data(bool inFlyout = false)
        {
            InitializeComponent();
            _loadedInFlyout = inFlyout;                 
        }

        #endregion


        #region Private Members

        private bool _loadedInFlyout = false;
        MicroGenExportDataTable rep = new MicroGenExportDataTable();  
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

        private void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                 var subscription = from p in Database.Tracker.PPayments
                                   where p.value_date == dtValueDate.SelectedDate.Value && p.tier == cmb_tier.Text
                                   select p;


               


                 rep.Clear();

                 foreach (PPayment p in subscription)
                 {
                 
                     string [] codes = TrackerPayment.get_microgen_data(p.company_code, p.tier);
                     DataRow newData = rep.NewRow();
                     newData["FundCode"] = codes[0];
                     newData["FundHolderCode"] = codes[1];
                     newData["TransReference"] = p.transaction_ref_no;
                     newData["TransUnitsGrp1"] = p.transaction_amount.ToString();
                     newData["DealCcyPayAmnt"] = p.transaction_amount.ToString();
                     newData["DealCcyDealAmnt"] = p.transaction_amount.ToString();
                     newData["PayCcyPayAmnt"] = p.transaction_amount.ToString();
                     newData["PayCcyDealAmnt"] = p.transaction_amount.ToString();
                     newData["DealDate"] = (string)p.transaction_date.ToString("yyyy-MM-dd");
                     newData["ValueDate"] = (string)p.value_date.ToString("yyyy-MM-dd");
                     newData["BookDate"] = (string)p.value_date.ToString("yyyy-MM-dd");
                     newData["PriceDate"] = (string)p.value_date.ToString("yyyy-MM-dd");
                     rep.Rows.Add(newData);

                 }
                 viewSubs.ItemsSource = rep.DefaultView;
                 
            }
            catch(Exception m)
            {
                MessageBox.Show("No Tier has been selected \n"+m.Message);
            }
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            cmb_tier.SelectedIndex = 0;
            dtValueDate.SelectedDate = DateTime.Now;
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            if (viewSubs.Items.Count > 0) { ExportToText.doExport(rep); } else { MessageBox.Show("No records found in query.", "Download", MessageBoxButton.OK, MessageBoxImage.Error); }         
        }

        #endregion

    }
}
