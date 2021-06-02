using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    //class and GUI (Jim)
    public class TabPhenoTypeViewModel : ViewModelBase
    {
        // private variables (Jim)
        private readonly PlantenDao _plantenDao = PlantenDao.Instance;
        private Plant _selectedPlant;

        private int? _selectedLeafSize;
        private string? _selectedLeafShape;
        private string? _selectedBloom;
        private string? _selectedHabitus;
        private string? _selectedLifeform;
        private string? _selectedSprout;
        private string? _selectedFenotypeProperties;
        private string? _selectedPhenoTypesMulti;
        private string? _selectedFenoMultiMonth;

        private FenotypeMulti? _selectedPlantFenoTypeMulti;

       
        private bool _IsManager;


        // Constructor
        public TabPhenoTypeViewModel(Plant selectedPlant, Gebruiker user)
        {
            SelectedPlant = selectedPlant;

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

            LoadAllProperties();
            SetAuthorizedActionsByRole(user);
        }

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

        //methods to load in the different lists in the comboboxes and listviews (Jim)
        public void LoadAllProperties()
        {
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
        }

        //check which roles the user has. and if the user is an old student(Gebruiker)
        //He can only observe the selected values of the plant (Davy,Jim)
        private void SetAuthorizedActionsByRole(Gebruiker user)
        {
            IsManager = user.Rol.ToLower() == "manager";
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


        public string? SelectedPhenoTypesMulti
        {
            private get => _selectedPhenoTypesMulti;
            set
            {
                _selectedPhenoTypesMulti = value;

                OnPropertyChanged();
            }
        }

        public FenotypeMulti? SelectedPlantPhenoTypeMulti
        {
            private get => _selectedPlantFenoTypeMulti;
            set
            {
                _selectedPlantFenoTypeMulti = value;
                OnPropertyChanged();
            }
        }


        public int? SelectedLeafSize
        {
            get => _selectedLeafSize;
            set
            {
                _selectedLeafSize = value;
                OnPropertyChanged();
            }
        }
        public string? SelectedLeafShape
        {
            private get => _selectedLeafShape;
            set
            {
                _selectedLeafShape = value;
                OnPropertyChanged();
            }
        }
        public string? SelectedBloom
        {
            private get => _selectedBloom;
            set
            {
                _selectedBloom = value;
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
        public string? SelectedLifeform
        {
            private get => _selectedLifeform;
            set
            {
                _selectedLifeform = value;
                OnPropertyChanged();
            }
        }
        public string? SelectedSproutPhenology
        {
            private get => _selectedSprout;
            set
            {
                _selectedSprout = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedPhenotypeProperties
        {
            private get => _selectedFenotypeProperties;
            set
            {
                _selectedFenotypeProperties = value;
                ChangeProperties();
                OnPropertyChanged();
            }
        }

        public string? SelectedPhenoMultiMonth
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
            var fenoLeafSize = _plantenDao.GetPhenoLeafSize()
               .Select(bg => Convert.ToInt32(bg));

            RefreshObservableCollection(FenoLeafSizes, fenoLeafSize);
        }

        private void LoadPhenoLeafShape()
        {
            RefreshObservableCollection(FenoLeafShapes, _plantenDao.GetPhenoLeafShape());

        }

        private void LoadPhenoInflorescence()
        {
            RefreshObservableCollection(FenoBlooms, _plantenDao.GetPhenoInflorescence());

        }

        private void LoadPhenoHabitat()
        {
            RefreshObservableCollection(FenoHabitussen, _plantenDao.GetPhenoHabitat());

        }
        private void LoadPhenoLifeform()
        {
            RefreshObservableCollection(FenoLifeforms, _plantenDao.GetPhenoLifeform());

        }
        private void LoadPhenoSproutPhenology()
        {
            RefreshObservableCollection(FenoSprouts, _plantenDao.GetPhenoSproutPhenology());

        }

        private void LoadPhenoTypesMultiPlant()
        {
            var fenotypesMulti = _plantenDao.GetPhenoMultis(SelectedPlant);

            PlantFenoTypesMulti.Clear();

            foreach (var fenotypeMulti in fenotypesMulti)
            {
                PlantFenoTypesMulti.Add(fenotypeMulti);
            }
        }

        private void LoadProperties()
        {
            RefreshObservableCollection(FenotypeProperties, _plantenDao.GetFenotypeProperties());


        }

        private void LoadColour()
        {
            var colorNames = _plantenDao.GetPhenoColour()
               .Select(fk => fk.NaamKleur);

            RefreshObservableCollection(FenoTypesMulti, colorNames);
        }
        private void LoadHeight()
        {
            var heightPossibilities = Enumerable.Range(0, PlantenDao.MaxLeafSize / 10)
                .Select(n => (n * 10)
                    .ToString());

            RefreshObservableCollection(FenoTypesMulti, heightPossibilities);

        }
        private void LoadPhenoMultiMonths()
        {
            var monthNames = new DateTimeFormatInfo().AbbreviatedMonthNames;

            RefreshObservableCollection(FenoMultiMonths, monthNames[0..12]);
        }
        private static void RefreshObservableCollection<T>(ICollection<T> collection, IEnumerable<T> data)
        {
            collection.Clear();

            foreach (var elem in data)
            {
                collection.Add(elem);
            }
        }
        //Load the selected standards in from the selected plant. If there is none the selected values are null (Jim)
        private void LoadSelectedValues()
        {
            var fenotype = _selectedPlant.Fenotype
                 .SingleOrDefault();

            if (fenotype?.Bladgrootte is null) return;

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
            var fenotype = _plantenDao.GetPhenotypeFromPlant(SelectedPlant);

            if (fenotype is null)
            {
                _plantenDao.AddPhenotype(SelectedPlant, (int)SelectedLeafSize!, SelectedLeafShape, null,
                    SelectedBloom, SelectedHabitus, SelectedLifeform, SelectedSproutPhenology);
            }
            else
            {
                _plantenDao.ChangePhenotype(SelectedPlant, SelectedLeafSize, SelectedLeafShape, null,
                    SelectedBloom, SelectedHabitus, SelectedLifeform, SelectedSproutPhenology);
            }
        }

        //add the selected fenotype multi to the current plant (Jim)
        private void AddPhenotypeMulti()
        {
            _plantenDao.AddMultiPhenotype(SelectedPlant, SelectedPhenotypeProperties, SelectedPhenoMultiMonth, SelectedPhenoTypesMulti);
            LoadPhenoTypesMultiPlant();

        }

        //edit the selected fenotype multi (Jim)

        private void EditPhenotypeMulti()
        {
            _plantenDao.ChangeMultiPhenotype(SelectedPlantPhenoTypeMulti, SelectedPhenotypeProperties, SelectedPhenoMultiMonth, SelectedPhenoTypesMulti);
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
                default:throw new NotImplementedException();
            }
        }

        //delete the selected fenotype multi from the plant (Jim)
        private void DeletePhenotypeMultiPlant()
        {

            if (SelectedPlantPhenoTypeMulti is not null)
            {

                _plantenDao.RemoveMultiPhenotype(SelectedPlantPhenoTypeMulti);
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
