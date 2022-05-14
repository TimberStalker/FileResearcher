using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace FileResearcher.Controls
{
    /// <summary>
    /// Interaction logic for AcceptableTextBox.xaml
    /// </summary>
    public partial class AcceptableTextBox : UserControl
    {
        public static readonly DependencyProperty textProperty = DependencyProperty.Register("Text", typeof(string), typeof(MainWindow));

        public string Text { 
            get => (string)GetValue(textProperty); 
            set
            {
                SetValue(textProperty, value);
                TextBoxControl.Text = value;
            }
        }

        public event Action<string, string>? TextChanged;

        public AcceptableTextBox()
        {
            InitializeComponent();

            TextBoxControl.Text = Text;
        }

        private void TextBoxControl_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    var oldText = Text;
                    Text = TextBoxControl.Text;
                    TextChanged?.Invoke(oldText ?? "", Text);
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                    break;
                case Key.Escape:
                    TextBoxControl.Text = Text;
                    MoveFocus(new TraversalRequest(FocusNavigationDirection.Up));
                    break;
                default:
                    break;
            }
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            var oldText = Text;
            Text = TextBoxControl.Text;
            TextChanged?.Invoke(oldText ?? "", Text);
        }
    }
}
