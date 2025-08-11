using BudgetAppLibray;
using Microcharts;
using SkiaSharp;

namespace BudgetAppProject {
    public partial class MainPage : ContentPage {
        public ExpensesViewModel ViewModel { get; set; }
        private readonly LocalDbService _dbService;

        //private Accout account = somthing
        ExpensesViewModel viewModel;


        // this will be removed or come for the profile
        private double IncomeAfterTax = 0;
        private List<DisplayExpense> displayExpenses = new List<DisplayExpense>(); //move thbis??

        List<SKColor> donutChartColors = new List<SKColor>{
            SKColor.Parse("#FF6384"), // Soft Red
            SKColor.Parse("#36A2EB"), // Sky Blue
            SKColor.Parse("#FFCE56"), // Warm Yellow
            SKColor.Parse("#4BC0C0"), // Teal
            SKColor.Parse("#9966FF"), // Violet
            SKColor.Parse("#FF9F40"), // Orange
            SKColor.Parse("#C9CBCF"), // Light Gray
            SKColor.Parse("#2ECC71"), // Green
            SKColor.Parse("#F39C12"), // Amber
            SKColor.Parse("#E74C3C")  // Crimson
        };

        public MainPage(LocalDbService dbService) {
            InitializeComponent();
            _dbService = dbService;

            viewModel = new ExpensesViewModel(_dbService);
            BindingContext = viewModel;
            // Want to select the first one or just anything

            InitializeAsync();
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();

            CancelButton_Clicked(null, null);
        }


        [Obsolete]
        private async Task InitializeAsync() {
            await _dbService.AddDefaultObjectsIfNeededAsync();
        }

        private void ProfilePicker_SelectedIndexChanged(object sender, EventArgs e) {
            refreshMoneyAfterIncomeAndTax();
        }

        private void EditButton_Clicked(object sender, EventArgs e) {
            Expenses.IsVisible=true;
            AddExpenseButton.IsVisible = true;
            RemoveProfileButton.IsVisible = true;
            SaveProfileButton.IsVisible = true;
            CancelButton.IsVisible = true;
            ProfileNameEntry.IsVisible = true;

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

        private async void refreshMoneyAfterIncomeAndTax() {
            DisplayExpenses.ItemsSource = null;
            displayExpenses.Clear();

            Account a = await _dbService.GetAccount();

            // Want to be able to use , or . for decimals
            
            double Tax = a.TaxRate;
            double Income = a.Income;
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
            

            // donut chart
            List<ChartEntry> chartEntries = new List<ChartEntry>();

            // just for normal for loop later
            int index = 0;

            foreach (var x in displayExpenses) {
                chartEntries.Add(new ChartEntry((float?)x.Value) { Color = donutChartColors[index] });
                index++;
            }

            ExpenseChart.Chart = new DonutChart { Entries = chartEntries };
            
        }

        private async void RemoveProfileButton_Clicked(object sender, EventArgs e) {
            await _dbService.DeleteProfile(viewModel.SelectedProfile);
            viewModel.Profiles.Remove(viewModel.SelectedProfile);

            exitEditMode();
            // maybe select the first profile?
        }

        private async void SaveProfileButton_Clicked(object sender, EventArgs e) {
            // needs to check if valid and then save to db (will be in a bunch of different things)

            //NEEds to remove the expenses related to the new profile

            // Update Profile's Expesnses in database
            foreach (var exp in viewModel.Expenses) {
                viewModel.SelectedProfile.Expenses.Add(exp);
            }

            // Update Profile name in database
            if (ProfileNameEntry.Text != "" && !ProfileNameEntry.Text.Equals(viewModel.SelectedProfile.Name)) {
                viewModel.SelectedProfile.Name = ProfileNameEntry.Text;
                await _dbService.SaveProfile(viewModel.SelectedProfile);
            }

            // Update the View
            var profilesFromDb = await _dbService.GetProfiles();
            viewModel.Profiles.Clear();
            foreach (var profile in profilesFromDb)
                viewModel.Profiles.Add(profile);

            // Will need to use a name or something like index the newest on in the list
            //viewModel.SelectedProfile = profilesFromDb.FirstOrDefault(p => p.Nam == viewModel.SelectedProfile.Id);

            // Update the UI
            exitEditMode();
        }

        private void exitEditMode() {
            Expenses.IsVisible = false;
            AddExpenseButton.IsVisible = false;
            RemoveProfileButton.IsVisible = false;
            SaveProfileButton.IsVisible = false;
            CancelButton.IsVisible = false;
            ProfileNameEntry.IsVisible = false;

            DisplayExpenses.IsVisible = true;
            EditButton.IsVisible = true;
            AddButtonProfile.IsVisible = true;
            ProfilePicker.IsVisible = true;
        }

        private void AddButtonProfile_Clicked(object sender, EventArgs e) {
            Profile newProfile = new Profile();
            viewModel.Profiles.Add(newProfile);
            viewModel.SelectedProfile = newProfile;

            EditButton_Clicked(sender, e);
        }

        private void CancelButton_Clicked(object sender, EventArgs e) {
            if (viewModel.SelectedProfile.Id == 0) { // 0 id means its new and not in the database yet
                viewModel.Profiles.Remove(viewModel.SelectedProfile);
            }

            exitEditMode();
        }
    }

}
