using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    public class TabFenoTypeViewModel : ViewModelBase
    {
        // private variabelen Davy & Jim
        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private int _selectedBladgrootte;
        private string _selectedBladvorm;
        private string _selectedBloeiWijze;
        private string _selectedHabitus;
        private string _selectedLevensvorm;
        private string _selectedSpruitFenologie;
        private string _selectedFenotypeEigenschappen;
        private string _selectedFenoTypesMulti;
        private string _selectedFenoMultiMaand;
        private FenotypeMulti _selectedPlantFenoTypeMulti;

        // private variabelen (Davy)
        private Gebruiker _selectedGebruiker;
        private bool _IsManager;


        // collecties (lijsten) Davy & Jim
        public ObservableCollection<int> FenoBladgroottes { get; set; }
        public ObservableCollection<string> FenoBladvormen { get; set; }
        public ObservableCollection<string> FenoBloeiwijzes { get; set; }
        public ObservableCollection<string> FenoHabitussen { get; set; }
        public ObservableCollection<string> FenoLevensvormen { get; set; }
        public ObservableCollection<string> FenoSpruitFenologieen { get; set; }

        public ObservableCollection<string> FenotypeEigenschappen { get; set; }

        public ObservableCollection<string> FenoMultiMaand { get; set; }
        public ObservableCollection<FenotypeMulti> PlantFenoTypesMulti { get; set; }

        public ObservableCollection<string> FenoTypesMulti { get; set; }

        // knop commando's fenotype Davy
        public ICommand EditFenoTypeCommand { get; set; }
        public ICommand AddFenotypeMultiCommand { get; set; }

        public ICommand EditFenotypeMultiCommand { get; set; }

        public ICommand DeleteFenotypeMultiPlantCommand { get; set; }

        // Constructor Davy
        public TabFenoTypeViewModel(Plant selectedPlant, Gebruiker gebruiker)
        {
            SelectedGebruiker = gebruiker;
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            //variabelen (Davy &Jim)
            FenoBladgroottes = new ObservableCollection<int>();

            FenoBladvormen = new ObservableCollection<string>();
            FenoBloeiwijzes = new ObservableCollection<string>();
            FenoHabitussen = new ObservableCollection<string>();
            FenoLevensvormen = new ObservableCollection<string>();
            FenoSpruitFenologieen = new ObservableCollection<string>();
            FenotypeEigenschappen = new ObservableCollection<string>();
            FenoTypesMulti = new ObservableCollection<string>();
            FenoMultiMaand = new ObservableCollection<string>();

            PlantFenoTypesMulti = new ObservableCollection<FenotypeMulti>();


            EditFenoTypeCommand = new DelegateCommand(EditFenoType);
            AddFenotypeMultiCommand = new DelegateCommand(AddFenotypeMulti);
            EditFenotypeMultiCommand = new DelegateCommand(EditFenotypeMulti);
            DeleteFenotypeMultiPlantCommand = new DelegateCommand(DeleteFenotypeMultiPlant);
            // methoden om comboboxen Fenotype in te laden (Davy & Jim)
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
            UserRole();
        }

        //herlaad de gegevens (Jim)
        //public void Reset()
        //{
        //    LoadFenoBladgrootte();
        //    LoadFenoBladvorm();
        //    LoadFenoBloeiwijze();
        //    LoadFenoHabitus();
        //    LoadFenoLevensVorm();
        //    LoadFenoSpruitFenologie();
        //    LoadFenoTypesMultiPlant();
        //}

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


        public string SelectedFenoTypesMulti
        {
            private get => _selectedFenoTypesMulti;
            set
            {
                _selectedFenoTypesMulti = value;

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


        public int SelectedBladgrootte
        {
            get => _selectedBladgrootte;
            set
            {
                _selectedBladgrootte = value;
                OnPropertyChanged();
            }
        }
        public string SelectedBladvorm
        {
            private get => _selectedBladvorm;
            set
            {
                _selectedBladvorm = value;
                OnPropertyChanged();
            }
        }
        public string SelectedBloeiwijze
        {
            private get => _selectedBloeiWijze;
            set
            {
                _selectedBloeiWijze = value;
                OnPropertyChanged();
            }
        }
        public string SelectedHabitus
        {
            private get => _selectedHabitus;
            set
            {
                _selectedHabitus = value;
                OnPropertyChanged();
            }
        }
        public string SelectedLevensvorm
        {
            private get => _selectedLevensvorm;
            set
            {
                _selectedLevensvorm = value;
                OnPropertyChanged();
            }
        }
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
                ChangeEigenschappen();
                OnPropertyChanged();
            }
        }

        public string SelectedFenoMultiMaand
        {
            private get => _selectedFenoMultiMaand;
            set
            {
                _selectedFenoMultiMaand = value;
                OnPropertyChanged();
            }
        }





        //laad de verschillende gegevens in de comboboxes (Davy & Jim)
        private void LoadFenoBladgrootte()
        {
            var bladgroottes = _dao.GetFenoBladGrootte();

            FenoBladgroottes.Clear();

            foreach (var bladgrootte in bladgroottes)
            {
                FenoBladgroottes.Add(Convert.ToInt32(bladgrootte));
            }
        }

        private void LoadFenoBladvorm()
        {
            var bladvormen = _dao.GetFenoBladVorm();

            FenoBladvormen.Clear();

            foreach (var bladvorm in bladvormen)
            {
                FenoBladvormen.Add(bladvorm);
            }
        }

        private void LoadFenoBloeiwijze()
        {
            var bloeiwijzes = _dao.GetFenoBloeiWijze();

            FenoBloeiwijzes.Clear();

            foreach (var bloeiwijze in bloeiwijzes)
            {
                FenoBloeiwijzes.Add(bloeiwijze);
            }
        }

        private void LoadFenoHabitus()
        {
            var habitussen = _dao.GetFenoHabitus();

            FenoHabitussen.Clear();

            foreach (var habitus in habitussen)
            {
                FenoHabitussen.Add(habitus);
            }
        }
        private void LoadFenoLevensVorm()
        {
            var levensvormen = _dao.GetFenoLevensVorm();

            FenoLevensvormen.Clear();

            foreach (var levensvorm in levensvormen)
            {
                FenoLevensvormen.Add(levensvorm);
            }
        }
        private void LoadFenoSpruitFenologie()
        {
            var fenologieen = _dao.GetFenoFenologie();

            FenoSpruitFenologieen.Clear();

            foreach (var fenologie in fenologieen)
            {
                FenoSpruitFenologieen.Add(fenologie);
            }
        }

        private void LoadFenoTypesMultiPlant()
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

            FenotypeEigenschappen.Add("bladhoogte");
            FenotypeEigenschappen.Add("bladkleur");
            FenotypeEigenschappen.Add("bloeihoogte");
            FenotypeEigenschappen.Add("bloeikleur");

        }

        private void LoadKleur()
        {
            var kleuren = _dao.GetFenoKleur();
            FenoTypesMulti.Clear();

            foreach (var kleur in kleuren)
            {
                FenoTypesMulti.Add(kleur.NaamKleur);
            }
        }
        private void LoadHoogte()
        {
            FenoTypesMulti.Clear();

            int maxHoogte = 300;
            int hoogte = 0;

            while (hoogte <= maxHoogte)
            {
                FenoTypesMulti.Add(hoogte.ToString());
                hoogte += 10;
            }

        }
        private void LoadFenoMultiMaanden()
        {
            FenoMultiMaand.Clear();
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
            var fenotype = _selectedPlant.Fenotype.SingleOrDefault();

            if (fenotype is null) return;

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
            var fenotype = _dao.GetFenotypeFromPlant(SelectedPlant);

            if (fenotype == null)
            {
                _dao.AddFenotype(SelectedPlant, SelectedBladgrootte, SelectedBladvorm, null, SelectedBloeiwijze, SelectedHabitus, SelectedLevensvorm,
                    SelectedSpruitFenologie);
            }
            else
            {
                _dao.ChangeFenotype(SelectedPlant, SelectedBladgrootte, SelectedBladvorm, null, SelectedBloeiwijze, SelectedHabitus, SelectedLevensvorm,
                    SelectedSpruitFenologie);
            }
        }

        //voeg de FenotypeMulti toe aan de plant met de geselecteerde waardes (Jim)
        private void AddFenotypeMulti()
        {
            _dao.AddMultiFenotype(SelectedPlant, SelectedFenotypeEigenschappen, SelectedFenoMultiMaand, SelectedFenoTypesMulti);
            LoadFenoTypesMultiPlant();

        }

        //wijzig de geselecteerde FenotypeMulti (Jim)

        private void EditFenotypeMulti()
        {
            _dao.ChangeMultiFenotype(SelectedPlantFenoTypeMulti, SelectedFenotypeEigenschappen, SelectedFenoMultiMaand, SelectedFenoTypesMulti);
            LoadFenoTypesMultiPlant();

        }

        //laad de geselecteerde waardes van een Fenotype multi in (Jim)
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

                _dao.RemoveMultiFenotype(SelectedPlantFenoTypeMulti);
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
