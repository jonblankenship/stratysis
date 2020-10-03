using System.Windows;
using Stratysis.Wpf.ViewModels;

namespace Stratysis.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
        }
    }
}
