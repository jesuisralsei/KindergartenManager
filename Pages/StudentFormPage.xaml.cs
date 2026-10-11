using System;
using System.Windows;
using System.Windows.Controls;
using KindergartenCRUD.Models;
using KindergartenCRUD.Services;

namespace KindergartenCRUD.Pages
{
    public partial class StudentFormPage : Page
    {
        // Null => registering a new student. Non-null => editing this one.
        private readonly Student? _editingStudent;

        public StudentFormPage(Student? studentToEdit)
        {
            InitializeComponent();
            _editingStudent = studentToEdit;

            Loaded += (s, e) =>
            {
                ApplyLanguage();
                PopulateIfEditing();
            };
        }

        private void PopulateIfEditing()
        {
            if (_editingStudent == null) return;

            NameBox.Text = _editingStudent.Name;
            DobPicker.SelectedDate = _editingStudent.DateOfBirth;
            AgeBox.Text = _editingStudent.Age.ToString();
            ClassBox.Text = _editingStudent.ClassName;
            FeeBox.Text = _editingStudent.MonthlyFee.ToString("0.00");
            NotesBox.Text = _editingStudent.Notes;
        }

        private void DobPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DobPicker.SelectedDate.HasValue)
            {
                var dob = DobPicker.SelectedDate.Value;
                var today = DateTime.Today;
                int age = today.Year - dob.Year;
                if (dob.Date > today.AddYears(-age)) age--;
                AgeBox.Text = Math.Max(age, 0).ToString();
            }
            else
            {
                AgeBox.Text = string.Empty;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameBox.Text))
            {
                ErrorText.Text = AppLocalization.IsArabic ? "الرجاء إدخال الاسم." : "Please enter the student's name.";
                return;
            }
            if (!DobPicker.SelectedDate.HasValue || !int.TryParse(AgeBox.Text, out int age))
            {
                ErrorText.Text = AppLocalization.IsArabic ? "الرجاء اختيار تاريخ الميلاد." : "Please select a date of birth.";
                return;
            }

            string className = ClassBox.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(className))
            {
                ErrorText.Text = AppLocalization.IsArabic ? "الرجاء إدخال الفصل." : "Please enter or select a class.";
                return;
            }
            if (!decimal.TryParse(FeeBox.Text, out decimal fee) || fee < 0)
            {
                ErrorText.Text = AppLocalization.IsArabic ? "قيمة الرسم الشهري غير صحيحة." : "Please enter a valid monthly fee.";
                return;
            }

            var student = _editingStudent ?? new Student();
            student.Name = NameBox.Text.Trim();
            student.DateOfBirth = DobPicker.SelectedDate.Value;
            student.Age = age;
            student.ClassName = className;
            student.MonthlyFee = fee;
            student.Notes = NotesBox.Text?.Trim() ?? string.Empty;

            if (_editingStudent == null)
                AppServices.Database.Insert(student);
            else
                AppServices.Database.Update(student);

            NavigationService?.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigationService?.GoBack();

        private void LangButton_Click(object sender, RoutedEventArgs e)
        {
            AppLocalization.IsArabic = !AppLocalization.IsArabic;
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            FlowDirection = AppLocalization.IsArabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            LangButton.Content = AppLocalization.IsArabic ? "English" : "العربية";
            TitleText.Text = _editingStudent == null ? AppLocalization.T("AddNew") : AppLocalization.T("Edit");
            NameLabel.Text = AppLocalization.T("Name");
            DobLabel.Text = AppLocalization.T("DateOfBirth");
            AgeLabel.Text = AppLocalization.T("Age");
            ClassLabel.Text = AppLocalization.T("Class");
            FeeLabel.Text = AppLocalization.T("MonthlyFee");
            NotesLabel.Text = AppLocalization.T("Notes");
            SaveButton.Content = AppLocalization.T("Save");
            CancelButton.Content = AppLocalization.T("Cancel");
        }
    }
}
