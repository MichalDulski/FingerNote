using FingerNote.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FingerNote.Data
{
    public class NoteDatabase
    {
        readonly SQLiteAsyncConnection _database;

        public NoteDatabase(string dbPath)
        {
            var options = new SQLiteConnectionString(dbPath, true);
            _database = new SQLiteAsyncConnection(options);
            _database.CreateTableAsync<Note>().Wait();
        }

        public Task<Note> GetNoteAsync()
        {
            var note = _database.Table<Note>()
                .FirstAsync();
            return note;
        }
        
        public Task<int> SaveNoteAsync(Note note)
        {
            var xd = _database.Table<Note>().CountAsync();
            if (xd.Result == 0)
                return _database.InsertAsync(note);
            return _database.UpdateAsync(note);
        }
    }
}
