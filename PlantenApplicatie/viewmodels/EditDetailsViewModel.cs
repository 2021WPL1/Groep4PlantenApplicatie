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
    public class EditDetailsViewModel : ViewModelBase
    {

        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private const string TextSeparator = ",\n";
        private Dictionary<string, List<string>> _prefixes;

        private string _selectedPrefixKey;
        private string _selectedType;
        private string _selectedFamily;
        private string _selectedGenus;
        private string _selectedSpecies;
        private string _selectedVariant;
        private string _textInputMin;
        private string _textInputMax;


        //observable collections
        public ObservableCollection<string> Types { get; set; }

        public ObservableCollection<string> Families { get; set; }

        public ObservableCollection<string> Genus { get; set; }

        public ObservableCollection<string> Species { get; set; }
        public ObservableCollection<string> Variants { get; set; }


        public ObservableCollection<string> PrefixKeys => _prefixKeys;

        private ObservableCollection<string> _prefixKeys;


        //button commands
        public ICommand SaveCommand { get; set; }

        //constructor 
        public EditDetailsViewModel(Plant selectedPlant)
        {

            SaveCommand = new DelegateCommand(Save);

            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            CreatePrefixesAndProperties();
            SelectedPrefixKey = _prefixKeys[0];

            Types = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();
            Species = new ObservableCollection<string>();
            Variants = new ObservableCollection<string>();

            LoadDetails();
            LoadSelectedValue();
        }

        public string DetailsPrefixes => string.Join(":\n", _prefixes[_selectedPrefixKey]) + ":";





        //selected values
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
            get =>  _textInputMin;
            
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

        public string SelectedPrefixKey
        {
            get => _selectedPrefixKey;
            set
            {
                _selectedPrefixKey = value;
                OnPropertyChanged("DetailsPrefixes");
            }

        }

        private void CreatePrefixesAndProperties()
        {
            _prefixes = new Dictionary<string, List<string>>();

            _prefixes["Plant"] = new List<string>()
            {
                "Type", "Familie", "Geslacht", "Soort", "Variant", "Minimum plantdichtheid",
                "Maximum plantdichtheid"
            };
            _prefixes["Fenotype"] = new List<string>()
            {
                "Bladgrootte", "Bladvorm", "Ratio Bloei/Blad", "Bloeiwijze", "Habitus", "Levensvorm"
            };
            _prefixes["Abiotiek"] = new List<string>()
            {
                "Bezonning", "Grondsoort", "Vochtbehoefte", "Voedingsbehoefte", "Antagonische omgeving",
                "Habitats"
            };
            _prefixes["Commensalisme"] = new List<string>()
            {
                "Ontwikkelsnelheid", "Strategie", "Sociabiliteit"
            };
            _prefixes["Extra eigenschappen"] = new List<string>()
            {
                "Nectarwaarde", "Pollenwaarde", "Bijvriendelijke", "Vlindervriendelijk", "Eetbaar",
                "Kruidgebruik", "Geurend", "Vorstgevoelig"
            };
            _prefixes["Beheer"] = new List<string>()
            {
                "Beheerdaad", "Omschrijving", "Datum bereik"
            };
            _prefixes["Foto"] = new List<string>()
            {
                "Eigenschap", "Locatie (URL)"
            };

            _prefixKeys = new ObservableCollection<string>(_prefixes.Keys);
        }


        //load de verschillende details op basis van welke key er is geselecteerd

        public void LoadDetails()
        {
            switch (SelectedPrefixKey)
            {
                case "Plant": LoadSubjectPlant();
                    break;
                  
            }

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
            foreach(var soort in species)
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