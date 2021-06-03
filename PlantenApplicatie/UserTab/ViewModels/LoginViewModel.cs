using PlantenApplicatie.Data;
using PlantenApplicatie.UserTab.Views;
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
        // button commands
        public ICommand LoginCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private readonly PlantenDao _dao;

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

        private void Login(PasswordBox passwordBox)
        {
            var isLoginSuccessfull = _dao.CheckLogin(TextInputLogin, passwordBox.Password, 
                out string message);

            if (!isLoginSuccessfull)
            {
                MessageBox.Show(message);
                return;
            }

            _loginWindow.Hide();

            var loginSuccessfulMessageBox = new LoginSuccessfulMessageBox();
            loginSuccessfulMessageBox.Show();
            
            var gebruiker =_dao.GetUser(TextInputLogin);

            var beheerPlanten = new BeheerPlanten(gebruiker);
            beheerPlanten.Show(); 
            
            loginSuccessfulMessageBox.Close();
            _loginWindow.Close();
        }

        //close the window when it gets cancelled
        public void Cancel()
        {
            _loginWindow.Close();
        }
    }
}
