using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

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

        private async void OnGetUsersClicked(object sender, EventArgs e)
        {
            var users = await GetUsersAsync();
            UsersLabel.Text = users;  // Display the fetched users in the label
            Console.WriteLine(users);  // Optionally log the users to the console
        }

        // Add new user function
        private async void OnAddUserClicked(object sender, EventArgs e)
        {

            // Create a user object
            var newUser = new
            {
                name = "Resul",
                email = "resul.xponent@gmail.com",
                phone = "01118886677"
            };

            // Serialize the user object to JSON
            var jsonContent = JsonConvert.SerializeObject(newUser);

            // Send the POST request
            try
            {
                var response = await _httpClient.PostAsync(
                    "http://localhost:3000/users", 
                    new StringContent(jsonContent, Encoding.UTF8, "application/json")
                );

                response.EnsureSuccessStatusCode(); // Ensure the response is successful
                var result = await response.Content.ReadAsStringAsync();

                // Optionally handle the response
                Console.WriteLine("User added successfully: " + result);
                await DisplayAlert("Success", "User added successfully", "OK");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding user: {ex.Message}");
                await DisplayAlert("Error", "Failed to add user", "OK");
            }
        }
    }
}
