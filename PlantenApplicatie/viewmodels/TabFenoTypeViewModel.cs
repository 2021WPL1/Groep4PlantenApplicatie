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
        private int _selectedBladgrootte;
        private string _selectedBladvorm;
        private string _selectedBloeiWijze;
        private string _selectedHabitus;
        private string _selectedKleur;
        private string _selectedLevensvorm;
        private string _selectedSpruitFenologie;

        private string _selectedFenotypeEigenschappen;
        private string _selectedFenoTypeMulti;
        private FenotypeMulti _selectedPlantFenoTypeMulti;

        // collecties (lijsten) Davy
        public ObservableCollection<int> FenoBladgroottes { get; set; }
        public ObservableCollection<string> FenoBladvormen { get; set; }
        public ObservableCollection<string> FenoBloeiwijzes { get; set; }
        public ObservableCollection<string> FenoHabitussen { get; set; }
        public ObservableCollection<string> FenoKleuren { get; set; }
        public ObservableCollection<string> FenoLevensvormen { get; set; }
        public ObservableCollection<string> FenoSpruitFenologieen { get; set; }

        public ObservableCollection<string> FenotypeEigenschappen { get; set; }
        public ObservableCollection<string> FenoTypesMulti { get; set; }
        public ObservableCollection<FenotypeMulti> PlantFenoTypesMulti { get; set; }


        // knop commando's fenotype Davy
        public ICommand EditFenoTypeCommand { get; set; }
        public ICommand AddFenotypeMultiCommand { get; set; }

        // Constructor Davy
        public TabFenoTypeViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            // onderstaande variabelen Davy voor tabblad Fenotype
            EditFenoTypeCommand = new DelegateCommand(EditFenoType);
            FenoBladgroottes = new ObservableCollection<int>();
            FenoBladvormen = new ObservableCollection<string>();
            FenoBloeiwijzes = new ObservableCollection<string>();
            FenoHabitussen = new ObservableCollection<string>();
            FenoKleuren = new ObservableCollection<string>();
            FenoLevensvormen = new ObservableCollection<string>();
            FenoSpruitFenologieen = new ObservableCollection<string>();

            //variabelen Jim
            AddFenotypeMultiCommand = new DelegateCommand(AddFenotypeMulti);
            FenotypeEigenschappen = new ObservableCollection<string>();
            FenoTypesMulti = new ObservableCollection<string>();
            PlantFenoTypesMulti = new ObservableCollection<FenotypeMulti>();
            // methoden om comboboxen Fenotype in te laden (Davy)
            LoadFenoBladgrootte();
            LoadFenoBladvorm();
            LoadFenoBloeiwijze();
            LoadFenoHabitus();
            LoadFenoKleur();
            LoadFenoLevensVorm();
            LoadFenoSpruitFenologie();
            LoadFenoTypesMulti();
            LoadEigenschappen();

        }

        public void Reset()
        {
            LoadFenoBladgrootte();
            LoadFenoBladvorm();
            LoadFenoBloeiwijze();
            LoadFenoHabitus();
            LoadFenoKleur();
            LoadFenoLevensVorm();
            LoadFenoSpruitFenologie();
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

        public string SelectedFenoTypeMulti
        {
            private get => _selectedFenoTypeMulti;
            set
            {
                _selectedFenoTypeMulti = value;
                OnPropertyChanged();
            }
        }

        public FenotypeMulti SelectedPlantFenoTypeMulti
        {
            private get => _selectedPlantFenoTypeMulti;
            set
            {
                _selectedPlantFenoTypeMulti = value;
                OnPropertyChanged();
            }
        }


        // Getters and setters selected waardes (Davy)
        public int SelectedBladgrootte
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

        public string SelectedFenotypeEigenschappen
        {
            private get => _selectedFenotypeEigenschappen;
            set
            {
                _selectedFenotypeEigenschappen = value;
                OnPropertyChanged();
            }
        }




        private void EditFenoType()
        {
            var fenotype = _dao.GetFenotypeFromPlant(SelectedPlant);
           
            if(fenotype == null)
            {
                _dao.AddFenotype(SelectedPlant, SelectedBladgrootte,SelectedBladvorm, null, SelectedBloeiwijze, SelectedHabitus, SelectedLevensvorm);
            }
        }

       

        // inladen gegevens (Davy)
        private void LoadFenoBladgrootte()
        {
            var bladgroottes = _dao.GetFenoBladGrootte();

            FenoBladgroottes.Clear();

            foreach (var bladgrootte in bladgroottes)
            {
                FenoBladgroottes.Add(Convert.ToInt32(bladgrootte));
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

        //de multi fenotypes inladen van de geselecteerde plant
        private void LoadFenoTypesMulti()
        {
            var fenotypesMulti = _dao.GetFenoMultis(SelectedPlant);

            PlantFenoTypesMulti.Clear();

            foreach (var fenotypeMulti in fenotypesMulti)
            {
                PlantFenoTypesMulti.Add(fenotypeMulti);
            }
        }

        private void LoadEigenschappen()
        {
            FenotypeEigenschappen.Clear();

            FenotypeEigenschappen.Add("Bladhoogte");
            FenotypeEigenschappen.Add("Bladkleur");

            FenotypeEigenschappen.Add("BloeiHoogte");

            FenotypeEigenschappen.Add("BloeiKleur");

        }

        private void AddFenotypeMulti()
        {
           _dao.AddMultiFenotype(SelectedPlant, SelectedFenotypeEigenschappen, null, SelectedFenoTypeMulti);
        }
    }
}
