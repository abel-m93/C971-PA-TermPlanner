using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermPlanner.Models;
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
    }
}