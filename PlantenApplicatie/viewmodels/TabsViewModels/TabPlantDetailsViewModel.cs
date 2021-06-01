using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;

namespace PlantenApplicatie.viewmodels
{
    // MVVM Detailscherm Lily,Jim  GUI: Jim&Liam
    public class TabPlantDetailsViewModel : ViewModelBase
    {
        //private variabelen  (Jim & Lily)
        private readonly PlantenDao _dao;
        private Plant _selectedPlant;

        private string _selectedType;
        private string _selectedFamily;
        private string _selectedGenus;
        private string _selectedSpecies;
        private string _selectedVariant;
        private string _textInputMin;
        private string _textInputMax;

        // private variabelen (Davy)
        private Gebruiker _selectedGebruiker;
        private bool _IsManager;

        //button command (Jim)
        public ICommand SaveCommand { get; set; }

        // Constructor Lily
        public TabPlantDetailsViewModel(Plant selectedPlant,Gebruiker gebruiker)
        {
            SelectedGebruiker = gebruiker;
            _selectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            // onderstaande variabelen voor tabblad details plant

            Types = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();
            Species = new ObservableCollection<string>();
            Variants = new ObservableCollection<string>();



            SaveCommand = new DelegateCommand(Save);
            //laad de gegevens in 
            LoadSubjectPlant();
            LoadSelectedValue();
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

        // Getters and setters selected waardes (Lily & Jim)
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


        // ObservableCollection om de plantdetails te kunnen weergeven (Lily)


        public ObservableCollection<string> Types { get; set; }

        public ObservableCollection<string> Families { get; set; }

        public ObservableCollection<string> Genus { get; set; }

        public ObservableCollection<string> Species { get; set; }
        public ObservableCollection<string> Variants { get; set; }




      
        //laad de waardes van de plant in (Jim)
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

        //laad de lijsten in de combobox (Jim)
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

        //sla de veranderde plant op (Jim)
        public void Save()
        {
            SelectedPlant = _dao.ChangePlant(SelectedPlant, SelectedType, SelectedFamily, SelectedGenus, SelectedSpecies, SelectedVariant,
                Convert.ToInt16(TextInputMin), Convert.ToInt16(TextInputMax));

        }
     

    }
}