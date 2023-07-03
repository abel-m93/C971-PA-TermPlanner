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

        private bool isInvalidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return !(addr.Address == email);
            }
            catch
            {
                return true;
            }
        }
        async void AddCourseBtn_Clicked(object sender, EventArgs e)
        {
            bool emailInvalid = isInvalidEmail(InstrEmail.Text);
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

            if (CourseStartPicker.Date >= CourseEndPicker.Date)
            {
                await DisplayAlert("Invalid Date", "Please ensure Course Start Date is before Course End Date", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(InstrName.Text))
            {
                await DisplayAlert("Incomplete Data", "Please enter Instructor name", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(InstrPhone.Text))
            {
                await DisplayAlert("Incomplete Data", "Please enter valid instructor phone number", "OK");
                return;
            }

            if (emailInvalid || string.IsNullOrWhiteSpace(InstrEmail.Text))
            {
                await DisplayAlert("Incomplete Data", "Please enter valid Instructor email", "OK");
                return;
            }

            await DatabaseServices.AddCourse(SelectedTermId, CourseName.Text, CourseStatusPicker.SelectedItem.ToString(),
                CourseStartPicker.Date, CourseEndPicker.Date, AlertOn.IsToggled, InstrName.Text,
                InstrPhone.Text, InstrEmail.Text, CourseNotes.Text);

            await DisplayAlert("Button Test", "Course Added Successfully", "OK");
            await Navigation.PopAsync();

            
        }
    }
}