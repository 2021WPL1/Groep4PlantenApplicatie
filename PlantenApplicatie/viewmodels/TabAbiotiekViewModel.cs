using PlantenApplicatie.Domain;
using System.Collections.ObjectModel;
using System.Windows;
using PlantenApplicatie.Data;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Davy, Lily)
    public class TabAbiotiekViewModel : ViewModelBase
    { 
        public TabAbiotiekViewModel(Plant selectedPlant)
        {
            var dao = PlantenDao.Instance;

            Insolations = new ObservableCollection<string>(dao.GetAbioBezonning());
            SoilTypes = new ObservableCollection<string>(dao.GetAbioGrondsoort());
            MoistureRequirements = new ObservableCollection<string>(dao.GetAbioVochtbehoefte());
            NutritionRequirements = new ObservableCollection<string>(dao.GetAbioVoedingsbehoefte());
            AntagonianEnvironments = new ObservableCollection<string>(dao.GetAbioAntagonischeOmgeving());
        }

        public ObservableCollection<string> Insolations { get; }
        public ObservableCollection<string> SoilTypes { get; }
        public ObservableCollection<string> MoistureRequirements { get; }
        public ObservableCollection<string> NutritionRequirements { get; }
        public ObservableCollection<string> AntagonianEnvironments { get; }

        public string? SelectedInsolation { get; set; }
        public string? SelectedSoilType { get; set; }
        public string? SelectedMoistureRequirement { get; set; }
        public string? SelectedNutritionRequirement { get; set; }
        public string? SelectedAntagonianEnvironment { get; set; }
    }
}
