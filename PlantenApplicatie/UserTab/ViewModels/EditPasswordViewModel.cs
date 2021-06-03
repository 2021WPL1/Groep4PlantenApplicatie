using System.Linq;
using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    //ViewModel Made by Davy
    public class EditPasswordViewModel : ViewModelBase
    {
        private readonly PlantenDao _dao;

        private byte[] _encryptedNewPassword;
        private string _passwordErrorMessage;

        // button commands
        public ICommand EditCommand { get; }

        public ICommand CloseCommand { get; }


        //private variables (Davy)
        private Gebruiker _selectedUser;
        private Window _editPasswordWindow;

        //constructor
        public EditPasswordViewModel(Window window, Gebruiker user)
        {
            SelectedUser = user;
            _editPasswordWindow = window;

            EditCommand = new DelegateCommand<PasswordBox>(Edit);
            CloseCommand = new DelegateCommand(Close);

            _dao = PlantenDao.Instance;
        }

        //getters and setters
        public Gebruiker SelectedUser
        {
            private get => _selectedUser;
            set
            {
                _selectedUser = value;
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

        public void PasswordChecker(string password, string passwordConfirm)
        {
            _encryptedNewPassword = Encryptor.GenerateMD5Hash(password);
            
            PasswordErrorMessage = password == passwordConfirm 
                ? string.Empty 
                : "Paswoorden zijn niet gelijk";
        }
        //edit the password of the user and updates it in the database

        private void Edit(PasswordBox passwordBox)
        {
            if (!Encryptor.GenerateMD5Hash(passwordBox.Password).SequenceEqual(SelectedUser.HashPaswoord))
            {
                MessageBox.Show("Oud wachtwoord is niet correct");
                return;
            }
            
            string message = _dao.UpdateUser(SelectedUser.Emailadres, _encryptedNewPassword);

            MessageBox.Show(message);
        }

        private void Close()
        {
            _editPasswordWindow.Close();
        }
    }
}
