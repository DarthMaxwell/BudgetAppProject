using BudgetAppLibray;

namespace BudgetAppProject;

public partial class AccountPage : ContentPage
{
    private readonly LocalDbService Db = App.Db;
    private Account CurrentAccount;

    public AccountPage()
	{
		InitializeComponent();
        LoadAccount();

        double[] taxPercentage = [.. Enumerable.Range(0, 101)];
        TaxPicker.ItemsSource = taxPercentage;
    }

    protected override void OnDisappearing() {
        base.OnDisappearing();
        LoadAccount();
    }

    // Event Functions
    private void IncomeEntry_TextChanged(object sender, TextChangedEventArgs e) {


        if (isValidIncome(sender)) {
            IncomeEntry.TextColor = Colors.Black; // not set to defualt color so dark mode breaks

            // this is cleaner but idk if it uses more by constanty chainging the attribute
            // SaveAccount.IsVisible = CheckIncomeAndTaxChange()
            if (CheckIncomeAndTaxChange()) {
                SaveAccount.IsVisible = true;
            } else {
                SaveAccount.IsVisible = false;
            }

        } else {
            IncomeEntry.TextColor = Colors.Red;
            SaveAccount.IsVisible = false;
        }
    }

    private void TaxPicker_SelectedIndexChanged(object sender, EventArgs e) {
        if (IncomeEntry.TextColor != Colors.Red && CheckIncomeAndTaxChange()) {
            SaveAccount.IsVisible = true;
        } else {
            SaveAccount.IsVisible = false;
        }
    }

    private async void SaveAccount_Clicked(object sender, EventArgs e) {
        await Db.SaveAccount((Account)AccountStack.BindingContext);
        LoadAccount();
        SaveAccount.IsVisible = false;
    }

    // Helper Functions
    private async void LoadAccount() {
        CurrentAccount = await Db.GetAccount();
        AccountStack.BindingContext = await Db.GetAccount();
    }

    private bool CheckIncomeAndTaxChange() {
        return (CurrentAccount.Income != Double.Parse(IncomeEntry.Text) || CurrentAccount.TaxRate != TaxPicker.SelectedIndex);
    }

    private bool isValidIncome(object sender) {
        double tmp;

        if (double.TryParse(((Entry)sender).Text, out tmp)) {
            return tmp >= 0;
        }

        return false;
    }
}