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

namespace petratracker.UserControls
{
    public partial class Schedules : UserControl
    {
        #region Private Members
        
        private TrackerSchedule _trackerSH = new TrackerSchedule();
        private readonly string[] _tiers = { "Tier 2", "Tier 3", "Tier 4" };
        
        #endregion

        #region Constructor

        public Schedules()
        {
            InitializeComponent();
            viewSchedules.ItemsSource = _trackerSH.GetSchedules();
            cbx_tiers.ItemsSource = _tiers;

            // Set companies
            cbx_companies.DisplayMemberPath = "_Value";
            cbx_companies.SelectedValuePath = "_Key";
            cbx_companies.ItemsSource = _trackerSH.GetCompanies();

            // Set contribution types
            cbx_contributiontype.DisplayMemberPath = "_Value";
            cbx_contributiontype.SelectedValuePath = "_Key";
            cbx_contributiontype.ItemsSource = _trackerSH.GetContributionTypes();

            // Set year combo
            cbx_year.DisplayMemberPath = "_Value";
            cbx_year.SelectedValuePath = "_Key";
            cbx_year.ItemsSource = _trackerSH.GetYears();
        }

        #endregion

        #region Event Handlers

        private void viewSchedules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                /*Schedule j = viewSchedules.SelectedItem as Schedule;
                viewSchedules.Visibility = Visibility.Collapsed;
                vSPageHolder.NavigationService.Navigate(new ScheduleView(j.id));
                vSPageHolder.Visibility = Visibility.Visible;
                 * */
            }
            catch (Exception subError)
            {
                vSPageHolder.Visibility = Visibility.Collapsed;
                viewSchedules.Visibility = Visibility.Visible;
                MessageBox.Show(subError.Message + subError.Source + subError.StackTrace);
            }
        }

        private void btnAddSchedule_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _trackerSH.AddSchedule(((ComboBoxPairs)cbx_companies.SelectedItem)._Value,
                                      ((ComboBoxPairs)cbx_companies.SelectedItem)._Key,
                                      cbx_tiers.SelectedValue.ToString(),
                                      cbx_month.SelectedValue.ToString(),
                                      ((ComboBoxPairs)cbx_year.SelectedItem)._Value);

                viewSchedules.ItemsSource = _trackerSH.GetSchedules();
                InnerSubTabControl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Upload Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

    }
}
