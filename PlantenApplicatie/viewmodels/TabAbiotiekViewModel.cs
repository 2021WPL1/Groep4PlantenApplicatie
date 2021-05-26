﻿using System;
using PlantenApplicatie.Domain;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using PlantenApplicatie.Data;
using Prism.Commands;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Davy, Lily)
    public class TabAbiotiekViewModel : ViewModelBase
    {
        private readonly PlantenDao _plantenDao;
        
        private readonly Plant _selectedPlant;
        
        public TabAbiotiekViewModel(Plant selectedPlant)
        {
            _plantenDao = PlantenDao.Instance;

            _selectedPlant = selectedPlant;

            Insolations = new ObservableCollection<string>(_plantenDao.GetAbioBezonning());
            SoilTypes = new ObservableCollection<string>(_plantenDao.GetAbioGrondsoort());
            MoistureRequirements = new ObservableCollection<string>(_plantenDao.GetAbioVochtbehoefte());
            NutritionRequirements = new ObservableCollection<string>(_plantenDao.GetAbioVoedingsbehoefte());
            AntagonianEnvironments = new ObservableCollection<string>(_plantenDao.GetAbioAntagonischeOmgeving());

            EditAbiotiekCommand = new DelegateCommand(EditAbiotiek);
            
            LoadStandards();
        }
        
        // TODO: auto property should become property to not have to use the OnPropertyChanged here
        private void LoadStandards()
        {
            var abiotiek = _selectedPlant.Abiotiek.SingleOrDefault();

            if (abiotiek is null) return;
            
            SelectedInsolation = abiotiek.Bezonning;
            SelectedSoilType = abiotiek.Grondsoort;
            SelectedMoistureRequirement = abiotiek.Vochtbehoefte;
            SelectedNutritionRequirement = abiotiek.Voedingsbehoefte;
            SelectedAntagonianEnvironment = abiotiek.AntagonischeOmgeving;
                
            OnPropertyChanged(nameof(SelectedInsolation));
            OnPropertyChanged(nameof(SelectedSoilType));
            OnPropertyChanged(nameof(SelectedMoistureRequirement));
            OnPropertyChanged(nameof(SelectedNutritionRequirement));
            OnPropertyChanged(nameof(SelectedAntagonianEnvironment));
        }

        public ObservableCollection<string> Insolations { get; }
        public ObservableCollection<string> SoilTypes { get; }
        public ObservableCollection<string> MoistureRequirements { get; }
        public ObservableCollection<string> NutritionRequirements { get; }
        public ObservableCollection<string> AntagonianEnvironments { get; }

        public string? SelectedInsolation { private get; set; }
        public string? SelectedSoilType { private get; set; }
        public string? SelectedMoistureRequirement { private get; set; }
        public string? SelectedNutritionRequirement { private get; set; }
        public string? SelectedAntagonianEnvironment { private get; set; }

        public ICommand EditAbiotiekCommand { get; }

        private void EditAbiotiek()
        {
            var abiotiek = _selectedPlant.Abiotiek.SingleOrDefault();
            
            Console.WriteLine(_selectedPlant.Abiotiek.Count);
            
            if (abiotiek is null)
            {
                _plantenDao.AddAbiotiek(_selectedPlant, SelectedInsolation, SelectedSoilType, 
                    SelectedMoistureRequirement, SelectedNutritionRequirement, 
                    SelectedAntagonianEnvironment);
            }
            else
            {
                _plantenDao.ChangeAbiotiek(abiotiek, SelectedInsolation, SelectedSoilType, 
                    SelectedMoistureRequirement, SelectedNutritionRequirement, 
                    SelectedAntagonianEnvironment);
            }
        }
    }
}