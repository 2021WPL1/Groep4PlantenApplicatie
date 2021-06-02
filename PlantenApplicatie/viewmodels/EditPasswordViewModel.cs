using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    //ViewModel Made by Davy
    public class EditPasswordViewModel : ViewModelBase
    {
        private readonly PlantenDao _dao;

        // button commands
        public ICommand EditCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        private string _textInputPassword;
        private string _textInputPasswordConfirm;

        //private variables (Davy)
        private Gebruiker _selectedUser;
        private Window _editPasswordWindow;

        //constructor
        public EditPasswordViewModel(Window window, Gebruiker user)
        {
            SelectedUser = user;
            _editPasswordWindow = window;

            EditCommand = new DelegateCommand(Edit);
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

        public string TextInputPassword
        {
            get => _textInputPassword;

            set
            {
                _textInputPassword = value;
                OnPropertyChanged(_textInputPassword);
            }
        }
        public string TextInputPasswordConfirm
        {
            get => _textInputPasswordConfirm;

            set
            {
                _textInputPasswordConfirm = value;
                OnPropertyChanged(_textInputPasswordConfirm);
            }
        }
        
        //edit the password of the user and updates it in the database
        private void Edit()
        {
            if (TextInputPassword == TextInputPasswordConfirm)
            {
                string message = _dao.UpdateUser(SelectedUser, TextInputPassword);

                MessageBox.Show(message);
            }
        }

        private void Close()
        {
            _editPasswordWindow.Close();
        }
    }
}
