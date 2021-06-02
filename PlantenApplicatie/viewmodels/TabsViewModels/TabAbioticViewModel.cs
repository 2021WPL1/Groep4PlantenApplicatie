using System;
using PlantenApplicatie.Domain;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PlantenApplicatie.Data;
using Prism.Commands;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Lily)
    public class TabAbioticViewModel : ViewModelBase
    {

        //variabelen instellen(Lily)
        private const string Property = "habitat";

        private readonly PlantenDao _plantenDao;
        private readonly Plant _selectedPlant;

        private string? _selectedInsolation;
        private string? _selectedSoilType;
        private string? _selectedMoistureRequirement;
        private string? _selectedNutritionRequirement;
        private string? _selectedAntagonianEnvironment;


        // private variabelen (Davy)
        private User _selectedUser;
        private bool _IsManager;

        //constructor(Lily)
        public TabAbioticViewModel(Plant selectedPlant, User user)
        {
            SelectedUser = user;
            _plantenDao = PlantenDao.Instance;
            _selectedPlant = selectedPlant;

            //observable collections voor de comboboxes, word opgevuld met de functies in DAO
            Insolations = new ObservableCollection<string>(_plantenDao.GetAbioInsolation());
            SoilTypes = new ObservableCollection<string>(_plantenDao.GetAbioSoilType());
            MoistureRequirements = new ObservableCollection<string>(_plantenDao.GetAbioMoistureRequirement());
            NutritionRequirements = new ObservableCollection<string>(_plantenDao.GetAbioNutritionRequirement());
            AntagonianEnvironments = new ObservableCollection<string>(_plantenDao.GetAbioAntagonianEnvironments());

            SelectedPlantHabitats = new ObservableCollection<string>(
                _plantenDao.GetAbioHabitatNames(_selectedPlant));
            PlantHabitats = new ObservableCollection<string>(_plantenDao.GetAbioHabitatNames());
            //Commands voor de buttons
            EditAbiotiekCommand = new DelegateCommand(EditAbiotic);
            RemoveHabitatCommand = new DelegateCommand(RemoveHabitat);
            AddHabitatCommand = new DelegateCommand(AddHabitat);

            //laad de geselecteerde values in
            LoadStandards();
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
            private get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        //observable collections
        public ObservableCollection<string> Insolations { get; }
        public ObservableCollection<string> SoilTypes { get; }
        public ObservableCollection<string> MoistureRequirements { get; }
        public ObservableCollection<string> NutritionRequirements { get; }
        public ObservableCollection<string> AntagonianEnvironments { get; }

        public ObservableCollection<string> SelectedPlantHabitats { get; }
        public ObservableCollection<string> PlantHabitats { get; }


        //getters en setters (Lily)
        public string? SelectedInsolation
        {
            private get { return _selectedInsolation; }
            set
            {
                _selectedInsolation = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedSoilType
        {
            private get { return _selectedSoilType; }
            set
            {
                _selectedSoilType = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedMoistureRequirement
        {
            private get { return _selectedMoistureRequirement; }
            set
            {
                _selectedMoistureRequirement = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedNutritionRequirement
        {
            private get { return _selectedNutritionRequirement; }
            set
            {
                _selectedNutritionRequirement = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedAntagonianEnvironment
        {
            private get { return _selectedAntagonianEnvironment; }
            set
            {
                _selectedAntagonianEnvironment = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedAbioPlantHabitat { get; set; }
        public string? SelectedAbioHabitat { get; set; }

        public ICommand EditAbiotiekCommand { get; }
        public ICommand RemoveHabitatCommand { get; }
        public ICommand AddHabitatCommand { get; }

        //geef de geselecteerde waardes weer in de combobox, als er geen gegevens is van de geselecteerde plant geef null terug (Lily)
        private void LoadStandards()
        {
            var abiotiek = _selectedPlant.Abiotiek.SingleOrDefault();

            if (abiotiek is null) return;

            SelectedInsolation = abiotiek.Bezonning;
            SelectedSoilType = abiotiek.Grondsoort;
            SelectedMoistureRequirement = abiotiek.Vochtbehoefte;
            SelectedNutritionRequirement = abiotiek.Voedingsbehoefte;
            SelectedAntagonianEnvironment = abiotiek.AntagonischeOmgeving;
        }

        //wijzig de abiotiek van een plant, als er geen is word er eerst een nieuwe abiotiek aangemaakt met de geselecteerde waardes (Lily)
        private void EditAbiotic()
        {
            var abiotiek = _selectedPlant.Abiotiek.SingleOrDefault();

            Console.WriteLine(_selectedPlant.Abiotiek.Count);

            if (abiotiek is null)
            {
                _plantenDao.AddAbiotic(_selectedPlant, SelectedInsolation, SelectedSoilType,
                    SelectedMoistureRequirement, SelectedNutritionRequirement,
                    SelectedAntagonianEnvironment);
            }
            else
            {
                _plantenDao.ChangeAbiotic(abiotiek, SelectedInsolation, SelectedSoilType,
                    SelectedMoistureRequirement, SelectedNutritionRequirement,
                    SelectedAntagonianEnvironment);
            }
        }

        //verwijder de geselecteerde habitat uit de lijst van de geselecteerde plant (Lily)

        private void RemoveHabitat()
        {
            var habitatAbbreviation = _plantenDao.GetAbioHabitatAbbreviation(SelectedAbioPlantHabitat);
            var abiothiekMulti = _plantenDao.GetAbioMulti(_selectedPlant)
                .Single(am => am.Eigenschap == Property && am.Waarde == habitatAbbreviation);

            _plantenDao.DeleteAbioticMulti(abiothiekMulti);

            SelectedPlantHabitats.Remove(SelectedAbioPlantHabitat);
        }

        //voeg een habitat toe aan de plant (Lily)

        private void AddHabitat()
        {
            if (SelectedPlantHabitats.Contains(SelectedAbioHabitat)) return;

            var habitatAbbreviation = _plantenDao.GetAbioHabitatAbbreviation(SelectedAbioHabitat);

            _plantenDao.AddAbioticMulti(_selectedPlant, Property, habitatAbbreviation);

            SelectedPlantHabitats.Add(SelectedAbioHabitat);
        }
    }
}