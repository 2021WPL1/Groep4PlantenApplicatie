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
        private string _selectedBladgrootte;
        private string _selectedBladvorm;
        private string _selectedBloeiWijze;
        private string _selectedHabitus;
        private string _selectedKleur;
        private string _selectedLevensvorm;
        private string _selectedSpruitFenologie;
        private Fenotype _selectedFenoType;

        private Gebruiker _selectedGebruiker;
        private bool _IsManager;

        private FenotypeMulti _selectedFenoTypeMulti;


        // collecties (lijsten) Davy
        public ObservableCollection<string> FenoBladgroottes { get; set; }
        public ObservableCollection<string> FenoBladvormen { get; set; }
        public ObservableCollection<string> FenoBloeiwijzes { get; set; }
        public ObservableCollection<string> FenoHabitussen { get; set; }
        public ObservableCollection<string> FenoKleuren { get; set; }
        public ObservableCollection<string> FenoLevensvormen { get; set; }
        public ObservableCollection<string> FenoSpruitFenologieen { get; set; }

        public ObservableCollection<Fenotype> Fenotypes { get; set; }
        public ObservableCollection<FenotypeMulti> FenoTypesMulti { get; set; }


        // knop commando's fenotype Davy
        public ICommand EditFenoTypeCommand { get; set; }

        // Constructor Davy
        public TabFenoTypeViewModel(Plant selectedPlant, Gebruiker gebruiker)
        {
            SelectedGebruiker = gebruiker;
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            // onderstaande variabelen Davy voor tabblad Fenotype
            EditFenoTypeCommand = new DelegateCommand(EditFenoType);
            FenoBladgroottes = new ObservableCollection<string>();
            FenoBladvormen = new ObservableCollection<string>();
            FenoBloeiwijzes = new ObservableCollection<string>();
            FenoHabitussen = new ObservableCollection<string>();
            FenoKleuren = new ObservableCollection<string>();
            FenoLevensvormen = new ObservableCollection<string>();
            FenoSpruitFenologieen = new ObservableCollection<string>();
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

            LoadFenoTypesMulti();
            UserRole();
        }
        public bool IsManager
        {
            get => _IsManager;
            set
            {
                _IsManager = value;
                OnPropertyChanged("IsManager");
            }
        }


        //controleer welke rol de gebruiker heeft
        private void UserRole()
        {
            switch (SelectedGebruiker.Rol.ToLower())
            {
                case "manager":
                    IsManager = true;
                    break;
                case "data-collector":
                    IsManager = false;
                    break;
                case "gebruiker":
                    IsManager = false;
                    break;
            }
        }
        public Gebruiker SelectedGebruiker
        {
            private get => _selectedGebruiker;
            set
            {
                _selectedGebruiker = value;
                OnPropertyChanged();
            }
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
        public string SelectedBladgrootte
        {
            private get => _selectedBladgrootte;
            set
            {
                _selectedBladgrootte = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public string SelectedBladvorm
        {
            private get => _selectedBladvorm;
            set
            {
                _selectedBladvorm = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public string SelectedBloeiwijze
        {
            private get => _selectedBloeiWijze;
            set
            {
                _selectedBloeiWijze = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public string SelectedHabitus
        {
            private get => _selectedHabitus;
            set
            {
                _selectedHabitus = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public string SelectedKleur
        {
            private get => _selectedKleur;
            set
            {
                _selectedKleur = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public string SelectedLevensvorm
        {
            private get => _selectedLevensvorm;
            set
            {
                _selectedLevensvorm = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected waardes (Davy)
        public string SelectedSpruitFenologie
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



       

        private void EditFenoType()
        {
           SelectedFenotype = _dao.ChangeFenotype(SelectedFenotype, Convert.ToInt32(SelectedBladgrootte), SelectedBladvorm,
               null, SelectedBloeiwijze, SelectedHabitus, SelectedLevensvorm);
        }

       

        // inladen gegevens (Davy)
        private void LoadFenoBladgrootte()
        {
            var bladgroottes = _dao.GetFenoBladGrootte();

            FenoBladgroottes.Clear();

            foreach (var bladgrootte in bladgroottes)
            {
                FenoBladgroottes.Add(bladgrootte);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoBladvorm()
        {
            var bladvormen = _dao.GetFenoBladVorm();

            FenoBladvormen.Clear();

            foreach (var bladvorm in bladvormen)
            {
                FenoBladvormen.Add(bladvorm);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoBloeiwijze()
        {
            var bloeiwijzes = _dao.GetFenoBloeiWijze();

            FenoBloeiwijzes.Clear();

            foreach (var bloeiwijze in bloeiwijzes)
            {
                FenoBloeiwijzes.Add(bloeiwijze);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoHabitus()
        {
            var habitussen = _dao.GetFenoHabitus();

            FenoHabitussen.Clear();

            foreach (var habitus in habitussen)
            {
                FenoHabitussen.Add(habitus);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoKleur()
        {
            var kleuren = _dao.GetFenoKleur();

            FenoKleuren.Clear();

            foreach (var kleur in kleuren)
            {
                FenoKleuren.Add(kleur);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoLevensVorm()
        {
            var levensvormen = _dao.GetFenoLevensVorm();

            FenoLevensvormen.Clear();

            foreach (var levensvorm in levensvormen)
            {
                FenoLevensvormen.Add(levensvorm);
            }
        }

        // inladen gegevens (Davy)
        private void LoadFenoSpruitFenologie()
        {
            var fenologieen = _dao.GetFenoFenologie();

            FenoSpruitFenologieen.Clear();

            foreach (var fenologie in fenologieen)
            {
                FenoSpruitFenologieen.Add(fenologie);
            }
        }

        

        private void LoadFenoTypesMulti()
        {
            var fenotypesMulti = _dao.GetFenoMultis(SelectedPlant);

            FenoTypesMulti.Clear();

            foreach (var fenotypeMulti in fenotypesMulti)
            {
                FenoTypesMulti.Add(fenotypeMulti);
            }
        }
    }
}
