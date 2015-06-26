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
using System.Data;

using petratracker.Models;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for schedulesView
    /// </summary>
    public partial class ScheduleView : Page
    {
        private int schedule_id;
        private TrackerSchedule trackerSH = new TrackerSchedule();

        public ScheduleView()
        {
            InitializeComponent();
        }

        public ScheduleView(int id) : this()
        {
            schedule_id = id;
        }
   
        private void load_schedule()
        {
            Schedule s = trackerSH.GetSchedule(this.schedule_id);
            string[] validationStatus = { "Not Validated", "Passed", "SSNIT Number Error", "Name Error", "New Employee" };


            lbl_company.Content = s.company;
            lbl_tier.Content = s.tier;
            
            lbl_owner.Content = string.Format("{0} {1}", s.User.first_name, s.User.last_name);
            lbl_month.Content = s.month.ToString(); //string.Format("{}MMMM, yyyy",s.month);

            ts_validated.IsChecked = s.validated;
            cbx_validationStatus.ItemsSource = validationStatus;
            cbx_validationStatus.SelectedIndex = Array.IndexOf(validationStatus, s.validation_status);

            viewScheduleItems.ItemsSource = trackerSH.GetSchedulesItems(s.id);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_schedule();
        }

        public void viewScheduleItems_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
