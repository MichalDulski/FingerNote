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

        public NoteDatabase(string dbPath, string key)
        {
            var options = new SQLiteConnectionString(dbPath, true, key: key);
            _database = new SQLiteAsyncConnection(options);
            _database.CreateTableAsync<Note>().Wait();
            _database.InsertAsync(new Note());
        }

        public Task<Note> GetNoteAsync()
        {
            var note = _database.Table<Note>()
                .FirstAsync();
            return note;
        }
        
        public Task<int> SaveNoteAsync(Note note)
        {
            return _database.UpdateAsync(note);
        }
    }
}
