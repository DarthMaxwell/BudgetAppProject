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

        public MainPage() {
            InitializeComponent();

            BindingContext = PEViewModel;
        }

        protected override void OnDisappearing() {
            base.OnDisappearing();

            CancelButton_Clicked(null, null);
            // Gets run when its minimized so maybe not what we want
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            var defaultsAdded = Preferences.Get("DefaultsAdded", false);

            if (defaultsAdded) {
                RefreshMoney();
            }
        }

        protected override void OnSizeAllocated(double width, double height) {
            base.OnSizeAllocated(width, height);

            // Make donut chart scale with screen size
            if (ExpenseChart != null) {
                // Only use half the height
                var availableHeight = height / 2;

                // Take smaller dimension so chart stays square
                var size = Math.Min(width, availableHeight) - 40;
                if (size < 0) size = 0;

                ExpenseChart.WidthRequest = size;
                ExpenseChart.HeightRequest = size;
            }
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
            // Percentage can have little rounding error too much or too little
            List<DisplayExpense> TempExpensesList = new List<DisplayExpense>();
            List<ChartEntry> chartEntries = new List<ChartEntry>();

            Account a = await Db.GetAccount();

            int colorIndex = 0;

            double IncomeAfterTax = Math.Round(a.Income - (a.Income * (a.TaxRate / 100.0)), 2);
            double tax = Math.Round(a.Income - IncomeAfterTax, 2);
            double extra = IncomeAfterTax;

            if (tax > 0) {
                TempExpensesList.Add(new DisplayExpense("Tax", tax, donutChartColors[colorIndex], a.TaxRate));
                chartEntries.Add(new ChartEntry((float?)tax) { Color = donutChartColors[colorIndex++]});
            }

            foreach (Expense e in PEViewModel.SelectedProfile.Expenses) {
                double x = (e.isPercentage) ? IncomeAfterTax * (e.Value / 100.0) : e.Value;
                double xper = (e.isPercentage) ? Math.Round((x / a.Income) * 100, 2) : Math.Round((e.Value / a.Income) * 100, 2);

                TempExpensesList.Add(new DisplayExpense(e.ExpenseName, x, donutChartColors[colorIndex], xper));
                chartEntries.Add(new ChartEntry((float?)x) { Color = donutChartColors[colorIndex++]});

                extra -= x;
            }

            if (extra > 0) {
               TempExpensesList.Add(new DisplayExpense("Extra", extra, donutChartColors[colorIndex], Math.Round((extra / a.Income) * 100, 2)));
                chartEntries.Add(new ChartEntry((float?)extra) { Color = donutChartColors[colorIndex]});
            }

            DisplayExpenses.ItemsSource = null;
            DisplayExpenses.ItemsSource = TempExpensesList;
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
