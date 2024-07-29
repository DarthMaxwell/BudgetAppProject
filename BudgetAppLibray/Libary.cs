using SQLite;
using System.Collections;
using System.Globalization;

namespace BudgetAppLibray {
    // All the code in this file is included in all platforms.
    public class Libary {
    }

    public class CustomSwitch1 : Switch {}

    public class Profile {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }

        [Ignore]
        public List<Expense> Expenses { get; set; } // THIS SEAMS TO JUST BE EMOTY

        
        public Profile() {
            Expenses = new List<Expense>();
        }

        public void removeExpense(Expense expense) { // WHY DO WE HAVE THIS THE THING IS ALREADYU ÅUBLIC
            Expenses.Remove(expense);
        }

        public void addExpense(Expense expense) {
            this.Expenses.Add(expense);
        }

        public List<Expense> GetExpenses() {
            return Expenses;
        }

        public override string ToString() {
            return Name;
        }
    }
    public class Expense {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int ProfileId { get; set; }
        public string Type { get; set; }
        public string ExpenseName { get; set; } = string.Empty;
        public double Value { get; set; }
        public bool Edit { get; set; } = false;

        public Expense() {
            // New and editable
            Type = "Static";
            Edit = true;
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
