using BudgetAppLibray;

namespace BudgetAppProject {

    public partial class App : Application {
        public static ExpensesViewModel MainProfileAndExpenseViewModel { get; private set; }
        public static LocalDbService Db {  get; private set; }


        public App(IServiceProvider serviceProvider) {
            InitializeComponent();

            Db = new LocalDbService();
            MainProfileAndExpenseViewModel = new ExpensesViewModel(Db);

            _ = InitAsync();

            MainPage = new AppShell();
        }

        private async Task InitAsync() {
            await Db.AddDefaultObjectsIfNeededAsync();
            MainProfileAndExpenseViewModel.Init();
        }
    }
}
