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
                // might want to rewrite this where we just ad each thing but use id so database not with a list
                Profile d = new Profile { Name = "Empty" };
                Profile d1 = new Profile { Name = "50 30 20 Rule" };

                Expense e1 = new Expense { Type = "Percent", ExpenseName = "Needs", Value = 50 };
                Expense e2 = new Expense { Type = "Percent", ExpenseName = "Wants", Value = 30 };
                Expense e3 = new Expense { Type = "Percent", ExpenseName = "Savings", Value = 20 };

                d1.Expenses.Add(e1);
                d1.Expenses.Add(e2);
                d1.Expenses.Add(e3);

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

        // should rename to save or add
        public async Task SaveProfile(Profile p) {
            if (_connection.Table<Profile>().FirstOrDefaultAsync() == null) {
                await AddProfile(p);
            }

            await _connection.UpdateAsync(p);
        }

        private async Task AddProfile(Profile p) {
            await _connection.InsertAsync(p);
        }

        public async Task SaveExpense(Expense e) {
            if (_connection.Table<Expense>().FirstOrDefaultAsync() == null) {
                await AddExpense(e);
            }

            await _connection.UpdateAsync(e);
        }

        public async Task DeleteExpense(Expense e) {
            await _connection.DeleteAsync(e);
        }

        public async Task AddExpense(Expense e) {
            await _connection.InsertAsync(e);
        }

        public async Task DeleteProfile(Profile selected) {
            List<Expense> exp = await GetExpenses(selected);

            foreach (var x in exp)
            {
                // Dont need to await bec we await to delete the profile so we cant access these expenses after
                await DeleteExpense(x);
            }            

            await _connection.DeleteAsync(selected);
        }

        // PURGE DATABASE func add here
    }
}
