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
using System.Windows.Shapes;

namespace petratracker.Pages
{
    /// <summary>
    /// Interaction logic for uploadDeal.xaml
    /// </summary>
    public partial class uploadDeal : Window
    {
        public uploadDeal()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();



            // Set filter for file extension and default file extension 

            dlg.DefaultExt = ".xlsx";

            dlg.Filter = "Text documents (.xlsx)|*.xlsx";



            // Display OpenFileDialog by calling ShowDialog method 

            Nullable<bool> result = dlg.ShowDialog();



            // Get the selected file name and display in a TextBox 

            if (result == true)
            {

                // Open document 

                string filename = dlg.FileName;

                txtfileLocation.Text = filename;
               

            }


        }

        private void btnUploadFile_Click(object sender, RoutedEventArgs e)
        {

            Models.Payments openExcel = new Models.Payments();
            openExcel.read_excel(txtfileLocation.Text);
        }




    }
}
