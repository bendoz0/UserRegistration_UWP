using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UserRegistration.Models;

namespace UserRegistration.ViewModels
{
    internal class DatiPersonaViewModel: INotifyPropertyChanged
    {
        public Persona Person = new();
        public ObservableCollection<Persona> ListaPersone { get; set; } = [];

        public Persona ObjectPersona
        {
            get => Person;
            set
            {
                Person = value;
                OnPropertyChanged(nameof(id));
                OnPropertyChanged(nameof(name));
                OnPropertyChanged(nameof(surname));
                OnPropertyChanged(nameof(birthDay));
                OnPropertyChanged(nameof(address));
                OnPropertyChanged(nameof(city));
                OnPropertyChanged(nameof(cap));
                OnPropertyChanged(nameof(phoneNumber));

            }
        }


        public int id
        {
            get => Person.Id;
            set
            {
                Person.Id = value;
                OnPropertyChanged(nameof(id));
            }
        }
        public string name
        {
            get => Person.Name;
            set
            {
                Person.Name = value;
                OnPropertyChanged(nameof(name));
            }
        }
        public string surname
        {
            get => Person.Surname;
            set
            {
                Person.Surname = value;
                OnPropertyChanged(nameof(surname));
            }
        }
        public DateTimeOffset birthDay
        {
            get => Person.BirthDay;
            set
            {
                Person.BirthDay = value.Date;
                OnPropertyChanged(nameof(birthDay));
            }
        }
        public string address
        {
            get => Person.Address;
            set
            {
                Person.Address = value;
                OnPropertyChanged(nameof(address));
            }
        }
        public string city
        {
            get => Person.City;
            set
            {
                Person.City = value;
                OnPropertyChanged(nameof(city));
            }
        }
        public int cap
        {
            get => Person.Cap;
            set
            {
                Person.Cap = value;
                OnPropertyChanged(nameof(cap));
            }
        }
        public int phoneNumber
        {
            get => Person.PhoneNumber;
            set
            {
                Person.PhoneNumber = value;
                OnPropertyChanged(nameof(phoneNumber));
            }
        }






        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
