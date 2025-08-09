using BudgetAppLibray;

namespace BudgetAppProject {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            bool auth = SecureStorage.GetAsync("isAuth").Result == "true";

            if (auth) {
                MainPage = new AppShell();
            } else {
                MainPage = new LoginShell();
            }
        }
    }
}
