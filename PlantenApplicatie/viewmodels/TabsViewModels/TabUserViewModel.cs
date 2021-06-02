using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels.TabsViewModels
{
    public class TabUserViewModel : ViewModelBase
    {

        public ICommand AddUserCommand { get; set; }

        public ICommand EditPasswordCommand { get; set; }

        // private variabelen (Davy)
        private User _selectedGebruiker;
        private bool _IsManager;

        public TabUserViewModel(User gebruiker)
        {
            SelectedUser = gebruiker;
            AddUserCommand = new DelegateCommand(AddUser);
            EditPasswordCommand = new DelegateCommand(EditPassword);

            UserRole();
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
        public User SelectedUser
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
            AddGebruiker addGebruiker = new AddGebruiker();
            addGebruiker.Show();
        }

        private void EditPassword()
        {
            WijzigWachtwoord wijzigWachtwoord = new WijzigWachtwoord(SelectedUser);
            wijzigWachtwoord.Show();
        }
    }
}
