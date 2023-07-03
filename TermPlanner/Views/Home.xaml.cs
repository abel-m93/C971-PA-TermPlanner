using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermPlanner.Services;
using TermPlanner.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.LocalNotifications;

namespace TermPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTerm());
        }

        protected override async void OnAppearing()
        {   
            base.OnAppearing();

            //Populates the Home Screen with existing Terms
            var allTerms = await DatabaseServices.GetTerms();
            TermCollectionView.ItemsSource = allTerms;

            var courseList = await DatabaseServices.GetCourses();
            var assessmentList = await DatabaseServices.GetAssessments();

            int notificationId = 0;

            foreach (Course course in courseList)
            {
                if (course.AlertOn == true)
                {
                    if (course.StartDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"{course.Name} starts today!", notificationId);
                        notificationId++;
                    }

                    if (course.EndDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"{course.Name} ends today!", notificationId);
                        notificationId++;
                    }
                }
            }

            foreach (Assessment assessment in assessmentList)
            {
                if (assessment.AlertOn == true)
                {
                    if(assessment.StartDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"{assessment.Name} Starts today!", notificationId);
                        notificationId++;
                    }
                    if (assessment.DueDate == DateTime.Today)
                    {
                        CrossLocalNotifications.Current.Show("Alert", $"{assessment.Name} Due today!", notificationId);
                        notificationId++;
                    }
                }
            }
            //ADD NOTIFICATION FUNCTIONALITY HERE to APPEAR ONCE HOME SCREEN IS ON SCREEN

        }


        private async void TermCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Term term = (Term)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new EditTerm(term));
            }
        }

        //TESTING METHODS
        //METHODS AND BUTTONS FOR TESTING PURPOSES
        private async void ClearDataBtn_Clicked(object sender, EventArgs e)
        {
            await DatabaseServices.ClearSampleData();
            await DisplayAlert("ATTENTION", "Sample Data Cleared", "OK");

            var allTerms = await DatabaseServices.GetTerms();
            TermCollectionView.ItemsSource = allTerms;

        }

        //METHOD AND BUTTON FOR TESTING PURPOSES
        private async void LoadDataBtn_Clicked(object sender, EventArgs e)
        {
            await DatabaseServices.LoadSampleData();
            await DisplayAlert("ATTENTION", "Sample Data Loaded", "OK");

            var allTerms = await DatabaseServices.GetTerms();
            TermCollectionView.ItemsSource = allTerms;


            //Testing if allTerms is Null
            /*            await DisplayAlert("Total Term Count", $"{allTerms.Count()}", "OK");
                        foreach (var term in allTerms)
                        {
                            await DisplayAlert("OBJECT NULL TEST", $"Current ID: {term.Id}\nName: {term.Name}\nStatus: {term.Status}\nStart Date: {term.StartDate.Date}", "OK");
                        }*/


            // CollectionView TermCollectionView = new CollectionView();
            //TermCollectionView.ItemsSource = allTerms;
            //TermCollectionView.ItemsSource = allTerms;         
        }
    }
}