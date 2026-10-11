using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using KindergartenCRUD.Models;
using KindergartenCRUD.Services;

namespace KindergartenCRUD.Pages
{
    public partial class DataPage : Page
    {
        private readonly ObservableCollection<Student> _students = new();
        private ICollectionView? _view;

        public DataPage()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                ApplyLanguage();
                ReloadFromDatabase();
            };
        }

        // Re-reads everything from SQLite. Called on first load and after any
        // add/edit/delete/payment so the grid always reflects what's stored.
        private void ReloadFromDatabase()
        {
            _students.Clear();
            foreach (var s in AppServices.Database.GetAll())
                _students.Add(s);

            _view = CollectionViewSource.GetDefaultView(_students);
            _view.Filter = FilterPredicate;
            StudentsGrid.ItemsSource = _view;

            RebuildClassFilterOptions();
            UpdateSummary();
        }

        private void RebuildClassFilterOptions()
        {
            ClassFilterBox.SelectionChanged -= ClassFilterBox_SelectionChanged;
            ClassFilterBox.Items.Clear();
            ClassFilterBox.Items.Add(AppLocalization.T("FilterClass"));
            foreach (var c in _students.Select(s => s.ClassName).Distinct().OrderBy(c => c))
                ClassFilterBox.Items.Add(c);
            ClassFilterBox.SelectedIndex = 0;
            ClassFilterBox.SelectionChanged += ClassFilterBox_SelectionChanged;
        }

        private bool FilterPredicate(object obj)
        {
            if (obj is not Student s) return false;

            string search = SearchBox.Text?.Trim() ?? string.Empty;
            bool matchesSearch = string.IsNullOrEmpty(search) ||
                                  s.Name.Contains(search, StringComparison.OrdinalIgnoreCase);

            string? selectedClass = ClassFilterBox.SelectedItem as string;
            bool matchesClass = string.IsNullOrEmpty(selectedClass) ||
                                 selectedClass == AppLocalization.T("FilterClass") ||
                                 s.ClassName == selectedClass;

            return matchesSearch && matchesClass;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _view?.Refresh();
            UpdateSummary();
        }

        private void ClassFilterBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _view?.Refresh();
            UpdateSummary();
        }

        // Small totals bar under the grid, based on whatever is currently visible
        // (i.e. respects the active search/class filter). Filters the strongly-typed
        // _students collection directly rather than casting the ICollectionView,
        // which keeps this a plain generic LINQ call with no ambiguity.
        private void UpdateSummary()
        {
            if (_view == null) return;

            var visible = _students.Where(s => FilterPredicate(s)).ToList();
            decimal collected = visible.Sum(s => s.TotalPaid);
            decimal remaining = visible.Sum(s => s.Remaining);

            SummaryText.Text =
                $"{AppLocalization.T("StudentsCount")}: {visible.Count}   •   " +
                $"{AppLocalization.T("TotalCollected")}: {collected:0.00}   •   " +
                $"{AppLocalization.T("TotalRemaining")}: {remaining:0.00}";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) =>
            NavigationService?.Navigate(new StudentFormPage(null));

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is not Student selected)
            {
                MessageBox.Show(AppLocalization.T("SelectRowFirst"));
                return;
            }
            NavigationService?.Navigate(new StudentFormPage(selected));
        }

        private void RecordPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is not Student selected)
            {
                MessageBox.Show(AppLocalization.T("SelectRowFirst"));
                return;
            }
            NavigationService?.Navigate(new PaymentPage(selected));
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is not Student selected)
            {
                MessageBox.Show(AppLocalization.T("SelectRowFirst"));
                return;
            }

            if (MessageBox.Show(AppLocalization.T("ConfirmDelete"), string.Empty, MessageBoxButton.YesNo)
                != MessageBoxResult.Yes)
                return;

            AppServices.Database.Delete(selected.Id);
            ReloadFromDatabase();
        }

        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            if (StudentsGrid.SelectedItem is not Student selected)
            {
                MessageBox.Show(AppLocalization.T("SelectRowFirst"));
                return;
            }
            ReceiptPrinter.PrintReceipt(selected, AppLocalization.IsArabic);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e) =>
            NavigationService?.Navigate(new LoginPage());

        private void LangButton_Click(object sender, RoutedEventArgs e)
        {
            AppLocalization.IsArabic = !AppLocalization.IsArabic;
            ApplyLanguage();
            RebuildClassFilterOptions();
            _view?.Refresh();
            UpdateSummary();
        }

        private void ApplyLanguage()
        {
            FlowDirection = AppLocalization.IsArabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            LangButton.Content = AppLocalization.IsArabic ? "English" : "العربية";
            TitleText.Text = AppLocalization.T("Students");
            LogoutButton.Content = AppLocalization.T("Logout");
            SearchLabel.Text = AppLocalization.T("Search") + ":";
            AddButton.Content = AppLocalization.T("AddNew");
            RecordPaymentButton.Content = AppLocalization.T("RecordPayment");
            EditButton.Content = AppLocalization.T("Edit");
            DeleteButton.Content = AppLocalization.T("Delete");
            PrintButton.Content = AppLocalization.T("Print");

            ColName.Header = AppLocalization.T("Name");
            ColAge.Header = AppLocalization.T("Age");
            ColClass.Header = AppLocalization.T("Class");
            ColNotes.Header = AppLocalization.T("Notes");

            ColJan.Header = AppLocalization.MonthName(1);
            ColFeb.Header = AppLocalization.MonthName(2);
            ColMar.Header = AppLocalization.MonthName(3);
            ColApr.Header = AppLocalization.MonthName(4);
            ColMay.Header = AppLocalization.MonthName(5);
            ColJun.Header = AppLocalization.MonthName(6);
            ColJul.Header = AppLocalization.MonthName(7);
            ColAug.Header = AppLocalization.MonthName(8);
            ColSep.Header = AppLocalization.MonthName(9);
            ColOct.Header = AppLocalization.MonthName(10);
            ColNov.Header = AppLocalization.MonthName(11);
            ColDec.Header = AppLocalization.MonthName(12);
        }
    }
}
