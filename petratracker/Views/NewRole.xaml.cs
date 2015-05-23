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
    /// Interaction logic for NewRole.xaml
    /// </summary>
    public partial class NewRole : Window
    {
        public NewRole()
        {
            InitializeComponent();
        }

        Data.connection openConn = new Data.connection();


        private bool verify_entires()
        { 
            bool cont = false;
            if (txtName.Text != string.Empty)
            {
                cont = true;
            }
            else
            {
                MessageBox.Show("Please specify the name of the role.");
                txtName.Focus();
            }

            return cont;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (verify_entires())
            {
                string cmd = "insert into tbl_roles (Name,Description) values('"+txtName.Text+"','"+txtDescription.Text+"')";
                openConn.executeCmd(cmd);
                this.Close();
            }
        }
    }
}
