using FileReading.Windows;
using System.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileReading
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[]? bytes;
        DynamicDictionary<string, DataType> types;

        public MainWindow()
        {
            types = new DynamicDictionary<string, DataType>();

            InitializeComponent();

            DataTypeView.ItemsSource = types;
            

            FillByteViewer(20, 16);
            byteStack.MouseWheel += (_, e) => 
            {
                byteScrollbar.Value -= e.Delta / 120;
                if(bytes is not null)
                    SetByteViewerContent((int)byteScrollbar.Value);
            };
            byteScrollbar.MouseWheel += (_, e) =>
            {
                byteScrollbar.Value -= e.Delta / 120;
                if (bytes is not null)
                    SetByteViewerContent((int)byteScrollbar.Value);
            };
            byteScrollbar.Scroll += (_, args) => SetByteViewerContent((int)args.NewValue);
        }

        private void modelButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            
            if (openFileDialog.ShowDialog() == true)
            {
                modelButton.Content = openFileDialog.FileName.Split('\\').Last();
                using var stream = openFileDialog.OpenFile();
                bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                SetByteViewerContent(0);
                byteScrollbar.Value = 0;
                byteScrollbar.Maximum = bytes.Length/16;
            }
        }
        private void SetByteViewerContent(int row)
        {
            int i = row * 16;
            foreach(Label label in byteStack.Children)
            {
                if (i >= bytes.Length)
                {
                    label.Content = null;

                }
                else
                {
                    label.Content = bytes[i];
                }
                i++;
            }
        }
        private void FillByteViewer(int rows, int columns)
        {
            byteStack.RowDefinitions.Clear();
            byteStack.ColumnDefinitions.Clear();


            for (int i = 0; i < rows; i++)
                byteStack.RowDefinitions.Add(new RowDefinition() { });

            for (int i = 0; i < columns; i++)
                byteStack.ColumnDefinitions.Add(new ColumnDefinition() { });

            for (int i = 0; i < rows; i++)
            {
                
                for(int j = 0; j < columns; j++)
                {
                    var label = new Label()
                    {
                        Padding = new Thickness(0),
                        Margin = new Thickness(0),
                        Content = 0,
                        ContentStringFormat = "X2",
                    };
                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    byteStack.Children.Add(label);
                }
            }
            byteScrollbar.Maximum = 0;
        }

        private void DataTypeView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var view = sender as ListView;

            if(view!.SelectedItem is not null && view.SelectedItem is DataType type)
            {
                var designWindow = new DesignTypeWindow(type);

                designWindow.ShowDialog();
            }
        }
    }
}
