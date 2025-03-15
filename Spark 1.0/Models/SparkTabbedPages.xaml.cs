using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Spark
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SparkTabbedPages : TabbedPage
    {
        public SparkTabbedPages()
        {
            InitializeComponent();
            var pages = Children.GetEnumerator();
            pages.MoveNext(); // First page
            pages.MoveNext(); // Second page
            CurrentPage = pages.Current;
        }
    }
}