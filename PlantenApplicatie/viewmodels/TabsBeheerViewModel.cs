using PlantenApplicatie.Domain;
using PlantenApplicatie.viewmodels.TabsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace PlantenApplicatie.viewmodels
{
    class TabsBeheerViewModel : ViewModelBase
    {
        ObservableCollection<object> _children;

        public TabsBeheerViewModel(Gebruiker user)
        {
            _children = new ObservableCollection<object>();
            _children.Add(new TabManagePlantenViewModel(user));
            _children.Add(new TabUserViewModel(user));
        }

        public ObservableCollection<object> Children { get { return _children; } }
    }
}
