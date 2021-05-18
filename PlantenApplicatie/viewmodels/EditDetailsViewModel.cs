using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

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




        public EditDetailsViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
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
        public string DetailsPrefixes => string.Join(":\n", _prefixes[_selectedPrefixKey]) + ":";
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
    }

    

}
