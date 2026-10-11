using System;
using System.Windows;
using System.Windows.Controls;
using KindergartenCRUD.Models;
using KindergartenCRUD.Services;

namespace KindergartenCRUD.Pages
{
    public partial class PaymentPage : Page
    {
        private readonly Student _student;

        public PaymentPage(Student student)
        {
            InitializeComponent();
            _student = student;

            Loaded += (s, e) =>
            {
                ApplyLanguage();
                PopulateMonths();
                UpdateAmountForSelectedMonth();
            };
        }

        private void PopulateMonths()
        {
            MonthBox.SelectionChanged -= MonthBox_SelectionChanged;
            MonthBox.Items.Clear();
            for (int m = 1; m <= 12; m++)
                MonthBox.Items.Add(AppLocalization.MonthName(m));

            // Default to the current month purely as a convenience - it still
            // has to be explicitly confirmed (or changed) before Save works.
            MonthBox.SelectedIndex = DateTime.Today.Month - 1;
            MonthBox.SelectionChanged += MonthBox_SelectionChanged;
        }

        private void MonthBox_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            UpdateAmountForSelectedMonth();

        private void UpdateAmountForSelectedMonth()
        {
            if (MonthBox.SelectedIndex < 0)
            {
                AmountBox.Text = string.Empty;
                return;
            }

            int month = MonthBox.SelectedIndex + 1;
            decimal existing = _student.GetMonthPaid(month);
            AmountBox.Text = existing > 0 ? existing.ToString("0.00") : string.Empty;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (MonthBox.SelectedIndex < 0)
            {
                ErrorText.Text = AppLocalization.T("SelectMonth");
                return;
            }
            if (!decimal.TryParse(AmountBox.Text, out decimal amount) || amount < 0)
            {
                ErrorText.Text = AppLocalization.IsArabic ? "قيمة المبلغ غير صحيحة." : "Please enter a valid amount.";
                return;
            }

            int month = MonthBox.SelectedIndex + 1;
            AppServices.Database.UpsertPayment(_student.Id, month, amount);
            _student.SetMonthPaid(month, amount);

            NavigationService?.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();

        private void LangButton_Click(object sender, RoutedEventArgs e)
        {
            AppLocalization.IsArabic = !AppLocalization.IsArabic;
            ApplyLanguage();
            PopulateMonths();
            UpdateAmountForSelectedMonth();
        }

        private void ApplyLanguage()
        {
            FlowDirection = AppLocalization.IsArabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            LangButton.Content = AppLocalization.IsArabic ? "English" : "العربية";
            TitleText.Text = $"{AppLocalization.T("RecordPayment")} — {_student.Name}";
            FeeText.Text = $"{AppLocalization.T("MonthlyFee")}: {_student.MonthlyFee:0.00}";
            MonthLabel.Text = AppLocalization.T("Month");
            AmountLabel.Text = AppLocalization.T("Amount");
            SaveButton.Content = AppLocalization.T("Save");
            CancelButton.Content = AppLocalization.T("Cancel");
        }
    }
}
