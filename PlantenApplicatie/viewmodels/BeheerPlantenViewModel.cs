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
        private string? _selectedType;
        private string? _selectedSoort;
        private string? _selectedGeslacht;
        private string? _selectedFamilie;
        private string? _selectedVariant;

        private string textInputPlantName;

        public BeheerPlantenViewModel(PlantenDao plantenDao)
        {
            showPlantDetailsCommand = new DelegateCommand(showPlantDetails);
            showPlantByNameCommand = new DelegateCommand(showPlantByName);
            showVariantByNameCommand = new DelegateCommand(showVariantByName);
            searchPlantsCommand = new DelegateCommand(SearchPlanten);
            resetCommand = new DelegateCommand(ResetInputs);

            Plants = new ObservableCollection<Plant>();
            Types = new ObservableCollection<string>();
            Soorten = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();
            Variants = new ObservableCollection<string>();

            _plantenDao = plantenDao;

            LoadPlants();
            FilterComboBoxes();
        }

        public void FilterComboBoxes()
        {
            SearchPlanten();
            
            LoadTypes();
            LoadSoorten();
            LoadFamilies();
            LoadGenus();
            LoadVariants();
        }

        public void ResetInputs()
        {
            TextInputPlantName = string.Empty;

            SelectedType = SelectedFamilie =
                SelectedSoort = SelectedFamilie = SelectedGeslacht = SelectedVariant = null;
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

        public string? SelectedSoort
        {
            get { return _selectedSoort; }
            set
            {
                _selectedSoort = value;
                FilterComboBoxes();
                OnPropertyChanged();
            }
        }
        public string? SelectedGeslacht
        {
            get { return _selectedGeslacht; }
            set
            {
                _selectedGeslacht = value;
                FilterComboBoxes();
                OnPropertyChanged();
            }
        }


        public string? SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                FilterComboBoxes();
                OnPropertyChanged();
            }
        }

        public string? SelectedFamilie
        {
            get { return _selectedFamilie; }
            set
            {
                _selectedFamilie = value;
                FilterComboBoxes();
                OnPropertyChanged();
            }
        }

        public string? SelectedVariant
        {
            get { return _selectedVariant; }
            set
            {
                _selectedVariant = value;
                FilterComboBoxes();
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
                FilterComboBoxes();
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
            
            UpdateObservableCollection(Types, types);
        }

        public void LoadSoorten()
        {
            var soorten = Plants.Select(p => p.Soort)
                .Distinct()
                .ToList();
            
            UpdateObservableCollection(Soorten, soorten);
        }

        public void LoadFamilies()
        {
            var families = Plants.Select(p => p.Familie)
                .Distinct()
                .ToList();
            
            UpdateObservableCollection(Families, families);
        }

        public void LoadGenus()
        {
            var genus = Plants.Select(p => p.Geslacht)
                .Distinct()
                .ToList();
            
            UpdateObservableCollection(Genus, genus);
        }

        public void LoadVariants()
        {
            var variants = Plants.Select(p => p.Variant)
                .Distinct()
                .ToList();
            
            UpdateObservableCollection(Variants, variants);
        }

        // TODO: remove
        public void LoadPlantsByVariant(string variant)
        {
            var plants = _plantenDao.SearchPlants(null, null, 
                null, null, variant, null);
            
            Plants.Clear();
            
            foreach (var plant in plants)
            {
                Plants.Add(plant);
            }
        }

        private static void UpdateObservableCollection<T>(ObservableCollection<T> collection, List<T> data)
        {
            foreach (var elem in collection.ToList().Where(elem => !data.Contains(elem)))
            {
                collection.Remove(elem);
            }

            foreach (var elem in data.Where(elem => !collection.Contains(elem)))
            {
                collection.Add(elem);
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
