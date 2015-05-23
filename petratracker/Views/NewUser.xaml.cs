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

namespace petratracker
{
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        public NewUser()
        {
            InitializeComponent();
        }

        Data.connection db_ops = new Data.connection();


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
          if(createUser())
          {
              this.Close();
          }
        }

        private bool validate_entries()
        {
            bool cont = false;
            if (txtFirstName.Text == string.Empty)
            {
                MessageBox.Show("Please provide the first name of the user.");
                txtFirstName.Focus();
            }
            else if(txtLastName.Text == string.Empty)
            {
                MessageBox.Show("Please provide the last name of the user.");
                txtLastName.Focus();
            }
            else if(txtEmail.Text == string.Empty)
            {
                MessageBox.Show("Pleaseprovide the email of the user.");
                txtEmail.Focus();
            }
            else if(txtPassword.Text == string.Empty)
            {
                MessageBox.Show("Please provide the password of the user.");
                txtFirstName.Focus();
            }
            else if (cmbDept.Text == string.Empty)
            {
                MessageBox.Show("Please select the department.");
            }
            else if (cmbUserRole.Text == string.Empty)
            {
                MessageBox.Show("Please select the role of the user.");
            }
            else
            {
                cont = true;
            }

            return cont;
        }

        private bool createUser()
        {
            bool cont = false;

            try
            {        
                if (validate_entries())
                {
                    string cmd = "call create_user('" + txtFirstName.Text + "','" + txtLastName.Text + "','" + cmbDept.Text + "','" + txtEmail.Text + "','" + txtPassword.Text + "','" + cmbUserRole.Text + "')";
                    if (db_ops.executeCmd(cmd))
                    {
                        cont = true;
                    }
                }
            }
            catch(Exception)
            {
                //Log Error
            }
            return cont;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        
        private void load_data()
        {
            try
            {
                string getDepartments = "select id,name from tbl_departments";
                db_ops.executeCmdToCombo(getDepartments, cmbDept, "name");

                string getUserRoles = "select name from tbl_roles";
                db_ops.executeCmdToCombo(getUserRoles, cmbUserRole, "name");

            }
            catch(Exception)
            {
                //Log Error
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            load_data();
        }
    }
}
