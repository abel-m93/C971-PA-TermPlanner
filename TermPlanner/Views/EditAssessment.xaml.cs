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
    public partial class EditAssessment : ContentPage
    {
        private readonly int SelectedAssessmentId;
        private readonly int SelectedCourseId;
        public EditAssessment()
        {
            InitializeComponent();
        }

        public EditAssessment(Assessment assessment)
        {
            InitializeComponent();

            SelectedAssessmentId = assessment.Id;
            SelectedCourseId = assessment.CourseId;

            //POPULATE FORM FIELDS WITH DATA PASSED FROM ASSESSMENT PARAMETER IN CONSTRUCTOR
            AssessmentId.Text = assessment.Id.ToString();
            AssessmentName.Text = assessment.Name;
            AssessmentTypePicker.SelectedItem = assessment.Type;
            DueDatePicker.Date = assessment.DueDate;
            AlertOn.IsToggled = assessment.AlertOn;

        }

        async void DeleteAssessment_Clicked(object sender, EventArgs e)
        {
            bool confirmDel = await DisplayAlert("Confirm?", "Delete the selected Assessment?", "Yes", "No");
            
            if (confirmDel)
            {
                await DatabaseServices.DeleteAssessment(SelectedAssessmentId);
                await DisplayAlert("Confirmation", "Assessment Deleted", "OK");
            }
            else
            {
                await DisplayAlert("Confirmation", "Delete Operation Canceled", "OK");
            }

            await Navigation.PopAsync();
        }

        async void SaveAssessment_Clicked(object sender, EventArgs e)
        {
            var existingAssessments = DatabaseServices.GetAssessments(SelectedCourseId);
            bool typeExists = false;
            if (string.IsNullOrWhiteSpace(AssessmentName.Text))
            {
                await DisplayAlert("Missing Name", "Please enter a name", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(AssessmentTypePicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Type", "Please enter a Type", "OK");
                return;
            }

            if (StartDatePicker.Date >= DueDatePicker.Date)
            {
                await DisplayAlert("Invalid Date", "Please ensure Start Date is before Due Date", "OK");
                return;
            }

            foreach (Assessment assessment in await existingAssessments)
            {
                if (assessment.Id.ToString() != AssessmentId.Text && assessment.Type == AssessmentTypePicker.SelectedItem.ToString())
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
                await DatabaseServices.UpdateAssessment(SelectedAssessmentId, AssessmentName.Text, AssessmentTypePicker.SelectedItem.ToString(),
                                            StartDatePicker.Date, DueDatePicker.Date, AlertOn.IsToggled);
                await DisplayAlert("Button Test", "Assessment Updated Successfully!", "OK");
                await Navigation.PopAsync();
            }
        }

    }
}