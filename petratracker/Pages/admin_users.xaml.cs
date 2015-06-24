using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using petratracker.Models;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for subscriptions.xaml
    /// </summary>
    public partial class AdminUsers : Page
    {
        TrackerDataContext trackerDB = (App.Current as App).TrackerDBo;

        public AdminUsers()
        {
            InitializeComponent();
            viewUsers.ItemsSource = this.GetUsers();
        }

        public IEnumerable<User> GetUsers()
        {
            return (from u in trackerDB.Users select u);
        }

        
        private void viewSubscriptions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            /*try
            {
                verifySubscription openVerification = new verifySubscription();
                Payment selVal = (Payment)viewSubscriptions.SelectedItem;
                openVerification.subID = selVal.id;
                openVerification.ShowDialog();
            }
            catch(Exception subError)
            {
                MessageBox.Show(subError.Message+subError.Source+subError.StackTrace);
            }*/
        }

        private void viewUsers_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            string[] hiddenHeaders = { "password", "id", "role_id", "first_login", "Payments", "Settings", 
                                       "Role", "email2", "email3", "signature", "logged_in", "middle_name" ,
                                       "updated_at","modified_by","created_at", "email1"};

            if (hiddenHeaders.Contains(e.Column.Header.ToString()))
            {
                e.Cancel = true;
            }

            if (e.Column.Header.ToString().Equals("first_name"))
            {
                e.Column.Header = "First Name";
            }

            if (e.Column.Header.ToString().Equals("last_name"))
            {
                e.Column.Header = "Last Name";
            }

            if (e.Column.Header.ToString().Equals("last_login"))
            {
                e.Column.Header = "Last Login";
            }
        }
    }
}
