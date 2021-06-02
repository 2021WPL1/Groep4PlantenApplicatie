using PlantenApplicatie.Domain;
using PlantenApplicatie.viewmodels.TabsViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;

namespace PlantenApplicatie.viewmodels
{
    class TabsBeheerViewModel : ViewModelBase
    {
        ObservableCollection<object> _children;

        public TabsBeheerViewModel(Window window, Gebruiker gebruiker)
        {
            _children = new ObservableCollection<object>();
            _children.Add(new TabBeheerPlantenViewModel(gebruiker));
            _children.Add(new TabGebruikerViewModel(window, gebruiker));
        }

        public ObservableCollection<object> Children { get { return _children; } }
    }
}
