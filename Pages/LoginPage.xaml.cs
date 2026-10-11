using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using KindergartenCRUD.Services;

namespace KindergartenCRUD.Pages
{
    public partial class LoginPage : Page
    {
        // Exactly one account exists. There is no registration, no guest mode,
        // and nothing else in the app is reachable without matching both values.
        private const string ValidUsername = "Yasin";
        private const string ValidPassword = "12345678";

        public LoginPage()
        {
            InitializeComponent();
            ApplyLanguage();

            UsernameBox.KeyDown += (s, e) => { if (e.Key == Key.Enter) TryLogin(); };
            PasswordBox.KeyDown += (s, e) => { if (e.Key == Key.Enter) TryLogin(); };
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e) => TryLogin();

        private void TryLogin()
        {
            if (UsernameBox.Text.Trim() == ValidUsername && PasswordBox.Password == ValidPassword)
            {
                ErrorText.Text = string.Empty;
                NavigationService?.Navigate(new DataPage());
            }
            else
            {
                ErrorText.Text = AppLocalization.T("InvalidCredentials");
            }
        }

        private void LangButton_Click(object sender, RoutedEventArgs e)
        {
            AppLocalization.IsArabic = !AppLocalization.IsArabic;
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            FlowDirection = AppLocalization.IsArabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            LangButton.Content = AppLocalization.IsArabic ? "English" : "العربية";
            TitleText.Text = AppLocalization.T("AppTitle");
            UserLabel.Text = AppLocalization.T("Username");
            PassLabel.Text = AppLocalization.T("Password");
            LoginButton.Content = AppLocalization.T("Login");
        }
    }
}
