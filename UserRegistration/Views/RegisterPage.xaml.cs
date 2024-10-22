using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using UserRegistration.Models;
using UserRegistration.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UserRegistration.Views
{
    public sealed partial class RegisterPage : Page
    {
        private readonly DatiPersonaViewModel _viewModel = new();
        private int _lastId = 1;


        public RegisterPage()
        {
            this.InitializeComponent();

            DataContext = _viewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await LoadPersonAsync();
        }

        private void AddPersona(object sender, RoutedEventArgs e)
        {
            _viewModel.ObjectPersona.Id = _lastId;
            DatiPersonaViewModel.ListaPersone.Add(_viewModel.ObjectPersona);
            _lastId += 1;
            _viewModel.ObjectPersona = new();
        }

        private void SavePersone(object sender, RoutedEventArgs e)
        {
            try
            {
                StorageFile file = ApplicationData.Current.LocalFolder
                    .CreateFileAsync(UtenteLoggato.Nome + ".json",
                        CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();

                FileIO.WriteTextAsync(file, JsonConvert
                        .SerializeObject(new List<Persona>(DatiPersonaViewModel.ListaPersone)))
                    .GetAwaiter().GetResult();
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Il file non è stato trovato: "+ ex);
            }
        }



        public async Task LoadPersonAsync()
        {
            try
            {
                string fileName = UtenteLoggato.Nome + ".json";
                // ottiene il file dalla cartella locale del progetto
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                // Legge il contenuto del file JSON
                string json = await FileIO.ReadTextAsync(file);
                // Deseriallizzazione del file JSON in una lista di oggetti Persona
                var listaPersone = JsonConvert.DeserializeObject<ObservableCollection<Persona>>(json);
                // Svuota il contenuto dell Observable
                DatiPersonaViewModel.ListaPersone.Clear();

                // Variabile per trovare l'id più alto
                int maxId = 0;

                // Carica gli oggetti nell Observable
                foreach (var persona in listaPersone)
                {
                    DatiPersonaViewModel.ListaPersone.Add(persona);
                    if (persona.Id > maxId)
                    {
                        maxId = persona.Id;
                    }
                }

                //impostiamo il valore dell'id per le nuove persone
                _lastId = maxId + 1;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Il file non è stato trovato: "+ ex);
            }
        }


    }
}
