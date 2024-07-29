using SQLite;
using System.Collections;

namespace BudgetAppLibray {
    public class LocalDbService {
        private const string DB_NAME = "profile_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService() {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));
            _connection.CreateTableAsync<Profile>().Wait();
            _connection.CreateTableAsync<Expense>().Wait();
        }

        public async Task AddDefaultObjectsIfNeededAsync() {
            var defaultsAdded = Preferences.Get("DefaultsAdded", false);

            if (!defaultsAdded) {
                // Add defaults to the database
                Profile d = new Profile { Name = "Empty" };
                Profile d1 = new Profile { Name = "50 30 20 Rule" };

                Expense e1 = new Expense { Type = "Percent", ExpenseName = "Needs", Value = 50 };
                Expense e2 = new Expense { Type = "Percent", ExpenseName = "Wants", Value = 30 };
                Expense e3 = new Expense { Type = "Percent", ExpenseName = "Savings", Value = 20 };

                d1.addExpense(e1);
                d1.addExpense(e2);
                d1.addExpense(e3);

                await InsertProfileWithExpensesAsync(d);
                await InsertProfileWithExpensesAsync(d1);

                Preferences.Set("DefaultsAdded", true);
            }
        }

        public async Task InsertProfileWithExpensesAsync(Profile profile) {
            await _connection.InsertAsync(profile);

            foreach (var expense in profile.Expenses) {
                expense.ProfileId = profile.Id;
                await _connection.InsertAsync(expense);
            }
        }

        public async Task<List<Expense>> GetExpenses(Profile profile) {
            return await _connection.Table<Expense>().Where(e => e.ProfileId == profile.Id).ToListAsync();
        }

        public async Task<List<Profile>> GetProfiles() {
            return await _connection.Table<Profile>().ToListAsync();
        }

        public async Task SaveProfile(Profile p) {
            await _connection.UpdateAsync(p);

            
            // maybe we need to save all the expenses to again
        }

        public async Task SaveExpense(Expense e) {
            await _connection.UpdateAsync(e);
        }

        public async Task DeleteExpense(Expense e) {
            await _connection.DeleteAsync(e);
        }

        public async Task AddExpense(Expense e) {
            await _connection.InsertAsync(e);
        }

        public async Task ClearEmptyProfile() {
            Profile Empty = await _connection.Table<Profile>().Where(p => p.Name.Equals("Empty")).FirstAsync();

            await _connection.Table<Expense>().Where(e => e.ProfileId == Empty.Id).DeleteAsync();
        }

        public async Task DeleteProfile(Profile selected) {
            await _connection.DeleteAsync(selected);
        }
    }
}
