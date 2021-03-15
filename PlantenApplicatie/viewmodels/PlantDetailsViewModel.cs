using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlantenApplicatie.Domain;

namespace PlantenApplicatie.viewmodels
{
    public class PlantDetailsViewModel : ViewModelBase
    {
        private Plant _selectedPlant;
        private Dictionary<string, List<string>> _prefixes;
        private ObservableCollection<string> _prefixKeys;
        private string _selectedPrefixKey;
        
        public PlantDetailsViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            CreatePrefixesAndProperties();
        }

        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> PrefixKeys => _prefixKeys;

        public string DetailsPrefixes => string.Join(":\n", _prefixes[_selectedPrefixKey]);

        public string Details => string.Join("\n", CreateDetailsList());

        public string SelectedPrefixKey
        {
            get => _selectedPrefixKey;
            set
            {
                _selectedPrefixKey = value;
                OnPropertyChanged("Details");
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
                "Sociabiliteit", "Ontwikkelsnelheid", "Strategie"
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
                "Eigenschap", "Locatie (URL)", "Thumbnail"
            };

            _prefixKeys = new ObservableCollection<string>(_prefixes.Keys);
        }

        private List<object> CreateDetailsList()
        {
            switch (_selectedPrefixKey)
            {
                case "Plant":
                    return CreatePlantDetailsList();
                case "Fenotype":
                    return CreateFenotypeDetailsList();
                case "Abiotiek":
                    return CreateAbiotiekDetailsList();
                case "Commensalisme":
                    return CreateCommensalismeDetailsList();
                case "Extra eigenschappen":
                    return CreateExtraEigenschappenDetailsList();
                case "Beheer":
                    return CreateBeheerDetailsList();
                case "Foto":
                    return CreateFotoDetailsList();
                default:
                    return new List<object>();
            }
        }

        private List<object> CreatePlantDetailsList()
        {
            return new() { _selectedPlant.Type, _selectedPlant.Familie, _selectedPlant.Geslacht, 
                _selectedPlant.Soort, _selectedPlant.Variant, _selectedPlant.PlantdichtheidMin, 
                _selectedPlant.PlantdichtheidMax };
        }
        
        private List<object> CreateFenotypeDetailsList()
        {
            return new List<object>();
        }
        
        private List<object> CreateAbiotiekDetailsList()
        {
            return new List<object>();
        }
        
        private List<object> CreateCommensalismeDetailsList()
        {
            return new List<object>();
        }
        
        private List<object> CreateExtraEigenschappenDetailsList()
        {
            return new List<object>();
        }
        
        private List<object> CreateBeheerDetailsList()
        {
            return new List<object>();
        }
        
        private List<object> CreateFotoDetailsList()
        {
            return new List<object>();
        }
    }
}