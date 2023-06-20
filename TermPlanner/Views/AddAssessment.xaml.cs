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
            if (string.IsNullOrWhiteSpace(AssessmentName.Text))
            {
                await DisplayAlert("Missing Name", "Please enter a name", "OK");
                return;
            }

            if(AssessmentTypePicker.SelectedItem == null || string.IsNullOrWhiteSpace(AssessmentTypePicker.SelectedItem.ToString())){
                await DisplayAlert("Missing Type", "Please select a Type", "OK");
                return;
            }

            await DatabaseServices.AddAssessment(SelectedCourseId, AssessmentName.Text,
                AssessmentTypePicker.SelectedItem.ToString(), DueDatePicker.Date, AlertOn.IsToggled);
            await DisplayAlert("Confirmation", "Assessment Added Successfully", "OK");
            await Navigation.PopAsync();
        }
    }
}