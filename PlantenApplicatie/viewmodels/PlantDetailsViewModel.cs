using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;

namespace PlantenApplicatie.viewmodels
{
    public class PlantDetailsViewModel : ViewModelBase
    {
        private readonly PlantenDao _dao;
        private Plant _selectedPlant;
        private Dictionary<string, List<string>> _prefixes;
        private ObservableCollection<string> _prefixKeys;
        private string _selectedPrefixKey;
        
        public PlantDetailsViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            CreatePrefixesAndProperties();
            SelectedPrefixKey = _prefixKeys[0];
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

        public string DetailsPrefixes => string.Join(":\n", _prefixes[_selectedPrefixKey]) + ":";

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
                "Eigenschap", "Locatie (URL)" //, "Thumbnail"
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
            return new List<object> { 
                _selectedPlant.Type, _selectedPlant.Familie, _selectedPlant.Geslacht, 
                _selectedPlant.Soort, _selectedPlant.Variant, _selectedPlant.PlantdichtheidMin, 
                _selectedPlant.PlantdichtheidMax 
            };
        }
        
        private List<object> CreateFenotypeDetailsList()
        {
            // fenotype_multi?????
            var fenotype = SelectedPlant.Fenotype.First();
            
            return new List<object>
            {
                fenotype.Bladgrootte, fenotype.Bladvorm, fenotype.RatioBloeiBlad,
                fenotype.Bloeiwijze, fenotype.Habitus, fenotype.Levensvorm
            };
        }
        
        private List<object> CreateAbiotiekDetailsList()
        {   
            var abiotiek = SelectedPlant.Abiotiek.First();
            var abiotiekMultiValues = SelectedPlant.AbiotiekMulti
                .Select(am => am.Waarde)
                .ToList();
            
            return new List<object>
            {
                abiotiek.Bezonning, abiotiek.Grondsoort, abiotiek.Vochtbehoefte, 
                abiotiek.Voedingsbehoefte, abiotiek.AntagonischeOmgeving, 
                string.Join(", ", _dao.GetHabitatsByValues(abiotiekMultiValues)
                    .Select(ah => ah.Waarde))
            };
        }
        
        private List<object> CreateCommensalismeDetailsList()
        {
            var commensalisme = SelectedPlant.Commensalisme.First();
            var commensalismeMultiValues = SelectedPlant.CommensalismeMulti
                .Select(cm => cm.Waarde)
                .ToList();

            return new List<object>
            {
                string.Join(", ", _dao.GetCommSociabiliteitByValues(commensalismeMultiValues)
                    .Select(cs => cs.Waarde)), 
                commensalisme.Ontwikkelsnelheid, commensalisme.Strategie 
            };
        }
        
        private List<object> CreateExtraEigenschappenDetailsList()
        {
            var extraEigenschappen = SelectedPlant.ExtraEigenschap.First();

            return new List<object> 
            { 
                extraEigenschappen.Nectarwaarde, extraEigenschappen.Pollenwaarde, extraEigenschappen.Bijvriendelijke, 
                extraEigenschappen.Vlindervriendelijk, extraEigenschappen.Eetbaar, extraEigenschappen.Kruidgebruik, 
                extraEigenschappen.Geurend, extraEigenschappen.Vorstgevoelig
            };
        }
        
        private List<object> CreateBeheerDetailsList()
        {
            return new List<object> { _selectedPlant.BeheerMaand
                    .SelectMany(b => new[] {
                        string.Join(", ", b.Beheerdaad),
                        string.Join(", ", b.Omschrijving)//,
                        // create method to generate month ranges
                    }) };
        }
        
        private List<object> CreateFotoDetailsList()
        {
            return new List<object> { _selectedPlant.Foto
                    .SelectMany(f => new[] {
                        string.Join(", ", f.Eigenschap),
                        string.Join(", ", f.UrlLocatie) // only url to image given
                    }) };
        }

        private List<string> GenerateMonthRanges(BeheerMaand beheerMaand)
        {
            var monthRanges = new Dictionary<int, int>();

            return null;
        }
    }
}