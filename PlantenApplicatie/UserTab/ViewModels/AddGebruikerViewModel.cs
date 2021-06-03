using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlantenApplicatie.viewmodels
{
    class AddGebruikerViewModel : ViewModelBase
    {
        //observable collection for the combobox (Jim)
        public ObservableCollection<string> Roles { get; set; }

        //buttoncommand to save an user in the database
        public ICommand AddUserCommand { get; set; }

        //variables Davy
        public ICommand CloseWindowCommand { get; set; }

        //ViewModel (Jim)
        private readonly PlantenDao _dao;

        //private variables for the GUI
        private string? _selectedRole;
        private string? _textInputNumber;
        private string? _textInputFirstName;
        private string? _textInputLastName;
        private string? _textInputEmail;
        private string _passwordErrorMessage;
        //observable collection for the combobox

        private readonly Gebruiker _user;
        public ObservableCollection<string> Roles { get; }

        //buttoncommand to save an user in the database
        public ICommand AddUserCommand { get; }

        //variables Davy
        public ICommand CloseWindowCommand { get; }
        private readonly Window _addGebruikerWindow;

        private Brush _color;

        public AddGebruikerViewModel(Window window, Gebruiker user)
        {
            _addGebruikerWindow = window; // Davy   
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

        public string? TextInputNumber
        {
            get => _textInputNumber;
            set
            {
                _textInputNumber = value;
                OnPropertyChanged();
            }
        }

        //getters setters (Jim)
        public string? TextInputFirstName
        {
            get => _textInputFirstName;
            set
            {
                _textInputFirstName = value;
                OnPropertyChanged();
            }
        }
        
        public string? TextInputLastName
        {
            get => _textInputLastName;
            set
            {
                _textInputLastName = value;
                OnPropertyChanged();
            }
        }

        public string? TextInputEmail
        {
            get => _textInputEmail;
            set
            {
                _textInputEmail = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
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
        
        //check the passwords on if they are equal
        public void PasswordChecker(string password, string passwordConfirm)
        {
            PasswordErrorMessage = password == passwordConfirm 
                ? string.Empty 
                : "Paswoorden zijn niet gelijk";
        }
        
        //load in the roles, for now database is empty so the roles are hardcoded to access the program (Jim)
        private void LoadRoles()
        {
            Roles.Add("manager");
            Roles.Add("data-collector");
            Roles.Add("gebruiker");
        }

        // add a user to the database
        private void AddUser(PasswordBox passwordBox)
        {
            if (TextInputNumber is null || TextInputFirstName is null || TextInputLastName is null 
                || SelectedRole is null || TextInputEmail is null)
            {
                MessageBox.Show("Niet alle velden zijn ingevuld");
                return;
            }
            if (!(TextInputEmail.EndsWith("@vives.be") || TextInputEmail.EndsWith("@student.vives.be")))
            {
                MessageBox.Show("Email mag alleen van het Vives domein zijn");
                return;
            }
            if (!IsEmailAddressValid(TextInputNumber, TextInputFirstName, TextInputLastName, TextInputEmail))
            {
                MessageBox.Show("Email is ongeldig, moet bestaan uit nummer of voornaam.achternaam");
                return;
            }
            
            var user = new Gebruiker
            {
                Vivesnr = TextInputNumber,
                Voornaam = TextInputFirstName,
                Achternaam = TextInputLastName,
                Rol = SelectedRole,
                Emailadres = TextInputEmail,
                HashPaswoord = Encryptor.GenerateMD5Hash(passwordBox.Password)
            };
            
            _dao.CreateLogin(user, out string message);
            
            MessageBox.Show(message);
        }

        private static bool IsEmailAddressValid(string? number, string? firstName, string? lastName, string? email)
        {
            return email is not null 
                   && Regex.IsMatch(
                       email, $@"^({number}|{firstName}\.{lastName})@(vives.be|student.vives.be)$");
        }

        private void CloseWindow()
        {
            // reload Users by starting new window BeheerPlanten
            BeheerPlanten managePlants = new(_user);
            managePlants.Show();

            _addGebruikerWindow.Close();
        }
    }
}
