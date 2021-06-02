using PlantenApplicatie.Data;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    //Viewmodel made by Davy
    public class LoginViewModel : ViewModelBase
    {
        private readonly PlantenDao _dao;

        // button commands
        public ICommand LoginCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private string _textInputLogin;

        private Window _loginWindow;

        //constructor
        public LoginViewModel(Window window)
        {
            _loginWindow = window;
            LoginCommand = new DelegateCommand<PasswordBox>(Login);
            CancelCommand = new DelegateCommand(Cancel);

            _dao = PlantenDao.Instance;
        }
        //getters and setters
        public string TextInputLogin
        {
            get => _textInputLogin;
            set
            {
                _textInputLogin = value;
                OnPropertyChanged(_textInputLogin);
            }
        }

        public void Login(PasswordBox passwordBox)
        {
            var isLoginSuccessfull = _dao.CheckLogin(TextInputLogin, passwordBox.Password, 
                out string message);

            MessageBox.Show(message);

            if (!isLoginSuccessfull) return;

            var gebruiker =_dao.GetGebruiker(TextInputLogin);
                
            _loginWindow.Hide();
                
            var beheerPlanten = new BeheerPlanten(gebruiker);
            beheerPlanten.Show(); 
        }

        //close the window when it gets cancelled
        public void Cancel()
        {
            this._loginWindow.Close();
        }
    }
}
