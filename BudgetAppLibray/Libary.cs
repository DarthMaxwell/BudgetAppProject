using SkiaSharp;
using SQLite;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace BudgetAppLibray {
    public class Libary {}

    public class CustomSwitch1 : Switch {}

    // Add Account class which need to refer to the profiles
    public class  Account {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public double Income { get; set; } = 0;
        public double TaxRate { get; set; } = 0;

        public Account() { }

        public Account(double i, double t) {
            Income = i;
            TaxRate = t;
        }
    }

    public class Profile {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = "New Profile";

        // need id of the account

        [Ignore]
        public ObservableCollection<Expense> Expenses { get; set; } = new ObservableCollection<Expense>();

        public Profile() {}

        public override string ToString() {
            return Name;
        }
    }


    public class Expense : INotifyPropertyChanged {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ProfileId { get; set; }

        private bool _isPercentage;
        public bool isPercentage {
            get => _isPercentage;
            set {
                if (_isPercentage != value) {
                    _isPercentage = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _expenseName;
        public string ExpenseName {
            get => _expenseName;
            set { _expenseName = value; OnPropertyChanged(); }
        }

        private double _value;
        public double Value {
            get => _value;
            set { _value = value; OnPropertyChanged(); }
        }
        public bool Edit { get; set; } = false; // need to remove this

        public Expense() {}

        public Expense(int ProfileId) {
            this.ProfileId = ProfileId;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // if % sum cant be 100% need to check on input on new one
    }

    public class DisplayExpense {
        public string Name { get; set; }
        public double Value { get; set; }

        public Color DisplayColor { get; set; }
        public double Percentage { get; set; }
        public string PercentageText => $"{Percentage}%";

        public DisplayExpense(string name, double value, SKColor color, double percentage) {
            Name = name;
            Value = value;
            DisplayColor = Color.FromRgb(color.Red, color.Green, color.Blue);
            Percentage = percentage;
        }

        public override string ToString() {
            return $"{Name} : {Value:N2}";
        }
    }

    public class BoolToTextConverter : IValueConverter {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return (value is bool b && b) ? "Percent" : "Static";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            // Not really needed
            return value?.ToString() == "Percent";
        }
    }
}
