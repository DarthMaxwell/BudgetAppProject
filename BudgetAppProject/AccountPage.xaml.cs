using BudgetAppLibray;

namespace BudgetAppProject;

public partial class AccountPage : ContentPage
{
    private readonly LocalDbService _dbService;

    public AccountPage()
	{
		InitializeComponent();
        _dbService = App.Services.GetService<LocalDbService>();

        LoadAccount();

        double[] taxPercentage = [.. Enumerable.Range(0, 100)];
        TaxPicker.ItemsSource = taxPercentage;
    }

    private async void LoadAccount() {
        AccountStack.BindingContext = await _dbService.GetAccount();
    }

    private async void SaveAccount_Clicked(object sender, EventArgs e) {
        await _dbService.SaveAccount((Account)AccountStack.BindingContext);
    }
}