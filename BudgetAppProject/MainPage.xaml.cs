using BudgetAppLibray;

namespace BudgetAppProject {
    public partial class MainPage : ContentPage {
        private readonly LocalDbService _dbService;


        private double Income = 0;
        private double Tax = 0;
        private double IncomeAfterTax = 0;
        private List<DisplayExpense> displayExpenses = new List<DisplayExpense>();
        //Profile EmptyProfile = new("Empty");
        //List<Profile> list = new List<Profile>();

        public MainPage(LocalDbService dbService) {
            InitializeComponent();
            _dbService = dbService;
            InitializeAsync();

            // Maybe clear Empty when open
        }

        [Obsolete]
        private async Task InitializeAsync() {
            await _dbService.AddDefaultObjectsIfNeededAsync();
            var profiles = await _dbService.GetProfiles();

            Device.BeginInvokeOnMainThread(() => {
                ProfilePicker.ItemsSource = profiles;
                ProfilePicker.SelectedIndex = 0;
            });
        }

        [Obsolete]
        private async void refresh() {
            Expenses.ItemsSource = null;

            Profile selected = (Profile)ProfilePicker.SelectedItem;

            if (selected != null) {
                var expenses = await _dbService.GetExpenses(selected);

                Device.BeginInvokeOnMainThread(() => {
                    Expenses.ItemsSource = expenses;
                });

                if (ProfilePicker.SelectedIndex != 0) {
                    SaveNewProfileStack.IsVisible = false;
                    ProfileNameEntry.Text = "";
                } else {
                    SaveNewProfileStack.IsVisible = true;
                }
            }

        }

        private void ProfilePicker_SelectedIndexChanged(object sender, EventArgs e) {
            refresh();
            refreshMoneyAfterIncomeAndTax();

            if (ProfilePicker.SelectedIndex == 0) {
                RemoveProfileButton.IsVisible = false;
            } else {
                RemoveProfileButton.IsVisible = true;
            }
        }

        private async void EditButton_Clicked(object sender, EventArgs e) { // THIS DONT WORK
            // Edit Expense
            Button button = (Button)sender;
            Expense exp = (Expense)button.Parent.BindingContext;

            if (exp != null ) {
                exp.Edit = (button.Text.Equals("Edit")) ? true : false; // Maybe we can use converter thign but idk how here

                await _dbService.SaveExpense(exp);
            }

            refresh();
            refreshMoneyAfterIncomeAndTax();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e) {
            // Delete Expense
            Button button = (Button)sender;
            Expense exp = (Expense)button.Parent.BindingContext;

            if (exp != null ) {
                Profile selected = (Profile)ProfilePicker.SelectedItem;

                //selected.removeExpense(exp); // idk if we need this
                // IDk if we need this
                //selected.Expenses.Remove(exp);

                //await _dbService.SaveProfile(selected); // idk if we need this
                await _dbService.DeleteExpense(exp);
            }

            refresh();
            refreshMoneyAfterIncomeAndTax();
        }

        private void IncomeEntry_Unfocused(object sender, FocusEventArgs e) {
            // Unfocues might not be good enough
            refreshMoneyAfterIncomeAndTax();
        }

        private void TaxEntry_Unfocused(object sender, FocusEventArgs e) {
            refreshMoneyAfterIncomeAndTax();
        }

        private async void refreshMoneyAfterIncomeAndTax() {
            DisplayExpenses.ItemsSource = null;
            displayExpenses.Clear();

            // Want to be able to use , or . for decimals
            if (Double.TryParse(IncomeEntry.Text, out Income) && Double.TryParse(TaxEntry.Text, out Tax)) {
                Profile selected = (Profile)ProfilePicker.SelectedItem;
                IncomeAfterTax = Income - (Income * (Tax / 100));
                double extra = IncomeAfterTax;

                if (selected != null ) {
                    var expenses = await _dbService.GetExpenses(selected);
                    displayExpenses.Add(new DisplayExpense("Tax", Math.Round(Income * (Tax / 100), 2)));

                    foreach (Expense e in expenses) {
                        double x = (e.Type == "Percent") ? IncomeAfterTax * (e.Value / 100) : e.Value;

                        displayExpenses.Add(new DisplayExpense(e.ExpenseName, Math.Round(x, 2)));
                        extra -= Math.Round(x, 2);
                    }

                    if (extra > 1) {
                        displayExpenses.Add(new DisplayExpense("Extra", Math.Round(extra, 2)));
                    }

                    DisplayExpenses.ItemsSource = displayExpenses;
                }
            }
        }

        private async void AddExpenseButton_Clicked(object sender, EventArgs e) {
            Profile selected = (Profile)ProfilePicker.SelectedItem;

            if (selected != null) {
                Expense exp = new();
                
                selected.addExpense(exp); // idk how much this is needed
                exp.ProfileId = selected.Id;

                await _dbService.AddExpense(exp);
                

                //We dont want to save anything to the db until it saved 

                // Add Expense to profile object and then save in db
                // Add Expens to databsae
                //selected.addExpense(new Expense());
                refresh();
            }
        }

        [Obsolete]
        private async void NewProfileButton_Clicked(object sender, EventArgs e) {
            if (ProfileNameEntry.Text != null && ProfileNameEntry.Text != "") { // Could have duplicates
                Profile EmptyProfile = (Profile)ProfilePicker.SelectedItem;
                Profile NewProfile = new Profile { Name = ProfileNameEntry.Text };

                List<Expense> temp = await _dbService.GetExpenses(EmptyProfile);

                foreach (Expense expense in temp) {
                    NewProfile.addExpense(expense);
                }

                await _dbService.InsertProfileWithExpensesAsync(NewProfile);

                await _dbService.ClearEmptyProfile();

                var profiles = await _dbService.GetProfiles();

                Device.BeginInvokeOnMainThread(() => {
                    ProfilePicker.ItemsSource = null;
                    ProfilePicker.ItemsSource = profiles;
                    ProfilePicker.SelectedIndex = profiles.Count - 1;
                });
            }
        }

        [Obsolete]
        private async void RemoveProfileButton_Clicked(object sender, EventArgs e) {
            Profile selected = (Profile)ProfilePicker.SelectedItem;

            await _dbService.DeleteProfile(selected);

            var profiles = await _dbService.GetProfiles();

            Device.BeginInvokeOnMainThread(() => {
                ProfilePicker.ItemsSource = null;
                ProfilePicker.ItemsSource = profiles;
                ProfilePicker.SelectedIndex = 0;
            });
        }
    }

}
