//using System.Net.Http;
//using System.Xml;
//using System.Threading.Tasks;
//using Newtonsoft.Json; // Install Newtonsoft.Json package if needed

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly HttpClient _httpClient = new HttpClient(); // Declare and initialize _httpClient

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            // Call GetUsersAsync method and handle the result
            var users = await GetUsersAsync();
            Console.WriteLine(users); // Output the users or handle them accordingly

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private async Task<string> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("http://localhost:3000/users"); // Adjust port if needed
                response.EnsureSuccessStatusCode(); // Ensure the response is successful

                var content = await response.Content.ReadAsStringAsync();
                var formattedUsers = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Formatting.Indented);
                return formattedUsers;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
