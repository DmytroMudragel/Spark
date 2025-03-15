using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Spark.Models
{
    public class DayData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime date { get; set; }
        public List<NewTask> Tasks { get; set; }

    }
}
