using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;

namespace PlantenApplicatie.viewmodels
{
    // MVVM Detailscherm Lily,Jim  GUI: Jim&Liam
    public class TabPlantDetailsViewModel : ViewModelBase
    {
        //button command (Jim)
        public ICommand SaveCommand { get; set; }

        // ObservableCollections binded to the comboboxes (Jim)
        public ObservableCollection<string> Types { get; set; }
        public ObservableCollection<string> Families { get; set; }
        public ObservableCollection<string> Genus { get; set; }
        public ObservableCollection<string> Species { get; set; }
        public ObservableCollection<string> Variants { get; set; }

        //private variables  (Jim & Lily)
        private readonly PlantenDao _dao;
        private Plant _selectedPlant;

        private string _selectedType;
        private string _selectedFamily;
        private string _selectedGenus;
        private string _selectedSpecies;
        private string _selectedVariant;
        private string _textInputMin;
        private string _textInputMax;

        // private variables (Davy)
        private Gebruiker _selectedUser;
        private bool _isManager;

        // Constructor Lily
        public TabPlantDetailsViewModel(Plant selectedPlant,Gebruiker user)
        {
            SelectedUser = user;
            _selectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
           
            //set the different values in the combobox as a observable collection (Jim)
            Types = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();
            Species = new ObservableCollection<string>();
            Variants = new ObservableCollection<string>();

            SaveCommand = new DelegateCommand(Save);
            //load the different values into the comboboxes
            LoadSubjectPlant();
            LoadSelectedValue();
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

        // Getters and setters selected values (Lily & Jim)
        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }

        public string SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }
        public string SelectedFamily
        {
            get => _selectedFamily;
            set
            {
                _selectedFamily = value;
                OnPropertyChanged();
            }
        }
        public string SelectedGenus
        {
            get => _selectedGenus;
            set
            {
                _selectedGenus = value;
                OnPropertyChanged();
            }
        }
        public string SelectedSpecies
        {
            get => _selectedSpecies;
            set
            {
                _selectedSpecies = value;
                OnPropertyChanged();
            }
        }
        public string SelectedVariant
        {
            get => _selectedVariant;
            set
            {
                _selectedVariant = value;
                OnPropertyChanged();
            }
        }

        public string TextInputMin
        {
            get => _textInputMin;

            set
            {
                _textInputMin = value;
                OnPropertyChanged();
            }
        }
        public string TextInputMax
        {
            get => _textInputMax;
            set
            {
                _textInputMax = value;
                OnPropertyChanged();
            }
        }

        //load the current values of the plant into the comboboxes (Jim)
        public void LoadSelectedValue()
        {
            var plant = _selectedPlant;

            SelectedType = plant.Type;
            SelectedFamily = plant.Familie;
            SelectedGenus = plant.Geslacht;
            SelectedSpecies = plant.Soort;
            SelectedVariant = plant.Variant;
            TextInputMin = plant.PlantdichtheidMin.ToString();
            TextInputMax = plant.PlantdichtheidMax.ToString();
        }

        //load all the different values into the comboboxes (Jim)
        public void LoadSubjectPlant()
        {
            var types = _dao.GetTypes();
            var families = _dao.GetUniqueFamilyNames();
            var genus = _dao.GetUniqueGenusNames();
            var species = _dao.GetUniqueSpeciesNames();
            var variants = _dao.GetUniqueVariantNames();

            Types.Clear();
            Families.Clear();
            Genus.Clear();
            Species.Clear();
            Variants.Clear();

            foreach (var type in types)
            {
                Types.Add(type);
            }
            foreach (var family in families)
            {
                Families.Add(family);
            }
            foreach (var geslacht in genus)
            {
                Genus.Add(geslacht);
            }
            foreach (var soort in species)
            {
                Species.Add(soort);
            }
            foreach (var variant in variants)
            {
                Variants.Add(variant);
            }
        }

        //save the current changes to the selectedplant (Jim)
        public void Save()
        {
            if (short.TryParse(TextInputMin, out var min) && short.TryParse(TextInputMax, out var max) 
                    && min >= 0 && min < max)
            {
                SelectedPlant = _dao.ChangePlant(SelectedPlant, SelectedType, SelectedFamily, SelectedGenus,
                    SelectedSpecies, SelectedVariant, min, max);
            }
            else
            {
                MessageBox.Show("Gelieve een geldige minimum en maximum plantdichtheid te geven",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}