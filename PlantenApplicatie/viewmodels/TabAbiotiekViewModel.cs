using PlantenApplicatie.Domain;
using System.Collections.ObjectModel;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Davy, Lily)
    public class TabAbiotiekViewModel : ViewModelBase
    { 
        public TabAbiotiekViewModel(Plant selectedPlant)
        {
            // TODO: load data into ObservableCollection by method from here
        }

        public ObservableCollection<string> Insolations { get; } = new();
        public ObservableCollection<string> SoilTypes { get; } = new();
        public ObservableCollection<string> MoistureRequirements { get; } = new();
        public ObservableCollection<string> NutritionRequirements { get; } = new();
        public ObservableCollection<string> AntagonianEnvironments { get; } = new();

        public string? SelectedInsolation { get; set; }
        public string? SelectedSoilType { get; set; }
        public string? SelectedMoistureRequirement { get; set; }
        public string? SelectedNutritionRequirement { get; set; }
        public string? SelectedAntagonianEnvironment { get; set; }
    }
}
