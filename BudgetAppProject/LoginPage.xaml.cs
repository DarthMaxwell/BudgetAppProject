namespace BudgetAppProject;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
        InitializeComponent();
        clearLoginForm();
        clearRegForm();
    }

    private async void LoginButton_Clicked(object sender, EventArgs e) {
        bool isAuth = await AuthUser();

        if (isAuth) {
            await SecureStorage.SetAsync("isAuth", "true");

            Application.Current.MainPage = new AppShell(); //changed this
        }
    }

    private async Task<bool> AuthUser() {
        // NEED TO IMPLEMENT LOGIC HERE and this should be in a library thign
        return true;
    }

    private void RegisterButton_Clicked(object sender, EventArgs e) {

    }

    private void SwapToRegButton_Clicked(object sender, EventArgs e) {
        loginFrame.IsVisible = false;
        regFrame.IsVisible = true;
        clearLoginForm();
    }

    private void SwapToLogButton_Clicked(object sender, EventArgs e) {
        regFrame.IsVisible = false;
        loginFrame.IsVisible = true;
        clearRegForm();
    }

    // If its enabled or disbaled it looks different??
    private void checkCanSignIn(object sender, TextChangedEventArgs e) {
        if (username.Text != "" && password.Text != "") {
            // Later can check for password length and other rules
            signInButton.IsEnabled = true;
        } else {
            signInButton.IsEnabled = false;
        }
    }

    private void checkCanReg(object sender, TextChangedEventArgs e) {
        if (regusername.Text != "" && regpassword.Text != "" && regpassword.Text.Equals(confirmpassword.Text)) {
            // also need to check with password rules
            regButton.IsEnabled = true;
        } else {
            regButton.IsEnabled = false;
        }
    }
    private void clearLoginForm() {
        username.Text = "";
        password.Text = "";
    }

    private void clearRegForm() {
        regusername.Text = "";
        regpassword.Text = "";
        confirmpassword.Text = "";
    }
}