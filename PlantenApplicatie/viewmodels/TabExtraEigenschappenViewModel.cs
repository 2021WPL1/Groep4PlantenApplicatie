using PlantenApplicatie.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlantenApplicatie.viewmodels
{
    public class TabExtraEigenschappenViewModel : ViewModelBase
    {
        private Plant _selectedPlant;

        public TabExtraEigenschappenViewModel(Plant selectedPlant)
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
