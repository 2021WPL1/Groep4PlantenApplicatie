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
    // MVVM Detailscherm Lily  GUI: Jim&Liam
    public class TabPlantDetailsViewModel : ViewModelBase
    {

        
        private readonly PlantenDao _dao;
        private Plant _selectedPlant;
        private Dictionary<string, List<string>> _prefixes;
        private ObservableCollection<string> _prefixKeys;
        private string _selectedPrefixKey;

        private string _selectedType;
        private string _selectedFamily;
        private string _selectedGenus;
        private string _selectedSpecies;
        private string _selectedVariant;
        private string _textInputMin;
        private string _textInputMax;
        //button commands

        public ICommand SaveCommand { get; set; }

        // Constructor Lily
        public TabPlantDetailsViewModel(Plant selectedPlant)
        {

            SaveCommand = new DelegateCommand(Save);
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            // onderstaande variabelen voor tabblad details plant
            CreatePrefixesAndProperties();
            SelectedPrefixKey = _prefixKeys[0];




            Types = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();
            Species = new ObservableCollection<string>();
            Variants = new ObservableCollection<string>();

            CreatePlantDetailsList();
            LoadSubjectPlant();
            LoadSelectedValue();
        }

        // Getters and setters selected waardes (Lily)
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
        // ObservableCollection + prefixes om de plantdetails te kunnen weergeven
        public ObservableCollection<string> PrefixKeys => _prefixKeys;


        public ObservableCollection<string> Types { get; set; }

        public ObservableCollection<string> Families { get; set; }

        public ObservableCollection<string> Genus { get; set; }

        public ObservableCollection<string> Species { get; set; }
        public ObservableCollection<string> Variants { get; set; }

        // string.join om de labels te veranderen per onderwerp
        public string DetailsPrefixes => string.Join(":\n", _prefixes[_selectedPrefixKey]) + ":";

        public string Details => string.Join("\n", CreatePlantDetailsList());

        public string SelectedPrefixKey
        {
            get => _selectedPrefixKey;
            set
            {
                _selectedPrefixKey = value;
                OnPropertyChanged("DetailsPrefixes");
            }
        }

        // Maakt een dictionary aan en een lijst van de keys.
        private void CreatePrefixesAndProperties()
        {
            _prefixes = new Dictionary<string, List<string>>();
            
            _prefixes["Plant"] = new List<string>()
            {
                "Type", "Familie", "Geslacht", "Soort", "Variant", "Minimum plantdichtheid", 
                "Maximum plantdichtheid"
            };
            _prefixKeys = new ObservableCollection<string>(_prefixes.Keys);
        }


        // Maakt de detail lijst op voor onderwerp plant
        private List<object> CreatePlantDetailsList()
        {
            return new List<object> { 
                _selectedPlant.Type, _selectedPlant.Familie, _selectedPlant.Geslacht, 
                _selectedPlant.Soort, _selectedPlant.Variant, _selectedPlant.PlantdichtheidMin, 
                _selectedPlant.PlantdichtheidMax 
            };
        }

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


        public void Save()
        {

            _dao.ChangePlant(SelectedPlant, SelectedType, SelectedFamily, SelectedGenus, SelectedSpecies, SelectedVariant, Convert.ToInt16(TextInputMin), Convert.ToInt16(TextInputMax));
            TextInputMin = string.Empty;
            TextInputMax = string.Empty;


        }
     

    }
}