using Spark.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using SQLite;

namespace Spark.Services
{
    public static class NewTaskService
    {
        static SQLiteAsyncConnection db;
        private static async Task Init()
        {
            if (db != null)
            {
                return;
            }
            var dataBasePath = Path.Combine(FileSystem.AppDataDirectory, "Tasks.db");
            db = new SQLiteAsyncConnection(dataBasePath);
            await db.CreateTableAsync<NewTask>();
        }

        public static async Task<int> AddTask(NewTask taskToAdd)
        {
            await Init();
            if (taskToAdd.Id != 0)
            {
                return await db.UpdateAsync(taskToAdd);
            }
            else
            {
                return await db.InsertAsync(taskToAdd);
            }
        }

        public static async Task RemoveTask(int id)
        {
            await Init();
            await db.DeleteAsync<NewTask>(id);
        }

        public static async Task<IEnumerable<NewTask>> GetAllTasks()
        {
            await Init();
            return await db.Table<NewTask>().ToListAsync();
        }

        public static async Task<int> RemoveAllTasks()
        {
            await Init();
            return await db.DeleteAllAsync<NewTask>();
        }

        public static async Task<NewTask> GetTask(int id)
        {
            await Init();
            if (id != 0)
            {
                return await db.GetAsync<NewTask>(id);
            }
            return null;
        }
    }
}
