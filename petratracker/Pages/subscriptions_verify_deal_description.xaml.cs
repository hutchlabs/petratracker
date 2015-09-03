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

using petratracker.Models;
using MahApps.Metro.Controls;
using petratracker.Utility;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic 
    /// </summary>
    public partial class subscriptions_verify_deal_description : Window
    {
        private int subID = -1;
        private bool isUpdate = false;

        public subscriptions_verify_deal_description(int subID,bool isUpdate = false)
        {
            this.DataContext = this;
            this.isUpdate = isUpdate;
            this.subID = subID;
            InitializeComponent();
        }

        public IEnumerable<ComboBoxPairs> ContributionTypes
        {
            private set { ; }
            get { return TrackerSchedule.GetContributionTypes(); }
        }

        private string[] get_years(int start_year, int end_year)
        {
            int diff = end_year - start_year;
            string[] ini_year = new string[diff+1];
            for (int ini = 0; ini <= diff; ini++)
            {
                ini_year[ini] = (start_year + ini).ToString();
            }
            return ini_year;
        }
       
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmb_period_year.ItemsSource = get_years(2008, DateTime.Now.Year + 5);
            cmb_period_year.SelectedIndex = (DateTime.Now.Year - 2008);
            if (isUpdate) { load_deal_desc(); }
        }

        private bool insert_deal_desc(int subID)
        {
            PDealDescription newDealDesc = new PDealDescription();
            newDealDesc.payment_id = subID;
            newDealDesc.month = cmb_period_month.SelectedIndex + 1;
            newDealDesc.year = int.Parse(cmb_period_year.Text);
            newDealDesc.contribution_type = cbxDealDescription.Text;
            newDealDesc.contribution_type_id = int.Parse(((ComboBoxPairs)cbxDealDescription.SelectedItem)._Key);
            newDealDesc.owner = TrackerUser.GetCurrentUser().id;
            newDealDesc.created_at = DateTime.Now;
            Database.Tracker.PDealDescriptions.InsertOnSubmit(newDealDesc);
            Database.Tracker.SubmitChanges();
            return true;
        }

        private bool update_deal_desc(int dealDescID)
        {
            var dealDesc = from pdd in Database.Tracker.PDealDescriptions
                               where pdd.id == dealDescID
                               select pdd;

            foreach (PDealDescription pdd in dealDesc)
            {
                pdd.month = cmb_period_month.SelectedIndex + 1;
                pdd.year = int.Parse(cmb_period_year.Text);
                pdd.contribution_type = ((ComboBoxPairs)cbxDealDescription.SelectedItem)._Value;
                pdd.contribution_type_id = int.Parse(((ComboBoxPairs)cbxDealDescription.SelectedItem)._Key);
                pdd.modified_by = TrackerUser.GetCurrentUser().id;
                pdd.updated_at = DateTime.Now;
                Database.Tracker.SubmitChanges();
            }
            return true;
        }

        private void load_deal_desc()
        {
            var payment_deal_deacription = (from pdd in Database.Tracker.PDealDescriptions
                                             where pdd.id == subID
                                             select pdd);

            foreach (PDealDescription pdd in payment_deal_deacription)
            {
                cmb_period_month.SelectedIndex = (pdd.month - 1);
                cmb_period_year.SelectedItem = pdd.year;
                cbxDealDescription.SelectedItem = pdd.contribution_type;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!isUpdate) { if (insert_deal_desc(subID)) { this.Close(); } }
            else { if (update_deal_desc(subID)) { this.Close(); } }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



    }
}
