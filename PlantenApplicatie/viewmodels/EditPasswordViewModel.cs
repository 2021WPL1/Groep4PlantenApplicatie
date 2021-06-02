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
    public class EditPasswordViewModel : ViewModelBase
    {
        private readonly PlantenDao _dao;

        // button commando's
        public ICommand EditCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        private string _textInputPassword;
        private string _textInputPasswordConfirm;

        // private variabelen (Davy)
        private User _selectedGebruiker;
        private Window _editPasswordWindow;

        public EditPasswordViewModel(Window window, User gebruiker)
        {
            SelectedGebruiker = gebruiker;
            _editPasswordWindow = window;

            EditCommand = new DelegateCommand(Edit);
            CloseCommand = new DelegateCommand(Close);

            _dao = PlantenDao.Instance;
        }

        public User SelectedGebruiker
        {
            private get => _selectedGebruiker;
            set
            {
                _selectedGebruiker = value;
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
        
        private void Edit()
        {
            if (TextInputPassword == TextInputPasswordConfirm)
            {
                string message = _dao.UpdateUser(SelectedGebruiker.Emailadres, TextInputPassword);

                MessageBox.Show(message);
            }
        }

        private void Close()
        {
            _editPasswordWindow.Close();
        }
    }
}
