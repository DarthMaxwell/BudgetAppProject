using SQLite;
using System.Collections;
using System.Threading.Tasks;

namespace BudgetAppLibray {
    public class LocalDbService { //NEEDS a clean up
        private const string DB_NAME = "profile_local_db.db3";
        private readonly SQLiteAsyncConnection _connection;

        public LocalDbService() {
            _connection = new SQLiteAsyncConnection(Path.Combine(FileSystem.AppDataDirectory, DB_NAME));

            //PURGE
            //_connection.ExecuteAsync("DROP TABLE IF EXISTS Profile");
            //_connection.ExecuteAsync("DROP TABLE IF EXISTS Expense");
            //_connection.ExecuteAsync("DROP TABLE IF EXISTS Account");
            //Preferences.Clear();

            _connection.CreateTableAsync<Profile>().Wait();
            _connection.CreateTableAsync<Expense>().Wait();
            _connection.CreateTableAsync<Account>().Wait();
        }

        public async Task AddDefaultObjectsIfNeededAsync() {
            var defaultsAdded = Preferences.Get("DefaultsAdded", false);

            if (!defaultsAdded) {
                // Add defaults to the database
                // might want to rewrite this where we just ad each thing but use id so database not with a list
                Profile d = new Profile { Name = "50 30 20 Rule" };

                Expense e1 = new Expense { Type = "Percent", ExpenseName = "Needs", Value = 50 };
                Expense e2 = new Expense { Type = "Percent", ExpenseName = "Wants", Value = 30 };
                Expense e3 = new Expense { Type = "Percent", ExpenseName = "Savings", Value = 20 };

                d.Expenses.Add(e1);
                d.Expenses.Add(e2);
                d.Expenses.Add(e3);

                await AddAccount(new Account(10000, 24));
                await InsertProfileWithExpensesAsync(d);

                Preferences.Set("DefaultsAdded", true);
            }
        }

        private async Task AddAccount(Account account) {
            await _connection.InsertAsync(account);
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
            if (p.Id == 0) { //new Profile dosnt have auto inc yet
                //await addProfile(p);
                await InsertProfileWithExpensesAsync(p);
            } else {
                await _connection.UpdateAsync(p);
            }
        }

        public async Task AddProfile(Profile p) {
            await _connection.InsertAsync(p);
        }

        public async Task SaveExpense(Expense e) {
            if (e.Id == 0) { //new Expense dosnt have auto inc yet
                await AddExpense(e);
            } else {  
                await _connection.UpdateAsync(e);
            }
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

        public async Task<Account> GetAccount() {
            return await _connection.Table<Account>().FirstAsync();
        }

        public async Task SaveAccount(Account a) {
            await _connection.UpdateAsync(a);
        }
    }
}
