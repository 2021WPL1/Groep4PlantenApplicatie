using System.Collections.Generic;
using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
        private Gebruiker _user;
        //observable collection for the combobox
        public ObservableCollection<string> Roles { get; set; }

        //buttoncommand to save an user in the database
        public ICommand AddUserCommand { get; set; }

        //variables Davy
        public ICommand CloseWindowCommand { get; set; }
        private Window _addGebruikerWindow;

        private Brush _color;

        public AddGebruikerViewModel(Window window, Gebruiker user)
        {
            _addGebruikerWindow = window;       // Davy
            _user = user;       // Davy
            _dao = PlantenDao.Instance;
            Roles = new ObservableCollection<string>();

            AddUserCommand = new DelegateCommand(AddUser);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            LoadRoles();
        }

        // getters setters (Davy)
        public Brush ChangeColor
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
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
                ChangeColor = Brushes.Red;
            }
            else
            {
                Check = "Paswoorden zijn gelijk";
                ChangeColor = Brushes.Green;
            }

        }

        public void AddUser()
        {
            string message;

            if (SelectedRole == null || TextInputVoornaam == null || _TextInputAchternaam == null ||
                TextInputEmail == null || TextInputPaswoord == null || TextInputPaswoordCheck == null)
            {
                MessageBox.Show("Niet alle velden zijn ingevuld");
            }
            else
            {
                if (TextInputEmail.Contains("@vives.be") || TextInputEmail.Contains("@student.vives.be"))
                {
                    if (TextInputPaswoord == TextInputPaswoordCheck)
                    {
                        var gebruiker = new Gebruiker
                        {
                            Voornaam = TextInputVoornaam,
                            Achternaam = TextInputAchternaam,
                            Rol = SelectedRole,
                            Emailadres = TextInputEmail,
                            HashPaswoord = Encryptor.GenerateMD5Hash(TextInputPaswoord)
                        };
                        _dao.CreateLogin(gebruiker, out message);
                        MessageBox.Show(message);

                        // herladen Users door nieuw venster BeheerPlanten op te starten
                        BeheerPlanten beheerPlanten = new BeheerPlanten(_user);
                        beheerPlanten.Show();

                        _addGebruikerWindow.Close();
                    }

                }
                else
                {
                    MessageBox.Show("Email mag alleen van het Vives domein zijn.");
                }
            }
        }

        private void CloseWindow()
        {
            // herladen Users door nieuw venster BeheerPlanten op te starten
            BeheerPlanten beheerPlanten = new BeheerPlanten(_user);
            beheerPlanten.Show();

            _addGebruikerWindow.Close();
        }

    }
}
