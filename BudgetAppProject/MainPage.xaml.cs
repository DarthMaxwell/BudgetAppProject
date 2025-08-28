using BudgetAppLibray;
using Microcharts;
using SkiaSharp;

namespace BudgetAppProject {
    public partial class MainPage : ContentPage {
        private readonly LocalDbService Db = App.Db;
        private readonly ExpensesViewModel PEViewModel = App.MainProfileAndExpenseViewModel;

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

        public MainPage(LocalDbService dbService) { // idk how we get this in the construtor but we can remvoe it
            InitializeComponent();

            //_dbService.AddDefaultObjectsIfNeededAsync(); still needed justy commnedt out

            BindingContext = PEViewModel;
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();

            CancelButton_Clicked(null, null);
            // Gets run when its minimized so maybe not what we want
        }

        // Onappear refresh incace the account was updated

        // Events

        // Helper Functions

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

            PEViewModel.Expenses.Remove(exp);
        }

        private void AddExpenseButton_Clicked(object sender, EventArgs e) {
            Expense newExpense = new Expense(PEViewModel.SelectedProfile.Id);

            PEViewModel.Expenses.Add(newExpense);
        }

        private async void refreshMoneyAfterIncomeAndTax() {
            List<DisplayExpense> TempExpensesList = new List<DisplayExpense>();
            Account a = await Db.GetAccount();

            DisplayExpenses.ItemsSource = null;

            double Tax = a.TaxRate;
            double Income = a.Income;
            double IncomeAfterTax = Income - (Income * (Tax / 100));
            double extra = IncomeAfterTax;

            if (PEViewModel.SelectedProfile != null ) { // will it ever be null bassicly no
                TempExpensesList.Add(new DisplayExpense("Tax", Math.Round(Income * (Tax / 100), 2)));

                foreach (Expense e in PEViewModel.Expenses) {
                    double x = (e.Type == "Percent") ? IncomeAfterTax * (e.Value / 100) : e.Value;

                    TempExpensesList.Add(new DisplayExpense(e.ExpenseName, Math.Round(x, 2)));
                    extra -= Math.Round(x, 2);
                }

                if (extra > 1) {
                   TempExpensesList.Add(new DisplayExpense("Extra", Math.Round(extra, 2)));
                }

                DisplayExpenses.ItemsSource = TempExpensesList;
            }
            

            // donut chart
            List<ChartEntry> chartEntries = new List<ChartEntry>();

            // just for normal for loop later
            int index = 0;

            foreach (var x in TempExpensesList) {
                chartEntries.Add(new ChartEntry((float?)x.Value) { Color = donutChartColors[index] });
                index++;
            }

            ExpenseChart.Chart = new DonutChart { Entries = chartEntries };
            
        }

        private async void RemoveProfileButton_Clicked(object sender, EventArgs e) {
            await Db.DeleteProfile(PEViewModel.SelectedProfile);
            PEViewModel.Profiles.Remove(PEViewModel.SelectedProfile);

            exitEditMode();
        }

        private async void SaveProfileButton_Clicked(object sender, EventArgs e) {
            Profile EditedProfile = PEViewModel.SelectedProfile;
            // needs to check if valid and then save to db (will be in a bunch of different things)

            //NEEds to remove the expenses related to the new profile

            // Update Profile's Expesnses in database
            foreach (var exp in PEViewModel.Expenses) {
                PEViewModel.SelectedProfile.Expenses.Add(exp);
            }

            // Update Profile name in database
            if (ProfileNameEntry.Text != "" && !ProfileNameEntry.Text.Equals(PEViewModel.SelectedProfile.Name)) {
                PEViewModel.SelectedProfile.Name = ProfileNameEntry.Text;
                await Db.SaveProfile(PEViewModel.SelectedProfile);
                EditedProfile = PEViewModel.SelectedProfile;
            }

            // Update the View
            var profilesFromDb = await Db.GetProfiles();
            PEViewModel.Profiles.Clear();
            foreach (var profile in profilesFromDb)
                PEViewModel.Profiles.Add(profile);

            PEViewModel.SelectedProfile = PEViewModel.Profiles.FirstOrDefault(p => p.Id == EditedProfile.Id);

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

        private void AddButtonProfile_Clicked(object sender, EventArgs e) { // not working fucks the names up??
            Profile newProfile = new Profile();
            PEViewModel.Profiles.Add(newProfile);
            PEViewModel.SelectedProfile = newProfile;

            EditButton_Clicked(sender, e);
        }

        private void CancelButton_Clicked(object sender, EventArgs e) {
            if (PEViewModel.SelectedProfile.Id == 0) { // 0 id means its new and not in the database yet
                PEViewModel.Profiles.Remove(PEViewModel.SelectedProfile);
            }

            exitEditMode();
        }
    }

}
