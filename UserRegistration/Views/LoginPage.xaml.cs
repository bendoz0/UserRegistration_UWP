﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
using Newtonsoft.Json;
using UserRegistration.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UserRegistration.Views
{
    public sealed partial class LoginPage : Page
    {

        private List<Credentials> listCredentials = [];

        public LoginPage()
        {
            this.InitializeComponent();
            LoadCredentialsAsync(); 
        }

        private void TextChanged(object sender, RoutedEventArgs routedEventArgs)
        {
            btnLogin.IsEnabled = !string.IsNullOrEmpty(inputUsername.Text) && !string.IsNullOrEmpty(inputPassword.Password);
        }


        private void BtnLoginClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (chkNewUser.IsChecked == true)
            {
                if (RegisterNewOperator())
                {
                    UtenteLoggato.Nome = inputUsername.Text;
                    UtenteLoggato.Password = inputPassword.Password;
                    Frame.Navigate(typeof(MenuPage));
                }
            }
            else
                CheckCredentials();                                                                                                 
        }

        private static async Task ErrorEffect(Control sender)
        {
            var defaultColor = sender.BorderBrush;                                                                             
                                                                                                                               
            sender.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Red);                                                    
                                                                                                                                
            await Task.Delay(2000);                                                                                                
            sender.BorderBrush = defaultColor;                                                                                    
        }


        private bool RegisterNewOperator()
        {
            bool result = false;                                                                                                   
            bool exist = false;

            try
            {
                string encryptedPassword = EncryptPassword(inputPassword.Password);                                                     
                foreach (Credentials cred2 in listCredentials)
                    exist = (cred2.Username == inputUsername.Text);

                if (exist)
                {
                    Debug.WriteLine("Username uguale");
                    var usernameErrorEffect = ErrorEffect(inputUsername);

                    Task.WhenAll(usernameErrorEffect);
                }
                else
                {
                    listCredentials.Add(new Credentials
                    {
                        Username = inputUsername.Text,
                        Password = encryptedPassword
                    });

                    StorageFile file = ApplicationData.Current.LocalFolder
                        .CreateFileAsync("credentials.json",
                        CreationCollisionOption.ReplaceExisting).GetAwaiter().GetResult();

                    FileIO.WriteTextAsync(file, JsonConvert
                        .SerializeObject(new List<Credentials>(listCredentials)))
                        .GetAwaiter().GetResult();

                    result = true;                                                                                                  
                }
            }
            catch (Exception ex)
            {
                //Gestione eccezione da implementare
            }
            return result;
        }

        private string EncryptPassword(string password)
        {
            string encryptedpwd = string.Empty;                                                                                     

            SHA256 sHA256 = SHA256.Create();                                                                                        
            byte[] bytes = sHA256.ComputeHash(Encoding.UTF8.GetBytes(password));                                            
                                                                                                                             

            for (int i = 0; i < bytes.Length; i++)
            {
                encryptedpwd += bytes[i].ToString("X2");                                                                     
            }

            return encryptedpwd;                                                                                                  
        }


        private async void CheckCredentials()
        {
            bool credentialOK = false;                                                                                             
            string valPwdEncrypted = EncryptPassword(inputPassword.Password);                                                      

            foreach (Credentials cred2 in listCredentials)
                credentialOK = (cred2.Username == inputUsername.Text && cred2.Password == valPwdEncrypted);                       

            if (credentialOK)
            {
                UtenteLoggato.Nome = inputUsername.Text;
                UtenteLoggato.Password = inputPassword.Password;
                Frame.Navigate(typeof(MenuPage));
            }
            else
            {
                var usernameErrorEffect = ErrorEffect(inputUsername);
                var pasErrorEffect = ErrorEffect(inputPassword);

                await Task.WhenAll(usernameErrorEffect, pasErrorEffect);
            }
        }



        private async void LoadCredentialsAsync()   
        {
            try
            {
                StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("credentials.json");
                string cred = await FileIO.ReadTextAsync(file);
                listCredentials = JsonConvert.DeserializeObject<List<Credentials>>(cred);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("Il file credentials.json non esiste nel percorso indicato");
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Errore: {e.Message}");
            }
        }

    }
}