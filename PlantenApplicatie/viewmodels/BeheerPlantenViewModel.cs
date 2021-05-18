using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    class BeheerPlantenViewModel : ViewModelBase
    {
        public ICommand showPlantDetailsCommand { get; set; }
        public ICommand showPlantByNameCommand { get; set; }

        public ICommand showVariantByNameCommand { get; set; }

        public ICommand searchPlantsCommand { get; set; }

        public ICommand resetCommand { get; set; }

        public ObservableCollection<Plant> Plants { get; set; }
        public ObservableCollection<string> Types { get; set; }
        public ObservableCollection<string> Soorten { get; set; }
        public ObservableCollection<string> Families { get; set; }
        public ObservableCollection<string> Genus { get; set; }
        public ObservableCollection<string> Variants { get; set; }

        // hiermee kunnen we de data opvragen aan de databank.
        public PlantenDao _plantenDao;

        private Plant _selectedPlant;
        private string _selectedType;
        private string _selectedSoort;
        private string _selectedGeslacht;
        private string _selectedFamilie;
        private string _selectedVariant;

        private string textInputPlantName;

        public BeheerPlantenViewModel(PlantenDao plantenDao)
        {
            showPlantDetailsCommand = new DelegateCommand(showPlantDetails);
            showPlantByNameCommand = new DelegateCommand(showPlantByName);
            showVariantByNameCommand = new DelegateCommand(showVariantByName);
            searchPlantsCommand = new DelegateCommand(SearchPlanten);
            resetCommand = new DelegateCommand(Reset);

            Plants = new ObservableCollection<Plant>();
            Types = new ObservableCollection<string>();
            Soorten = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();
            Variants = new ObservableCollection<string>();

            _plantenDao = plantenDao;
            
            LoadPlants();
            Reset();
        }

        public void Reset()
        {
            //TextInputPlantName = string.Empty;

            LoadTypes();
            LoadSoorten();
            LoadFamilies();
            LoadGenus();
            LoadVariants();
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

        public string SelectedSoort
        {
            get { return _selectedSoort; }
            set
            {
                _selectedSoort = value;
                SearchPlanten();
                Reset();
                OnPropertyChanged();
            }
        }
        public string SelectedGeslacht
        {
            get { return _selectedGeslacht; }
            set
            {
                _selectedGeslacht = value;
                SearchPlanten();
                Reset();
                OnPropertyChanged();
            }
        }


        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                SearchPlanten();
                Reset();
                OnPropertyChanged();
            }
        }

        public string SelectedFamilie
        {
            get { return _selectedFamilie; }
            set
            {
                _selectedFamilie = value;
                SearchPlanten();
                Reset();
                OnPropertyChanged();
            }
        }

        public string SelectedVariant
        {
            get { return _selectedVariant; }
            set
            {
                _selectedVariant = value;
                SearchPlanten();
                Reset();
                OnPropertyChanged();
            }

        }

        public string TextInputPlantName
        {
            get
            {
                return textInputPlantName;
            }
            set
            {
                textInputPlantName = value;
                SearchPlanten();
                Reset();
                OnPropertyChanged();
            }
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
            var types = Plants.Select(p => p.Type)
                .Distinct()
                .ToList();
            
            Types.Clear();

            foreach(var type in types)
            {
                Types.Add(type);
            }
        }

        public void LoadSoorten()
        {
            var soorten = Plants.Select(p => p.Soort)
                .Distinct()
                .ToList();
            
            Soorten.Clear();
            
            foreach(var soort in soorten)
            {
                Soorten.Add(soort);
            }
        }

        public void LoadFamilies()
        {
            var families = Plants.Select(p => p.Familie)
                .Distinct()
                .ToList();
            
            Families.Clear();

            foreach (var familie in families)
            {
                Families.Add(familie);
            }
        }

        public void LoadGenus()
        {
            var genus = Plants.Select(p => p.Geslacht)
                .Distinct()
                .ToList();
            
            Genus.Clear();
            
            foreach (var gene in genus)
            {
                Genus.Add(gene);
            }
        }

        public void LoadVariants()
        {
            var variants = Plants.Select(p => p.Variant)
                .Distinct()
                .ToList();
            
            Variants.Clear();
            
            foreach (var v in variants)
            {
                Variants.Add(v);
            }
        }

        // TODO: remove
        public void LoadPlantsByVariant(string variant)
        {
            var plants = _plantenDao.SearchPlants(null, null, null, null, variant, null);
            Plants.Clear();
            foreach (var plant in plants)
            {
                Plants.Add(plant);
            }
        }

        public void showVariantByName()
        {
            LoadPlantsByVariant(SelectedVariant);
        }

        private void showPlantDetails()
        {
            if (_selectedPlant != null)
            {
                // nieuw venster initialiseren
                new PlantDetails(SelectedPlant).Show();
            } else { 
                MessageBox.Show("Gelieve een plant te selecteren uit de listview", "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void showPlantByName()
        {
            // string str = TextInput;
            LoadPlantsByName(TextInputPlantName);
        }

        private void SearchPlanten()
        {
            var type = SelectedType;
            var familie = SelectedFamilie;
            var geslacht = SelectedGeslacht;
            var soort = SelectedSoort;
            var variant = SelectedVariant;
            
            var plants = _plantenDao.SearchPlants(type, familie, 
                geslacht, soort, variant, TextInputPlantName);
                
            Plants.Clear();
            
            foreach (var plant in plants)
            {
                Plants.Add(plant);
            }
        }
    }
}
