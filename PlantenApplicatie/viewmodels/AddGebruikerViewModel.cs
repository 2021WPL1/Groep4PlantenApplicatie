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
        //(Jim)
        private readonly PlantenDao _dao;

        private string _SelectedRole;
        private string? _TextInputVoornaam;
        private string? _TextInputAchternaam;
        private string? _TextInputEmail;
        private string _passwordErrorMessage;

        public ObservableCollection<string> Roles { get; set; }

        public ICommand AddUserCommand { get; set; }

        // variabelen Davy
        public ICommand CloseWindowCommand { get; set; }
        private Window _addGebruikerWindow;

        private Brush _color;

        public AddGebruikerViewModel(Window window)
        {
            _addGebruikerWindow = window;       // Davy
            _dao = PlantenDao.Instance;
            Roles = new ObservableCollection<string>();

            AddUserCommand = new DelegateCommand<PasswordBox>(AddUser);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            LoadRoles();
        }

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

        public void LoadRoles()
        {
            Roles.Add("manager");
            Roles.Add("data-collector");
            Roles.Add("gebruiker");
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
            _addGebruikerWindow.Close();
        }
    }
}
