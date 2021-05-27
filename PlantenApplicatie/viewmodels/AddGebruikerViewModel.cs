using System.Collections.Generic;
using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    class AddGebruikerViewModel : ViewModelBase
    {
        //(Jim)
        private readonly PlantenDao _dao;

        private string _SelectedRole;
        private string _TextInputVoornaam;
        private string _TextInputAchternaam;
        private string _TextInputEmail;
        private string _TextInputPaswoord;
        private string _TextInputPaswoordCheck;
        private string _Check;

        public ObservableCollection<string> Roles { get; set; }

        public ICommand AddUserCommand { get; set; }

        public AddGebruikerViewModel()
        {
            _dao = PlantenDao.Instance;
            Roles = new ObservableCollection<string>();

            AddUserCommand = new DelegateCommand(AddUser);
            LoadRoles();
        }

        //getters setters (Jim)

        public string TextInputVoornaam
        {
            get => _TextInputVoornaam;
            set
            {
                _TextInputVoornaam = value;
                OnPropertyChanged();
            }
        }
        public string TextInputAchternaam
        {
            get => _TextInputAchternaam;
            set
            {
                _TextInputAchternaam = value;
                OnPropertyChanged();
            }
        }

        public string TextInputEmail
        {
            get => _TextInputEmail;
            set
            {
                _TextInputEmail = value;
                OnPropertyChanged();
            }
        }

        public string TextInputPaswoord
        {
            get => _TextInputPaswoord;
            set
            {
                _TextInputPaswoord = value;
                OnPropertyChanged();
            }
        }
        public string TextInputPaswoordCheck
        {
            get => _TextInputPaswoordCheck;
            set
            {
                _TextInputPaswoordCheck = value;
                OnPropertyChanged();
                PasswordChecker();
            }
        }

        public string SelectedRole
        {
            get => _SelectedRole;
            set
            {
                _SelectedRole = value;
                OnPropertyChanged();
            }
        }

        public string Check
        {
            get => _Check;
            set
            {
                _Check = value;
                OnPropertyChanged();
            }

        }

        public void LoadRoles()
        {
            Roles.Add("manager");
            Roles.Add("data-collector");
            Roles.Add("gebruiker");

        }

        public void PasswordChecker()
        {
            if(TextInputPaswoord != TextInputPaswoordCheck)
            {
                Check = "Paswoorden zijn niet gelijk";
            }
            else
            {
                Check = "Paswoorden zijn gelijk";
            }

        }

        public void AddUser()
        {

            if(TextInputEmail.Contains("@vives.be") || TextInputEmail.Contains("@student.vives.be"))
            {
                
                var gebruiker = new Gebruiker
                {
                    Voornaam = TextInputVoornaam,
                    Achternaam = TextInputAchternaam,
                    Rol = SelectedRole,
                    Emailadres = TextInputEmail,
                    HashPaswoord = Encryptor.GenerateMD5Hash(TextInputPaswoord)
                };


                MessageBox.Show($"gebruiker: {TextInputVoornaam} {TextInputAchternaam} werd aangemaakt");

                _dao.CreateLogin(gebruiker);
            }
            else
            {
                MessageBox.Show("Email mag alleen van het Vives domein zijn.");
            }
        }

    }
}
