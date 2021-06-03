using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    //class and GUI (Davy)
    public class TabExtraPropertiesViewModel : ViewModelBase
    {
        // button commands (Davy)
        public ICommand AddExtraCommand { get; set; }
        public ICommand EditExtraCommand { get; set; }
        public ICommand RemoveExtraCommand { get; set; }

        //observable collections for the listview/combobox (Davy)
        public ObservableCollection<ExtraEigenschap> ManageExtraProperties { get; set; }
        public ObservableCollection<string> Nectars { get; set; }
        public ObservableCollection<string> Pollen { get; set; }

        //private variables (Davy)
        private Plant _selectedPlant;
        private ExtraEigenschap _selectedExtraProperties;

        //DAO to access the methods (Davy)
        private readonly PlantenDao _plantenDao;

        private string _selectedNectarValue;
        private string _selectedPollenValue;

        private bool _isCheckedBeeFriendly;
        private bool _isCheckedButterflyFriendly;
        private bool _isCheckedEdible;
        private bool _isCheckedHerbUse;
        private bool _isCheckedFragrant;
        private bool _isCheckedFrostSensitive;

        private Gebruiker _selectedUser;
        private bool _IsManager;

        //constructor with the given user and selected plant
        public TabExtraPropertiesViewModel(Plant selectedPlant, Gebruiker user)
        {
            SelectedPlant = selectedPlant;
            _plantenDao = PlantenDao.Instance;
            SelectedUser = user;

            ManageExtraProperties = new ObservableCollection<ExtraEigenschap>();
            Nectars = new ObservableCollection<string>();
            Pollen = new ObservableCollection<string>();

            AddExtraCommand = new DelegateCommand(AddExtra);
            EditExtraCommand = new DelegateCommand(EditExtra);
            RemoveExtraCommand = new DelegateCommand(RemoveExtra);

            //load the different values into the comboboxes (Davy)
            LoadManagerExtraProperties();
            LoadNectars();
            LoadPollen();

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

        //getters setters for the plant and different checkboxes (Davy)
        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }
        public ExtraEigenschap SelectedExtraProperty
        {
            private get => _selectedExtraProperties;
            set
            {
                _selectedExtraProperties = value;
                OnPropertyChanged();
            }
        }
        public bool IsCheckedBeeFriendly
        {
            get { return _isCheckedBeeFriendly; }
            set
            {
                if (_isCheckedBeeFriendly == value)
                {
                    return;
                }

                _isCheckedBeeFriendly = value;
            }
        }

        public bool IsCheckedButterflyFriendly
        {
            get { return _isCheckedButterflyFriendly; }
            set
            {
                if (_isCheckedButterflyFriendly == value)
                {
                    return;
                }

                _isCheckedButterflyFriendly = value;
            }
        }

        public bool IsCheckedEdible
        {
            get { return _isCheckedEdible; }
            set
            {
                if (_isCheckedEdible == value)
                {
                    return;
                }

                _isCheckedEdible = value;
            }
        }

        public bool IsCheckedFragrant
        {
            get { return _isCheckedFragrant; }
            set
            {
                if (_isCheckedFragrant == value)
                {
                    return;
                }

                _isCheckedFragrant = value;
            }
        }

        public bool IsCheckedFrostSensitive
        {
            get { return _isCheckedFrostSensitive; }
            set
            {
                if (_isCheckedFrostSensitive == value)
                {
                    return;
                }

                _isCheckedFrostSensitive = value;
            }
        }

        public bool IsCheckedHerbUse
        {
            get { return _isCheckedHerbUse; }
            set
            {
                if (_isCheckedHerbUse == value)
                {
                    return;
                }

                _isCheckedHerbUse = value;
            }
        }

        public string SelectedNectarValue
        {
            private get => _selectedNectarValue;
            set
            {
                _selectedNectarValue = value;
                OnPropertyChanged();
            }
        }

        public string SelectedPollenValue
        {
            private get => _selectedPollenValue;
            set
            {
                _selectedPollenValue = value;
                OnPropertyChanged();
            }
        }

        //load the different values into the comboboxes (Davy)

        private void LoadManagerExtraProperties()
        {
            var properties = _plantenDao.GetExtraProperties(SelectedPlant);

            ManageExtraProperties.Clear();

            foreach (var property in properties)
            {
                ManageExtraProperties.Add(property);
            }
        }

        private void LoadNectars()
        {
            var nectars = _plantenDao.GetExtraNectarValue();

            Nectars.Clear();

            foreach (var nectar in nectars)
            {
                Nectars.Add(nectar);
            }
        }

        private void LoadPollen()
        {
            var pollen = _plantenDao.GetExtraPollenValue();

            Pollen.Clear();

            foreach (var pol in pollen)
            {
                Pollen.Add(pol);
            }
        }

      
        //if there is no extra properties for the plant, one will be made. Plant can only have 1(Davy)
        public void AddExtra()
        {
            ExtraEigenschap extraProperty = new ExtraEigenschap();
            extraProperty.PlantId = SelectedPlant.PlantId;
            extraProperty.Nectarwaarde = SelectedNectarValue;
            extraProperty.Pollenwaarde = SelectedPollenValue;
            extraProperty.Bijvriendelijke = IsCheckedBeeFriendly;
            extraProperty.Vlindervriendelijk = IsCheckedButterflyFriendly;
            extraProperty.Eetbaar = IsCheckedEdible;
            extraProperty.Kruidgebruik = IsCheckedHerbUse;
            extraProperty.Geurend = IsCheckedFragrant;
            extraProperty.Vorstgevoelig = IsCheckedFrostSensitive;

            string message = _plantenDao.CreateExtraProperty(extraProperty);

            if (message != String.Empty)
            {
                MessageBox.Show(message);
            }


            //reload the extra properties
            LoadManagerExtraProperties();
        }
        //edit the extra properties of the plant(Davy)
        public void EditExtra()
        {
            ExtraEigenschap extraProperty = SelectedExtraProperty;

            if (extraProperty != null)
            {
                extraProperty.Nectarwaarde = SelectedNectarValue;
                extraProperty.Pollenwaarde = SelectedPollenValue;
                extraProperty.Bijvriendelijke = IsCheckedBeeFriendly;
                extraProperty.Vlindervriendelijk = IsCheckedButterflyFriendly;
                extraProperty.Eetbaar = IsCheckedEdible;
                extraProperty.Kruidgebruik = IsCheckedHerbUse;
                extraProperty.Geurend = IsCheckedFragrant;
                extraProperty.Vorstgevoelig = IsCheckedFrostSensitive;

                _plantenDao.EditExtraProperty(extraProperty);
            }
            else
            {
                MessageBox.Show("Gelieve eerst een extra eigenschap te selecteren.");
            }
            LoadManagerExtraProperties();
        }

        //delete the extra property of the plant (Davy)
        public void RemoveExtra()
        {
            //set a variable extra eigenschap to the selected extra properties to delete
            ExtraEigenschap extraProperty = SelectedExtraProperty;
                    
            if (SelectedExtraProperty != null)
            {
                _plantenDao.DeleteExtraProperty(extraProperty);
            }
            else
            {
                MessageBox.Show("Gelieve eerst een extra eigenschap te selecteren uit de lijst.");
            }

            //reload the listview
            LoadManagerExtraProperties();
        }

        //check which roles the user has. and if the user is an old student(Gebruiker)
        //He can only observe the selected values of the plant (Davy,Jim)
        private void UserRole()
        {
            IsManager = SelectedUser.Rol.ToLower() == "manager";
        }
    }
    
}
