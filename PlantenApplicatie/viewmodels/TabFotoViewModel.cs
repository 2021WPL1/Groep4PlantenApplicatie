using PlantenApplicatie.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantenApplicatie.viewmodels
{
    public class TabFotoViewModel : ViewModelBase
    {
        private Plant _selectedPlant;

        public TabFotoViewModel(Plant selectedplant)
        {
            SelectedPlant = selectedplant;
        }

        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }
    }
}
