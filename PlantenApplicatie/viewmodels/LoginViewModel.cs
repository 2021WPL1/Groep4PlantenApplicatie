using PlantenApplicatie.Data;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
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
        private string _textInputPassword;

        private Window _loginWindow;

        public LoginViewModel(Window window)
        {
            _loginWindow = window;
            LoginCommand = new DelegateCommand(Login);
            CancelCommand = new DelegateCommand(Cancel);
            ForgotPasswordCommand = new DelegateCommand(ForgotPassword);

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

        public string TextInputPassword
        {
            get => _textInputPassword;

            set
            {
                _textInputPassword = value;
                OnPropertyChanged(_textInputPassword);
            }
        }

        public void Login()
        {
            string message = String.Empty;

            bool myBool = _dao.CheckLogin(TextInputLogin, TextInputPassword, out message);

            if (myBool == true)
            {
                MessageBox.Show(message);
                _loginWindow.Hide();
                BeheerPlanten beheerPlanten = new BeheerPlanten();
                beheerPlanten.Show(); 
                
            } else
            {
                MessageBox.Show(message);
            }
        }

        // geef de textboxen weer leeg terug
        public void Cancel()
        {
            TextInputLogin = String.Empty;
            TextInputPassword = String.Empty;
        }

        public void ForgotPassword()
        {
            Wachtwoord_vergeten wachtwoord_Vergeten = new Wachtwoord_vergeten();
            wachtwoord_Vergeten.Show();
            _loginWindow.Hide();
        }
    }
}
