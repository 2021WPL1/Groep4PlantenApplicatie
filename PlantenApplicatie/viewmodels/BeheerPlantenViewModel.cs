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
            
            OnPropertyChanged("Types");
            OnPropertyChanged("Soorten");
            OnPropertyChanged("Families");
            OnPropertyChanged("Genus");
            OnPropertyChanged("Variants");
        }

        public void ResetInputs()
        {
            _selectedType = _selectedSoort = _selectedFamilie = _selectedGeslacht = _selectedVariant = null;
            
            TextInputPlantName = string.Empty;
            
            OnPropertyChanged("SelectedType");
            OnPropertyChanged("SelectedSoort");
            OnPropertyChanged("SelectedFamilie");
            OnPropertyChanged("SelectedGeslacht");
            OnPropertyChanged("SelectedVariant");
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
                if (value == _selectedSoort)
                {
                    return;
                }
                
                var oldSoort = _selectedSoort;
                _selectedSoort = value;
                
                if (value is not null) {
                    FilterComboBoxes();
                }
                else
                {
                    _selectedSoort = oldSoort;
                }

                OnPropertyChanged();
            }
        }
        public string? SelectedGeslacht
        {
            get { return _selectedGeslacht; }
            set
            {
                if (value == _selectedGeslacht)
                {
                    return;
                }
                
                var oldGeslacht = _selectedGeslacht;
                _selectedGeslacht = value;

                if (value is not null) {
                    FilterComboBoxes();
                }
                else
                {
                    _selectedGeslacht = oldGeslacht;
                }
                
                OnPropertyChanged();
            }
        }


        public string? SelectedType
        {
            get { return _selectedType; }
            set
            {
                if (value == _selectedType)
                {
                    return;
                }
                
                var oldType = _selectedType;
                _selectedType = value;

                if (value is not null)
                {
                    FilterComboBoxes();
                }
                else
                {
                    _selectedType = oldType;
                }

                OnPropertyChanged();
            }
        }

        public string? SelectedFamilie
        {
            get { return _selectedFamilie; }
            set
            {
                if (value == _selectedFamilie)
                {
                    return;
                }
                
                var oldFamilie = _selectedFamilie;
                _selectedFamilie = value;

                if (value is not null)
                {
                    FilterComboBoxes();
                }
                else {
                    _selectedFamilie = oldFamilie;
                }
                
                OnPropertyChanged();
            }
        }

        public string? SelectedVariant
        {
            get { return _selectedVariant; }
            set
            {
                if (value == _selectedVariant)
                {
                    return;
                }
                
                var oldVariant = _selectedVariant;
                _selectedVariant = value;

                if (value is not null)
                {
                    FilterComboBoxes();
                }
                else
                {
                    _selectedVariant = oldVariant;
                }

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
            var types = Plants.Select(p => PlantenParser.ParseSearchText(p.Type))
                .Distinct()
                .OrderBy(t => t)
                .ToList();

            Types = new ObservableCollection<string>(types);
        }

        public void LoadSoorten()
        {
            var soorten = Plants.Select(p => PlantenParser.ParseSearchText(p.Soort))
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            Soorten = new ObservableCollection<string>(soorten);
        }

        public void LoadFamilies()
        {
            var families = Plants.Select(p => PlantenParser.ParseSearchText(p.Familie))
                .Distinct()
                .OrderBy(f => f)
                .ToList();

            Families = new ObservableCollection<string>(families);
        }

        public void LoadGenus()
        {
            var genus = Plants.Select(p => PlantenParser.ParseSearchText(p.Geslacht))
                .Distinct()
                .OrderBy(g => g)
                .ToList();

            Genus = new ObservableCollection<string>(genus);
        }

        public void LoadVariants()
        {
            var variants = Plants.Select(p => PlantenParser.ParseSearchText(p.Variant))
                .Distinct()
                .OrderBy(v => v)
                .ToList();

            // TODO: change implementation to only parse within OrderBy, then remove method invocation on the line below
            if (variants.Contains(PlantenParser.ParseSearchText(PlantenDao.NoVariant)))
            {
                variants.Remove(PlantenDao.NoVariant);
                variants.Insert(0, PlantenDao.NoVariant);
            }

            Variants = new ObservableCollection<string>(variants);
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

        public void showVariantByName()
        {
            LoadPlantsByVariant(SelectedVariant);
        }

        private void showPlantDetails()
        {
            if (_selectedPlant is not null)
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
            var plants = _plantenDao.SearchPlants(SelectedType, SelectedFamilie, 
                SelectedGeslacht, SelectedSoort, SelectedVariant, TextInputPlantName);
                
            Plants.Clear();
            
            foreach (var plant in plants)
            {
                Plants.Add(plant);
            }
        }
    }
}
