using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    public class TabFenoTypeViewModel : ViewModelBase
    {
        // private variabelen Davy & Jim
        private readonly PlantenDao _plantenDao = PlantenDao.Instance;
        
        private Plant _selectedPlant;
        
        private int? _selectedBladgrootte;
        
        private string? _selectedBladvorm;
        private string? _selectedBloeiWijze;
        private string? _selectedHabitus;
        private string? _selectedLevensvorm;
        private string? _selectedSpruitFenologie;
        private string? _selectedFenotypeEigenschappen;
        private string? _selectedFenoTypesMulti;
        private string? _selectedFenoMultiMaand;
        
        private FenotypeMulti? _selectedPlantFenoTypeMulti;

        // private variabelen (Davy)
        private bool _isManager;

        // Constructor Davy
        public TabFenoTypeViewModel(Plant selectedPlant, Gebruiker gebruiker)
        {
            SelectedPlant = selectedPlant;
            //variabelen (Davy &Jim)

            EditFenoTypeCommand = new DelegateCommand(EditFenoType);
            AddFenotypeMultiCommand = new DelegateCommand(AddFenotypeMulti);
            EditFenotypeMultiCommand = new DelegateCommand(EditFenotypeMulti);
            DeleteFenotypeMultiPlantCommand = new DelegateCommand(DeleteFenotypeMultiPlant);
            
            // methoden om comboboxen Fenotype in te laden (Davy & Jim)
            LoadAllProperties();
            
            SetAuthorizedActionsByRole(gebruiker);
        }
        
        // collecties (lijsten) Davy & Jim
        public ObservableCollection<int> FenoBladgroottes { get; } = new();
        
        public ObservableCollection<string> FenoBladvormen { get; } = new();
        public ObservableCollection<string> FenoBloeiwijzes { get; } = new();
        public ObservableCollection<string> FenoHabitussen { get; } = new();
        public ObservableCollection<string> FenoLevensvormen { get; } = new();
        public ObservableCollection<string> FenoSpruitFenologieen { get; } = new();
        public ObservableCollection<string> FenotypeEigenschappen { get; } = new();
        public ObservableCollection<string> FenoMultiMaand { get; } = new();
        
        public ObservableCollection<FenotypeMulti> PlantFenoTypesMulti { get; } = new();

        public ObservableCollection<string> FenoTypesMulti { get; } = new();

        // knop commando's fenotype Davy
        public ICommand EditFenoTypeCommand { get; }
        public ICommand AddFenotypeMultiCommand { get; }
        public ICommand EditFenotypeMultiCommand { get; }
        public ICommand DeleteFenotypeMultiPlantCommand { get; }

        public bool IsManager
        {
            get => _isManager;
            private set
            {
                _isManager = value;
                OnPropertyChanged("IsManager");
            }
        }

        // Getters and setters selected waardes (Davy & Jim)
        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedFenoTypesMulti
        {
            private get => _selectedFenoTypesMulti;
            set
            {
                _selectedFenoTypesMulti = value;

                OnPropertyChanged();
            }
        }

        public FenotypeMulti? SelectedPlantFenoTypeMulti
        {
            private get => _selectedPlantFenoTypeMulti;
            set
            {
                _selectedPlantFenoTypeMulti = value;
                OnPropertyChanged();
            }
        }

        // TODO: check
        public int? SelectedBladgrootte
        {
            get => _selectedBladgrootte;
            set
            {
                _selectedBladgrootte = value;
                OnPropertyChanged();
            }
        }
        public string? SelectedBladvorm
        {
            private get => _selectedBladvorm;
            set
            {
                _selectedBladvorm = value;
                OnPropertyChanged();
            }
        }
        
        public string? SelectedBloeiwijze
        {
            private get => _selectedBloeiWijze;
            set
            {
                _selectedBloeiWijze = value;
                OnPropertyChanged();
            }
        }
        
        public string? SelectedHabitus
        {
            private get => _selectedHabitus;
            set
            {
                _selectedHabitus = value;
                OnPropertyChanged();
            }
        }
        
        public string? SelectedLevensvorm
        {
            private get => _selectedLevensvorm;
            set
            {
                _selectedLevensvorm = value;
                OnPropertyChanged();
            }
        }
        
        public string? SelectedSpruitFenologie
        {
            private get => _selectedSpruitFenologie;
            set
            {
                _selectedSpruitFenologie = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedFenotypeEigenschappen
        {
            private get => _selectedFenotypeEigenschappen;
            set
            {
                _selectedFenotypeEigenschappen = value;
                ChangeEigenschappen();
                OnPropertyChanged();
            }
        }

        public string? SelectedFenoMultiMaand
        {
            private get => _selectedFenoMultiMaand;
            set
            {
                _selectedFenoMultiMaand = value;
                OnPropertyChanged();
            }
        }

        private void LoadAllProperties()
        {
            LoadFenoBladgrootte();
            LoadFenoBladvorm();
            LoadFenoBloeiwijze();
            LoadFenoHabitus();
            LoadFenoLevensVorm();
            LoadFenoSpruitFenologie();
            LoadFenoTypesMultiPlant();
            LoadEigenschappen();
            LoadFenoMultiMaanden();
            LoadKleur();
            LoadSelectedValues();
        }
        
        //controleer welke rol de gebruiker heeft
        private void SetAuthorizedActionsByRole(Gebruiker gebruiker)
        {
            IsManager = gebruiker.Rol.ToLower() == "manager";
        }

        private static void RefreshObservableCollection<T>(ICollection<T> collection, IEnumerable<T> data) 
        {
            collection.Clear();

            foreach (var elem in data)
            {
                collection.Add(elem);
            }
        }

        //laad de verschillende gegevens in de comboboxes (Davy & Jim)
        private void LoadFenoBladgrootte()
        {
            RefreshObservableCollection(FenoBladgroottes, _plantenDao.GetFenoBladGrootte()
                .Select(bg => Convert.ToInt32(bg)));
        }

        private void LoadFenoBladvorm()
        {
            RefreshObservableCollection(FenoBladvormen, _plantenDao.GetFenoBladVorm());
        }

        private void LoadFenoBloeiwijze()
        {
            RefreshObservableCollection(FenoBloeiwijzes, _plantenDao.GetFenoBloeiWijze());
        }

        private void LoadFenoHabitus()
        {
            RefreshObservableCollection(FenoHabitussen, _plantenDao.GetFenoHabitus());
        }
        
        private void LoadFenoLevensVorm()
        {
            RefreshObservableCollection(FenoLevensvormen, _plantenDao.GetFenoLevensVorm());
        }
        
        private void LoadFenoSpruitFenologie()
        {
            RefreshObservableCollection(FenoSpruitFenologieen, _plantenDao.GetFenoFenologie());
        }

        private void LoadFenoTypesMultiPlant()
        {
            RefreshObservableCollection(PlantFenoTypesMulti, _plantenDao.GetFenoMultis(SelectedPlant));
        }

        // TODO: create constant somewhere for this
        private void LoadEigenschappen()
        {
            FenotypeEigenschappen.Clear();

            FenotypeEigenschappen.Add("bladhoogte");
            FenotypeEigenschappen.Add("bladkleur");
            FenotypeEigenschappen.Add("bloeihoogte");
            FenotypeEigenschappen.Add("bloeikleur");
        }

        private void LoadKleur()
        {
            RefreshObservableCollection(FenoTypesMulti, _plantenDao.GetFenoKleur()
                .Select(fk => fk.NaamKleur));
        }
        
        private void LoadHoogte()
        {
            // TODO: constants please
            RefreshObservableCollection(FenoTypesMulti, Enumerable.Range(0, 30)
                .Select(n => (n * 10)
                    .ToString()));
        }
        
        private void LoadFenoMultiMaanden()
        {
            FenoMultiMaand.Clear();
            
            // TODO: this should not be hard coded 
            FenoMultiMaand.Add("Jan");
            FenoMultiMaand.Add("Feb");
            FenoMultiMaand.Add("Mar");
            FenoMultiMaand.Add("Apr");
            FenoMultiMaand.Add("May");
            FenoMultiMaand.Add("Jun");
            FenoMultiMaand.Add("Jul");
            FenoMultiMaand.Add("Aug");
            FenoMultiMaand.Add("Sep");
            FenoMultiMaand.Add("Okt");
            FenoMultiMaand.Add("Nov");
            FenoMultiMaand.Add("Dec");
        }

        private void LoadSelectedValues()
        {
            var fenotype = _selectedPlant.Fenotype
                .SingleOrDefault();

            if (fenotype?.Bladgrootte is null) return;

            SelectedBladgrootte = (int)fenotype.Bladgrootte;
            SelectedBladvorm = fenotype.Bladvorm;
            SelectedBloeiwijze = fenotype.Bloeiwijze;
            SelectedSpruitFenologie = fenotype.Spruitfenologie;
            SelectedHabitus = fenotype.Habitus;
            SelectedLevensvorm = fenotype.Levensvorm;
        }

        //wijzig de fenotype van de plant. Als een plant nog geen fenotype heeft word er eentje aangemaakt met de geselecteerde waardes (Jim)
        private void EditFenoType()
        {
            var fenotype = _plantenDao.GetFenotypeFromPlant(SelectedPlant);

            if (fenotype is null)
            {
                _plantenDao.AddFenotype(SelectedPlant, (int)SelectedBladgrootte!, SelectedBladvorm, null, 
                    SelectedBloeiwijze, SelectedHabitus, SelectedLevensvorm, SelectedSpruitFenologie);
            }
            else
            {
                _plantenDao.ChangeFenotype(SelectedPlant, SelectedBladgrootte, SelectedBladvorm, null, 
                    SelectedBloeiwijze, SelectedHabitus, SelectedLevensvorm, SelectedSpruitFenologie);
            }
        }

        //voeg de FenotypeMulti toe aan de plant met de geselecteerde waardes (Jim)
        private void AddFenotypeMulti()
        {
            _plantenDao.AddMultiFenotype(SelectedPlant, SelectedFenotypeEigenschappen, SelectedFenoMultiMaand, 
                SelectedFenoTypesMulti);
            
            LoadFenoTypesMultiPlant();
        }

        //wijzig de geselecteerde FenotypeMulti (Jim)

        private void EditFenotypeMulti()
        {
            _plantenDao.ChangeMultiFenotype(SelectedPlantFenoTypeMulti, SelectedFenotypeEigenschappen, 
                SelectedFenoMultiMaand, SelectedFenoTypesMulti);
            
            LoadFenoTypesMultiPlant();
        }

        //laad de geselecteerde waardes van een Fenotype multi in (Jim)
        // TODO: fix this 
        private void LoadSelectedValuesMulti()
        {
            SelectedFenotypeEigenschappen = SelectedPlantFenoTypeMulti.Eigenschap;
            SelectedFenoMultiMaand = SelectedPlantFenoTypeMulti.Maand;
            SelectedFenoTypesMulti = SelectedPlantFenoTypeMulti.Waarde;
        }

        //laat de verschillende waardes in op basis van de eigenschap dat geselecteerd is (Jim)
        private void ChangeEigenschappen()
        {
            switch (SelectedFenotypeEigenschappen.ToLower())
            {
                case "bladhoogte":
                    LoadHoogte();
                    break;
                case "bladkleur":
                    LoadKleur();
                    break;
                case "bloeihoogte":
                    LoadHoogte();
                    break;
                case "bloeikleur":
                    LoadKleur();
                    break;
            }
        }

        //verwijder de geselecteerde fenotypemulti van de listview (Jim)
        private void DeleteFenotypeMultiPlant()
        {
            if (SelectedPlantFenoTypeMulti is not null)
            {

                _plantenDao.RemoveMultiFenotype(SelectedPlantFenoTypeMulti);
            }
            else
            {
                MessageBox.Show("Gelieve een fenotype te selecteren uit de listview",
                   "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            LoadFenoTypesMultiPlant();
        }
    }
}
