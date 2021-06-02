using PlantenApplicatie.Domain;
using PlantenApplicatie.viewmodels.TabsViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace PlantenApplicatie.viewmodels
{
    class TabsBeheerViewModel : ViewModelBase
    {
        ObservableCollection<object> _children;

        public TabsBeheerViewModel(Window window, Gebruiker user)
        {
            _children = new ObservableCollection<object>();
            _children.Add(new TabManagePlantenViewModel(user));
            _children.Add(new TabUserViewModel(user, window));
        }

        public ObservableCollection<object> Children { get { return _children; } }
    }
}
