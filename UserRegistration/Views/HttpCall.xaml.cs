using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Web.Http;
using UserRegistration.Models;
using HttpClient = System.Net.Http.HttpClient;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UserRegistration.Views
{

    public sealed partial class HttpCall : Page
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _urlComments = "https://jsonplaceholder.typicode.com/comments";
        private readonly string _urlToDo = "https://jsonplaceholder.typicode.com/todos";
        private readonly string _urlUsers = "https://jsonplaceholder.typicode.com/users";


        public HttpCall()
        {
            this.InitializeComponent();
        }

        private async Task GetPlaceholderComments()
        {
            var comments = await _httpClient.GetFromJsonAsync<List<Comment>>(_urlComments);
            CommentsList.ItemsSource = comments;
        }

        private async Task GetPlaceholderToDo()
        {
            var products = await _httpClient.GetFromJsonAsync<List<ToDo>>(_urlToDo);
            ProductsList.ItemsSource = products;
        }

        private async Task GetPlaceholderUsers()
        {
            var utenti = await _httpClient.GetFromJsonAsync<List<User>>(_urlUsers);
            UsersList.ItemsSource = utenti;
        }



        private void ClickComments(object sender, RoutedEventArgs e)
        {
            ProductsList.Visibility = Visibility.Collapsed;
            UsersList.Visibility = Visibility.Collapsed;
            CommentsList.Visibility = Visibility.Visible;
            _ = GetPlaceholderComments();
        }

        private void ClickProducts(object sender, RoutedEventArgs e)
        {
            CommentsList.Visibility = Visibility.Collapsed;
            UsersList.Visibility = Visibility.Collapsed;
            ProductsList.Visibility = Visibility.Visible;
            _ = GetPlaceholderToDo();
        }

        private void ClickUsers(object sender, RoutedEventArgs e)
        {
            CommentsList.Visibility = Visibility.Collapsed;
            ProductsList.Visibility = Visibility.Collapsed;
            UsersList.Visibility = Visibility.Visible;
            _ = GetPlaceholderUsers();
        }

    }
}
