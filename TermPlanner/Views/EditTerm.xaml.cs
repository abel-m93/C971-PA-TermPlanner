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
    public partial class EditTerm : ContentPage
    {
        //this will be grabbed from Term being paased from previous page
        private readonly int SelectedTermId;
        public EditTerm()
        {
            InitializeComponent();
        }

        public EditTerm(Term term)
        {
            InitializeComponent();

            //Term ID from passed term will be set to the readonly to associate Term with Courses
            SelectedTermId = term.Id;

            TermId.Text = term.Id.ToString();
            TermName.Text = term.Name;
            TermStatusPicker.SelectedItem = term.Status;
            TermStartPicker.Date = term.StartDate;
            TermEndPicker.Date = term.EndDate;

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var courses = await DatabaseServices.GetCourses(SelectedTermId);
            int courseCount = courses.Count();
            courseCountLabel.Text = $"Course Count:\t{courseCount}";
            CourseCollectionView.ItemsSource = await DatabaseServices.GetCourses(SelectedTermId);
        }

        async void SaveTerm_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TermName.Text))
            {
                await DisplayAlert("Missing Name", "Please enter a name", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(TermStatusPicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Status", "Please enter a Status", "OK");
                return;
            }

            if (TermStartPicker.Date >= TermEndPicker.Date)
            {
                await DisplayAlert("Invalid Date", "Please ensure Term Start Date is before Term End Date", "OK");
                return;
            }

            await DatabaseServices.UpdateTerm(SelectedTermId, TermName.Text, TermStatusPicker.SelectedItem.ToString(),
                               TermStartPicker.Date, TermEndPicker.Date);
            await DisplayAlert("Button Test", "Term Updated Successfully!", "OK");
            await Navigation.PopAsync();
        }

        async void DeleteTerm_Clicked(object sender, EventArgs e)
        {
            bool confirmDel = await DisplayAlert("Confirm?", "Delete the selected term and its courses?", "Yes", "No");
            var delRelatedCourse = await DatabaseServices.GetCourses(SelectedTermId);
            if (confirmDel)
            {
                foreach (var course in delRelatedCourse) 
                {
                    await DatabaseServices.DeleteCourse(course.Id);
                }
                await DatabaseServices.DeleteTerm(SelectedTermId);
                await DisplayAlert("Confirmation", "Term Deleted", "OK");
            }
            else
            {
                await DisplayAlert("Confirmation", "Delete Operation Canceled", "OK");
            }

            await Navigation.PopAsync();

        }


        //This Method will navigate to Add Course Screen. Clicking on ADD COURSE button
        async void AddCourse_Clicked(object sender, EventArgs e)
        {
            var courses = await DatabaseServices.GetCourses(SelectedTermId);
            int courseCount = courses.Count();

            if(courseCount == 6)
            {
                await DisplayAlert("Too Many Courses", "Each term can only have a maximum of 6 course", "OK");
            }
            else
            {
                await Navigation.PushAsync(new CourseAdd(SelectedTermId));
            }
            
        }

        //CLicking on a Course will navigate to EDIT COURSE page.
        async void CourseCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Course course = (Course)e.CurrentSelection.FirstOrDefault();

            if (e.CurrentSelection != null)
            {
                await Navigation.PushAsync(new CourseEdit(course));
            }
        }
    }
}