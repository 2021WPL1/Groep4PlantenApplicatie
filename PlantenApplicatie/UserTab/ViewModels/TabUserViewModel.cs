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

        //button commands 
        public ICommand AddUserCommand { get; set; }
        public ICommand EditUserCommand { get; set; }
        public ICommand EditPasswordCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

        private PlantenDao _dao;

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
        
        //check which roles the user has. and if the user is an old student(Gebruiker)
        //He can only observe the selected values of the plant (Davy,Jim)
        private void UserRole()
        {
            IsManager = SelectedUser.Rol.ToLower() == "manager";

        }

        // show all the users in listview
        private void LoadUsers()
        {
            var users = _dao.GetUsers();

            Users.Clear();

            foreach (var user in users)
            {
                Users.Add(user);
            }
        }

        //make a new window to add a user
        private void AddUser()
        {
            AddGebruiker addUser = new AddGebruiker(SelectedUser);
            _tabUserWindow.Hide();
            addUser.ShowDialog();
            LoadUsers();
            _tabUserWindow.Show();
            
        }

        // edit a user (Jim & Davy)
        private void EditUser()
        {
            if (SelectedUser is not null)
            {
                EditGebruiker editUser = new EditGebruiker(SelectedUser);
                _tabUserWindow.Hide();
                editUser.ShowDialog();
                LoadUsers();
                _tabUserWindow.Show();
            }
            else
            {
                MessageBox.Show("Gelieve een gebruiker te selecteren", "Gebruiker", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //edit the current password the user has
        private void EditPassword()
        {
            WijzigWachtwoord editPassword = new WijzigWachtwoord(OriginalUser);
            editPassword.Show();
        }

        // delete a user
        private void DeleteUser()
        {
            _dao.RemoveUser(SelectedUser);
            LoadUsers();
        }

        // log out of the application
        private void LogOut()
        {
            if (MessageBox.Show("Weet u zeker dat u wilt uitloggen?", "Logout", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            Inlogscherm loginScreen = new Inlogscherm();
            loginScreen.Show();
            _tabUserWindow.Close();
        }
    }
}
