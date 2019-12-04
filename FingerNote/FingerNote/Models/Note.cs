using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace FingerNote.Models
{
    public class Note
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Content { get; set; }
    }
}
