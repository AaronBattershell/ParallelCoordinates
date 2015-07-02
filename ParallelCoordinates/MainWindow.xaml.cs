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
using System.IO;
using DataReader;

namespace ParallelCordinates
{
    public partial class MainWindow : Window
    {
        private List<DataEntry> UserData;
        string VisualizationMethod;

        private void FileBrowserBtn(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog fileSelector = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            fileSelector.DefaultExt = ".xlsx";
            fileSelector.Filter = "Excel or Text Files|*.xlsx;*.txt|All File Types|*.*";

            // Get the selected file name and display in a TextBox 
            if (fileSelector.ShowDialog() ?? false)
            {
                try
                {
                    UserData = new ExcelReader(fileSelector.FileName).ds;
                    AverageLineWidthTxtBx.Text = ((int)(UserData.Count*0.5 + 0.5)).ToString();
                    DataInterpretationBtn.IsEnabled = true;
                    LaunchBtn.IsEnabled = true;
                    FileNameLbl.Content = fileSelector.FileName.Substring(fileSelector.FileName.LastIndexOf("\\") + 1);
                }
                catch (IOException)
                {
                    FileNameLbl.Content = "Cannot open file (currently in use)";
                    DataInterpretationBtn.IsEnabled = false;
                    LaunchBtn.IsEnabled = false;
                }
                catch 
                {
                    FileNameLbl.Content = "Error parsing data";
                    DataInterpretationBtn.IsEnabled = false;
                    LaunchBtn.IsEnabled = false;
                }
            }
        }

        private void DisplayData(object sender, RoutedEventArgs e)
        {
            List<DataEntry> minimizedData = new List<DataEntry>();
            for (int i = 0; i < UserData.Count; ++i)
            {

                minimizedData.Add(new DataEntry(UserData[i].ColumnName));
                for (int j = 0; j < 20 && j < UserData[i].Data.Count; ++j)
                {
                    minimizedData[i].Data.Add(UserData[i].Data[j]);
                }
            }

            DataView page = new DataView(minimizedData, UserData[0].Data.Count);
            
            if (!page.BrokenView)
            {
                page.Show();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var visualizationRadioBtn = (RadioButton)sender;
            VisualizationMethod = (string)visualizationRadioBtn.Content;
        }

        private void LaunchBtn_Click(object sender, RoutedEventArgs e)
        {
            ParallelCoordinates2D page = new ParallelCoordinates2D(UserData, Int32.Parse(MinColumnWidthTxtBx.Text), Int32.Parse(BeginNumericAproxTxtBx.Text), Int32.Parse(MaxUniqueEntriesTxtBx.Text), Int32.Parse(AverageLineWidthTxtBx.Text), LineWidthVarianceSlider.Value, FilterTxtBx.IsChecked ?? false);
            page.Show();

            // Code remnants from when there could have been multiple visulization methods
            // This is being kept in the far fetched hopes that might actually happen
            //if (VisualizationMethod == "Parallel Coordinates in 2D")
            //{
            //    ParallelCoordinates2D page = new ParallelCoordinates2D(UserData, Int32.Parse(MinColumnWidthTxtBx.Text), Int32.Parse(BeginNumericAproxTxtBx.Text), Int32.Parse(MaxUniqueEntriesTxtBx.Text), (bool)FilterTxtBx.IsChecked);
            //    page.Show();
            //}
            //else if (VisualizationMethod == "2D in 3D Spiral")
            //{
            //    FileNameLbl.Content = "2D in 3D Spiral";
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Error selecting visualization method.", "Unexpected Error", System.Windows.MessageBoxButton.OK);
            //}
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int val;
            e.Handled = !Int32.TryParse(e.Text, out val); 
        }

        private void ValidateCount(object sender, RoutedEventArgs e)
        {
            // Fill textboxes is empty
            if (BeginNumericAproxTxtBx.Text == "")
            {
                BeginNumericAproxTxtBx.Text = "10";
            }

            if (MaxUniqueEntriesTxtBx.Text == "")
            {
                MaxUniqueEntriesTxtBx.Text = "25";
            }

            if (MinColumnWidthTxtBx.Text == "")
            {
                MinColumnWidthTxtBx.Text = "300";
            }

            if (AverageLineWidthTxtBx.Text == "")
            {
                MinColumnWidthTxtBx.Text = "50";
            }

            // Ensure NumericAproximation is not greater than total unique
            BeginNumericAproxTxtBx.Text = (Int32.Parse(BeginNumericAproxTxtBx.Text) > Int32.Parse(MaxUniqueEntriesTxtBx.Text) ? MaxUniqueEntriesTxtBx.Text : BeginNumericAproxTxtBx.Text);
        }
    }
}
