using MvvmHelpers;
using MvvmHelpers.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Spark.ViewModels
{
    public class TimerViewModel : BaseViewModel
    {
        public static Stopwatch stopwatch = new Stopwatch();

        public ICommand GoPauseBtnClick { get; set; }
        public ICommand RestartBtnClick { get; set; }
        
        public TimerViewModel()
        {
            GoPauseBtnClick = new MvvmHelpers.Commands.Command(OnGoPauseBtnWasClicked);
            RestartBtnClick = new MvvmHelpers.Commands.Command(OnRestartBtnWasClicked);
            Device.StartTimer(new TimeSpan(0,0,0,0,1), () =>
            {
                CurrentTimeOnTimer = stopwatch.Elapsed.ToString("hh\\:mm\\:ss\\:fff");
                return true;
            });
            
        }

        void OnGoPauseBtnWasClicked()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
                GoPauseBtnColor = "Beige";
                GoPauseBtnTextColor = "#1b1b1b";
                GoPauseBtnText = "Pause";
                RestartBtnColor = "#1b1b1b";
                RestartBtnTextColor = "Beige";
            }
            else 
            {
                stopwatch.Stop();
                GoPauseBtnColor = "Beige";
                GoPauseBtnTextColor = "#1b1b1b";
                GoPauseBtnText = "Go";
            }
        }

        void OnRestartBtnWasClicked()
        {
            HistoryLable7 = HistoryLable6;
            HistoryLable6 = HistoryLable5;
            HistoryLable5 = HistoryLable4;
            HistoryLable4 = HistoryLable3;
            HistoryLable3 = HistoryLable2;
            HistoryLable2 = HistoryLable1;
            HistoryLable1 = PreviousTimeOnTimer;
            PreviousTimeOnTimer = stopwatch.Elapsed.ToString("hh\\:mm\\:ss\\:fff");
            stopwatch.Reset();
            CurrentTimeOnTimer = stopwatch.Elapsed.ToString("hh\\:mm\\:ss\\:fff");   
            RestartBtnColor = "Beige";
            RestartBtnTextColor = "#1b1b1b";
            GoPauseBtnColor = "#1b1b1b";
            GoPauseBtnTextColor = "Beige";
            GoPauseBtnText = "Go";
        }


        private string goPauseBtnText = "Go";
        public string GoPauseBtnText
        {
            get => goPauseBtnText;
            set => SetProperty(ref goPauseBtnText, value);
        }


        private string goPauseBtnTextColor = "Beige";
        public string GoPauseBtnTextColor
        {
            get => goPauseBtnTextColor;
            set => SetProperty( ref goPauseBtnTextColor, value);
        }


        private string goPauseBtnColor = "#1b1b1b";
        public string GoPauseBtnColor
        {
            get => goPauseBtnColor;
            set => SetProperty (ref goPauseBtnColor, value);    
        }


        private string restartBtnColor = "Beige";
        public string RestartBtnColor
        {
            get => restartBtnColor;
            set => SetProperty (ref restartBtnColor, value);
        }


        private string restartBtnTextColor = "#1b1b1b";
        public string RestartBtnTextColor
        {
            get => restartBtnTextColor;
            set => SetProperty(ref restartBtnTextColor, value);
        }


        private string currentTimeOnTimer = "00:00:00:000";
        public string CurrentTimeOnTimer
        {
            get => currentTimeOnTimer;
            set => SetProperty(ref currentTimeOnTimer, value);
        }


        private string previousTimeOnTimer = "00:00:00:000";
        public string PreviousTimeOnTimer
        {
            get => previousTimeOnTimer;
            set => SetProperty( ref previousTimeOnTimer, value);
        }


        private string historyLable1 = "00:00:00:000";
        public string HistoryLable1
        {
            get => historyLable1;
            set => SetProperty (ref historyLable1, value);
        }


        private string historyLable2 = "00:00:00:000";
        public string HistoryLable2
        {
            get => historyLable2;
            set => SetProperty (ref historyLable2, value);
        }


        private string historyLable3 = "00:00:00:000";
        public string HistoryLable3
        {
            get => historyLable3;
            set => SetProperty (ref historyLable3, value);
        }


        private string historyLable4 = "00:00:00:000";
        public string HistoryLable4
        {
            get => historyLable4;
            set => SetProperty(ref historyLable4, value);        
        }


        private string historyLable5 = "00:00:00:000";
        public string HistoryLable5
        {
            get => historyLable5;
            set => SetProperty (ref historyLable5, value);
        }


        private string historyLable6 = "00:00:00:000";
        public string HistoryLable6
        {
            get => historyLable6;
            set => SetProperty (ref historyLable6, value);
        }


        private string historyLable7 = "00:00:00:000";
        public string HistoryLable7
        {
            get => historyLable7;
            set => SetProperty (ref historyLable7, value);
        }
    }
}
