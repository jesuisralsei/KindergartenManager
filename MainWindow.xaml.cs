using System.Windows;
using KindergartenCRUD.Pages;

namespace KindergartenCRUD
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RootFrame.Navigate(new LoginPage());
        }
    }
}
