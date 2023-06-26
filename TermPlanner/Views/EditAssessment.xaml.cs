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
        public EditAssessment()
        {
            InitializeComponent();
        }

        public EditAssessment(Assessment assessment)
        {
            InitializeComponent();

            SelectedAssessmentId = assessment.Id;

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

            await DatabaseServices.UpdateAssessment(SelectedAssessmentId, AssessmentName.Text, AssessmentTypePicker.SelectedItem.ToString(),
                               DueDatePicker.Date, AlertOn.IsToggled);
            await DisplayAlert("Button Test", "Term Updated Successfully!", "OK");
            await Navigation.PopAsync();
        }
    }
}