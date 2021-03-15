using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            this._plantenDao = plantenDao;
        }

        public void Reset()
        {
            TextInputPlantName = string.Empty;

            LoadTypes();
            LoadSoorten();
            LoadFamilies();
            LoadGenus();
            LoadVariants();
        }


        #region geselecteerde properties getters and setters
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
                OnPropertyChanged();
            }
        }
        public string SelectedGeslacht
        {
            get { return _selectedGeslacht; }
            set
            {
                _selectedGeslacht = value;
                OnPropertyChanged();
            }
        }


        public string SelectedType
        {
            get { return _selectedType; }
            set
            {
                _selectedType = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFamilie
        {
            get { return _selectedFamilie; }
            set
            {
                _selectedFamilie = value;
                OnPropertyChanged();
            }
        }

        public string SelectedVariant
        {
            get { return _selectedVariant; }
            set
            {
                _selectedVariant = value;
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
                OnPropertyChanged();
            }
        }
        #endregion

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
            var plants = _plantenDao.SearchByProperties(name, null, null, null, null, null);
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

        public void LoadVariants()
        {
            var variants = _plantenDao.GetUniqueVariantNames();
            Variants.Clear();
            foreach (var v in variants)
            {
                Variants.Add(v);
            }
        }

        public void LoadPlantsByVariant(string variant)
        {
            var plants = _plantenDao.SearchByProperties(null, null, null, null, null, variant);
            Plants.Clear();
            foreach (var plant in plants)
            {
                Plants.Add(plant);
            }
        }

        public void showVariantByName()
        {
            _plantenDao.SearchPlantenByVariant(_plantenDao.GetPlanten(), SelectedVariant);

            LoadPlantsByVariant(SelectedVariant);
        }


        private void showPlantDetails()
        {
            if (_selectedPlant != null)
            {
                // nieuw venster initialiseren
                PlantDetails plantDetails = new PlantDetails();

                // initialiseer labels en waarden
                plantDetails.lblPlantnaam.Content = _selectedPlant.Fgsv;
                plantDetails.lblFamilie.Content = _selectedPlant.Familie;
                plantDetails.lblType.Content = _selectedPlant.Type;
                plantDetails.lblGeslacht.Content = _selectedPlant.Geslacht;
                plantDetails.lblSoort.Content = _selectedPlant.Soort;
                plantDetails.lblVariant.Content = _selectedPlant.Variant;

                // toon plantdetails venster
                plantDetails.Show();
            } else { 
                MessageBox.Show("Gelieve een plant te selecteren uit de listview", "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void showPlantByName()
        {
            // string str = TextInput;
            _plantenDao.SearchPlantenByName(_plantenDao.GetPlanten(), TextInputPlantName);

            LoadPlantsByName(TextInputPlantName);
        }

        private void SearchPlanten()
        {
            
            var type = SelectedType ;
            var familie = SelectedFamilie;
            var geslacht = SelectedGeslacht ;
            var soort = SelectedSoort ;
            var variant = SelectedVariant ;


                var list = _plantenDao.SearchByProperties(TextInputPlantName, type,
                familie, geslacht, soort, variant);

            

            //lvPlanten.ItemsSource = list;


            Plants.Clear();
            foreach (var plant in list)
            {
                Plants.Add(plant);
            }
        }
    }
}
