using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using MahApps.Metro.Controls;
using petratracker.Models;
using System.IO;

namespace petratracker.Models
{
    public static class ExportToText
    {
        

        public static bool doExport(DataTable dataTable)
        {
            try
            {
                System.Windows.Forms.SaveFileDialog filePath = new System.Windows.Forms.SaveFileDialog();
                filePath.ShowDialog();
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(string.Join("\t", dataTable.Columns.Cast<DataColumn>().Select(arg => arg.ColumnName)));
                foreach (DataRow dataRow in dataTable.Rows)
                stringBuilder.AppendLine(string.Join("\t", dataRow.ItemArray.Select(arg => arg.ToString())));
              

                if (filePath.FileName != String.Empty)
                { 
                    File.WriteAllText(filePath.FileName+".txt", stringBuilder.ToString());
                    MessageBox.Show("Export complete.","Done",MessageBoxButton.OK,MessageBoxImage.Information);
                    return true; 
                }
                else  
                {
                    MessageBox.Show("Export Failed.", "Done", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false; 
                }
     
            }
            catch(Exception)
            { 
                return false; 
            }
        }

        public static DataTable GetDataTableFromDGV(DataGrid dataGrid)
        {
            return  ((DataView)dataGrid.ItemsSource).ToTable();
        }


    }
}
