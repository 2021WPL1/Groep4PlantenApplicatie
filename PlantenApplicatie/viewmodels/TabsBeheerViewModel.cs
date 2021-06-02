using PlantenApplicatie.Domain;
using PlantenApplicatie.viewmodels.TabsViewModels;
using System.Collections.ObjectModel;

namespace PlantenApplicatie.viewmodels
{
    class TabsBeheerViewModel : ViewModelBase
    {
        ObservableCollection<object> _children;

        public TabsBeheerViewModel(Gebruiker gebruiker)
        {
            _children = new ObservableCollection<object>();
            _children.Add(new TabBeheerPlantenViewModel(gebruiker));
            _children.Add(new TabGebruikerViewModel(gebruiker));
        }

        public ObservableCollection<object> Children { get { return _children; } }
    }
}
