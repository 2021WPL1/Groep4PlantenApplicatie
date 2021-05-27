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

        public TabsViewModel(Plant selectedPlant, Gebruiker gebruiker)
        {
            _children = new ObservableCollection<object>();
            _children.Add(new TabPlantDetailsViewModel(selectedPlant));
            _children.Add(new TabWijzigPlantViewModel(selectedPlant));
            _children.Add(new TabFenoTypeViewModel(selectedPlant, gebruiker));
            _children.Add(new TabAbiotiekViewModel(selectedPlant, gebruiker));
            _children.Add(new TabCommensalismeViewModel(selectedPlant));
            _children.Add(new TabExtraEigenschappenViewModel(selectedPlant));
            _children.Add(new TabBeheerDadenViewModel(selectedPlant));
            _children.Add(new TabFotoViewModel(selectedPlant));
        }

        public ObservableCollection<object> Children { get { return _children; } }

    }
}
