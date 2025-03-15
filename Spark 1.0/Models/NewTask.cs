using Spark.ViewModels;
using SQLite;
using System;
using Xamarin.Forms;

namespace Spark.Models
{
    public class NewTask
    {
        [PrimaryKey, AutoIncrement]

        public int Id { get; set; }
        public string TaskText { get; set; }
        public string TaskColor { get; set; }
        public string TaskTextColor { get; set; }
        public int TaskHeight { get; set; }
        public string TaskTypeLable { get; set; }
        public DateTime TaskDate { get; set; }
        public bool IsCanWriteText { get; set; }
        public TimeSpan TaskDuration { get; set; }
        public int MaxLinesVisible { get; set; }

        public NewTask()
        {
            Id = 0;
            TaskText = "";
            TaskColor = TasksPageViewModel.GetHexString((Color)Application.Current.Resources["NoteColorBeige"]);
            TaskHeight = 0;
            TaskTextColor = TasksPageViewModel.GetHexString((Color)Application.Current.Resources["PrimaryDark"]);
            TaskTypeLable = "FREE";
            TaskDate = DateTime.Now;
            IsCanWriteText = false;
            TaskDuration = TimeSpan.Zero;
            MaxLinesVisible = 1;
        }

        public NewTask(int id, string taskText, string taskColor, string taskTextColor,
            int taskHeight, string taskTypeLable, bool isCanWriteText, TimeSpan taskDuration, int maxLinesVisible)
        {
            Id = id;
            TaskText = taskText;
            TaskColor = taskColor;
            TaskTextColor = taskTextColor;
            TaskHeight = taskHeight;
            TaskTypeLable = taskTypeLable;
            TaskDate = DateTime.Now;
            IsCanWriteText = isCanWriteText;
            TaskDuration = taskDuration;
            MaxLinesVisible = maxLinesVisible;
        }
    }
}
