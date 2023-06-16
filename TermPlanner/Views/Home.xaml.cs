using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TermPlanner.Services;
using TermPlanner.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TermPlanner.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : ContentPage
    {
        public Home()
        {
            InitializeComponent();
            
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTerm());
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //DatabaseServices.LoadSampleData();
            var allTerms = await DatabaseServices.GetTerms();
            TermCollectionView.ItemsSource = allTerms;
        }


        //METHOD AND BUTTON FOR TESTING PURPOSES
        private async void ClearDataBtn_Clicked(object sender, EventArgs e)
        {
            await DatabaseServices.ClearSampleData();
            await DisplayAlert("ATTENTION", "Sample Data Cleared", "OK");
        }

        //METHOD AND BUTTON FOR TESTING PURPOSES
        private async void LoadDataBtn_Clicked(object sender, EventArgs e)
        {
            var allTerms = await DatabaseServices.GetTerms();
            //Testing if allTerms is Null
/*            await DisplayAlert("Total Term Count", $"{allTerms.Count()}", "OK");
            foreach (var term in allTerms)
            {
                await DisplayAlert("OBJECT NULL TEST", $"Current ID: {term.Id}\nName: {term.Name}\nStatus: {term.Status}\nStart Date: {term.StartDate.Date}", "OK");
            }*/


            // CollectionView TermCollectionView = new CollectionView();
            //TermCollectionView.ItemsSource = allTerms;
            //TermCollectionView.ItemsSource = allTerms;         
        }

        private async void TermCollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection != null)
            {
                Term term = (Term)e.CurrentSelection.FirstOrDefault();
                await Navigation.PushAsync(new EditTerm(term));
            }
        }
    }
}