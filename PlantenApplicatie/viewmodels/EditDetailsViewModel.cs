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

        private const string TextSeparator = ",\n";

        private readonly PlantenDao _dao;
        private Dictionary<string, List<string>> _prefixes;
        private ObservableCollection<string> _prefixKeys;
        private string _selectedPrefixKey;
        
        
        public ICommand SaveCommand { get; set; }
        private string _selectedType;
        public ObservableCollection<string> Types { get; set; }


        public EditDetailsViewModel(Plant selectedPlant)
        {

            SaveCommand = new DelegateCommand(Save);

            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            CreatePrefixesAndProperties();
            SelectedPrefixKey = _prefixKeys[0];
            Types = new ObservableCollection<string>();

            LoadDetails();
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

        public string SelectedType
        {
            get => _selectedType;
            set
            {
                _selectedType = value;
                OnPropertyChanged();
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


        public void LoadDetails()
        {
            switch (SelectedPrefixKey)
            {
                case "Plant":
                    var types = _dao.GetTypes();

                    Types.Clear();

                    foreach (var type in types)
                    {
                        Types.Add(type);
                    }
                    break;

            }

        }

        public void Save()
        {
            _dao.ChangePlant(SelectedType, null,null,null,null);
        }
      
    }

    

}
