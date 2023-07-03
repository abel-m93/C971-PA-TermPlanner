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
    public partial class AddAssessment : ContentPage
    {
        private readonly int SelectedCourseId;
        
        public AddAssessment()
        {
            InitializeComponent();
        }

        public AddAssessment(int courseId)
        {
            InitializeComponent();
            SelectedCourseId = courseId;
            
        }

        async void AddAssessmentBtn_Clicked(object sender, EventArgs e)
        {
            var existingAssessments = DatabaseServices.GetAssessments(SelectedCourseId);
            bool typeExists = false;
            if (string.IsNullOrWhiteSpace(AssessmentName.Text))
            {
                await DisplayAlert("Missing Name", "Please enter a name", "OK");
                return;
            }

            if(AssessmentTypePicker.SelectedItem == null || string.IsNullOrWhiteSpace(AssessmentTypePicker.SelectedItem.ToString())){
                await DisplayAlert("Missing Type", "Please select a Type", "OK");
                return;
            }

            if (StartDatePicker.Date >= DueDatePicker.Date)
            {
                await DisplayAlert("Invalid Date", "Please ensure Start Date is before  Date", "OK");
                return;
            }

            foreach (Assessment assessment in await existingAssessments)
            {
                if (assessment.Type == AssessmentTypePicker.SelectedItem.ToString())
                {
                    typeExists = true;
                }
            }

            if (typeExists)
            {
                await DisplayAlert("Invalid Data", $"An assessment of type: {AssessmentTypePicker.SelectedItem} already exists", "OK");
                return;
            }
            else
            {
                await DatabaseServices.AddAssessment(SelectedCourseId, AssessmentName.Text,
                                     AssessmentTypePicker.SelectedItem.ToString(), StartDatePicker.Date, DueDatePicker.Date, AlertOn.IsToggled);
                await DisplayAlert("Confirmation", "Assessment Added Successfully", "OK");
                await Navigation.PopAsync();
            }


        }
    }
}