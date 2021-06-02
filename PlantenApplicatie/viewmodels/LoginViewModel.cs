using PlantenApplicatie.Data;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
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
        public ICommand ForgotPasswordCommand { get; set; }

        private string _textInputLogin;
        private string _textInputPassword;

        private Window _loginWindow;

        //constructor
        public LoginViewModel(Window window)
        {
            _loginWindow = window;
            LoginCommand = new DelegateCommand(Login);
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

        public string TextInputPassword
        {
            get => _textInputPassword;

            set
            {
                _textInputPassword = value;
                OnPropertyChanged(_textInputPassword);
            }
        }

        //Login function to check if the given values match a value in the database
        public void Login()
        {
            string message = String.Empty;

            bool myBool = _dao.CheckLogin(TextInputLogin, TextInputPassword, out message);

            if (myBool == true)
            {
                var gebruiker =_dao.GetUser(TextInputLogin);
                MessageBox.Show(message);
                _loginWindow.Hide();
                BeheerPlanten beheerPlanten = new BeheerPlanten(gebruiker);
                beheerPlanten.Show(); 
                
            } else
            {
                MessageBox.Show(message);
            }
        }

        //close the window when it gets cancelled
        public void Cancel()
        {
            this._loginWindow.Close();
        }
    }
}
