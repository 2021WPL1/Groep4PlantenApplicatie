using PlantenApplicatie.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Davy)
    public class TabAbiotiekViewModel : ViewModelBase
    {
        private Plant _selectedPlant;

        public TabAbiotiekViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
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
