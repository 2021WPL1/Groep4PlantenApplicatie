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
    //class and GUI (Jim)
    public class TabPhenoTypeViewModel : ViewModelBase
    {
        // private variables (Jim)
        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private int _selectedLeafSize;
        private string _selectedLeafShape;
        private string _selectedBloom;
        private string _selectedHabitus;
        private string _selectedLifeform;
        private string _selectedSprout;
        private string _selectedFenotypeProperties;
        private string _selectedPhenoTypesMulti;
        private string _selectedFenoMultiMonth;
        private FenotypeMulti _selectedPlantFenoTypeMulti;

       
        private Gebruiker _selectedUser;
        private bool _IsManager;


        //observable collections for the different comboboxes and listviews ( Jim)
        public ObservableCollection<int> FenoLeafSizes { get; set; }
        public ObservableCollection<string> FenoLeafShapes { get; set; }
        public ObservableCollection<string> FenoBlooms { get; set; }
        public ObservableCollection<string> FenoHabitussen { get; set; }
        public ObservableCollection<string> FenoLifeforms { get; set; }
        public ObservableCollection<string> FenoSprouts { get; set; }

        public ObservableCollection<string> FenotypeProperties { get; set; }

        public ObservableCollection<string> FenoMultiMonths { get; set; }
        public ObservableCollection<FenotypeMulti> PlantFenoTypesMulti { get; set; }

        public ObservableCollection<string> FenoTypesMulti { get; set; }

        //button commands  (Jim)
        public ICommand EditFenoTypeCommand { get; set; }
        public ICommand AddFenotypeMultiCommand { get; set; }

        public ICommand EditFenotypeMultiCommand { get; set; }

        public ICommand DeleteFenotypeMultiPlantCommand { get; set; }

        // Constructor
        public TabPhenoTypeViewModel(Plant selectedPlant, Gebruiker user)
        {
            SelectedUser = user;
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            //variables (Jim)
            FenoLeafSizes = new ObservableCollection<int>();
            FenoLeafShapes = new ObservableCollection<string>();
            FenoBlooms = new ObservableCollection<string>();
            FenoHabitussen = new ObservableCollection<string>();
            FenoLifeforms = new ObservableCollection<string>();
            FenoSprouts = new ObservableCollection<string>();
            FenotypeProperties = new ObservableCollection<string>();
            FenoTypesMulti = new ObservableCollection<string>();
            FenoMultiMonths = new ObservableCollection<string>();

            PlantFenoTypesMulti = new ObservableCollection<FenotypeMulti>();


            EditFenoTypeCommand = new DelegateCommand(EditPhenotype);
            AddFenotypeMultiCommand = new DelegateCommand(AddPhenotypeMulti);
            EditFenotypeMultiCommand = new DelegateCommand(EditPhenotypeMulti);
            DeleteFenotypeMultiPlantCommand = new DelegateCommand(DeletePhenotypeMultiPlant);
            //methods to load in the different lists in the comboboxes and listviews (Jim)
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

        //boolean to check which functions the user can perform on the application (Davy)
        public bool IsManager
        {
            get => _IsManager;
            set
            {
                _IsManager = value;
                OnPropertyChanged("IsManager");
            }
        }


        //check which roles the user has. and if the user is an old student(Gebruiker)
        //He can only observe the selected values of the plant (Davy,Jim)
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
        //the selected user is the account with which you login. This getter setter is given at the start and passes to all other viewmodels (Davy)
        public Gebruiker SelectedUser
        {
            private get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }


        // Getters and setters (Davy & Jim)
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
            private get => _selectedPhenoTypesMulti;
            set
            {
                _selectedPhenoTypesMulti = value;

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
            get => _selectedLeafSize;
            set
            {
                _selectedLeafSize = value;
                OnPropertyChanged();
            }
        }
        public string SelectedLeafShape
        {
            private get => _selectedLeafShape;
            set
            {
                _selectedLeafShape = value;
                OnPropertyChanged();
            }
        }
        public string SelectedBloom
        {
            private get => _selectedBloom;
            set
            {
                _selectedBloom = value;
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
            private get => _selectedLifeform;
            set
            {
                _selectedLifeform = value;
                OnPropertyChanged();
            }
        }
        public string SelectedSproutPhenology
        {
            private get => _selectedSprout;
            set
            {
                _selectedSprout = value;
                OnPropertyChanged();
            }
        }

        public string SelectedPhenotypeProperties
        {
            private get => _selectedFenotypeProperties;
            set
            {
                _selectedFenotypeProperties = value;
                ChangeProperties();
                OnPropertyChanged();
            }
        }

        public string SelectedPhenoMultiMonth
        {
            private get => _selectedFenoMultiMonth;
            set
            {
                _selectedFenoMultiMonth = value;
                OnPropertyChanged();
            }
        }





        //load the different values into the comboboxes  (Davy & Jim)
        private void LoadPhenoLeafSize()
        {
            var bladgroottes = _dao.GetPhenoLeafSize();

            FenoLeafSizes.Clear();

            foreach (var bladgrootte in bladgroottes)
            {
                FenoLeafSizes.Add(Convert.ToInt32(bladgrootte));
            }
        }

        private void LoadPhenoLeafShape()
        {
            var bladvormen = _dao.GetPhenoLeafShape();

            FenoLeafShapes.Clear();

            foreach (var bladvorm in bladvormen)
            {
                FenoLeafShapes.Add(bladvorm);
            }
        }

        private void LoadPhenoInflorescence()
        {
            var bloeiwijzes = _dao.GetPhenoInflorescence();

            FenoBlooms.Clear();

            foreach (var bloeiwijze in bloeiwijzes)
            {
                FenoBlooms.Add(bloeiwijze);
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

            FenoLifeforms.Clear();

            foreach (var levensvorm in levensvormen)
            {
                FenoLifeforms.Add(levensvorm);
            }
        }
        private void LoadPhenoSproutPhenology()
        {
            var fenologieen = _dao.GetPhenoSproutPhenology();

            FenoSprouts.Clear();

            foreach (var fenologie in fenologieen)
            {
                FenoSprouts.Add(fenologie);
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
            FenotypeProperties.Clear();

            FenotypeProperties.Add("bladhoogte");
            FenotypeProperties.Add("bladkleur");
            FenotypeProperties.Add("bloeihoogte");
            FenotypeProperties.Add("bloeikleur");

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
            FenoMultiMonths.Clear();
            FenoMultiMonths.Add("Jan");
            FenoMultiMonths.Add("Feb");
            FenoMultiMonths.Add("Mar");
            FenoMultiMonths.Add("Apr");
            FenoMultiMonths.Add("May");
            FenoMultiMonths.Add("Jun");
            FenoMultiMonths.Add("Jul");
            FenoMultiMonths.Add("Aug");
            FenoMultiMonths.Add("Sep");
            FenoMultiMonths.Add("Okt");
            FenoMultiMonths.Add("Nov");
            FenoMultiMonths.Add("Dec");
        }
        //Load the selected standards in from the selected plant. If there is none the selected values are null (Jim)
        private void LoadSelectedValues()
        {
            var fenotype = _selectedPlant.Fenotype.SingleOrDefault();

            if (fenotype is null) return;

            SelectedLeafSize = (int)fenotype.Bladgrootte;
            SelectedLeafShape = fenotype.Bladvorm;
            SelectedBloom = fenotype.Bloeiwijze;
            SelectedSproutPhenology = fenotype.Spruitfenologie;
            SelectedHabitus = fenotype.Habitus;
            SelectedLifeform = fenotype.Levensvorm;
        }




        //edit the fenotype of the selected plant, if there is none a fenotype will be added to the plant with the selected values  (Jim)
        private void EditPhenotype()
        {
            var fenotype = _dao.GetPhenotypeFromPlant(SelectedPlant);

            if (fenotype == null)
            {
                _dao.AddPhenotype(SelectedPlant, SelectedLeafSize, SelectedLeafShape, null, SelectedBloom, SelectedHabitus, SelectedLifeform,
                    SelectedSproutPhenology);
            }
            else
            {
                _dao.ChangePhenotype(SelectedPlant, SelectedLeafSize, SelectedLeafShape, null, SelectedBloom, SelectedHabitus, SelectedLifeform,
                    SelectedSproutPhenology);
            }
        }

        //add the selected fenotype multi to the current plant (Jim)
        private void AddPhenotypeMulti()
        {
            _dao.AddMultiPhenotype(SelectedPlant, SelectedPhenotypeProperties, SelectedPhenoMultiMonth, SelectedPhenoTypesMulti);
            LoadPhenoTypesMultiPlant();

        }

        //edit the selected fenotype multi (Jim)

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

        //load the different properties in when the head property changes (Jim)
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

        //delete the selected fenotype multi from the plant (Jim)
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
