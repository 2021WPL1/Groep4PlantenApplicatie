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

        //observable collection for the combobox (Jim)
        public ObservableCollection<string> Roles { get; set; }

        //buttoncommand to save an user in the database
        public ICommand AddUserCommand { get; set; }

        //variables Davy
        public ICommand CloseWindowCommand { get; set; }

        //ViewModel (Jim)
        private readonly PlantenDao _dao;

        //private variables for the GUI
        private string _SelectedRole;
        private string? _TextInputFirstName;
        private string? _TextInputLastName;
        private string? _TextInputEmail;
        private string _passwordErrorMessage;
        private Gebruiker _user;
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
        public string? TextInputFirstName
        {
            get => _TextInputFirstName;
            set
            {
                _TextInputFirstName = value;
                OnPropertyChanged();
            }
        }
        
        public string? TextInputLastName
        {
            get => _TextInputLastName;
            set
            {
                _TextInputLastName = value;
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
            
                Roles.Add("manager");
                Roles.Add("data-collector");
                Roles.Add("gebruiker");
           
        }
        //check the passwords on if they are equal
        public void PasswordChecker(string password, string passwordConfirm)
        {
            PasswordErrorMessage = password == passwordConfirm ? string.Empty : "Paswoorden zijn niet gelijk";
        }

        // add a user to the database
        private void AddUser(PasswordBox passwordBox)
        {
            if (TextInputFirstName is null || TextInputLastName is null || TextInputEmail is null)
            {
                MessageBox.Show("Niet alle velden zijn ingevuld");
                return;
            }

            if (!(TextInputEmail.EndsWith("@vives.be") || TextInputEmail.EndsWith("@student.vives.be")))
            {
                MessageBox.Show("Email mag alleen van het Vives domein zijn");
                return;
            }
            
            var user = new Gebruiker
            {
                Voornaam = TextInputFirstName,
                Achternaam = TextInputLastName,
                Rol = SelectedRole,
                Emailadres = TextInputEmail,
                HashPaswoord = Encryptor.GenerateMD5Hash(passwordBox.Password)
            };
            
            _dao.CreateLogin(user, out string message);
            
            MessageBox.Show(message);
        }

        private void CloseWindow()
        {
            // reload Users by starting new window BeheerPlanten
            BeheerPlanten ManagePlants = new BeheerPlanten(_user);
            ManagePlants.Show();

            _addGebruikerWindow.Close();
        }
    }
}
