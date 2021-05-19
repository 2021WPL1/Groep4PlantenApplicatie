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
    public class PlantDetailsViewModel : ViewModelBase
    {

        private const string TextSeparator = ",\n";
        
        private readonly PlantenDao _dao;
        private Plant _selectedPlant;
        private Dictionary<string, List<string>> _prefixes;
        private ObservableCollection<string> _prefixKeys;
        private string _selectedPrefixKey;


        // private variabelen Davy
        private BeheerMaand _selectedBeheerMaand;
        private string _textInputBeheerdaad;
        private string _textInputDescription;
        private bool _isCheckedJanuary;
        private bool _isCheckedFebruary;
        private bool _isCheckedMarch;
        private bool _isCheckedApril;
        private bool _isCheckedMay;
        private bool _isCheckedJune;
        private bool _isCheckedJuly;
        private bool _isCheckedAugust;
        private bool _isCheckedSeptember;
        private bool _isCheckedOctober;
        private bool _isCheckedNovember;
        private bool _isCheckedDecember;
        private FenoBladgrootte _selectedBladgrootte;
        private FenoBladvorm _selectedBladvorm;
        private FenoBloeiwijze _selectedBloeiWijze;
        private FenoHabitus _selectedHabitus;
        private FenoKleur _selectedKleur;
        private FenoLevensvorm _selectedLevensvorm;
        private FenoSpruitfenologie _selectedSpruitFenologie;
        private Fenotype _selectedFenoType;


        // collecties (lijsten) Davy
        public ObservableCollection<FenoBladgrootte> FenoBladgroottes { get; set; }
        public ObservableCollection<FenoBladvorm> FenoBladvormen { get; set; }
        public ObservableCollection<FenoBloeiwijze> FenoBloeiwijzes { get; set; }
        public ObservableCollection<FenoHabitus> FenoHabitussen { get; set; }
        public ObservableCollection<FenoKleur> FenoKleuren { get; set; }
        public ObservableCollection<FenoLevensvorm> FenoLevensvormen { get; set; }
        public ObservableCollection<FenoSpruitfenologie> FenoSpruitFenologieen { get; set; }

        public ObservableCollection<Fenotype> Fenotypes { get; set; }



        //button commands
        public ICommand EditDetailsCommand { get; set; }

        // knop commando's beheersdaden Davy
        public ICommand AddManagementActCommand { get; set; }
        public ICommand EditManagementActCommand { get; set; }
        public ICommand RemoveManagementActCommand { get; set; }

        // knop commando's fenotype Davy
        public ICommand AddFenoTypeCommand { get; set; }
        public ICommand EditFenoTypeCommand { get; set; }
        public ICommand RemoveFenoTypeCommand { get; set; }


        // Constructor Lily
    public PlantDetailsViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            // onderstaande variabelen voor tabblad details plant
            CreatePrefixesAndProperties();
            SelectedPrefixKey = _prefixKeys[0];
            EditDetailsCommand = new DelegateCommand(EditPlantDetails);

            // onderstaande variabelen Davy voor tabblad beheersdaden
            AddManagementActCommand = new DelegateCommand(AddManagementAct);
            EditManagementActCommand = new DelegateCommand(EditManagementAct);
            RemoveManagementActCommand = new DelegateCommand(RemoveManagementAct);
            BeheerMaanden = new ObservableCollection<BeheerMaand>();
            LoadBeheerMaanden();

            // onderstaande variabelen Davy voor tabblad Fenotype
            AddFenoTypeCommand = new DelegateCommand(AddFenoType);
            EditFenoTypeCommand = new DelegateCommand(EditFenoType);
            RemoveFenoTypeCommand = new DelegateCommand(RemoveFenoType);
            FenoBladgroottes = new ObservableCollection<FenoBladgrootte>();
            FenoBladvormen = new ObservableCollection<FenoBladvorm>();
            FenoBloeiwijzes = new ObservableCollection<FenoBloeiwijze>();
            FenoHabitussen = new ObservableCollection<FenoHabitus>();
            FenoKleuren = new ObservableCollection<FenoKleur>();
            FenoLevensvormen = new ObservableCollection<FenoLevensvorm>();
            FenoSpruitFenologieen = new ObservableCollection<FenoSpruitfenologie>();
            Fenotypes = new ObservableCollection<Fenotype>();

            // methoden om comboboxen Fenotype in te laden (Davy)
            LoadFenoBladgrootte();
            LoadFenoBladvorm();
            LoadFenoBloeiwijze();
            LoadFenoHabitus();
            LoadFenoKleur();
            LoadFenoLevensVorm();
            LoadFenoSpruitFenologie();

            LoadFenoTypes();
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

        // Getters and setters selected waardes (Davy)
        public FenoBladgrootte SelectedBladgrootte
        {
            private get => _selectedBladgrootte;
            set
            {
                _selectedBladgrootte = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public FenoBladvorm SelectedBladvorm
        {
            private get => _selectedBladvorm;
            set
            {
                _selectedBladvorm = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public FenoBloeiwijze SelectedBloeiwijze
        {
            private get => _selectedBloeiWijze;
            set
            {
                _selectedBloeiWijze = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public FenoHabitus SelectedHabitus
        {
            private get => _selectedHabitus;
            set
            {
                _selectedHabitus = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public FenoKleur SelectedKleur
        {
            private get => _selectedKleur;
            set
            {
                _selectedKleur = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public FenoLevensvorm SelectedLevensvorm
        {
            private get => _selectedLevensvorm;
            set
            {
                _selectedLevensvorm = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public FenoSpruitfenologie SelectedSpruitFenologie
        {
            private get => _selectedSpruitFenologie;
            set
            {
                _selectedSpruitFenologie = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public Fenotype SelectedFenotype
        {
            private get => _selectedFenoType;
            set
            {
                _selectedFenoType = value;
                OnPropertyChanged();
            }
        }


        // ObservableCollection + prefixes om de plantdetails te kunnen weergeven
        public ObservableCollection<string> PrefixKeys => _prefixKeys;



        // ObservableCollection om de beheermaanden weer te geven (Davy)
        public ObservableCollection<BeheerMaand> BeheerMaanden { get; set; }

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

        private void EditPlantDetails()
        {
            new EditPlantDetails(SelectedPlant).Show();
        }

        // Getters and setters selected waardes (Davy)        
        public BeheerMaand SelectedBeheerMaand
        {
            private get => _selectedBeheerMaand;
            set
            {
                _selectedBeheerMaand = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters TextInput (Davy)   
        public string TextInputBeheerdaad
        {
            get { return _textInputBeheerdaad; }
            set
            {
                _textInputBeheerdaad = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters TextInput (Davy)  
        public string TextInputDescription
        {
            get { return _textInputDescription; }
            set
            {
                _textInputDescription = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedJanuary
        {
            get { return _isCheckedJanuary; }
            set
            {
                if (_isCheckedJanuary == value)
                {
                    return;
                }

                _isCheckedJanuary = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedFebruary
        {
            get { return _isCheckedFebruary; }
            set
            {
                if (_isCheckedFebruary == value)
                {
                    return;
                }

                _isCheckedFebruary = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedMarch
        {
            get { return _isCheckedMarch; }
            set
            {
                if (_isCheckedMarch == value)
                {
                    return;
                }

                _isCheckedMarch = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedApril
        {
            get { return _isCheckedApril; }
            set
            {
                if (_isCheckedApril == value)
                {
                    return;
                }

                _isCheckedApril = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedMay
        {
            get { return _isCheckedMay; }
            set
            {
                if (_isCheckedMay == value)
                {
                    return;
                }

                _isCheckedMay = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedJune
        {
            get { return _isCheckedJune; }
            set
            {
                if (_isCheckedJune == value)
                {
                    return;
                }

                _isCheckedJune = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedJuly
        {
            get { return _isCheckedJuly; }
            set
            {
                if (_isCheckedJuly == value)
                {
                    return;
                }

                _isCheckedJuly = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedAugust
        {
            get { return _isCheckedAugust; }
            set
            {
                if (_isCheckedAugust == value)
                {
                    return;
                }

                _isCheckedAugust = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedSeptember
        {
            get { return _isCheckedSeptember; }
            set
            {
                if (_isCheckedSeptember == value)
                {
                    return;
                }

                _isCheckedSeptember = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedOctober
        {
            get { return _isCheckedOctober; }
            set
            {
                if (_isCheckedOctober == value)
                {
                    return;
                }

                _isCheckedOctober = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedNovember
        {
            get { return _isCheckedNovember; }
            set
            {
                if (_isCheckedNovember == value)
                {
                    return;
                }

                _isCheckedNovember = value;
            }
        }

        // Getters and setters bool (Davy)  
        public bool IsCheckedDecember
        {
            get { return _isCheckedDecember; }
            set
            {
                if (_isCheckedDecember == value)
                {
                    return;
                }

                _isCheckedDecember = value;
            }
        }

        // geef de BeheerMaanden weer in de listview (Davy)
        public void LoadBeheerMaanden()
        {
            var beheermaanden = _dao.GetBeheerMaanden();

            BeheerMaanden.Clear();

            foreach (var beheermaand in beheermaanden)
            {
                BeheerMaanden.Add(beheermaand);
            }
        }

        // maak een beheerdaad aan (Davy)
        private void AddManagementAct()
        {
            BeheerMaand beheerMaand = new BeheerMaand();
            beheerMaand.PlantId = SelectedPlant.PlantId;
            beheerMaand.Beheerdaad = TextInputBeheerdaad;
            beheerMaand.Omschrijving = TextInputDescription;
            beheerMaand.Jan = IsCheckedJanuary;
            beheerMaand.Feb = IsCheckedFebruary;
            beheerMaand.Mrt = IsCheckedMarch;
            beheerMaand.Apr = IsCheckedApril;
            beheerMaand.Mei = IsCheckedMay;
            beheerMaand.Jun = IsCheckedJune;
            beheerMaand.Jul = IsCheckedJuly;
            beheerMaand.Aug = IsCheckedAugust;
            beheerMaand.Sept = IsCheckedSeptember;
            beheerMaand.Okt = IsCheckedOctober;
            beheerMaand.Nov = IsCheckedNovember;
            beheerMaand.Dec = IsCheckedDecember;

            _dao.CreateBeheerMaand(beheerMaand);

            // weergeef de aangepaste lijst
            LoadBeheerMaanden();
        }

        // wijzig een beheerdaad (Davy)
        private void EditManagementAct()
        {
            BeheerMaand beheerMaand = SelectedBeheerMaand;
            beheerMaand.PlantId = SelectedPlant.PlantId;
            beheerMaand.Beheerdaad = TextInputBeheerdaad;
            beheerMaand.Omschrijving = TextInputDescription;
            beheerMaand.Jan = IsCheckedJanuary;
            beheerMaand.Feb = IsCheckedFebruary;
            beheerMaand.Mrt = IsCheckedMarch;
            beheerMaand.Apr = IsCheckedApril;
            beheerMaand.Mei = IsCheckedMay;
            beheerMaand.Jun = IsCheckedJune;
            beheerMaand.Jul = IsCheckedJuly;
            beheerMaand.Aug = IsCheckedAugust;
            beheerMaand.Sept = IsCheckedSeptember;
            beheerMaand.Okt = IsCheckedOctober;
            beheerMaand.Nov = IsCheckedNovember;
            beheerMaand.Dec = IsCheckedDecember;

            _dao.EditBeheerMaand(beheerMaand);

            LoadBeheerMaanden();
        }

        // verwijder beheerdaad (Davy)
        private void RemoveManagementAct()
        {
            // toewijzen object BeheerMaand aan geselecteerd object BeheerMaand uit listview
            BeheerMaand beheerMaand = SelectedBeheerMaand;

            // verwijder BeheerMaand uit database
            _dao.RemoveBeheerMaand(beheerMaand);

            // toon opnieuw de listview met lijst BeheerMaanden
            LoadBeheerMaanden();
        }


        private void AddFenoType() 
        {
            Fenotype fenotype = new Fenotype();
            fenotype.Bladgrootte = Int32.Parse(SelectedBladgrootte.Bladgrootte);
            fenotype.Bladvorm = SelectedBladvorm.Vorm;
            fenotype.Bloeiwijze = SelectedBloeiwijze.Naam;
            fenotype.Habitus = SelectedHabitus.Naam;
            fenotype.Levensvorm = SelectedLevensvorm.Levensvorm;
            fenotype.Spruitfenologie = SelectedSpruitFenologie.Fenologie;
            fenotype.Plant = SelectedPlant;

            _dao.CreateFenoType(fenotype);

            LoadFenoTypes();

        }

        private void EditFenoType()
        {
            Fenotype fenotype = _selectedFenoType;
            fenotype.Bladgrootte = Int32.Parse(SelectedBladgrootte.Bladgrootte);
            fenotype.Bladvorm = SelectedBladvorm.Vorm;
            fenotype.Bloeiwijze = SelectedBloeiwijze.Naam;
            fenotype.Habitus = SelectedHabitus.Naam;
            fenotype.Levensvorm = SelectedLevensvorm.Levensvorm;
            fenotype.Spruitfenologie = SelectedSpruitFenologie.Fenologie;
            fenotype.Plant = SelectedPlant;

            _dao.EditFenoType(fenotype);

            LoadFenoTypes();
        }

        private void RemoveFenoType()
        {
            Fenotype fenotype = _selectedFenoType;
            _dao.RemoveFenoType(fenotype);

            LoadFenoTypes();
        }

        // inladen gegevens (Davy)
        private void LoadFenoBladgrootte()
        {
            var bladgroottes = _dao.GetFenoBladgroottes();

            FenoBladgroottes.Clear();

            foreach (var bladgrootte in bladgroottes)
            {
                FenoBladgroottes.Add(bladgrootte);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoBladvorm()
        {
            var bladvormen = _dao.GetFenoBladvormen();

            FenoBladvormen.Clear();

            foreach (var bladvorm in bladvormen)
            {
                FenoBladvormen.Add(bladvorm);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoBloeiwijze()
        {
            var bloeiwijzes = _dao.GetFenoBloeiwijzes();

            FenoBloeiwijzes.Clear();

            foreach (var bloeiwijze in bloeiwijzes)
            {
                FenoBloeiwijzes.Add(bloeiwijze);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoHabitus()
        {
            var habitussen = _dao.GetFenoHabitussen();

            FenoHabitussen.Clear();

            foreach (var habitus in habitussen)
            {
                FenoHabitussen.Add(habitus);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoKleur()
        {
            var kleuren = _dao.getFenoKleuren();

            FenoKleuren.Clear();

            foreach (var kleur in kleuren)
            {
                FenoKleuren.Add(kleur);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoLevensVorm()
        {
            var levensvormen = _dao.GetFenoLevensvormen();

            FenoLevensvormen.Clear();

            foreach (var levensvorm in levensvormen)
            {
                FenoLevensvormen.Add(levensvorm);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoSpruitFenologie()
        {
            var fenologieen = _dao.GetFenoSpruitFenologieen();

            FenoSpruitFenologieen.Clear();

            foreach (var fenologie in fenologieen)
            {
                FenoSpruitFenologieen.Add(fenologie);
            }
        }

        private void LoadFenoTypes()
        {
            var fenotypes = _dao.GetFenoTypes();

            Fenotypes.Clear();

            foreach (var fenotype in fenotypes)
            {
                Fenotypes.Add(fenotype);
            }
        }
    }
}