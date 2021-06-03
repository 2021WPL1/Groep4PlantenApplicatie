using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels.TabsViewModels
{
    public class TabUserViewModel : ViewModelBase
    {
        //MVVM + GUI Davy

        public ObservableCollection<Gebruiker> Users { get; set; }

        private PlantenDao _dao;
        //button commands 
        public ICommand AddUserCommand { get; set; }

        public ICommand EditUserCommand { get; set; }

        public ICommand EditPasswordCommand { get; set; }

        public ICommand DeleteUserCommand { get; set; }

        public ICommand LogOutCommand { get; set; }

        private Gebruiker _selectedUser;
        private Gebruiker OriginalUser;
        private bool _isManager;
        private Window _tabUserWindow;

        //constructor given with user as parameter
        public TabUserViewModel(Gebruiker user,Window window)
        {
            _tabUserWindow = window;
            SelectedUser = user;
            // original owner gets the value to change the user's own password 
            // instead of a random selected user
            OriginalUser = user;
            _dao = PlantenDao.Instance;
            AddUserCommand = new DelegateCommand(AddUser);
            EditUserCommand = new DelegateCommand(EditUser);
            EditPasswordCommand = new DelegateCommand(EditPassword);
            DeleteUserCommand = new DelegateCommand(DeleteUser);
            LogOutCommand = new DelegateCommand(LogOut);

            Users = new ObservableCollection<Gebruiker>();

            LoadUsers();
            UserRole();
        }

        private void LoadUsers()
        {
            var users = _dao.GetUsers();

            Users.Clear();

            foreach (var user in users)
            {
                Users.Add(user);
            }
        }
        //boolean to check which functions the user can perform on the application (Davy)

        public bool IsManager
        {
            get => _isManager;
            set
            {
                _isManager = value;
                OnPropertyChanged("IsManager");
            }
        }

        //check which roles the user has. and if the user is an old student(Gebruiker)
        //He can only observe the selected values of the plant (Davy,Jim)
        private void UserRole()
        {
            switch (SelectedUser.Rol.ToLower())
            {
                case "manager":
                    IsManager = true;
                    break;
                case "data-collector":
                    IsManager = false;
                    break;
                case "gebruiker":
                    IsManager = false;
                    break;
            }
        }
        //the selected user is the account with which you login. This getter setter is given at the start and passes to all other viewmodels (Davy)
        public Gebruiker SelectedUser
        {
            private get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        //make a new window to add a user
        private void AddUser()
        {
            AddGebruiker addGebruiker = new AddGebruiker(SelectedUser);
            addGebruiker.Show();
            _tabUserWindow.Close();
        }

        private void EditUser()
        {
            EditGebruiker editGebruiker = new EditGebruiker(SelectedUser);
            editGebruiker.Show();
            _tabUserWindow.Close();
        }
        //edit the current password the user has
        private void EditPassword()
        {
            WijzigWachtwoord wijzigWachtwoord = new WijzigWachtwoord(OriginalUser);
            wijzigWachtwoord.Show();
        }

        private void DeleteUser()
        {
            _dao.RemoveUser(SelectedUser);
            LoadUsers();
        }
        private void LogOut()
        {
            if (MessageBox.Show("Weet u zeker dat u wilt uitloggen?", "Logout", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            this._tabUserWindow.Close();
            Inlogscherm inlogscherm = new Inlogscherm();
            inlogscherm.Show();
        }
    }
}
