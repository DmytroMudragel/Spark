using Spark.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using SQLite;

namespace Spark.Services
{
    public static class TaskDataStorage
    {
        static SQLiteAsyncConnection TasksStoragedb;
        private static async Task Init()
        {
            if (TasksStoragedb != null)
            {
                return;
            }
            var dataBasePath = Path.Combine(FileSystem.AppDataDirectory, "DataStorage.db");
            TasksStoragedb = new SQLiteAsyncConnection(dataBasePath);
            await TasksStoragedb.CreateTableAsync<DayData>();
        }

        public static async Task<int> AddDay(DayData dayToAdd)
        {
            await Init();
            if (dayToAdd.Id != 0)
            {
                return await TasksStoragedb.UpdateAsync(dayToAdd);
            }
            else
            {
                return await TasksStoragedb.InsertAsync(dayToAdd);
            }
        }

        public static async Task RemoveDay(int id)
        {
            await Init();
            await TasksStoragedb.DeleteAsync<DayData>(id);
        }

        public static async Task<IEnumerable<DayData>> GetAllDays()
        {
            await Init();
            return await TasksStoragedb.Table<DayData>().ToListAsync();
        }

        public static async Task<int> RemoveAllDays()
        {
            await Init();
            return await TasksStoragedb.DeleteAllAsync<DayData>();
        }

        public static async Task<DayData> GetDay(int id)
        {
            await Init();
            if (id != 0)
            {
                return await TasksStoragedb.GetAsync<DayData>(id);
            }
            return null;
        }
    }
}

