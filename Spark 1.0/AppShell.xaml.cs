using Spark.ViewModels;
using Spark.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Spark
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(NotesPage), typeof(NotesPage));
            Routing.RegisterRoute(nameof(TasksPage), typeof(TasksPage));
            Routing.RegisterRoute(nameof(TimerPage), typeof(TimerPage));
        }
    }
}
