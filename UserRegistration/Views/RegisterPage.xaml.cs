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
        private int _statusAddPerson = 0;
        private bool _statusSave = false;


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

        private async void GoBackMenu(object sender, RoutedEventArgs e)
        {
            if (_statusAddPerson > 0)
            {
                if (!_statusSave)
                {
                    ContentDialog confirmDialog = new ContentDialog
                    {
                        Title = "Conferma Go Back",
                        Content = "Non hai salvato tutti i dati! Se torni indietro perderai i dati recenti.",
                        PrimaryButtonText = "OK",
                        CloseButtonText = "Annulla"
                    };
                    // Mostra la finestra di dialogo e aspetta la risposta dell'utente
                    ContentDialogResult result = await confirmDialog.ShowAsync();

                    // Se l'utente ha cliccato "OK", chiudi l'applicazione
                    if (result == ContentDialogResult.Primary)
                    {
                        Frame.Navigate(typeof(MenuPage));
                    }
                }
                else
                {
                    Frame.Navigate(typeof(MenuPage));
                }
            }
            else
            {
                Frame.Navigate(typeof(MenuPage));
            }
        }

        private void AddPersona(object sender, RoutedEventArgs e)
        {
            _viewModel.ObjectPersona.Id = _lastId;
            DatiPersonaViewModel.ListaPersone.Add(_viewModel.ObjectPersona);
            _lastId += 1;
            _statusAddPerson += 1;
            _viewModel.ObjectPersona = new();
            _statusSave = false;
            btnSave.IsEnabled = true;
        }

        private void AggiornaPersona(object sender, RoutedEventArgs e)
        {
            if (_viewModel.SelectedPersona != null)
            {
               _viewModel.SelectedPersona.Name = _viewModel.name;
                _viewModel.SelectedPersona.Surname = _viewModel.surname;
                _viewModel.SelectedPersona.BirthDay = _viewModel.birthDay;
                _viewModel.SelectedPersona.Address = _viewModel.address;
                _viewModel.SelectedPersona.City = _viewModel.city;
                _viewModel.SelectedPersona.Cap = _viewModel.cap;
                _viewModel.SelectedPersona.PhoneNumber = _viewModel.phoneNumber;

                //_viewModel.OnPropertyChanged(nameof(DatiPersonaViewModel.ListaPersone));
                var index = DatiPersonaViewModel.ListaPersone.IndexOf(_viewModel.SelectedPersona);
                if (index != -1)
                {
                    DatiPersonaViewModel.ListaPersone[index] = _viewModel.SelectedPersona;
                }
                _viewModel.ObjectPersona = new ();
                _statusSave = false;
                btnSave.IsEnabled = true;
            }
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

                _statusSave = true;
                btnSave.IsEnabled = false;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Il file non è stato trovato: "+ ex);
            }
        }


        private void TestLetterInput(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            // Entra nell'If solo se il tasto premuto è tutto tranne delle lettere o il backspace
            if (!char.IsLetter((char)e.Key) && e.Key != Windows.System.VirtualKey.Tab && e.Key != Windows.System.VirtualKey.Back)
            {
                // Impedisce che l'input venga elaborato se non è valido
                e.Handled = true;
            }

            btnInserisci.IsEnabled = !string.IsNullOrEmpty(inputName.Text) && !string.IsNullOrEmpty(inputSurname.Text);
        }

        private void TestNumberInput(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            var textBox = sender as TextBox;

            // Entra nell'If solo se il tasto premuto è tutto tranne delle lettere o il backspace
            if (!char.IsNumber((char)e.Key) && e.Key != Windows.System.VirtualKey.Tab && e.Key != Windows.System.VirtualKey.Back)
            {
                // Impedisce che l'input venga elaborato se non è valido
                e.Handled = true;
            }

            if (textBox.Name.Equals("inputTelefono"))
            {
                bool isValid = (textBox.Text.Length == 10 || textBox.Text.Length == 0);
                if (!isValid)
                {
                    inputTelefono.Header = "10 numeri";
                }
                else
                {
                    inputTelefono.Header = "Telefono:";
                }
            }

            if (textBox.Name.Equals("inputCap"))
            {
                bool isValid = (textBox.Text.Length == 5 || textBox.Text.Length == 0);
                if (!isValid)
                {
                    inputCap.Header = "5 numeri";
                }
                else
                {
                    inputCap.Header = "Cap:";
                }
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
