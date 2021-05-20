using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    public class TabFenoTypeViewModel : ViewModelBase
    {
        // private variabelen Davy
        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private FenoBladgrootte _selectedBladgrootte;
        private FenoBladvorm _selectedBladvorm;
        private FenoBloeiwijze _selectedBloeiWijze;
        private FenoHabitus _selectedHabitus;
        private FenoKleur _selectedKleur;
        private FenoLevensvorm _selectedLevensvorm;
        private FenoSpruitfenologie _selectedSpruitFenologie;
        private Fenotype _selectedFenoType;

        private FenotypeMulti _selectedFenoTypeMulti;


        // collecties (lijsten) Davy
        public ObservableCollection<FenoBladgrootte> FenoBladgroottes { get; set; }
        public ObservableCollection<FenoBladvorm> FenoBladvormen { get; set; }
        public ObservableCollection<FenoBloeiwijze> FenoBloeiwijzes { get; set; }
        public ObservableCollection<FenoHabitus> FenoHabitussen { get; set; }
        public ObservableCollection<FenoKleur> FenoKleuren { get; set; }
        public ObservableCollection<FenoLevensvorm> FenoLevensvormen { get; set; }
        public ObservableCollection<FenoSpruitfenologie> FenoSpruitFenologieen { get; set; }

        public ObservableCollection<Fenotype> Fenotypes { get; set; }
        public ObservableCollection<FenotypeMulti> FenoTypesMulti { get; set; }


        // knop commando's fenotype Davy
        public ICommand AddFenoTypeCommand { get; set; }
        public ICommand EditFenoTypeCommand { get; set; }
        public ICommand RemoveFenoTypeCommand { get; set; }

        // Constructor Davy
        public TabFenoTypeViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
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
            FenoTypesMulti = new ObservableCollection<FenotypeMulti>();

            // methoden om comboboxen Fenotype in te laden (Davy)
            LoadFenoBladgrootte();
            LoadFenoBladvorm();
            LoadFenoBloeiwijze();
            LoadFenoHabitus();
            LoadFenoKleur();
            LoadFenoLevensVorm();
            LoadFenoSpruitFenologie();

            LoadFenoTypes();
            LoadFenoTypesMulti();
        }
        
        // Getters and setters selected waardes (Davy)
        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }

        public FenotypeMulti SelectedFenoTypeMulti
        {
            private get => _selectedFenoTypeMulti;
            set
            {
                _selectedFenoTypeMulti = value;
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

        private void LoadFenoTypesMulti()
        {
            var fenotypesMulti = _dao.GetFenoTypesMulti();

            FenoTypesMulti.Clear();

            foreach (var fenotypeMulti in fenotypesMulti)
            {
                FenoTypesMulti.Add(fenotypeMulti);
            }
        }
    }
}
