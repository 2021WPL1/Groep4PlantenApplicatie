using PlantenApplicatie.Data;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly PlantenDao _dao;

        // button commando's
        public ICommand LoginCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }

        private string _textInputLogin;

        private Window _loginWindow;

        public LoginViewModel(Window window)
        {
            _loginWindow = window;
            LoginCommand = new DelegateCommand<string>(Login);
            CancelCommand = new DelegateCommand(Cancel);

            _dao = PlantenDao.Instance;
        }

        public string TextInputLogin
        {
            get => _textInputLogin;
            set
            {
                _textInputLogin = value;
                OnPropertyChanged(_textInputLogin);
            }
        }

        public void Login(string password)
        {
            bool isLoginSuccessfull = _dao.CheckLogin(TextInputLogin, password, out string message);

            MessageBox.Show(message);

            if (!isLoginSuccessfull) return;

            var gebruiker =_dao.GetGebruiker(TextInputLogin);
                
            _loginWindow.Hide();
                
            var beheerPlanten = new BeheerPlanten(gebruiker);
            beheerPlanten.Show(); 
        }

        // geef de textboxen weer leeg terug
        public void Cancel()
        {
            TextInputLogin = String.Empty;
        }
    }
}
