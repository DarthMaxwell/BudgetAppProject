using SQLite;
using System.Collections;
using System.Globalization;

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
        public List<Expense> Expenses { get; set; } // WE only use this for setting up the first items

        public Profile() {
            Expenses = new List<Expense>();
        }

        public override string ToString() {
            return Name;
        }
    }
    public class Expense {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string Type { get; set; } = "Static";
        public string ExpenseName { get; set; } = string.Empty;
        public double Value { get; set; } = 0;
        public bool Edit { get; set; } = false; // need to remove this

        public Expense() {}

        public Expense(int ProfileId) {
            this.ProfileId = ProfileId;
        }

        // if % sum cant be 100% need to check on input on new one
    }

    public class DisplayExpense {
        public string Name { get; set; }
        public double Value { get; set; }

        public DisplayExpense(string name, double value) {
            Name = name;
            Value = value;
        }
    }

    // Can remove some of the converts that arnt used rn

    public class Text2BoolSwitchConverter : IValueConverter {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (value is string text) {
                switch (text) {
                    case "Static":
                        return false;
                    case "Percent":
                        return true;
                }
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (value is bool b) {
                switch (b) {
                    case false:
                        return "Static";
                    case true:
                        return "Percent";
                }
            }

            return null;
        }
    }
    
    public class Bool2TextButtonConverter : IValueConverter {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (value is bool b) {
                return (b) ? "Save" : "Edit";
            }

            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) {
            if (value is string text) {
                switch (text) {
                    case "Save":
                        return false;
                    case "Edit":
                        return true;
                }
            }

            return null;
        }
    }
}
