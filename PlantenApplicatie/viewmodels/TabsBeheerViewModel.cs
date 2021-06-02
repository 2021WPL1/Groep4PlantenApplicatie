﻿using PlantenApplicatie.Domain;
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

        public TabsBeheerViewModel(User gebruiker)
        {
            _children = new ObservableCollection<object>();
            _children.Add(new TabManagePlantenViewModel(gebruiker));
            _children.Add(new TabUserViewModel(gebruiker));
        }

        public ObservableCollection<object> Children { get { return _children; } }
    }
}
