using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Linq;


namespace PlantenApplicatie.viewmodels
{
    //class and GUI Liam
    public class TabCommensalismViewModel : ViewModelBase
    {
        //private selecters and the dao (Liam)
        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private string _selectedDevelopmentSpeed;
        private string _selectedStrategy;
        private string _selectedCommenProperty;
        private string _selectedCommensalismMulti;
        private CommensalismeMulti _selectedCommenMulti;

        // private variables (Davy)
        private Gebruiker _selectedUser;
        private bool _IsManager;

        //observable collections for the list and comboboxes (Liam)
        public ObservableCollection<string> CommenStrategies { get; set; }
        public ObservableCollection<string> CommenDevelopmentSpeeds { get; set; }
        public ObservableCollection<string> CommenProperties { get; set; }
        public ObservableCollection<CommensalismeMulti> CommensalismMulti { get; set; }
        public ObservableCollection<string> CommenMulti { get; set; }

        //button commands (Liam)
        public ICommand EditCommensalismCommand { get; set; }
        public ICommand AddCommensalismMultiCommand { get; set; }
        public ICommand EditCommensalismMultiCommand { get; set; }
        public ICommand RemoveCommensalismMultiCommand { get; set; }

        //constructor (Liam)
        public TabCommensalismViewModel(Plant selectedPlant, Gebruiker user)
        {
            SelectedUser = user;
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;

            CommenDevelopmentSpeeds = new ObservableCollection<string>();
            CommenStrategies = new ObservableCollection<string>();
            CommenProperties = new ObservableCollection<string>();
            CommenMulti = new ObservableCollection<string>();
            CommensalismMulti = new ObservableCollection<CommensalismeMulti>();

            //the delegate commands for the buttons (Liam)
            EditCommensalismCommand = new DelegateCommand(EditCommensalism);
            AddCommensalismMultiCommand = new DelegateCommand(AddCommenMulti);
            EditCommensalismMultiCommand = new DelegateCommand(EditCommenMulti);
            RemoveCommensalismMultiCommand = new DelegateCommand(RemoveCommenMulti);

            LoadComm();
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

        //reload the comboboxes (Liam)
        public void LoadComm()
        {
            // load the different lists in for the comboboxes and listview(Liam) 
             LoadCommenDevelopmentSpeed();
            LoadCommenStrategy();
            LoadCommenMulti();
            LoadCommenProperties();
            LoadLifeform();
            LoadSociability();
            LoadSelectedValues();
            UserRole();
        }

        //getters and setters (Liam)
        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommensalismMulti
        {
            private get => _selectedCommensalismMulti;
            set
            {
                _selectedCommensalismMulti = value;
                OnPropertyChanged();
            }
        }

        public CommensalismeMulti SelectedCommenMulti
        {
            private get => _selectedCommenMulti;
            set
            {
                _selectedCommenMulti = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommenDevelopmentSpeed
        {
            private get => _selectedDevelopmentSpeed;
            set
            {
                _selectedDevelopmentSpeed = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommenStrategy
        {
            private get => _selectedStrategy;
            set
            {
                _selectedStrategy = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommenProperties
        {
            private get => _selectedCommenProperty;
            set
            {
                _selectedCommenProperty = value;
                ChangeCommProperties();
                OnPropertyChanged();
            }
        }

        //load the different lists into the comboboxes + listview (Liam)
        private void LoadCommenDevelopmentSpeed()
        {
            var developmentSpeeds = _dao.GetCommDevelopmentSpeed();

            CommenDevelopmentSpeeds.Clear();

            foreach (var developmentspeed in developmentSpeeds)
            {
                CommenDevelopmentSpeeds.Add(developmentspeed);
            }
        }

        private void LoadCommenStrategy()
        {
            var strategies = _dao.GetCommStrategy();

            CommenStrategies.Clear();

            foreach (var strategy in strategies)
            {
                CommenStrategies.Add(strategy);
            }
        }

        private void LoadCommenMulti()
        {
            var commensalismMultis = _dao.GetCommensalismeMulti(SelectedPlant);

            CommensalismMulti.Clear();

            foreach (var commensalismMulti in commensalismMultis)
            {
                CommensalismMulti.Add(commensalismMulti);
            }
        }

        private void LoadCommenProperties()
        {
            CommenProperties.Clear();

            CommenProperties.Add("Sociabiliteit");
            CommenProperties.Add("Levensvorm");
        }

        private void LoadSociability()
        {
            var sociabilities = _dao.GetCommSociability();

            CommenMulti.Clear();

            foreach (var sociability in sociabilities)
            {
                CommenMulti.Add(sociability.Sociabiliteit);
            }
        }

        private void LoadLifeform()
        {
            var lifeforms = _dao.GetCommLifeform();

            CommenMulti.Clear();

            foreach (var lifeform in lifeforms)
            {
                CommenMulti.Add(lifeform.Levensvorm);
            }
        }

        //load the selected values into the different lists (Liam)
        private void LoadSelectedValues()
        {
            var commensalism = _selectedPlant.Commensalisme.SingleOrDefault();

            if (commensalism is null) return;

            SelectedCommenDevelopmentSpeed = commensalism.Ontwikkelsnelheid;
            SelectedCommenStrategy = commensalism.Strategie;
        }

        //Edit the commensalisme of a plant, if there is none for the current plant a new one will be made with the selected values(Liam)
        private void EditCommensalism()
        {
            var commensialism = _dao.GetCommensialism(SelectedPlant);

            if (MessageBox.Show("Wilt u de veranderingen opslaan?", "Commensalisme", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (commensialism == null)
                {
                    _dao.AddCommensalism(SelectedPlant, SelectedCommenDevelopmentSpeed, SelectedCommenStrategy);
                }
                else
                {
                    _dao.ChangeCommensalism(SelectedPlant, SelectedCommenDevelopmentSpeed, SelectedCommenStrategy);
                }
            }
        }

        //add a commensalisme multi to the plant(Liam)

        private void AddCommenMulti()
        {            
            if (SelectedCommensalismMulti is not null && SelectedCommenProperties is not null)
            {
                _dao.AddCommensalismMulti(SelectedPlant, SelectedCommenProperties, SelectedCommensalismMulti);
                LoadCommenMulti();
            }
            else
            {
                MessageBox.Show("Gelieve eerst een Eigenschap te selecteren om toe te voegen",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            } 
        }

        //edit the selected commensalisme multi of the plant (Liam)
        private void EditCommenMulti()
        {
            _dao.ChangeCommensalismMulti(SelectedCommenMulti, SelectedCommenProperties, SelectedCommensalismMulti);
            LoadCommenMulti(); 
        }



        //edit the properties of the combobox depending on which head property is selected (Liam)
        private void ChangeCommProperties()
        {
            if (SelectedCommenProperties is not null)
            {
                switch (SelectedCommenProperties.ToLower())
                {
                    case "sociabiliteit":
                        LoadSociability();
                        break;
                    case "levensvorm":
                        LoadLifeform();
                        break;
                    default:
                        break;
                }
            }
        }

        //delete the selected commensialisme multi (Liam)
        private void RemoveCommenMulti()
        {
            if (SelectedCommenMulti is not null)
            {
                _dao.DeleteCommensialismMulti(SelectedCommenMulti);
            }
            else
            {
                MessageBox.Show("Gelieve eerst een Eigenschap te selecteren om te verwijderen uit de listview",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            LoadCommenMulti();
        }
        //check which roles the user has. and if the user is an old student(Gebruiker)
        //He can only observe the selected values of the plant (Davy,Jim)
        private void UserRole()
        {
            IsManager = SelectedUser.Rol.ToLower() == "manager";

        }
    }
}
