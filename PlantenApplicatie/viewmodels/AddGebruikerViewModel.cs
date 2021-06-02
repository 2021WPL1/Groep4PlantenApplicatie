using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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
        private string? _TextInputVoornaam;
        private string? _TextInputAchternaam;
        private string? _TextInputEmail;
        private string _passwordErrorMessage;
        //observable collection for the combobox


        private Gebruiker _user;
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
            _user = user; // Davy
            _dao = PlantenDao.Instance;
            Roles = new ObservableCollection<string>();

            AddUserCommand = new DelegateCommand<PasswordBox>(AddUser);
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
        public string? TextInputVoornaam
        {
            get => _TextInputVoornaam;
            set
            {
                _TextInputVoornaam = value;
                OnPropertyChanged();
            }
        }
        
        public string? TextInputAchternaam
        {
            get => _TextInputAchternaam;
            set
            {
                _TextInputAchternaam = value;
                OnPropertyChanged();
            }
        }

        public string? TextInputEmail
        {
            get => _TextInputEmail;
            set
            {
                _TextInputEmail = value;
                OnPropertyChanged();
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

        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            private set
            {
                _passwordErrorMessage = value;
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

        public void PasswordChecker(string password, string passwordConfirm)
        {
            PasswordErrorMessage = password == passwordConfirm ? string.Empty : "Paswoorden zijn niet gelijk";
        }

        private void AddUser(PasswordBox passwordBox)
        {
            if (TextInputVoornaam is null || TextInputAchternaam is null || TextInputEmail is null)
            {
                MessageBox.Show("Niet alle velden zijn ingevuld");
                return;
            }

            if (!(TextInputEmail.EndsWith("@vives.be") || TextInputEmail.EndsWith("@student.vives.be")))
            {
                MessageBox.Show("Email mag alleen van het Vives domein zijn");
                return;
            }
            
            var gebruiker = new Gebruiker
            {
                Voornaam = TextInputVoornaam,
                Achternaam = TextInputAchternaam,
                Rol = SelectedRole,
                Emailadres = TextInputEmail,
                HashPaswoord = Encryptor.GenerateMD5Hash(passwordBox.Password)
            };
            
            _dao.CreateLogin(gebruiker, out string message);
            
            MessageBox.Show(message);
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
