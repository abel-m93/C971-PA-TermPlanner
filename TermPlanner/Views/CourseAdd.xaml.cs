using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermPlanner.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CourseAdd : ContentPage
    {
        private readonly int SelectedTermId;
        public CourseAdd()
        {
            InitializeComponent();
        }

        public CourseAdd(int termId)
        {
            InitializeComponent();
            SelectedTermId = termId;
        }

        async void AddCourseBtn_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CourseName.Text))
            {
                await DisplayAlert("Incomplete Data", "Please enter a name", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CourseStatusPicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Status", "Please enter a Status", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(InstrName.Text))
            {
                await DisplayAlert("Incomplete Data", "Please enter Instructor name", "OK");
                return;
            }

            await DatabaseServices.AddCourse(SelectedTermId, CourseName.Text, CourseStatusPicker.SelectedItem.ToString(),
                CourseStartPicker.Date, CourseEndPicker.Date, AlertOn.IsToggled, InstrName.Text,
                InstrPhone.Text, InstrEmail.Text, CourseNotes.Text);

            await Navigation.PopAsync();

            
        }
    }
}