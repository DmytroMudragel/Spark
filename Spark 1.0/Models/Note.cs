using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Spark.Models
{
    public class Note
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NoteTitle { get; set; }
        public string NoteText { get; set; }
        public string NoteColor { get; set; }
        public string NoteTextColor { get; set; }
    }
}
