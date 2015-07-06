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
using petratracker.Utility;
using petratracker.Pages;

namespace petratracker.Controls
{
    public partial class Schedules : UserControl
    {
        #region Private Members
        
        private readonly string[] _tiers = { "Tier 2", "Tier 3", "Tier 4" };
        private IEnumerable<ComboBoxPairs> _parentSchedules;

        #endregion

        #region Public Properties

        public string[] Tiers
        {
            private set { ; }
            get { return _tiers; }
        }

        public IEnumerable<Schedule> MySchedules
        {
            private set { ; }
            get { return TrackerSchedule.GetSchedules(); }
        }

        public IEnumerable<ComboBoxPairs> ParentSchedules
        {
            set { _parentSchedules = value;  }
            get { return _parentSchedules;  }
        }

        public IEnumerable<ComboBoxPairs> Companies
        {
            private set { ; }
            get { return TrackerSchedule.GetCompanies();  }
        }

        public IEnumerable<ComboBoxPairs> ContributionTypes
        {
            private set { ; }
            get { return TrackerSchedule.GetContributionTypes(); }
        }

        public IEnumerable<ComboBoxPairs> Years
        {
            private set { ; }
            get { return TrackerSchedule.GetYears(); }
        }

        #endregion

        #region Constructor

        public Schedules()
        {
            InitializeComponent();
            this.DataContext = this;  
        }

        #endregion

        #region Event Handlers
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                _parentSchedules = TrackerSchedule.GetCBSchedules();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Schedules load Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void chx_reval_Checked(object sender, RoutedEventArgs e)
        {
            cbx_schedules.IsEnabled = chx_reval.IsChecked.Value;
            
            if (cbx_schedules.IsEnabled)
            {
                cbx_schedules.SelectedIndex = 0;
            }
        }

        private void cbx_companies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string company_id = ((ComboBoxPairs)cbx_companies.SelectedItem)._Key;
            //cbx_schedules.ItemsSource = TrackerSchedule.GetCBSchedules(company_id);
        }

        private void viewSchedules_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Schedule j = viewSchedules.SelectedItem as Schedule;
                viewSchedules.Visibility = Visibility.Collapsed;
                vSPageHolder.NavigationService.Navigate(new ScheduleView(j.id, this));
                vSPageHolder.Visibility = Visibility.Visible;
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
                int parent_id = (chx_reval.IsChecked == true) ? int.Parse(((ComboBoxPairs)cbx_schedules.SelectedItem)._Key) : 0;
                TrackerSchedule.AddSchedule(((ComboBoxPairs)cbx_companies.SelectedItem)._Value,
                                            ((ComboBoxPairs)cbx_companies.SelectedItem)._Key,
                                            cbx_tiers.SelectedValue.ToString(),
                                            ((ComboBoxPairs)cbx_contributiontype.SelectedItem)._Value,
                                            cbx_month.SelectedValue.ToString(),
                                            ((ComboBoxPairs)cbx_year.SelectedItem)._Value,
                                            parent_id);

                viewSchedules.ItemsSource = TrackerSchedule.GetSchedules();
                InnerSubTabControl.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Add Scchedule Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #endregion

        #region Public Methods

        public void SwitchToGrid()
        {
            vSPageHolder.Visibility = Visibility.Collapsed;
            viewSchedules.Visibility = Visibility.Visible;
        }

        #endregion


    }
}
