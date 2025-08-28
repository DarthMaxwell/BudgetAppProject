using BudgetAppLibray;

namespace BudgetAppProject {

    public partial class App : Application {
        public static ExpensesViewModel MainProfileAndExpenseViewModel { get; private set; }
        public static LocalDbService Db {  get; private set; }


        public App(IServiceProvider serviceProvider) {
            InitializeComponent();

            Db = new LocalDbService();
            MainProfileAndExpenseViewModel = new ExpensesViewModel(Db);

            _ = MainProfileAndExpenseViewModel.InitAsync();

            MainPage = new AppShell();
        }
    }
}
