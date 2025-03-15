using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Spark.ViewModels;

namespace Spark
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new SparkTabbedPages();
        }

        protected override void OnStart()
        {
            //tasksPageViewModel.lastTime = DateTime.Now;
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {
        }
    }
}
