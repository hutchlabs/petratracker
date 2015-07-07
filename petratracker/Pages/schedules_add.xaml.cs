using MahApps.Metro.Controls;
using petratracker.Models;
using petratracker.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace petratracker.Pages
{
    public partial class AddSchedule : UserControl, INotifyPropertyChanged
    {
        #region Private Members

        private readonly string[] _tiers = { "Tier 2", "Tier 3", "Tier 4" };
        private bool _loadedInFlyout = false;
        private string _textNotEmptyProperty;

        #endregion

        #region Public Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        public string TextNotEmptyProperty
        {
            get { return this._textNotEmptyProperty; }
            set
            {
                if (Equals(value, _textNotEmptyProperty))
                {
                    return;
                }

                _textNotEmptyProperty = value;
                RaisePropertyChanged("TextNotEmptyProperty");
            }
        }

        public string[] Tiers
        {
            private set { ; }
            get { return _tiers; }
        }

        public IEnumerable<ComboBoxPairs> Companies
        {
            private set { ; }
            get { return TrackerSchedule.GetCompanies(); }
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

        public AddSchedule(bool inFlyout = false)
        {
            this.DataContext = this;
            InitializeComponent();
            _loadedInFlyout = inFlyout;
            cbx_schedules.ItemsSource = TrackerSchedule.GetCBSchedules();
        }

        #endregion

        #region Event Handlers

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
            string company_id = ((ComboBoxPairs)cbx_companies.SelectedItem)._Key;
            string tier = (cbx_tiers.SelectedIndex==-1) ? "" : cbx_tiers.SelectedValue.ToString();
            cbx_schedules.ItemsSource = TrackerSchedule.GetCBSchedules(company_id, tier);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (valid_entries())
                {
                    int parent_id = (chx_reval.IsChecked == true) ? int.Parse(((ComboBoxPairs)cbx_schedules.SelectedItem)._Key) : 0;
                    TrackerSchedule.AddSchedule(((ComboBoxPairs)cbx_companies.SelectedItem)._Value,
                                                ((ComboBoxPairs)cbx_companies.SelectedItem)._Key,
                                                cbx_tiers.SelectedValue.ToString(),
                                                ((ComboBoxPairs)cbx_contributiontype.SelectedItem)._Value,
                                                cbx_month.SelectedValue.ToString(),
                                                ((ComboBoxPairs)cbx_year.SelectedItem)._Value,
                                                parent_id);
                    if (_loadedInFlyout)
                        close_flyout();
                }
            }
            catch (Exception ex)
            {
                Utility.LogUtil.LogError("AddSchedule", "btnSave_Click", ex);
                MessageBox.Show("Could not create schedule: " + ex.GetBaseException().ToString(), "System Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (_loadedInFlyout)
                close_flyout();
        }

        #endregion

        #region Private Helper Methods

        private bool valid_entries()
        {
            bool cont = false;

            if ( (cbx_companies.SelectedIndex == -1) ||
                 (cbx_tiers.SelectedIndex == -1) ||
                 (cbx_contributiontype.SelectedIndex == -1) ||
                 (cbx_month.SelectedIndex == -1) ||
                 (cbx_year.SelectedIndex == -1))
            {
                MessageBox.Show("Please fill out all the fields.");
            }
            else
            {
                cont = true;
            }

            return cont;
        }

        private void close_flyout()
        {
            Window parentWindow = Window.GetWindow(this);
            object obj = parentWindow.FindName("surrogateFlyout");
            Flyout flyout = (Flyout)obj;
            flyout.Content = null;
            flyout.IsOpen = !flyout.IsOpen;
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "TextNotEmptyProperty" && this.TextNotEmptyProperty.Equals(string.Empty))
                {
                    return "Field cannot be empty. Please enter a value";
                }
                return null;
            }
        }

        #endregion
    }
}
