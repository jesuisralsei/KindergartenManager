using System;
using System.ComponentModel;
using System.Linq;

namespace KindergartenCRUD.Models
{
    /// <summary>
    /// A single kindergarten student, including a monthly fee and 12 months
    /// (Jan-Dec) of payment tracking. Implements INotifyPropertyChanged so the
    /// DataGrid reflects edits live.
    /// </summary>
    public class Student : INotifyPropertyChanged
    {
        private string _id = Guid.NewGuid().ToString();
        private string _name = string.Empty;
        private int _age;
        private DateTime _dateOfBirth = DateTime.Today.AddYears(-5);
        private string _className = string.Empty;
        private decimal _monthlyFee;
        private string _notes = string.Empty;

        // Index 0 = January ... index 11 = December.
        private readonly decimal[] _monthlyPaid = new decimal[12];

        public string Id
        {
            get => _id;
            set { _id = value; OnPropertyChanged(nameof(Id)); }
        }

        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }

        public int Age
        {
            get => _age;
            set { _age = value; OnPropertyChanged(nameof(Age)); }
        }

        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set { _dateOfBirth = value; OnPropertyChanged(nameof(DateOfBirth)); }
        }

        public string ClassName
        {
            get => _className;
            set { _className = value; OnPropertyChanged(nameof(ClassName)); }
        }

        /// <summary>How much this student owes per month.</summary>
        public decimal MonthlyFee
        {
            get => _monthlyFee;
            set
            {
                _monthlyFee = value;
                OnPropertyChanged(nameof(MonthlyFee));
                OnPropertyChanged(nameof(TotalDue));
                OnPropertyChanged(nameof(Remaining));
            }
        }

        /// <summary>Free-text notes (allergies, pickup instructions, anything staff wants to remember).</summary>
        public string Notes
        {
            get => _notes;
            set { _notes = value; OnPropertyChanged(nameof(Notes)); }
        }

        public decimal GetMonthPaid(int month) => _monthlyPaid[month - 1];

        public void SetMonthPaid(int month, decimal amount)
        {
            _monthlyPaid[month - 1] = amount;
            OnPropertyChanged(MonthPropertyName(month));
            OnPropertyChanged(nameof(TotalPaid));
            OnPropertyChanged(nameof(Remaining));
        }

        // One property per month so the DataGrid can bind directly to Jan..Dec
        // without needing converters or dynamically-generated columns.
        public decimal Jan => _monthlyPaid[0];
        public decimal Feb => _monthlyPaid[1];
        public decimal Mar => _monthlyPaid[2];
        public decimal Apr => _monthlyPaid[3];
        public decimal May => _monthlyPaid[4];
        public decimal Jun => _monthlyPaid[5];
        public decimal Jul => _monthlyPaid[6];
        public decimal Aug => _monthlyPaid[7];
        public decimal Sep => _monthlyPaid[8];
        public decimal Oct => _monthlyPaid[9];
        public decimal Nov => _monthlyPaid[10];
        public decimal Dec => _monthlyPaid[11];

        public decimal TotalPaid => _monthlyPaid.Sum();
        public decimal TotalDue => MonthlyFee * 12;
        public decimal Remaining => TotalDue - TotalPaid;

        private static string MonthPropertyName(int month) => month switch
        {
            1 => nameof(Jan), 2 => nameof(Feb), 3 => nameof(Mar), 4 => nameof(Apr),
            5 => nameof(May), 6 => nameof(Jun), 7 => nameof(Jul), 8 => nameof(Aug),
            9 => nameof(Sep), 10 => nameof(Oct), 11 => nameof(Nov), 12 => nameof(Dec),
            _ => string.Empty,
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
