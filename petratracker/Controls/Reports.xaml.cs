﻿using System;
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

namespace petratracker.Controls
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : UserControl
    {
        public Reports()
        {
            InitializeComponent();
        }

        #region reports menus

        private string[] main_menu_items = new string[7] { "Payment-Schedule Status", "Slack Days", "Schedules Status", "Schedules Pending", "Summary data on clients", "Transref match", "RM Company update" };

        private string[] sub_menus_1 = { "Overpayment", "Underpayment", "No schedule", "Schedule available" };
        private string[] sub_menus_2 = { "Receipts sent", "Validation", "Files downloaded", "Files loaded", "Reminders sent", "Ticket resolution", "Revalidation" };
        private string[] sub_menus_3 = { "Payment received", "Payment not received", "Ssnit no errors", "Name errors", "New employees", "Passed" };
        private string[] sub_menus_4 = { "Schedules to be validated", "Files to be downloaded", "Receipts to be sent", "Payments not linked to schedules" };
        private string[] sub_menus_5 = { "Missing payments", "Missing schedules", "Incomplete schedules" };
        private string[] sub_menus_6 = { "Transref match"};
        private string[] sub_menus_7 = { "RM Company update" };

       

        private void load_query_sub_menus(string[] sub_menus)
        {
            cmbQuery.Items.Clear();
            foreach (string sub_menu_item in sub_menus)
            {
                cmbQuery.Items.Add(new ComboBoxItem() { Content = sub_menu_item });
            }
        }

        private void load_query_menus(string menuContent)
        {
            if ((string)menuContent == "Payment-Schedule Status") { load_query_sub_menus(sub_menus_1); }
            else if ((string)menuContent == "Slack Days") { load_query_sub_menus(sub_menus_2); }
            else if ((string)menuContent == "Schedules Status") { load_query_sub_menus(sub_menus_3); }
            else if ((string)menuContent == "Schedules Pending") { load_query_sub_menus(sub_menus_4); }
            else if ((string)menuContent == "Summary data on clients") { load_query_sub_menus(sub_menus_5); }
            else if ((string)menuContent == "Transref match") { load_query_sub_menus(sub_menus_6); }
            else if ((string)menuContent == "RM Company update") { load_query_sub_menus(sub_menus_7); }

        }


        private void load_main_menus()
        {
            //Loading main menu items
   
            cmbReportType.Items.Clear();

            foreach (string menu_item in main_menu_items)
            {
                cmbReportType.Items.Add(new ComboBoxItem() { Content = menu_item });
            }

        }



        #endregion

        #region private_variables

        int rec_found = 0;

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load_main_menus();
        }

      
        private void cmbReportType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ComboBoxItem id = (ComboBoxItem)cmbReportType.SelectedItem;
                load_query_menus(id.Content.ToString());
            }
            catch(Exception)
            {
            
            }
        }

        private void btn_Query_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch(cmbReportType.Text)
                {
                    case "Payment-Schedule Status":
                        dgReport.ItemsSource = Models.TrackerReports.get_payment_schedule_status_rep(cmbQuery.Text);
                        break;

                    case "Slack Days":
                        dgReport.ItemsSource = Models.TrackerReports.get_Slack_Days(cmbQuery.Text);
                        break;

                    case "Schedules Status":
                        dgReport.ItemsSource = Models.TrackerReports.get_Schedules_Status(cmbQuery.Text);
                        break;

                    case "Schedules Pending":
                        dgReport.ItemsSource = Models.TrackerReports.get_Schedules_Pending(cmbQuery.Text);
                        break;

                    case "Transref match":
                        dgReport.ItemsSource = Models.TrackerReports.get_Trans_Ref_Match();
                        break;

                    case "RM Company update":
                        dgReport.ItemsSource = Models.TrackerReports.get_Trans_Ref_Match();
                        break;

                    default:
                        break;
                }

                lblRecords.Content = dgReport.Items.Count.ToString() + " record(s) found.";
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void btn_Download_Payments_Click(object sender, RoutedEventArgs e)
        {
           

            switch (cmbReportType.Text)
            {
                case "Payment-Schedule Status":
                    Models.ExportToExcel<Models.Tracker_Report_Payment_Schedule_Status> exportPaymentScheduleStatus = new Models.ExportToExcel<Models.Tracker_Report_Payment_Schedule_Status>();
                    exportPaymentScheduleStatus.dataToPrint = dgReport.ItemsSource.Cast<Models.Tracker_Report_Payment_Schedule_Status>().ToList();
                    exportPaymentScheduleStatus.GenerateReport();
                    break;

                case "Slack Days":
                    Models.ExportToExcel<Models.Tracker_Report_Slack_Days> exportSlackDays = new Models.ExportToExcel<Models.Tracker_Report_Slack_Days>();
                    exportSlackDays.dataToPrint = dgReport.ItemsSource.Cast<Models.Tracker_Report_Slack_Days>().ToList();
                    exportSlackDays.GenerateReport();
                    break;

                case "Schedules Status":
                    Models.ExportToExcel<Models.Tracker_Report_Schedule_Status> exportScheduleStatus = new Models.ExportToExcel<Models.Tracker_Report_Schedule_Status>();
                    exportScheduleStatus.dataToPrint = dgReport.ItemsSource.Cast<Models.Tracker_Report_Schedule_Status>().ToList();
                    exportScheduleStatus.GenerateReport();
                    break;


                case "Schedules Pending":
                    Models.ExportToExcel<Models.Tracker_Report_Schedules_Pending> exportSchedulePending = new Models.ExportToExcel<Models.Tracker_Report_Schedules_Pending>();
                    exportSchedulePending.dataToPrint = dgReport.ItemsSource.Cast<Models.Tracker_Report_Schedules_Pending>().ToList();
                    exportSchedulePending.GenerateReport();
                    break;

                case "Summary data on clients":
                    break;

                case "Tranref match":
                    break;

                default:
                    break;
            }

        }
    }
}
