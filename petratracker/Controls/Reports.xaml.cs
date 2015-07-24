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

        private string[] main_menu_items = new string[7]{ "Payment-Schedule Status","Slack Days","Schedules Status","Schedules Pending","Summary data on clients", "Tranref match", "RM Company update" };

        private string[] sub_menus_1 = { "Overpayment","Underpayment","No schedule","Schedule available"};
        private string[] sub_menus_2 = { "Receipts sent", "Validation", "Files downloaded", "Files loaded", "Reminders sent", "Ticket resolution", "Revalidation" };
        private string[] sub_menus_3 = { "Payment received", "Payment not received", "Ssnit no errors", "Name errors", "New employees", "Passed" };
        private string[] sub_menus_4 = { "Schedules to be validated", "Files to be downloaded", "Receipts to be sent ", "Payments not linked to schedules" };
        private string[] sub_menus_5 = { "Missing payments", "Missing schedules", "Incomplete schedules" };
        private string[] sub_menus_6 = { "", "", "", "" };
        private string[] sub_menus_7 = { "Payments made v schedules received", "Validation status", "Statement status", "Reminder status", "RM responsible", "Client Category" };

        private void add_sub_menus(TreeViewItem main_menu_item, string [] sub_menus)
        {
                foreach (string sub_menu_item in sub_menus)
                {
                    var sub_item = new TreeViewItem() { Header = sub_menu_item };
                    (main_menu_item as TreeViewItem).Items.Add(sub_item);
                }
        }


        private void load_main_menus()
        {
            //Loading main menu items
            treeReportsMenu.Items.Clear();

            foreach (string menu_item in main_menu_items)
            {          
                treeReportsMenu.Items.Add(new TreeViewItem() { Header = menu_item });
            }
            


            //Loading submenu items
            foreach(TreeViewItem main_menu_item in treeReportsMenu.Items)
            {
                if ((string)main_menu_item.Header == "Payment-Schedule Status") { add_sub_menus(main_menu_item, sub_menus_1); }
                else if ((string)main_menu_item.Header == "Slack Days") { add_sub_menus(main_menu_item, sub_menus_2); }
                else if ((string)main_menu_item.Header == "Schedules Status") { add_sub_menus(main_menu_item, sub_menus_3); }
                else if ((string)main_menu_item.Header == "Schedules Pending") { add_sub_menus(main_menu_item, sub_menus_4); }
                else if ((string)main_menu_item.Header == "Summary data on clients") { add_sub_menus(main_menu_item, sub_menus_5); }
                //else if ((string)main_menu_item.Header == "Tranref match") { add_sub_menus(main_menu_item, sub_menus_6); }
                else if ((string)main_menu_item.Header == "RM Company update") { add_sub_menus(main_menu_item, sub_menus_7); }
            }

        }


       
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            load_main_menus();
        }

    }
}
