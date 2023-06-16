using System;
using TermPlanner.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermPlanner
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var homePage = new Home();
            var navPage = new NavigationPage(homePage);
            MainPage = navPage; 
            //MainPage = new NavigationPage(new Home());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
