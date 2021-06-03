using System.Collections.Generic;
using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    //Class and GUI (Davy, Lily) 
    class TabManagePlantenViewModel : ViewModelBase
    {
        //private variables
        private readonly PlantenDao _plantenDao;

        private Plant? _selectedPlant;

        private bool _IsManager;
        private Gebruiker _selectedUser;
        private string? _selectedType;
        private string? _selectedFamily;
        private string? _selectedGenus;
        private string? _selectedSpecies;
        private string? _selectedVariant;

        // The GUI binds to this variable through a property, therefore it will not be null,
        // so we tell the compiler it is not null (Lily)
        private string _plantName = null!; 
        

        //button commands
        public ICommand AddUserCommand { get; }
        public ICommand ShowDetailsCommand { get; }
        public ICommand ResetCommand { get; }

        //observable collections to load in Comboboxes + Listview (Davy,Lily)
        public ObservableCollection<Plant> Plants { get; }

        public ObservableCollection<string> Types { get; private set; }
        public ObservableCollection<string> Families { get; private set; }
        public ObservableCollection<string> Genus { get; private set; }
        public ObservableCollection<string> Species { get; private set; }
        public ObservableCollection<string> Variants { get; private set; }
        
        //constructor, user as parameter 
        public TabManagePlantenViewModel(Gebruiker user)
        {
            SelectedUser = user;
            _plantenDao = PlantenDao.Instance;

            ShowDetailsCommand = new DelegateCommand(ShowDetails);
            ResetCommand = new DelegateCommand(ResetInputs);
            AddUserCommand = new DelegateCommand(AddUser);

            Plants = new ObservableCollection<Plant>();

            Types = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();
            Species = new ObservableCollection<string>();
            Variants = new ObservableCollection<string>();


            UserRole();
            FilterComboBoxes();
        }
        //getters and setters
        public Gebruiker SelectedUser
        {
            private get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
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

        public bool IsManager
        {
            get => _IsManager;
            set
            {
                _IsManager = value;
                OnPropertyChanged("IsManager");
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

        public string? SelectedType
        {
            get => _selectedType;
            set
            {
                MaintainCorrectFieldValueAfterFilter(ref _selectedType, value);
                OnPropertyChanged();
            }
        }

        public string? SelectedFamily
        {
            get => _selectedFamily;
            set
            {
                MaintainCorrectFieldValueAfterFilter(ref _selectedFamily, value);
                OnPropertyChanged();
            }
        }

        public string? SelectedGenus
        {
            get => _selectedGenus;
            set
            {
                MaintainCorrectFieldValueAfterFilter(ref _selectedGenus, value);
                OnPropertyChanged();
            }
        }

        public string? SelectedSpecies
        {
            get => _selectedSpecies;
            set
            {
                MaintainCorrectFieldValueAfterFilter(ref _selectedSpecies, value);
                OnPropertyChanged();
            }
        }

        public string? SelectedVariant
        {
            get => _selectedVariant;
            set
            {
                MaintainCorrectFieldValueAfterFilter(ref _selectedVariant, value);
                OnPropertyChanged();
            }
        }

        private void MaintainCorrectFieldValueAfterFilter(ref string? field, string? value)
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

        //reload the different comboboxes for the searchfunction (Lily)
        private void FilterComboBoxes()
        {
            SearchPlanten();

            LoadTypes();
            LoadFamilies();
            LoadGenus();
            LoadSpecies();
            LoadVariants();
        }

        //load the different types in
        private void LoadTypes()
        {
            var types = Plants.Select(p => PlantenParser.ParseSearchText(p.Type))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            Types = new ObservableCollection<string>(types);

            OnPropertyChanged(nameof(Types));
        }
        //load the different families in

        private void LoadFamilies()
        {
            var families = Plants.Select(p => PlantenParser.ParseSearchText(p.Familie))
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            Families = new ObservableCollection<string>(families);

            OnPropertyChanged(nameof(Families));
        }
        //load the different genus(geslachten) in
        private void LoadGenus()
        {
            var genus = Plants.Select(p => PlantenParser.ParseSearchText(p.Geslacht))
                .Distinct()
                .OrderBy(g => g)
                .ToList();

            Genus = new ObservableCollection<string>(genus);

            OnPropertyChanged(nameof(Genus));
        }
        //load the different species in
        private void LoadSpecies()
        {
            var species = Plants.Select(p => PlantenParser.ParseSearchText(p.Soort))
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            Species = new ObservableCollection<string>(species);

            OnPropertyChanged(nameof(Species));
        }
        //load the different variants in
        private void LoadVariants()
        {
            var variants = Plants.Select(p => PlantenParser.ParseSearchText(p.Variant))
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            // TODO: change implementation to only parse within OrderBy, then remove method invocation on the line below
            if (variants.Contains(PlantenParser.ParseSearchText(PlantenDao.NoVariant)))
            {
                variants.Remove(PlantenParser.ParseSearchText(PlantenDao.NoVariant));
                variants.Insert(0, PlantenDao.NoVariant);
            }

            Variants = new ObservableCollection<string>(variants);

            OnPropertyChanged(nameof(Variants));
        }
        //if a plant is selected you can click the show details button to show the current properties the plant has (Lily)
        private void ShowDetails()
        {
            if (SelectedPlant is not null)
            {
                new PlantDetails(SelectedPlant, SelectedUser).Show();
            } else { 
                MessageBox.Show("Gelieve een plant te selecteren uit de listview", 
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //search the different plants that correspond to the different selected values(Lily)
        private void SearchPlanten()
        {
            var plants = _plantenDao.SearchPlants(SelectedType, SelectedFamily,
                SelectedGenus, SelectedSpecies, SelectedVariant, PlantName);

            ClearAndAddAll(Plants, plants);
        }

        //reset the different selected values in the comboboxes + textbox
        private void ResetInputs()
        {
            _selectedType = _selectedFamily = _selectedGenus = _selectedSpecies = _selectedVariant = null;

            PlantName = string.Empty;

            OnPropertyChanged(nameof(SelectedType));
            OnPropertyChanged(nameof(SelectedFamily));
            OnPropertyChanged(nameof(SelectedGenus));
            OnPropertyChanged(nameof(SelectedSpecies));
            OnPropertyChanged(nameof(SelectedVariant));
        }

        private static void ClearAndAddAll<T>(ObservableCollection<T> collection, List<T> data)
        {
            collection.Clear();

            foreach (var elem in data)
            {
                collection.Add(elem);
            }
        }
        //check which roles the user has. and if the user is an old student(Gebruiker)
        //He can only observe the selected values of the plant (Davy,Jim)
        private void UserRole()
        {
            IsManager = SelectedUser.Rol.ToLower() == "manager";

        }

        private void AddUser()
        {
            // doorgeven SelectedGebruiker omdat je venster sluit zodat de applicatie onthoudt wie er bezig is
            new AddGebruiker(SelectedUser).Show();
        }
    }
}
