using System.Linq;
using MvvmHelpers;
using System.Windows.Input;
using Spark.Models;
using MvvmHelpers.Commands;
using System.Threading.Tasks;
using Xamarin.Forms;
using Spark.Services;
using System;
using System.Collections.Generic;

namespace Spark.ViewModels
{
    public class TasksPageViewModel : BaseViewModel
    {
        const int minHeightForTextWrite = 30;

        public ICommand AddNewTaskCommand { get; set; }

        public ObservableRangeCollection<NewTask> Tasks { get; set; }

        public MvvmHelpers.Commands.Command<Note> SelectedCommand { get; set; }

        public AsyncCommand SaveTaskCommand { get; }

        public ICommand BackFromTaskEditorCommand { get; }

        public ICommand Set1ColorCommandT { get; }

        public ICommand Set2ColorCommandT { get; }

        public ICommand Set3ColorCommandT { get; }

        public ICommand Set4ColorCommandT { get; }

        public ICommand Set5ColorCommandT { get; }

        public ICommand Set6ColorCommandT { get; }

        public ICommand Set7ColorCommandT { get; }

        public ICommand Set8ColorCommandT { get; }

        public NewTask CurrSelectedTask { get; set; }

        public DateTime lastTime = DateTime.Now;

        public MyStopwatch newTaskTime = new MyStopwatch();

        private MyStopwatch stopwatch = new MyStopwatch(new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));  

        public TasksPageViewModel()
        {
            AddNewTaskCommand = new MvvmHelpers.Commands.Command(AddNewTaskBtnClick);
            BackFromTaskEditorCommand = new MvvmHelpers.Commands.Command(BackFromTaskEditorBtnClick);
            SaveTaskCommand = new AsyncCommand(SaveTaskBtnClick);

            Set1ColorCommandT = new MvvmHelpers.Commands.Command(Set1ColorClick);
            Set2ColorCommandT = new MvvmHelpers.Commands.Command(Set2ColorClick);
            Set3ColorCommandT = new MvvmHelpers.Commands.Command(Set3ColorClick);
            Set4ColorCommandT = new MvvmHelpers.Commands.Command(Set4ColorClick);
            Set5ColorCommandT = new MvvmHelpers.Commands.Command(Set5ColorClick);
            Set6ColorCommandT = new MvvmHelpers.Commands.Command(Set6ColorClick);
            Set7ColorCommandT = new MvvmHelpers.Commands.Command(Set7ColorClick);
            Set8ColorCommandT = new MvvmHelpers.Commands.Command(Set8ColorClick);

            Tasks = new ObservableRangeCollection<NewTask>() { };
            stopwatch.Start();

            Device.StartTimer(new TimeSpan(0, 0, 0, 0,300), () =>
            {
               
                if (TimeProgressBarValue <= 15)
                {
                    CurrentTime = "";
                }
                else
                {
                    CurrentTime = DateTime.Now.ToString("HH:mm:ss");
                }

                if (stopwatch.Elapsed.Hours == 0 && stopwatch.Elapsed.Minutes == 0 && stopwatch.Elapsed.Seconds == 0)
                {
                    Task.Run(async () => await LoadTasks());
                }

                TimeProgressBarValue = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
                int res = 0;
                if (Tasks.Count != 0)
                {
                    foreach (var task in Tasks)
                    {
                        res += task.TaskHeight;
                    }
                    DefaultFrameHeight = Math.Abs(TimeProgressBarValue - res);
                }
                else
                {
                    DefaultFrameHeight = TimeProgressBarValue;
                    newTaskTime = stopwatch;
                }
                if (DefaultFrameHeight <= 50)
                {
                    PlusButtonImage = "";
                }
                else
                {
                    PlusButtonImage = "outline_add_circle_outline_black_242.png";
                }
                return true;
            });
            Task.Run(async () => await LoadTasks());
            //Task.Run(async () => await NewTaskService.RemoveAllTasks());
        }



        #region NoteEditor

