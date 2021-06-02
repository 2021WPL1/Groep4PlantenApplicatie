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
    public class TabGebruikerViewModel : ViewModelBase
    {
        public ObservableCollection<Gebruiker> Users { get; set; }

        private PlantenDao _dao;

        public ICommand AddUserCommand { get; set; }

        public ICommand EditUserCommand { get; set; }

        public ICommand EditPasswordCommand { get; set; }

        public ICommand DeleteUserCommand { get; set; }

        private Gebruiker _selectedGebruiker;
        private Gebruiker OrigineleGebruiker;
        private bool _IsManager;

        private Window _tabGebruikerWindow;

        public TabGebruikerViewModel(Window window, Gebruiker gebruiker)
        {
            _tabGebruikerWindow = window;
            SelectedGebruiker = gebruiker;
            // originelegebruiker gaat gebruiker als waarde krijgen en is vereist om het wachtwoord te wijzigen van jezelf
            // ipv een geselecteerde gebruiker uit de lijst
            OrigineleGebruiker = gebruiker;
            _dao = PlantenDao.Instance;
            AddUserCommand = new DelegateCommand(AddUser);
            EditUserCommand = new DelegateCommand(EditUser);
            EditPasswordCommand = new DelegateCommand(EditPassword);
            DeleteUserCommand = new DelegateCommand(DeleteUser);

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

        public bool IsManager
        {
            get => _IsManager;
            set
            {
                _IsManager = value;
                OnPropertyChanged("IsManager");
            }
        }


        //controleer welke rol de gebruiker heeft
        private void UserRole()
        {
            switch (SelectedGebruiker.Rol.ToLower())
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
        public Gebruiker SelectedGebruiker
        {
            private get => _selectedGebruiker;
            set
            {
                _selectedGebruiker = value;
                OnPropertyChanged();
            }
        }

        private void AddUser()
        {
            AddGebruiker addGebruiker = new AddGebruiker(SelectedGebruiker);
            addGebruiker.Show();
            _tabGebruikerWindow.Close();
        }

        private void EditUser()
        {
            EditGebruiker editGebruiker = new EditGebruiker(SelectedGebruiker);
            editGebruiker.Show();
            _tabGebruikerWindow.Close();
        }

        private void EditPassword()
        {
            WijzigWachtwoord wijzigWachtwoord = new WijzigWachtwoord(OrigineleGebruiker);
            wijzigWachtwoord.Show();
        }

        private void DeleteUser()
        {
            _dao.RemoveGebruiker(SelectedGebruiker);
            LoadUsers();
        }
    }
}
