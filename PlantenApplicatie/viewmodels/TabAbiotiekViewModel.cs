using System;
using PlantenApplicatie.Domain;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using PlantenApplicatie.Data;
using Prism.Commands;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Davy, Lily)
    public class TabAbiotiekViewModel : ViewModelBase
    {
        private const string Eigenschap = "habitat";
        
        private readonly PlantenDao _plantenDao;
        private readonly Plant _selectedPlant;

        private string? _selectedInsolation;
        private string? _selectedSoilType;
        private string? _selectedMoistureRequirement;
        private string? _selectedNutritionRequirement;
        private string? _selectedAntagonianEnvironment;
        
        public TabAbiotiekViewModel(Plant selectedPlant)
        {
            _plantenDao = PlantenDao.Instance;
            _selectedPlant = selectedPlant;

            Insolations = new ObservableCollection<string>(_plantenDao.GetAbioBezonning());
            SoilTypes = new ObservableCollection<string>(_plantenDao.GetAbioGrondsoort());
            MoistureRequirements = new ObservableCollection<string>(_plantenDao.GetAbioVochtbehoefte());
            NutritionRequirements = new ObservableCollection<string>(_plantenDao.GetAbioVoedingsbehoefte());
            AntagonianEnvironments = new ObservableCollection<string>(_plantenDao.GetAbioAntagonischeOmgeving());

            SelectedPlantHabitats = new ObservableCollection<string>(
                _plantenDao.GetAbioHabitatNames(_selectedPlant));
            PlantHabitats = new ObservableCollection<string>(_plantenDao.GetAbioHabitatNames());

            EditAbiotiekCommand = new DelegateCommand(EditAbiotiek);
            RemoveHabitatCommand = new DelegateCommand(RemoveHabitat);
            AddHabitatCommand = new DelegateCommand(AddHabitat);
            
            LoadStandards();
        }

        public ObservableCollection<string> Insolations { get; }
        public ObservableCollection<string> SoilTypes { get; }
        public ObservableCollection<string> MoistureRequirements { get; }
        public ObservableCollection<string> NutritionRequirements { get; }
        public ObservableCollection<string> AntagonianEnvironments { get; }
        
        public ObservableCollection<string> SelectedPlantHabitats { get; }
        public ObservableCollection<string> PlantHabitats { get; }

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

        private void EditAbiotiek()
        {
            var abiotiek = _selectedPlant.Abiotiek.SingleOrDefault();
            
            Console.WriteLine(_selectedPlant.Abiotiek.Count);
            
            if (abiotiek is null)
            {
                _plantenDao.AddAbiotiek(_selectedPlant, SelectedInsolation, SelectedSoilType, 
                    SelectedMoistureRequirement, SelectedNutritionRequirement, 
                    SelectedAntagonianEnvironment);
            }
            else
            {
                _plantenDao.ChangeAbiotiek(abiotiek, SelectedInsolation, SelectedSoilType, 
                    SelectedMoistureRequirement, SelectedNutritionRequirement, 
                    SelectedAntagonianEnvironment);
            }
        }

        private void RemoveHabitat()
        {
            var habitatAbbreviation = _plantenDao.GetAbioHabitatAbbreviation(SelectedAbioPlantHabitat);
            var abiothiekMulti = _plantenDao.GetAbioMulti(_selectedPlant)
                .Single(am => am.Eigenschap == Eigenschap && am.Waarde == habitatAbbreviation);

            _plantenDao.DeleteAbiotiekMulti(abiothiekMulti);

            SelectedPlantHabitats.Remove(SelectedAbioPlantHabitat);
        }

        private void AddHabitat()
        {
            if (SelectedPlantHabitats.Contains(SelectedAbioHabitat)) return;
            
            var habitatAbbreviation = _plantenDao.GetAbioHabitatAbbreviation(SelectedAbioHabitat);
            
            _plantenDao.AddAbiotiekMulti(_selectedPlant, Eigenschap, habitatAbbreviation);
            
            SelectedPlantHabitats.Add(SelectedAbioHabitat);
        }
    }
}