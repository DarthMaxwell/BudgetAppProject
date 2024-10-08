using BudgetAppLibray;

namespace BudgetAppProject {
    public partial class MainPage : ContentPage {
        private readonly LocalDbService _dbService;

        private double Income = 0;
        private double Tax = 0;
        private double IncomeAfterTax = 0;
        private List<DisplayExpense> displayExpenses = new List<DisplayExpense>();

        public MainPage(LocalDbService dbService) {
            InitializeComponent();
            _dbService = dbService;

            InitializeAsync();
            _dbService.ClearEmptyProfile();

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
            // Maybe we want a refresh and then a refresh async to seprate things

            Profile selected = (Profile)ProfilePicker.SelectedItem;
            
            Expenses.ItemsSource = null;

            if (selected != null) {
                var expenses = await _dbService.GetExpenses(selected);

                Device.BeginInvokeOnMainThread(() => {
                    Expenses.ItemsSource = expenses;
                });

                if (ProfilePicker.SelectedIndex != 0) {
                    SaveNewProfileStack.IsVisible = false;
                    RemoveProfileButton.IsVisible = true;
                    ProfileNameEntry.Text = "";
                } else {
                    RemoveProfileButton.IsVisible = false;
                    SaveNewProfileStack.IsVisible = true;
                }

                refreshMoneyAfterIncomeAndTax();
            }

        }

        private void ProfilePicker_SelectedIndexChanged(object sender, EventArgs e) {
            refresh();

            // Save or clear the expenses ones that didnt get saved??
        }

        private async void EditButton_Clicked(object sender, EventArgs e) { // THIS DONT WORK
            // Edit Expense
            Button button = (Button)sender;
            Expense exp = (Expense)button.Parent.BindingContext;

            if (exp != null ) {
                exp.Edit = (button.Text.Equals("Edit")) ? true : false; // Maybe we can use converter thign but idk how here

                await _dbService.SaveExpense(exp); // we only want to save the object if we select save
            }

            refresh();
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e) {
            // Delete Expense
            Button button = (Button)sender;
            Expense exp = (Expense)button.Parent.BindingContext;

            if (exp != null ) {
                Profile selected = (Profile)ProfilePicker.SelectedItem;

                await _dbService.DeleteExpense(exp);
            }

            refresh();
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
                
                selected.Expenses.Add(exp); // idk how much this is needed
                exp.ProfileId = selected.Id;

                await _dbService.AddExpense(exp);
                

                //We dont want to save anything to the db until it saved 

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
                    NewProfile.Expenses.Add(expense);
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
