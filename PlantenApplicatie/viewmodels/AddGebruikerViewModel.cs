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
        //ViewModel (Jim)
        private readonly PlantenDao _dao;

        //private variables for the GUI
        private string _SelectedRole;
        private string _TextInputVoornaam;
        private string _TextInputAchternaam;
        private string _TextInputEmail;
        private string _TextInputPaswoord;
        private string _TextInputPaswoordCheck;
        private string _Check;

        //observable collection for the combobox
        public ObservableCollection<string> Roles { get; set; }

        //buttoncommand to save an user in the database
        public ICommand AddUserCommand { get; set; }

        //variables Davy
        public ICommand CloseWindowCommand { get; set; }
        private Window _addGebruikerWindow;

        //constructor
        public AddGebruikerViewModel(Window window)
        {
            _addGebruikerWindow = window;       // Davy
            _dao = PlantenDao.Instance;
            Roles = new ObservableCollection<string>();

            AddUserCommand = new DelegateCommand(AddUser);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
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
        //load in the roles, for now database is empty so the roles are hardcoded to access the program (Jim)
        public void LoadRoles()
        {
            var roles = _dao.GetRoles();

            if (roles is null)
            {
                Roles.Add("manager");
                Roles.Add("data-collector");
                Roles.Add("gebruiker");
            }
            else
            {
                foreach(var role in roles)
                {
                    roles.Add(role);
                }
            }
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
                
                var gebruiker = new User
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

        private void CloseWindow()
        {
            _addGebruikerWindow.Close();
        }

    }
}
