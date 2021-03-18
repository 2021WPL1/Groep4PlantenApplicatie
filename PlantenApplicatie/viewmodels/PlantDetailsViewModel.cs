using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;

namespace PlantenApplicatie.viewmodels
{
    // MVVM Detailscherm Lily  GUI: Jim&Liam
    public class PlantDetailsViewModel : ViewModelBase
    {
        private const string TextSeparator = ",\n";
        
        private readonly PlantenDao _dao;
        private Plant _selectedPlant;
        private Dictionary<string, List<string>> _prefixes;
        private ObservableCollection<string> _prefixKeys;
        private string _selectedPrefixKey;
        
        // Constructor Lily
        public PlantDetailsViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            CreatePrefixesAndProperties();
            SelectedPrefixKey = _prefixKeys[0];
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

        // ObservableCollection + prefixes om de plantdetails te kunnen weergeven
        public ObservableCollection<string> PrefixKeys => _prefixKeys;

        // string.join om de labels te veranderen per onderwerp
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

        // Maakt een dictionary aan en een lijst van de keys.
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

        // Roept de correcte methode op afhangent van de geselecteerde comboboxitem
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

        // Maakt de detail lijst op voor onderwerp plant
        private List<object> CreatePlantDetailsList()
        {
            return new List<object> { 
                _selectedPlant.Type, _selectedPlant.Familie, _selectedPlant.Geslacht, 
                _selectedPlant.Soort, _selectedPlant.Variant, _selectedPlant.PlantdichtheidMin, 
                _selectedPlant.PlantdichtheidMax 
            };
        }

        // Maakt de detail lijst op voor onderwerp fenotype
        private List<object> CreateFenotypeDetailsList()
        {
            var fenotype = SelectedPlant.Fenotype.FirstOrDefault();
            
            return new List<object>
            {
                fenotype?.Bladgrootte, fenotype?.Bladvorm, fenotype?.RatioBloeiBlad,
                fenotype?.Bloeiwijze, fenotype?.Habitus, fenotype?.Levensvorm
            };
        }

        // Maakt de detail lijst op voor onderwerp abiotiek het returned een nieuwe lijst,
        // zodat het gejoined kan worden in de string
        private List<object> CreateAbiotiekDetailsList()
        {   
            var abiotiek = SelectedPlant.Abiotiek.FirstOrDefault();
            var abiotiekMultiValues = SelectedPlant.AbiotiekMulti
                .Select(am => am.Waarde)
                .ToList();
            
            return new List<object>
            {
                abiotiek?.Bezonning, abiotiek?.Grondsoort, abiotiek?.Vochtbehoefte, 
                abiotiek?.Voedingsbehoefte, abiotiek?.AntagonischeOmgeving, 
                string.Join(TextSeparator, _dao.GetHabitatsByValues(abiotiekMultiValues)
                    .Select(ah => ah.Waarde))
            };
        }

        // Maakt de detail lijst op voor onderwerp commensalisme het returned een nieuwe lijst,
        // zodat het gejoined kan worden in de string
        private List<object> CreateCommensalismeDetailsList()
        {
            var commensalisme = SelectedPlant.Commensalisme.FirstOrDefault();
            var commensalismeMultiValues = SelectedPlant.CommensalismeMulti
                .Select(cm => cm.Waarde)
                .ToList();

            return new List<object>
            {
                commensalisme?.Ontwikkelsnelheid, commensalisme?.Strategie, 
                string.Join(TextSeparator, _dao.GetCommSociabiliteitByValues(commensalismeMultiValues)
                    .Select(cs => cs.Waarde)), 
            };
        }

        // Maakt de detail lijst op voor onderwerp Extra Eigenschappen het returned een nieuwe lijst,
        // zodat het gejoined kan worden in de string
        private List<object> CreateExtraEigenschappenDetailsList()
        {
            var extraEigenschappen = SelectedPlant.ExtraEigenschap.FirstOrDefault();

            return new List<object> 
            { 
                extraEigenschappen.Nectarwaarde, extraEigenschappen.Pollenwaarde, extraEigenschappen.Bijvriendelijke, 
                extraEigenschappen.Vlindervriendelijk, extraEigenschappen.Eetbaar, extraEigenschappen.Kruidgebruik, 
                extraEigenschappen.Geurend, extraEigenschappen.Vorstgevoelig
            };
        }

        // Maakt de detail lijst op voor onderwerp beheer het returned een nieuwe lijst,
        // zodat het gejoined kan worden in de string
        // maar het geeft een return op omdat we niet weten uit welke relaties het bestaat.
        private List<object> CreateBeheerDetailsList()
        {
            return new List<object>();
        }
        // Maakt de detail lijst op voor onderwerp Foto het returned een nieuwe lijst,
        // zodat het gejoined kan worden in de string
        private List<object> CreateFotoDetailsList()
        {
            return _selectedPlant.Foto
                    .SelectMany(f => new[] {
                        string.Join(TextSeparator, f.Eigenschap),
                        string.Join(TextSeparator, f.UrlLocatie) // only url to image given
                    })
                    .Cast<object>()
                    .ToList();
        }
    }
}