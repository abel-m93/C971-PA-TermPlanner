using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermPlanner.Models;
using TermPlanner.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseEdit : ContentPage
    {
        private readonly int SelectedCourseId;
        public CourseEdit()
        {
            InitializeComponent();
        }

        public CourseEdit(Course course)
        {
            InitializeComponent();

            SelectedCourseId = course.Id;

            CourseId.Text = course.Id.ToString();
            CourseName.Text = course.Name;
            CourseStatusPicker.SelectedItem = course.Status;
            CourseStartPicker.Date = course.StartDate;
            CourseEndPicker.Date = course.EndDate;
            AlertOn.IsToggled = course.AlertOn;
            InstrName.Text = course.InstrName;
            InstrPhone.Text = course.InstrPhone;
            InstrEmail.Text = course.InstrEmail;
            CourseNotes.Text = course.Notes;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //var assessments = await DatabaseServices.GetAssessments(SelectedCourseId);
            //int courseCount = courses.Count();
            //courseCountLabel.Text = $"Course Count:\t{courseCount}/0";
            AssessmentCollectionView.ItemsSource = await DatabaseServices.GetAssessments(SelectedCourseId);
        }
        async void SaveCourse_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CourseName.Text))
            {
                await DisplayAlert("Incomplete Data", "Please enter a name", "OK");
                return;
            }

            if (CourseStatusPicker.SelectedItem == null || string.IsNullOrWhiteSpace(CourseStatusPicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Status", "Please enter a Status", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(InstrName.Text))
            {
                await DisplayAlert("Incomplete Data", "Please enter Instructor name", "OK");
                return;
            }

            await DatabaseServices.UpdateCourse(SelectedCourseId, CourseName.Text, CourseStatusPicker.SelectedItem.ToString(),
                CourseStartPicker.Date, CourseEndPicker.Date, AlertOn.IsToggled, InstrName.Text,
                InstrPhone.Text, InstrEmail.Text, CourseNotes.Text);
            await DisplayAlert("Button Test", "Course Updated Successfully!", "OK");
            await Navigation.PopAsync();
        }

        async void DeleteCourse_Clicked(object sender, EventArgs e)
        {
            bool confirmDel = await DisplayAlert("Confirm?", "Delete the selected course?", "Yes", "No");

            if (confirmDel)
            {
                await DatabaseServices.DeleteCourse(SelectedCourseId);
                await DisplayAlert("Confirmation", "Term Deleted", "OK");
            }
            else
            {
                await DisplayAlert("Confirmation", "Delete Operation Canceled", "OK");
            }

            await Navigation.PopAsync();
        }

        async void AddAssessment_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddAssessment(SelectedCourseId));
        }



        /*
         * READ UP ON SHARE FUNCTIONS IN XAMARIN
         * 
         * SHARE FUNCTIONS, IMPLEMENT LATER
         * async void ShareEventHandler(object sender, Eventsarg e){
         *  string sharedCourseText = CourseName.Text;
         *  await Share.RequestAsync(new ShareTextRequest{
         *      Text = sharedCourseText;
         *      Title = "Share Course"
         *      });
         * }
         * 
         * BUTTON FOR URI
        ** async void ShareEventHandler(object sender, Eventsarg e){
        *  string sharedCourseURI = www.someurl.com/this/is/full/path
        *  await Share.RequestAsync(new ShareTextRequest{
        *      Uri = sharedCourseURI;
        *      Title = "Share Web Link to Course"
        *      });
         * }
         */
    }
}