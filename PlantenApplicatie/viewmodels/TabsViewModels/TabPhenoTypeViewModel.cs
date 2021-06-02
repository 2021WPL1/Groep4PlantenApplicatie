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
    public class TabPhenoTypeViewModel : ViewModelBase
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
        private User _selectedGebruiker;
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
        public TabPhenoTypeViewModel(Plant selectedPlant, User gebruiker)
        {
            SelectedUser = gebruiker;
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


            EditFenoTypeCommand = new DelegateCommand(EditPhenotype);
            AddFenotypeMultiCommand = new DelegateCommand(AddPhenotypeMulti);
            EditFenotypeMultiCommand = new DelegateCommand(EditPhenotypeMulti);
            DeleteFenotypeMultiPlantCommand = new DelegateCommand(DeletePhenotypeMultiPlant);
            // methoden om comboboxen Fenotype in te laden (Davy & Jim)
            LoadPhenoLeafSize();
            LoadPhenoLeafShape();
            LoadPhenoInflorescence();
            LoadPhenoHabitat();
            LoadPhenoLifeform();
            LoadPhenoSproutPhenology();
            LoadPhenoTypesMultiPlant();
            LoadProperties();
            LoadPhenoMultiMonths();
            LoadColour();
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
            switch (SelectedUser.Rol.ToLower())
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
        public User SelectedUser
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


        public string SelectedPhenoTypesMulti
        {
            private get => _selectedFenoTypesMulti;
            set
            {
                _selectedFenoTypesMulti = value;

                OnPropertyChanged();
            }
        }

        public FenotypeMulti SelectedPlantPhenoTypeMulti
        {
            private get => _selectedPlantFenoTypeMulti;
            set
            {
                _selectedPlantFenoTypeMulti = value;
                OnPropertyChanged();
            }
        }


        public int SelectedLeafSize
        {
            get => _selectedBladgrootte;
            set
            {
                _selectedBladgrootte = value;
                OnPropertyChanged();
            }
        }
        public string SelectedLeafShape
        {
            private get => _selectedBladvorm;
            set
            {
                _selectedBladvorm = value;
                OnPropertyChanged();
            }
        }
        public string SelectedInflorescence
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
        public string SelectedLifeform
        {
            private get => _selectedLevensvorm;
            set
            {
                _selectedLevensvorm = value;
                OnPropertyChanged();
            }
        }
        public string SelectedSproutPhenology
        {
            private get => _selectedSpruitFenologie;
            set
            {
                _selectedSpruitFenologie = value;
                OnPropertyChanged();
            }
        }

        public string SelectedPhenotypeProperties
        {
            private get => _selectedFenotypeEigenschappen;
            set
            {
                _selectedFenotypeEigenschappen = value;
                ChangeProperties();
                OnPropertyChanged();
            }
        }

        public string SelectedPhenoMultiMonth
        {
            private get => _selectedFenoMultiMaand;
            set
            {
                _selectedFenoMultiMaand = value;
                OnPropertyChanged();
            }
        }





        //laad de verschillende gegevens in de comboboxes (Davy & Jim)
        private void LoadPhenoLeafSize()
        {
            var bladgroottes = _dao.GetPhenoLeafSize();

            FenoBladgroottes.Clear();

            foreach (var bladgrootte in bladgroottes)
            {
                FenoBladgroottes.Add(Convert.ToInt32(bladgrootte));
            }
        }

        private void LoadPhenoLeafShape()
        {
            var bladvormen = _dao.GetPhenoLeafShape();

            FenoBladvormen.Clear();

            foreach (var bladvorm in bladvormen)
            {
                FenoBladvormen.Add(bladvorm);
            }
        }

        private void LoadPhenoInflorescence()
        {
            var bloeiwijzes = _dao.GetPhenoInflorescence();

            FenoBloeiwijzes.Clear();

            foreach (var bloeiwijze in bloeiwijzes)
            {
                FenoBloeiwijzes.Add(bloeiwijze);
            }
        }

        private void LoadPhenoHabitat()
        {
            var habitussen = _dao.GetPhenoHabitat();

            FenoHabitussen.Clear();

            foreach (var habitus in habitussen)
            {
                FenoHabitussen.Add(habitus);
            }
        }
        private void LoadPhenoLifeform()
        {
            var levensvormen = _dao.GetPhenoLifeform();

            FenoLevensvormen.Clear();

            foreach (var levensvorm in levensvormen)
            {
                FenoLevensvormen.Add(levensvorm);
            }
        }
        private void LoadPhenoSproutPhenology()
        {
            var fenologieen = _dao.GetPhenoSproutPhenology();

            FenoSpruitFenologieen.Clear();

            foreach (var fenologie in fenologieen)
            {
                FenoSpruitFenologieen.Add(fenologie);
            }
        }

        private void LoadPhenoTypesMultiPlant()
        {
            var fenotypesMulti = _dao.GetPhenoMultis(SelectedPlant);

            PlantFenoTypesMulti.Clear();

            foreach (var fenotypeMulti in fenotypesMulti)
            {
                PlantFenoTypesMulti.Add(fenotypeMulti);
            }
        }

        private void LoadProperties()
        {
            FenotypeEigenschappen.Clear();

            FenotypeEigenschappen.Add("bladhoogte");
            FenotypeEigenschappen.Add("bladkleur");
            FenotypeEigenschappen.Add("bloeihoogte");
            FenotypeEigenschappen.Add("bloeikleur");

        }

        private void LoadColour()
        {
            var kleuren = _dao.GetPhenoColour();
            FenoTypesMulti.Clear();

            foreach (var kleur in kleuren)
            {
                FenoTypesMulti.Add(kleur.NaamKleur);
            }
        }
        private void LoadHeight()
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
        private void LoadPhenoMultiMonths()
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

            SelectedLeafSize = (int)fenotype.Bladgrootte;
            SelectedLeafShape = fenotype.Bladvorm;
            SelectedInflorescence = fenotype.Bloeiwijze;
            SelectedSproutPhenology = fenotype.Spruitfenologie;
            SelectedHabitus = fenotype.Habitus;
            SelectedLifeform = fenotype.Levensvorm;
        }




        //wijzig de fenotype van de plant. Als een plant nog geen fenotype heeft word er eentje aangemaakt met de geselecteerde waardes (Jim)
        private void EditPhenotype()
        {
            var fenotype = _dao.GetPhenotypeFromPlant(SelectedPlant);

            if (fenotype == null)
            {
                _dao.AddPhenotype(SelectedPlant, SelectedLeafSize, SelectedLeafShape, null, SelectedInflorescence, SelectedHabitus, SelectedLifeform,
                    SelectedSproutPhenology);
            }
            else
            {
                _dao.ChangePhenotype(SelectedPlant, SelectedLeafSize, SelectedLeafShape, null, SelectedInflorescence, SelectedHabitus, SelectedLifeform,
                    SelectedSproutPhenology);
            }
        }

        //voeg de FenotypeMulti toe aan de plant met de geselecteerde waardes (Jim)
        private void AddPhenotypeMulti()
        {
            _dao.AddMultiPhenotype(SelectedPlant, SelectedPhenotypeProperties, SelectedPhenoMultiMonth, SelectedPhenoTypesMulti);
            LoadPhenoTypesMultiPlant();

        }

        //wijzig de geselecteerde FenotypeMulti (Jim)

        private void EditPhenotypeMulti()
        {
            _dao.ChangeMultiPhenotype(SelectedPlantPhenoTypeMulti, SelectedPhenotypeProperties, SelectedPhenoMultiMonth, SelectedPhenoTypesMulti);
            LoadPhenoTypesMultiPlant();

        }

        //laad de geselecteerde waardes van een Fenotype multi in (Jim)
        private void LoadSelectedValuesMulti()
        {
            SelectedPhenotypeProperties = SelectedPlantPhenoTypeMulti.Eigenschap;
            SelectedPhenoMultiMonth = SelectedPlantPhenoTypeMulti.Maand;
            SelectedPhenoTypesMulti = SelectedPlantPhenoTypeMulti.Waarde;
        }

        //laat de verschillende waardes in op basis van de eigenschap dat geselecteerd is (Jim)
        private void ChangeProperties()
        {
            switch (SelectedPhenotypeProperties.ToLower())
            {
                case "bladhoogte":
                    LoadHeight();
                    break;
                case "bladkleur":
                    LoadColour();
                    break;
                case "bloeihoogte":
                    LoadHeight();

                    break;
                case "bloeikleur":
                    LoadColour();
                    break;
            }
        }

        //verwijder de geselecteerde fenotypemulti van de listview (Jim)
        private void DeletePhenotypeMultiPlant()
        {

            if (SelectedPlantPhenoTypeMulti is not null)
            {

                _dao.RemoveMultiPhenotype(SelectedPlantPhenoTypeMulti);
            }
            else
            {
                MessageBox.Show("Gelieve een fenotype te selecteren uit de listview",
                   "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            LoadPhenoTypesMultiPlant();

        }
    }
}
