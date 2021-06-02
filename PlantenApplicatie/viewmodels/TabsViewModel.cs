using PlantenApplicatie.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PlantenApplicatie.viewmodels
{
    // klasse dat een viewmodel toewijst per tabcontrol (Davy)
    public class TabsViewModel : ViewModelBase
    {
        ObservableCollection<object> _children;

        public TabsViewModel(Plant selectedPlant, User gebruiker)
        {
            _children = new ObservableCollection<object>();
            _children.Add(new TabPlantDetailsViewModel(selectedPlant, gebruiker));
            _children.Add(new TabPhenoTypeViewModel(selectedPlant, gebruiker));
            _children.Add(new TabAbioticViewModel(selectedPlant, gebruiker));
            _children.Add(new TabCommensalismViewModel(selectedPlant, gebruiker));
            _children.Add(new TabExtraPropertiesViewModel(selectedPlant, gebruiker));
            _children.Add(new TabManagmentActsViewModel(selectedPlant, gebruiker));
            _children.Add(new TabPhotoViewModel(selectedPlant, gebruiker));
        }

        public ObservableCollection<object> Children { get { return _children; } }

    }
}
