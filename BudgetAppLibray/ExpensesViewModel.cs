using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BudgetAppLibray {
    public class ExpensesViewModel : INotifyPropertyChanged {
        private readonly LocalDbService _dbService;
        public ObservableCollection<Profile> Profiles { get; set; } = new();

        private Profile selectedProfile;
        public Profile SelectedProfile {
            get => selectedProfile;
            set {
                if (selectedProfile != value) {
                    selectedProfile = value;
                    OnPropertyChanged();
                }
            }
        }

        public ExpensesViewModel(LocalDbService dbService) {
            _dbService = dbService;
        }

        public void Init() {
            var profilesFromDb = _dbService.GetProfilesAndExpenses();
            Profiles.Clear();
            foreach (var profile in profilesFromDb)
                Profiles.Add(profile);

            SelectedProfile = Profiles.First();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
