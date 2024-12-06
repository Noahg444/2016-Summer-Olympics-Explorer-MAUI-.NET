using System.Linq;
using Microsoft.Maui.Controls;
using SQLite;
using System.Collections.Generic;

namespace OlympicsMauiApp
{
    public partial class AthletesPage : ContentPage
    {
        public AthletesPage()
        {
            InitializeComponent();

            LoadCountries();
            LoadSportsForCountry("United States");
            LoadEventsForCountryAndSport("United States", "Archery");
            RefreshMedals();
        }

        private void LoadCountries()
        {
            countries.ItemsSource = (from p in DB.conn.Table<Participant2016Summer>()
                                     select p.Country).Distinct().OrderBy(e => e).ToList();
        }

        private void LoadSportsForCountry(string country)
        {
            sports.ItemsSource = (from p in DB.conn.Table<Participant2016Summer>()
                                  where p.Country == country
                                  select p.Sport).Distinct().OrderBy(e => e).ToList();
        }

        private void LoadEventsForCountryAndSport(string country, string sport)
        {
            events.ItemsSource = (from p in DB.conn.Table<Participant2016Summer>()
                                  where p.Country == country && p.Sport == sport
                                  select p.Event).Distinct().OrderBy(e => e).ToList();
        }

        private void RefreshMedals()
        {
            var countrySelected = countries.SelectedItem as string;
            var sportSelected = sports.SelectedItem as string;
            var eventSelected = events.SelectedItem as string;

            var query = DB.conn.Table<Participant2016Summer>();

            if (countrySelected != null)
            {
                query = query.Where(p => p.Country == countrySelected);
            }
            if (countrySelected != null)
            {
                query = query.Where(p => p.Sport == sportSelected);
            }
            if (countrySelected != null)
            {
                query = query.Where(p => p.Event == eventSelected);
            }

            medals.ItemsSource = query.OrderByDescending(a => a.Medal).ToList();
            countries.SelectedItem = countrySelected;
            sports.SelectedItem = sportSelected;
            events.SelectedItem = eventSelected;
            
        }

        void countries_ItemSelected(System.Object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCountry = countries.SelectedItem as string;

            LoadSportsForCountry(selectedCountry);
            LoadEventsForCountryAndSport(selectedCountry, sports.SelectedItem as string);
            RefreshMedals();
        }

        void sports_ItemSelected(System.Object sender, SelectedItemChangedEventArgs e)
        {
            var selectedCountry = countries.SelectedItem as string;
            var selectedSport = sports.SelectedItem as string;

            LoadEventsForCountryAndSport(selectedCountry, selectedSport);
            RefreshMedals();
        }

        void events_ItemSelected(System.Object sender, SelectedItemChangedEventArgs e)
        {
            RefreshMedals();
        }

        async void medals_ItemTapped(System.Object sender, ItemTappedEventArgs e)
        {
            var selectedAthlete = e.Item as Participant2016Summer;
            var eventList = (from p in DB.conn.Table<Participant2016Summer>()
                             where p.Name == selectedAthlete.Name
                             select p.Event).Distinct().OrderBy(el => el).ToList();

            string message = string.Join("\n", eventList);
            await DisplayAlert($"Events for {selectedAthlete.Name}", message, "OK");
        }
    }
}
