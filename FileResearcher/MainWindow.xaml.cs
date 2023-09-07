using FileReading.ReadingData.Types;
using FileResearcher.Windows;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileResearcher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
