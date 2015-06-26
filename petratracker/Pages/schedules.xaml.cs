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
using petratracker.Code;
using System.Collections;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for schedules.xaml
    /// </summary>

    public partial class schedules : Page
    {
        TrackerSchedule trackerSH = new TrackerSchedule();
        string[] tiers = { "Tier 1", "Tier 2", "Tier 3", "Tier 4" };

        public schedules()
        {
            InitializeComponent();
            viewSchedules.ItemsSource = trackerSH.GetSchedules();
            cbx_tiers.ItemsSource = tiers;

            var companies = Utils.GetCompanies();
           
            foreach (var c in companies)
            {
                cbx_companies.Items.Add(c.FullName);
            }
        }

        private void viewSchedules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Schedule j = sender as Schedule; 
                viewSchedules.Visibility = Visibility.Collapsed;
                vSPageHolder.NavigationService.Navigate(new ScheduleView(j.id));
                vSPageHolder.Visibility = Visibility.Visible;
            }
            catch (Exception subError)
            {
                vSPageHolder.Visibility = Visibility.Collapsed;
                viewSchedules.Visibility = Visibility.Visible;
                MessageBox.Show(subError.Message + subError.Source + subError.StackTrace);
            }
        }

        private void btnBrowseSchedules_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".xls";
            dlg.Filter = "Text documents (.xls)|*.xls";

            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                //txtfileLocation.Tag = dlg.FileName;
               // txtfileLocation.Text = dlg.SafeFileName;
            }
        }

        private void btnUploadSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               // trackerSH.AddScheduleFromFileUpload(txtfileLocation.Tag.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,"Upload Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
        }

        private void btnAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                trackerSH.AddSchedule(cbx_companies.SelectedValue.ToString(),
                                      cbx_tiers.SelectedValue.ToString(), 
                                      dp_month.SelectedDate.Value);

                viewSchedules.ItemsSource = trackerSH.GetSchedules();
                scheduleMenu.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Upload Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
