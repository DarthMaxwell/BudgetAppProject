using BudgetAppLibray;

namespace BudgetAppProject {
    public partial class MainPage : ContentPage {
        public ExpensesViewModel ViewModel { get; set; }
        private readonly LocalDbService _dbService;

        //private Accout account = somthing
        ExpensesViewModel viewModel;


        // this will be removed or come for the profile
        private double Income = 0;
        private double Tax = 0;
        private double IncomeAfterTax = 0;
        private List<DisplayExpense> displayExpenses = new List<DisplayExpense>(); //move thbis??

        public MainPage(LocalDbService dbService) {
            InitializeComponent();
            _dbService = dbService;

            InitializeAsync();
            viewModel = new ExpensesViewModel(_dbService);
            BindingContext = viewModel;
            ProfilePicker.SelectedIndex = 0;
            // Maybe clear Empty when open
        }


        [Obsolete]
        private async Task InitializeAsync() {
            await _dbService.AddDefaultObjectsIfNeededAsync();

            double[] taxPercentage = [.. Enumerable.Range(0, 100)];
            TaxPicker.ItemsSource = taxPercentage;
            TaxPicker.SelectedIndex = 24;
            // select index based on acount values
        }

        [Obsolete]
        private async void ProfilePicker_SelectedIndexChanged(object sender, EventArgs e) {
            refreshMoneyAfterIncomeAndTax();
        }

        private void EditButton_Clicked(object sender, EventArgs e) {
            IncomeEntry.IsEnabled = true;
            TaxPicker.IsEnabled = true;
            Expenses.IsVisible=true;
            AddExpenseButton.IsVisible = true;
            RemoveProfileButton.IsVisible = true;
            SaveProfileButton.IsVisible = true;
            ProfileNameEntry.IsVisible = true;
            ProfileNameEntry.Placeholder = viewModel.SelectedProfile.Name;

            DisplayExpenses.IsVisible = false;
            EditButton.IsVisible = false;
            AddButtonProfile.IsVisible = false;
            ProfilePicker.IsVisible = false;
        }

        private void DeleteButton_Clicked(object sender, EventArgs e) {
            Button button = (Button)sender;
            Expense exp = (Expense)button.Parent.BindingContext;

            viewModel.Expenses.Remove(exp);
        }

        private void AddExpenseButton_Clicked(object sender, EventArgs e) {
            Expense newExpense = new Expense(viewModel.SelectedProfile.Id);

            viewModel.Expenses.Add(newExpense);
        }

        private void IncomeEntry_Unfocused(object sender, FocusEventArgs e) {
            // Unfocues might not be good enough
            refreshMoneyAfterIncomeAndTax();
        }

        private void TaxPicker_SelectedIndexChanged(object sender, EventArgs e) {
            refreshMoneyAfterIncomeAndTax();
        }

        private void refreshMoneyAfterIncomeAndTax() {
            DisplayExpenses.ItemsSource = null;
            displayExpenses.Clear();

            // Want to be able to use , or . for decimals
            if (Double.TryParse(IncomeEntry.Text, out Income)) {
                Tax = Convert.ToDouble(TaxPicker.SelectedItem);
                IncomeAfterTax = Income - (Income * (Tax / 100));
                double extra = IncomeAfterTax;

                if (viewModel.SelectedProfile != null ) {
                    displayExpenses.Add(new DisplayExpense("Tax", Math.Round(Income * (Tax / 100), 2)));

                    foreach (Expense e in viewModel.Expenses) {
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

        private async void RemoveProfileButton_Clicked(object sender, EventArgs e) {
            await _dbService.DeleteProfile(viewModel.SelectedProfile);
            viewModel.Profiles.Remove(viewModel.SelectedProfile);
            // maybe select the first profile?
        }

        private async void SaveProfileButton_Clicked(object sender, EventArgs e) {
            // needs to check if valid and then save to db
           

            // If name changed update the database
            if (ProfileNameEntry.Text != "") {
                viewModel.SelectedProfile.Name = ProfileNameEntry.Text;
                await _dbService.SaveProfile(viewModel.SelectedProfile);
            }

            //Loop though all the expsenses and update the database

            //await _dbService.AddExpense(newExpense);

            IncomeEntry.IsEnabled = false;
            TaxPicker.IsEnabled = false;
            Expenses.IsVisible = false;
            AddExpenseButton.IsVisible = false;
            RemoveProfileButton.IsVisible = false;
            SaveProfileButton.IsVisible = false;
            ProfileNameEntry.IsVisible = false;

            DisplayExpenses.IsVisible = true;
            EditButton.IsVisible = true;
            AddButtonProfile.IsVisible = true;
            ProfilePicker.IsVisible = true;

            // update the UI to relfect new database info
            var profilesFromDb = await _dbService.GetProfiles();
            viewModel.Profiles.Clear();
            foreach (var profile in profilesFromDb)
                viewModel.Profiles.Add(profile);

        }

        private void AddButtonProfile_Clicked(object sender, EventArgs e) {
            Profile newProfile = new Profile();
            viewModel.Profiles.Add(newProfile);
            viewModel.SelectedProfile = newProfile;
            
            EditButton_Clicked(sender, e);
        }
    }

}