        public static string GetHexString(Color color)
        {
            var red = (int)(color.R * 255);
            var green = (int)(color.G * 255);
            var blue = (int)(color.B * 255);
            var alpha = (int)(color.A * 255);
            var hex = $"#{alpha:X2}{red:X2}{green:X2}{blue:X2}";
            return hex;
        }

        private bool SaveTaskHandlerAlreadyRunning = false;

        private async Task SaveTaskBtnClick()
        {

            if (SaveTaskHandlerAlreadyRunning) { return;}
            SaveTaskHandlerAlreadyRunning = true;
            IsTaskEditorPageIsActive = false;
            var dbTasks = await NewTaskService.GetAllTasks();
            var todayTasks = dbTasks.Where(task => task.TaskDate.Day == DateTime.Now.Day);
            List<int> dbIds = new List<int>() { };
            foreach (var task in todayTasks)
            {
                dbIds.Add(task.Id);
            }
            if (dbIds.Contains(CurrSelectedTask.Id))
            {
                NewTask newTask = new NewTask(CurrSelectedTask.Id, TaskEditorEditorText, GetHexString(TaskChosenColor),
                  GetHexString((Color)Application.Current.Resources["PrimaryDark"]), CurrSelectedTask.TaskHeight, TaskChosenType,
                  CurrSelectedTask.IsCanWriteText, CurrSelectedTask.TaskDuration, (int)Math.Floor((double)(CurrSelectedTask.TaskHeight - 30) / 20));
                await NewTaskService.AddTask(newTask);
            }
            else
            {
                NewTask newTask = new NewTask(CurrSelectedTask.Id, TaskEditorEditorText,
                    GetHexString(TaskChosenColor), GetHexString((Color)Application.Current.Resources["PrimaryDark"]),
                    DefaultFrameHeight, TaskChosenType,DefaultFrameHeight >= minHeightForTextWrite,
                    new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second) - SumAllDurations(todayTasks),
                    (int)Math.Floor((double)(DefaultFrameHeight - 30) / 20));
                int res = 0;
                foreach (var dbTask in todayTasks)
                {
                    res += dbTask.TaskHeight;
                }
                if (res + newTask.TaskHeight > TimeProgressBarValue)
                {
                    newTask.TaskHeight = TimeProgressBarValue - res;
                }
                await NewTaskService.AddTask(newTask);
                DefaultFrameHeight = 0;
                newTaskTime.Restart();
            }
            await RefreshTasks(); 
            OnPropertyChanged(nameof(Tasks));
            BackFromTaskEditorBtnClick();
            SaveTaskHandlerAlreadyRunning = false; 
        }

        private TimeSpan SumAllDurations(IEnumerable<NewTask> tasks)
        {
            TimeSpan sum = new TimeSpan();
            foreach (var task in tasks)
            {
                sum += task.TaskDuration;
            }
            return sum;
        }

        private async Task RefreshTasks()
        {
            if (Tasks.Count.Equals(null))
            {
                return;
            }

            //var dbTasksCount = await NewTaskService.GetAllTasks();
            //List<int> ids = new List<int>();
            //foreach (var task in Tasks)
            //{
            //    ids.Add(task.Id);
            //}
            //for (int i = 0; i < dbTasksCount.Count(); i++)
            //{
            //    var result = dbTasksCount.ElementAt(i);
            //    if (!ids.Contains(result.Id))
            //    {
            //        Tasks.Add(result);
            //    }
            //    foreach (var task in Tasks)
            //    {
            //        if (task != result && task.Id == result.Id)
            //        {
            //            task.TaskText = result.TaskText;
            //            task.TaskColor = result.TaskColor;
            //            task.TaskTypeLable = result.TaskTypeLable;
            //            task.MaxLinesVisible = result.MaxLinesVisible;
            //        }
            //    }
            //}
            //OnPropertyChanged(nameof(Tasks));

            Tasks.Clear();
            await LoadTasks();
        }

        private async Task LoadTasks()
        {
            var db = await NewTaskService.GetAllTasks();
            var todayTasks = db.Where(task => task.TaskDate.Day == DateTime.Now.Day);
            Tasks.AddRange(todayTasks);
        }

        private void BackFromTaskEditorBtnClick()
        {
            IsTaskEditorPageIsActive = false;
            TaskEditorEditorText = "";
            TaskChosenColor = (Color)Application.Current.Resources["NoteColorBeige"];
            TaskChosenType = TaskTypeLblForFree;
        }

        private void AddNewTaskBtnClick()
        {
            CurrSelectedTask = new NewTask() { };
            IsTaskEditorPageIsActive = true;
        }

        private void TaskIsSelected(NewTask currentSelectedTask)
        {
            CurrSelectedTask = currentSelectedTask;
            IsTaskEditorPageIsActive = true;
            TaskEditorEditorText = CurrSelectedTask.TaskText;
            CurrSelectedTask.TaskTextColor = GetHexString((Color)Application.Current.Resources["PrimaryDark"]);
            TaskChosenColor = Color.FromHex(CurrSelectedTask.TaskColor);
        }

        private NewTask selctedtask;
        public NewTask SelectedTask
        {
            get => selctedtask;
            set
            {
                if (value != null)
                {
                    TaskIsSelected(value);
                    value = null;
                }
                selctedtask = value;
                OnPropertyChanged(nameof(SelectedTask));
            }
        }

        private int timeProgressBarValue = DateTime.Now.Hour * 60 + DateTime.Now.Minute;// - 57;
        public int TimeProgressBarValue
        {
            get => DateTime.Now.Hour * 60 + DateTime.Now.Minute;// - 57;
            set => SetProperty(ref timeProgressBarValue, value);
        }

        private static int defaultFrameHeight = 0;
        public int DefaultFrameHeight
        {
            get => defaultFrameHeight;
            set => SetProperty(ref defaultFrameHeight, value);
        }

        private int hourCellHeaderHeight = 15;
        public int HourCellHeaderHeight
        {
            get => hourCellHeaderHeight;
            set => SetProperty(ref hourCellHeaderHeight, value);
        }

        private int hourCellHeight = 20;
        public int HourCellHeight
        {
            get => hourCellHeight;
            set => SetProperty(ref hourCellHeight, value);
        }

        private string plusButtonImage = "outline_add_circle_outline_black_242.png";
        public string PlusButtonImage
        {
            get => plusButtonImage;
            set => SetProperty(ref plusButtonImage, value);
        }

        private string currentTime = DateTime.Now.ToString("HH:mm:ss");
        public string CurrentTime
        {
            get => currentTime;
            set => SetProperty(ref currentTime, value);
        }

        private bool isTaskEditorPageIsActive = false;
        public bool IsTaskEditorPageIsActive
        {
            get => isTaskEditorPageIsActive;
            set => SetProperty(ref isTaskEditorPageIsActive, value);
        }

        private bool isCanWriteText = false;
        public bool IsCanWriteText
        {
            get => isCanWriteText;
            set => SetProperty(ref isCanWriteText, value);
        }

        private string taskEditorEditorText = "";
        public string TaskEditorEditorText
        {
            get => taskEditorEditorText;
            set => SetProperty(ref taskEditorEditorText, value);
        }

        #region Colors Circles

        private Color taskChosenColor = (Color)Application.Current.Resources["NoteColorBeige"];
        public Color TaskChosenColor
        {
            get => taskChosenColor;
            set => SetProperty(ref taskChosenColor, value);
        }


        private Color color1T = (Color)Application.Current.Resources["NoteColorBeige"];
        public Color Color1T
        {
            get => color1T;
            set => SetProperty(ref color1T, value);
        }
        private void Set1ColorClick()
        {
            TaskChosenColor = Color1T;
            TaskChosenType = taskTypeLblForFree;
        }

        private Color color2T = (Color)Application.Current.Resources["NoteColorOrange"];

        public Color Color2T
        {
            get => color2T;
            set => SetProperty(ref color2T, value);
        }
        private void Set2ColorClick()
        {
            TaskChosenColor = Color2T;
            TaskChosenType = taskTypeLblForMinor;
        }

        private Color color3T = (Color)Application.Current.Resources["NoteColorYellow"];
        public Color Color3T
        {
            get => color3T;
            set => SetProperty(ref color3T, value);
        }
        private void Set3ColorClick()
        {
            TaskChosenColor = Color3T;
            TaskChosenType = taskTypeLblForTrain;
        }

        private Color color4T = (Color)Application.Current.Resources["NoteColorLightGreen"];
        public Color Color4T
        {
            get => color4T;
            set => SetProperty(ref color4T, value);
        }
        private void Set4ColorClick()
        {
            TaskChosenColor = Color4T;
            TaskChosenType = taskTypeLblForEat;
        }

        private Color color5T = (Color)Application.Current.Resources["CoolBlue"];
        public Color Color5T
        {
            get => color5T;
            set => SetProperty(ref color5T, value);
        }
        private void Set5ColorClick()
        {
            TaskChosenColor = Color5T;
            TaskChosenType = taskTypeLblForStudy;
        }

        private Color color6T = (Color)Application.Current.Resources["NoteColorLigthBlue"];
        public Color Color6T
        {
            get => color6T;
            set => SetProperty(ref color6T, value);
        }
        private void Set6ColorClick()
        {
            TaskChosenColor = Color6T;
            TaskChosenType = taskTypeLblForSleep;
        }

        private Color color7T = (Color)Application.Current.Resources["NoteColorPurple"];
        public Color Color7T
        {
            get => color7T;
            set => SetProperty(ref color7T, value);
        }
        private void Set7ColorClick()
        {
            TaskChosenColor = Color7T;
            TaskChosenType = taskTypeLblForFun;
        }

        private Color color8T = (Color)Application.Current.Resources["CoolRed"];
        public Color Color8T
        {
            get => color8T;
            set => SetProperty(ref color8T, value);
        }
        private void Set8ColorClick()
        {
            TaskChosenColor = Color8T;
            TaskChosenType = TaskTypeLblForWork;
        }

        private string taskChosentype = "FREE";
        public string TaskChosenType
        {
            get => taskChosentype;
            set => SetProperty(ref taskChosentype, value);
        }

        private string taskTypeLblForFree = "FREE";
        public string TaskTypeLblForFree
        {
            get => taskTypeLblForFree;
            set => SetProperty(ref taskTypeLblForFree, value);
        }

        private string taskTypeLblForEat = "EAT";
        public string TaskTypeLblForEat
        {
            get => taskTypeLblForEat;
            set => SetProperty(ref taskTypeLblForEat, value);
        }

        private string taskTypeLblForSleep = "SLEEP";
        public string TaskTypeLblForSleep
        {
            get => taskTypeLblForSleep;
            set => SetProperty(ref taskTypeLblForSleep, value);
        }

        private string taskTypeLblForTrain = "TRAIN";
        public string TaskTypeLblForTrain
        {
            get => taskTypeLblForTrain;
            set => SetProperty(ref taskTypeLblForTrain, value);
        }

        private string taskTypeLblForWork = "WORK";
        public string TaskTypeLblForWork
        {
            get => taskTypeLblForWork;
            set => SetProperty(ref taskTypeLblForWork, value);
        }

        private string taskTypeLblForMinor = "MINOR";
        public string TaskTypeLblForMinor
        {
            get => taskTypeLblForMinor;
            set => SetProperty(ref taskTypeLblForMinor, value);
        }

        private string taskTypeLblForStudy = "STUDY";
        public string TaskTypeLblForStudy
        {
            get => taskTypeLblForStudy;
            set => SetProperty(ref taskTypeLblForStudy, value);
        }

        private string taskTypeLblForFun = "FUN";
        public string TaskTypeLblForFun
        {
            get => taskTypeLblForFun;
            set => SetProperty(ref taskTypeLblForFun, value);
        }


        #endregion

        #endregion
    }
}
