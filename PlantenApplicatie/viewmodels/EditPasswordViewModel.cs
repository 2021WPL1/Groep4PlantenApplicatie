using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    public class EditPasswordViewModel : ViewModelBase
    {
        private readonly PlantenDao _dao;

        private string _passwordErrorMessage;

        // button commando's
        public ICommand EditCommand { get; }

        public ICommand CloseCommand { get; }

        // private variabelen (Davy)
        private Gebruiker _selectedGebruiker;
        private readonly Window _editPasswordWindow;

        public EditPasswordViewModel(Window window, Gebruiker gebruiker)
        {
            SelectedGebruiker = gebruiker;
            _editPasswordWindow = window;

            EditCommand = new DelegateCommand<PasswordBox>(Edit);
            CloseCommand = new DelegateCommand(Close);

            _dao = PlantenDao.Instance;
        }

        public Gebruiker SelectedGebruiker
        {
            private get => _selectedGebruiker;
            set
            {
                _selectedGebruiker = value;
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
            PasswordErrorMessage = password == passwordConfirm ? string.Empty : "Paswoorden zijn niet gelijk";
        }

        private void Edit(PasswordBox passwordBox)
        {
            string message = _dao.UpdateUser(SelectedGebruiker.Emailadres, passwordBox.Password);

            MessageBox.Show(message);
        }

        private void Close()
        {
            _editPasswordWindow.Close();
        }
    }
}
