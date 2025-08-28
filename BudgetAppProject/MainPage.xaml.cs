using BudgetAppLibray;
using Microcharts;
using SkiaSharp;

namespace BudgetAppProject {
    public partial class MainPage : ContentPage {
        private readonly LocalDbService Db = App.Db;
        private readonly ExpensesViewModel PEViewModel = App.MainProfileAndExpenseViewModel;
        private List<Expense> DeleteExpenses = new List<Expense>();
        private List<SKColor> donutChartColors = new List<SKColor>{
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

            _ = Db.AddDefaultObjectsIfNeededAsync();

            BindingContext = PEViewModel;
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();

            CancelButton_Clicked(null, null);
            // Gets run when its minimized so maybe not what we want
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            RefreshMoney();
        }

        // Events
        private void ProfilePicker_SelectedIndexChanged(object sender, EventArgs e) {
            RefreshMoney();
        }

        private void EditButton_Clicked(object sender, EventArgs e) {
            Expenses.IsVisible = true;
            AddExpenseButton.IsVisible = true;
            RemoveProfileButton.IsVisible = true;
            SaveProfileButton.IsVisible = true;
            CancelButton.IsVisible = true;
            ProfileNameEntry.IsVisible = true;

            DisplayExpenses.IsVisible = false;
            EditButton.IsVisible = false;
            AddButtonProfile.IsVisible = false;
            ProfilePicker.IsVisible = false;

            DeleteExpenses.Clear();
        }

        private void AddButtonProfile_Clicked(object sender, EventArgs e) { // not working fucks the names up??
            Profile newProfile = new Profile();
            PEViewModel.Profiles.Add(newProfile);
            PEViewModel.SelectedProfile = newProfile;

            EditButton_Clicked(sender, e);
        }

        // Events in edit mode

        // NEED VALIDATION FUNCTIONS

        // Remove selected Expense from SelectedProfile (Still need to save)
        private void DeleteButton_Clicked(object sender, EventArgs e) {
            Button button = (Button)sender;
            Expense exp = (Expense)button.Parent.BindingContext;

            PEViewModel.SelectedProfile.Expenses.Remove(exp);
            DeleteExpenses.Add(exp);
        }

        // Add new Expense to the SelectedProfile (Still need to save)
        private void AddExpenseButton_Clicked(object sender, EventArgs e) {
            Expense newExpense = new Expense(PEViewModel.SelectedProfile.Id);

            PEViewModel.SelectedProfile.Expenses.Add(newExpense);
        }

        // Removes Profile (instantly affects database)
        private async void RemoveProfileButton_Clicked(object sender, EventArgs e) {
            await Db.DeleteProfile(PEViewModel.SelectedProfile);
            PEViewModel.Profiles.Remove(PEViewModel.SelectedProfile);

            exitEditMode();
        }

        // Saves the SelectedProfile to the database
        private async void SaveProfileButton_Clicked(object sender, EventArgs e) {
            int id = PEViewModel.SelectedProfile.Id;

            if (ProfileNameEntry.Text != "" && !ProfileNameEntry.Text.Equals(PEViewModel.SelectedProfile.Name)) {
                PEViewModel.SelectedProfile.Name = ProfileNameEntry.Text;
            }

            // We assume if your going to hit save somthing has changed
            await Db.SaveProfile(PEViewModel.SelectedProfile);
            DeleteExpenses.ForEach(async e => await Db.DeleteExpense(e)); 

            // Update the UI
            PEViewModel.Init();
            exitEditMode();

            if (id == 0) { 
                // This means its a new Profile so it will be at the end
                PEViewModel.SelectedProfile = PEViewModel.Profiles.Last();
            } else {
                PEViewModel.SelectedProfile = PEViewModel.Profiles.Where(p => p.Id == id).First();
            }
        }

        // Dosnt save your changes (We dont check for changes)
        private void CancelButton_Clicked(object sender, EventArgs e) {
            int id = PEViewModel.SelectedProfile.Id;

            PEViewModel.Init();
            exitEditMode();

            if (id != 0) { 
                PEViewModel.SelectedProfile = PEViewModel.Profiles.Where(p => p.Id == id).First();
            }
        }

        // Helper Functions
        private async void RefreshMoney() {
            List<DisplayExpense> TempExpensesList = new List<DisplayExpense>();
            Account a = await Db.GetAccount();
            double IncomeAfterTax = a.Income - (a.Income * (a.TaxRate / 100));
            double extra = IncomeAfterTax;

            TempExpensesList.Add(new DisplayExpense("Tax", Math.Round(a.Income - IncomeAfterTax, 2)));

            foreach (Expense e in PEViewModel.SelectedProfile.Expenses) {
                double x = (e.Type == "Percent") ? IncomeAfterTax * (e.Value / 100) : e.Value; // could round here

                TempExpensesList.Add(new DisplayExpense(e.ExpenseName, Math.Round(x, 2)));
                extra -= Math.Round(x, 2);
            }

            if (extra > 1) {
               TempExpensesList.Add(new DisplayExpense("Extra", Math.Round(extra, 2)));
            }

            DisplayExpenses.ItemsSource = null;
            DisplayExpenses.ItemsSource = TempExpensesList;
            

            // Donut chart
            List<ChartEntry> chartEntries = new List<ChartEntry>();

            for (int i = 0; i < TempExpensesList.Count; i++) {
                chartEntries.Add(new ChartEntry((float?)TempExpensesList[i].Value) { Color = donutChartColors[i] });
            }

            ExpenseChart.Chart = new DonutChart { Entries = chartEntries };
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
    }
}
