using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    class BeheerPlantenViewModel : ViewModelBase
    {
        private readonly PlantenDao _plantenDao;

        private Plant? _selectedPlant;
        
        private string? _selectedType;
        private string? _selectedSpecies;
        private string? _selectedGenus;
        private string? _selectedFamily;
        private string? _selectedVariant;

        // The GUI binds to this variable through a property, therefore it will not be null,
        // so we tell the compiler it is not null
        private string _plantName = null!; 
        
        public ICommand ShowDetailsCommand { get; }
        public ICommand ResetCommand { get; }

        public ObservableCollection<Plant> Plants { get; set; }
        
        public ObservableCollection<string> Types { get; set; }
        public ObservableCollection<string> Species { get; set; }
        public ObservableCollection<string> Families { get; set; }
        public ObservableCollection<string> Genus { get; set; }
        public ObservableCollection<string> Variants { get; set; }
        
        public BeheerPlantenViewModel()
        {
            _plantenDao = PlantenDao.Instance;
            
            ShowDetailsCommand = new DelegateCommand(ShowDetails);
            ResetCommand = new DelegateCommand(ResetInputs);

            Plants = new ObservableCollection<Plant>();
            
            Types = new ObservableCollection<string>();
            Species = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();
            Variants = new ObservableCollection<string>();
            
            FilterComboBoxes();
        }
        
        public Plant? SelectedPlant
        {
            get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedSpecies
        {
            get => _selectedSpecies;
            set
            {
                MaintainCorrectSetterValueAfterFilter(ref _selectedSpecies, value);
                OnPropertyChanged();
            }
        }
        public string? SelectedGenus
        {
            get => _selectedGenus;
            set
            {
                MaintainCorrectSetterValueAfterFilter(ref _selectedGenus, value);
                OnPropertyChanged();
            }
        }

        public string? SelectedType
        {
            get => _selectedType;
            set
            {
                MaintainCorrectSetterValueAfterFilter(ref _selectedType, value);
                OnPropertyChanged();
            }
        }

        public string? SelectedFamily
        {
            get => _selectedFamily;
            set
            {
                MaintainCorrectSetterValueAfterFilter(ref _selectedFamily, value);
                OnPropertyChanged();
            }
        }

        public string? SelectedVariant
        {
            get => _selectedVariant;
            set
            {
                MaintainCorrectSetterValueAfterFilter(ref _selectedVariant, value);
                OnPropertyChanged();
            }
        }

        private void MaintainCorrectSetterValueAfterFilter(ref string? field, string? value)
        {
            if (value == field) return;

            var oldValue = field;
            field = value;

            if (value is not null)
            {
                FilterComboBoxes();
            }
            else
            {
                field = oldValue;
            }
        }

        public string PlantName
        {
            get => _plantName;
            set
            {
                _plantName = value;
                FilterComboBoxes();
                OnPropertyChanged();
            }
        }

        public void FilterComboBoxes()
        {
            SearchPlanten();

            LoadTypes();
            LoadSpecies();
            LoadFamilies();
            LoadGenus();
            LoadVariants();
            
            OnPropertyChanged("Types");
            OnPropertyChanged("Species");
            OnPropertyChanged("Families");
            OnPropertyChanged("Genus");
            OnPropertyChanged("Variants");
        }

        public void ResetInputs()
        {
            _selectedType = _selectedSpecies = _selectedFamily = _selectedGenus = _selectedVariant = null;
            
            PlantName = string.Empty;
            
            OnPropertyChanged("SelectedType");
            OnPropertyChanged("SelectedSpecies");
            OnPropertyChanged("SelectedFamily");
            OnPropertyChanged("SelectedGenus");
            OnPropertyChanged("SelectedVariant");
        }

        public void LoadTypes()
        {
            var types = Plants.Select(p => PlantenParser.ParseSearchText(p.Type))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            Types = new ObservableCollection<string>(types);
        }

        public void LoadSpecies()
        {
            var soorten = Plants.Select(p => PlantenParser.ParseSearchText(p.Soort))
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            Species = new ObservableCollection<string>(soorten);
        }

        public void LoadFamilies()
        {
            var families = Plants.Select(p => PlantenParser.ParseSearchText(p.Familie))
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            Families = new ObservableCollection<string>(families);
        }

        public void LoadGenus()
        {
            var genus = Plants.Select(p => PlantenParser.ParseSearchText(p.Geslacht))
                .Distinct()
                .OrderBy(g => g)
                .ToList();

            Genus = new ObservableCollection<string>(genus);
        }

        public void LoadVariants()
        {
            var variants = Plants.Select(p => PlantenParser.ParseSearchText(p.Variant))
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            // TODO: change implementation to only parse within OrderBy, then remove method invocation on the line below
            if (variants.Contains(PlantenParser.ParseSearchText(PlantenDao.NoVariant)))
            {
                variants.Remove(PlantenDao.NoVariant);
                variants.Insert(0, PlantenDao.NoVariant);
            }

            Variants = new ObservableCollection<string>(variants);
        }

        private void ShowDetails()
        {
            if (SelectedPlant is not null)
            {
                new PlantDetails(SelectedPlant).Show();
            } else { 
                MessageBox.Show("Gelieve een plant te selecteren uit de listview", 
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchPlanten()
        {
            var plants = _plantenDao.SearchPlants(SelectedType, SelectedFamily, 
                SelectedGenus, SelectedSpecies, SelectedVariant, PlantName);

            Plants.Clear();
            
            foreach (var plant in plants)
            {
                Plants.Add(plant);
            }
        }
    }
}
