using Spark.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;
using SQLite;

namespace Spark.Services
{
    public static class NoteService
    {
        static SQLiteAsyncConnection db;
        private static async Task Init()
        {
            if (db != null)
            {
                return;
            }
            var dataBasePath = Path.Combine(FileSystem.AppDataDirectory, "Notes.db");
            db = new SQLiteAsyncConnection(dataBasePath);
            await db.CreateTableAsync<Note>();
        }

        public static async Task<int> AddNote(Note noteToAdd)
        {
            await Init();   
            if (noteToAdd.Id != 0)
            {
                return await db.UpdateAsync(noteToAdd);
            }
            else
            {
                return await db.InsertAsync(noteToAdd);
            }
        }

        public static async Task RemoveNote(int id)
        {
            await Init();
            await db.DeleteAsync<Note>(id);
        }

        public static async Task<IEnumerable<Note>> GetAllNotes()
        {
            await Init();
            return await db.Table<Note>().ToListAsync();
        }

        public static async Task<Note> GetNote(int id)
        {
            await Init();
            if (id != 0)
            {
                return await db.GetAsync<Note>(id);
            }
            return null;
        }
    }
}
