using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    class BeheerPlantenViewModel : ViewModelBase
    {
        public ICommand showPlantDetailsCommand { get; set; }
        public ICommand showPlantByNameCommand { get; set; }
        public ObservableCollection<Plant> Plants { get; set; }
        public ObservableCollection<TfgsvType> Types { get; set; }
        public ObservableCollection<string> Soorten { get; set; }
        public ObservableCollection<string> Families { get; set; }
        public ObservableCollection<string> Genus { get; set; }
        // hiermee kunnen we de data opvragen aan de databank.
        public PlantenDao _plantenDao;
        // dit stelt de huidige geselecteerde plant voor
        private Plant _selectedPlant;
        private string textInput;

        public BeheerPlantenViewModel(PlantenDao plantenDao)
        {
            showPlantDetailsCommand = new DelegateCommand(showPlantDetails);
            showPlantByNameCommand = new DelegateCommand(showPlantByName);

            Plants = new ObservableCollection<Plant>();
            Types = new ObservableCollection<TfgsvType>();
            Soorten = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();

            _plantenDao = plantenDao;
        }

        public void LoadPlants()
        {
            var plants = _plantenDao.GetPlanten();
            Plants.Clear();
            foreach(var plant in plants)
            {
                Plants.Add(plant);
            }
        }

        public void LoadPlantsByName(string name)
        {
            var plants = _plantenDao.SearchPlants(null, null, null, null, null, name);
            Plants.Clear();
            foreach(var plant in plants)
            {
                Plants.Add(plant);
            }
        }

        public void LoadTypes()
        {
            var types = _plantenDao.GetTypes();
            Types.Clear();
            foreach(var type in types)
            {
                Types.Add(type);
            }
        }

        public void LoadSoorten()
        {
            var soorten = _plantenDao.GetUniqueSpeciesNames();
            Soorten.Clear();
            foreach(var soort in soorten)
            {
                Soorten.Add(soort);
            }
        }

        public void LoadFamilies()
        {
            var families = _plantenDao.GetUniqueFamilyNames();
            Families.Clear();
            foreach (var familie in families)
            {
                Families.Add(familie);
            }
        }

        public void LoadGenus()
        {
            var genus = _plantenDao.GetUniqueGenusNames();
            Genus.Clear();
            foreach (var gene in genus)
            {
                Genus.Add(gene);
            }
        }

        public Plant SelectedPlant
        {
            get { return _selectedPlant; }
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }


        private void showPlantDetails()
        {
            // toon plantdetails venster
            new PlantDetails(SelectedPlant).Show();
        }

        private void showPlantByName()
        {
            string str = TextInput;
            //_plantenDao.SearchPlants(_plantenDao.GetPlanten(), TextInput);

            LoadPlantsByName(TextInput);
        }

        public string TextInput
        {
            get
            {
                return textInput;
            }
            set
            {
                textInput = value;
                OnPropertyChanged();
            }
        }
    }
}
