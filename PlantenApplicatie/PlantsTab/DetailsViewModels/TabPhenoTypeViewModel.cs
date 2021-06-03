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
        
        // Constructor
        public TabPhenoTypeViewModel(Plant selectedPlant, Gebruiker user)
        {
            SelectedPlant = selectedPlant;
            
            EditFenoTypeCommand = new DelegateCommand(EditPhenotype);
            AddFenotypeMultiCommand = new DelegateCommand(AddPhenotypeMulti);
            EditFenotypeMultiCommand = new DelegateCommand(EditPhenotypeMulti);
            DeleteFenotypeMultiPlantCommand = new DelegateCommand(DeletePhenotypeMultiPlant);

            LoadAllProperties();
            
            IsManager = user.Rol.ToLower() == "manager";
        }

        //observable collections for the different comboboxes and listviews ( Jim)
        public ObservableCollection<int> FenoLeafSizes { get; } = new();
        
        public ObservableCollection<string> FenoLeafShapes { get; } = new();
        public ObservableCollection<string> FenoBlooms { get; } = new();
        public ObservableCollection<string> FenoHabitussen { get; } = new();
        public ObservableCollection<string> FenoLifeforms { get; } = new();
        public ObservableCollection<string> FenoSprouts { get; } = new();
        public ObservableCollection<string> FenotypeProperties { get; } = new();
        public ObservableCollection<string> FenoMultiMonths { get; } = new();
        
        public ObservableCollection<FenotypeMulti> PlantFenoTypesMulti { get; } = new();

        public ObservableCollection<string> FenoTypesMulti { get; } = new();
        
        //button commands  (Jim)
        public ICommand EditFenoTypeCommand { get; }
        public ICommand AddFenotypeMultiCommand { get; }
        public ICommand EditFenotypeMultiCommand { get; }
        public ICommand DeleteFenotypeMultiPlantCommand { get; }
        
        public bool IsManager { get; }

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
            private get => _selectedLeafSize;
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
                OnPropertyChanged();
                ChangeProperties();
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
            RefreshObservableCollection(PlantFenoTypesMulti, _plantenDao.GetPhenoMultis(SelectedPlant));
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
                 .FirstOrDefault();

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
            var phenotype = _plantenDao.GetPhenotypeFromPlant(SelectedPlant);

            if (MessageBox.Show("Wilt u de veranderingen opslaan?", "Fenotype",
                MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            if (phenotype is null)
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
            if (SelectedPhenotypeProperties is not null && SelectedPhenoMultiMonth is not null 
                                                        && SelectedPhenoTypesMulti is not null) 
            {
                _plantenDao.AddMultiPhenotype(SelectedPlant, SelectedPhenotypeProperties, 
                    SelectedPhenoMultiMonth, SelectedPhenoTypesMulti);

                LoadPhenoTypesMultiPlant();
            }
            else
            {
                MessageBox.Show("Gelieve eerst alle velden in te vullen!",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //edit the selected fenotype multi (Jim)
        private void EditPhenotypeMulti()
        {
            if (SelectedPlantPhenoTypeMulti is not null) {
                _plantenDao.ChangeMultiPhenotype(SelectedPlantPhenoTypeMulti, SelectedPhenotypeProperties,
                    SelectedPhenoMultiMonth, SelectedPhenoTypesMulti);

                LoadPhenoTypesMultiPlant();
            }
            else
            {
                MessageBox.Show("Gelieve eerst een eigenschap te selecteren!",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
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
                
                default:
                    throw new NotImplementedException();
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
