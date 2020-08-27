using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
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
using VectorThing.Utility;

namespace VectorThing
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

        public void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if (Toolbox == null) return;
            if (Toolbox.IsVisible)
            {
                Toolbox.Visibility = Visibility.Hidden;
            }
            else
            {
                Toolbox.Visibility = Visibility.Visible;
            }
        }

        public void ColorCheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if (ColorPickerPanel == null) return;
            if (ColorPickerPanel.IsVisible)
            {
                ColorPickerPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                ColorPickerPanel.Visibility = Visibility.Visible;
            }
        }
    }
}
