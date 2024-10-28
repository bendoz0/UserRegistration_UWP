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
        private readonly string _urlProducts = "https://jsonplaceholder.typicode.com/todos";


        public HttpCall()
        {
            this.InitializeComponent();
        }

        private async Task GetPlaceholderComments()
        {
            var comments = await _httpClient.GetFromJsonAsync<List<Comment>>(_urlComments);
            CommentsList.ItemsSource = comments;
        }

        private async Task GetPlaceholderProducts()
        {
            var products = await _httpClient.GetFromJsonAsync<List<Product>>(_urlProducts);
            Debug.WriteLine(products.ToString());
            ProductsList.ItemsSource = products;
        }



        private void ClickComments(object sender, RoutedEventArgs e)
        {
            ProductsList.Visibility = Visibility.Collapsed;
            CommentsList.Visibility = Visibility.Visible;
            _ = GetPlaceholderComments();
        }

        private void ClickProducts(object sender, RoutedEventArgs e)
        {
            CommentsList.Visibility = Visibility.Collapsed;
            ProductsList.Visibility = Visibility.Visible;
            _ = GetPlaceholderProducts();
        }
    }
}
