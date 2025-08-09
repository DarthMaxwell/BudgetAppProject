using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BudgetAppLibray {
    public class ExpensesViewModel : INotifyPropertyChanged {
        private readonly LocalDbService _dbService;
        public ObservableCollection<Profile> Profiles { get; set; } = new();
        public ObservableCollection<Expense> Expenses { get; set; } = new();

        private Profile selectedProfile;
        public Profile SelectedProfile {
            get => selectedProfile;
            set {
                if (selectedProfile != value) {
                    selectedProfile = value;
                    OnPropertyChanged();
                    LoadExpensesForProfile(selectedProfile);
                }
            }
        }

        public ExpensesViewModel(LocalDbService dbService) {
            _dbService = dbService;
            LoadProfiles();
        }

        private async void LoadProfiles() {
            var profilesFromDb = await _dbService.GetProfiles();
            Profiles.Clear();
            foreach (var profile in profilesFromDb)
                Profiles.Add(profile);
        }

        private async void LoadExpensesForProfile(Profile profile) {
            if (profile == null) {
                Expenses.Clear();
                return;
            }

            var expensesFromDb = await _dbService.GetExpenses(profile);

            Expenses.Clear();
            foreach (var expense in expensesFromDb)
                Expenses.Add(expense);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
