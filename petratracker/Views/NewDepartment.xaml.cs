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

namespace petratracker.Views
{
    /// <summary>
    /// Interaction logic for NewDepartment.xaml
    /// </summary>
    public partial class NewDepartment : Window
    {
        public NewDepartment()
        {
            InitializeComponent();
        }

        Data.connection db_ops = new Data.connection();


        private bool validate_entry()
        {
             
            bool val = false;
            try
            {
                if (txtName.Text == string.Empty)
                {
                    MessageBox.Show("Please specify the name of the department.");
                    txtName.Focus();
                }
                else if (txtDescription.Text == string.Empty)
                {
                    MessageBox.Show("Please specify the name of the department.");
                    txtDescription.Focus();
                }
                else
                {
                    val = true;
                }
            }
            catch(Exception)
            {
                //Log Error 
            }

            return val;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (validate_entry())
            {
                db_ops.executeCmd("insert into tbl_departments (name,description,created_at) values ('"+txtName.Text+"','"+txtDescription.Text+"',NOW())");
            }
        }



    }
}
