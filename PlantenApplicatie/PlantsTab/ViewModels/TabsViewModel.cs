using PlantenApplicatie.Domain;
using System.Collections.ObjectModel;

namespace PlantenApplicatie.viewmodels
{
    // klasse dat een viewmodel toewijst per tabcontrol (Davy)
    public class TabsViewModel : ViewModelBase
    {
        ObservableCollection<object> _children;

        public TabsViewModel(Plant selectedPlant, Gebruiker user)
        {
            _children = new ObservableCollection<object>();
            _children.Add(new TabPlantDetailsViewModel(selectedPlant, user));
            _children.Add(new TabPhenoTypeViewModel(selectedPlant, user));
            _children.Add(new TabAbioticViewModel(selectedPlant, user));
            _children.Add(new TabCommensalismViewModel(selectedPlant, user));
            _children.Add(new TabExtraPropertiesViewModel(selectedPlant, user));
            _children.Add(new TabManagmentActsViewModel(selectedPlant, user));
            _children.Add(new TabPhotoViewModel(selectedPlant, user));
        }

        public ObservableCollection<object> Children { get { return _children; } }

    }
}
