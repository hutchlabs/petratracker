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
using petratracker.Models;
using petratracker.Utility;


namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for selectCompany.xaml
    /// </summary>
    public partial class selectCompany : Window
    {
        public selectCompany()
        {
            this.DataContext = this;
            InitializeComponent();
   
        }

        public IEnumerable<ComboBoxPairs> Companies
        {
            private set { ; }
            get { return TrackerSchedule.GetCompanies(); }
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
