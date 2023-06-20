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
    public partial class AddTerm : ContentPage
    {
        public AddTerm()
        {
            InitializeComponent();
        }

        async void SaveTermBtn_Clicked(object sender, EventArgs e)
        {
            //await DisplayAlert("Button Test", "Confirm functioniong of button", "OK");

            if (string.IsNullOrWhiteSpace(TermName.Text))
            {
                await DisplayAlert("Missing Name", "Please enter a name", "OK");
                return;
            }

            if (TermStatusPicker.SelectedItem == null || string.IsNullOrWhiteSpace(TermStatusPicker.SelectedItem.ToString()))
            {
                await DisplayAlert("Missing Status", "Please select a Status", "OK");
                return;
            }

            await DatabaseServices.AddTerm(TermName.Text, TermStatusPicker.SelectedItem.ToString(),
                                           TermStartPicker.Date, TermEndPicker.Date);
            await DisplayAlert("Button Test", "Term Added Successfully", "OK");
            await Navigation.PopAsync();
        }
    }
}