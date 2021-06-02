using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels.TabsViewModels
{
    public class TabGebruikerViewModel : ViewModelBase
    {
        //MVVM + GUI Davy

        //button commands 
        public ICommand AddUserCommand { get; set; }

        public ICommand EditPasswordCommand { get; set; }

        // private variables 
        private Gebruiker _selectedGebruiker;
        private bool _IsManager;

        //constructor given with user as parameter
        public TabGebruikerViewModel(Gebruiker gebruiker)
        {
            SelectedGebruiker = gebruiker;
            AddUserCommand = new DelegateCommand(AddUser);
            EditPasswordCommand = new DelegateCommand(EditPassword);

            UserRole();
        }
        //boolean to check which functions the user can perform on the application (Davy)
        public bool IsManager
        {
            get => _IsManager;
            set
            {
                _IsManager = value;
                OnPropertyChanged("IsManager");
            }
        }

        //check which roles the user has. and if the user is an old student(Gebruiker)
        //He can only observe the selected values of the plant (Davy,Jim)
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
        //the selected user is the account with which you login. This getter setter is given at the start and passes to all other viewmodels (Davy)
        public Gebruiker SelectedGebruiker
        {
            private get => _selectedGebruiker;
            set
            {
                _selectedGebruiker = value;
                OnPropertyChanged();
            }
        }

        //make a new window to add a user
        private void AddUser()
        {
            AddGebruiker addGebruiker = new AddGebruiker();
            addGebruiker.Show();
        }
        //edit the current password the user has
        private void EditPassword()
        {
            WijzigWachtwoord wijzigWachtwoord = new WijzigWachtwoord(SelectedGebruiker);
            wijzigWachtwoord.Show();
        }
    }
}
