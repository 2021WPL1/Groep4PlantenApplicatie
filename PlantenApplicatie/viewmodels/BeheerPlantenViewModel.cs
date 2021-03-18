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
    // MVVM Toepassing (Davy) 
    class BeheerPlantenViewModel : ViewModelBase
    {
        // Button commands
        public ICommand ShowPlantDetailsCommand { get; set; }
        public ICommand ShowPlantByNameCommand { get; set; }
        public ICommand ShowVariantByNameCommand { get; set; }
        public ICommand SearchPlantsCommand { get; set; }
        public ICommand ResetCommand { get; set; }

        // Observable collections, ipv strings gebruikten we de tfgsv klasses maar distinct/order by kon niet samen gebruikt worden (Davy&Jim)
        public ObservableCollection<Plant> Plants { get; set; }
        public ObservableCollection<string> Types { get; set; }
        public ObservableCollection<string> Soorten { get; set; }
        public ObservableCollection<string> Families { get; set; }
        public ObservableCollection<string> Genus { get; set; }
        public ObservableCollection<string> Variants { get; set; }

        // Hiermee kunnen we de data opvragen aan de databank.
        private readonly PlantenDao _plantenDao;

        private Plant _selectedPlant;
        private string _selectedType;
        private string _selectedSoort;
        private string _selectedGeslacht;
        private string _selectedFamilie;
        private string _selectedVariant;

        private string _textInputPlantName;

        // Constructor (Davy & Jim)
        public BeheerPlantenViewModel()
        {
            ShowPlantDetailsCommand = new DelegateCommand(ShowPlantDetails);
            ShowPlantByNameCommand = new DelegateCommand(ShowPlantByName);
            ShowVariantByNameCommand = new DelegateCommand(ShowVariantByName);
            SearchPlantsCommand = new DelegateCommand(SearchPlanten);
            ResetCommand = new DelegateCommand(Reset);

            Plants = new ObservableCollection<Plant>();
            Types = new ObservableCollection<string>();
            Soorten = new ObservableCollection<string>();
            Families = new ObservableCollection<string>();
            Genus = new ObservableCollection<string>();
            Variants = new ObservableCollection<string>();

            _plantenDao = PlantenDao.Instance;

            LoadPlants();
            Reset();
        }

        // Wanneer er op de reset knop geklikt word reset de CMB en TextBox (Davy)
        public void Reset()
        {
            TextInputPlantName = string.Empty;

            LoadTypes();
            LoadSoorten();
            LoadFamilies();
            LoadGenus();
            LoadVariants();
        }

        // Getters en setters voor de selected values (Davy&Jim)
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
                return _textInputPlantName;
            }
            set
            {
                _textInputPlantName = value;
                OnPropertyChanged();
            }
        }

        // Geeft alle planten weer (Davy & Lily)
        public void LoadPlants()
        {
            var plants = _plantenDao.GetPlanten();

            Plants.Clear();
            
            foreach(var plant in plants)
            {
                Plants.Add(plant);
            }
        }

        // Geeft alle planten weer op naam (Davy & Lily)
        public void LoadPlantsByName(string name)
        {
            var plants = _plantenDao.SearchPlants(null, null, null, null, null, name);

            Plants.Clear();
            
            foreach(var plant in plants)
            {
                Plants.Add(plant);
            }
        }

        // Geeft alle verschillende planten weer in de combobox (Davy & Lily)
        public void LoadTypes()
        {
            var types = _plantenDao.GetTypes();

            Types.Clear();

            foreach (var type in types)
            {
                Types.Add(type);
            }
        }

        // Geeft alle soorten weer in de combobox (Davy & Lily)
        public void LoadSoorten()
        {
            var soorten = _plantenDao.GetUniqueSpeciesNames();

            Soorten.Clear();
            
            foreach(var soort in soorten)
            {
                Soorten.Add(soort);
            }
        }

        // Geeft alle families weer in de combobox (Davy & Lily)
        public void LoadFamilies()
        {
            var families = _plantenDao.GetUniqueFamilyNames();
            
            Families.Clear();

            foreach (var familie in families)
            {
                Families.Add(familie);
            }
        }

        // Geeft alle geslacht weer in de combobox (Davy & Lily)
        public void LoadGenus()
        {
            var genus = _plantenDao.GetUniqueGenusNames();

            Genus.Clear();
            
            foreach (var gene in genus)
            {
                Genus.Add(gene);
            }
        }

        // Geeft alle varianten weer in de combobox (Davy & Lily)
        public void LoadVariants()
        {
            var variants = _plantenDao.GetUniqueVariantNames();

            Variants.Clear();
            
            foreach (var v in variants)
            {
                Variants.Add(v);
            }
        }

        //Zoek op alle varianten (Davy & Lily)
        public void LoadPlantsByVariant(string variant)
        {
            var plants = _plantenDao.SearchPlants(null, null, null, null, variant, null);

            Plants.Clear();

            foreach (var plant in plants)
            {
                Plants.Add(plant);
            }
        }

        // Geeft de planten op variant weer (Davy & Lily & Jim)
        public void ShowVariantByName()
        {
            LoadPlantsByVariant(SelectedVariant);
        }

        // Als er geen plant geselecteerd is word er een messagebox geshowed (Lily&Davy)
        private void ShowPlantDetails()
        {
            if (_selectedPlant != null)
            {
                // nieuw venster initialiseren
                new PlantDetails(SelectedPlant).Show();
            } else { 
                MessageBox.Show("Gelieve een plant te selecteren uit de listview", "Fout", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // Geeft de plant weer op naam in de lijst (Davy & Jim)
        private void ShowPlantByName()
        {
            LoadPlantsByName(TextInputPlantName);
        }

        // Zoek de planten op zijn verschillende eigenschappen (Davy & Jim)
        private void SearchPlanten()
        {
            var type = SelectedType;
            var familie = SelectedFamilie;
            var geslacht = SelectedGeslacht;
            var soort = SelectedSoort;
            var variant = SelectedVariant;

            var list = _plantenDao.SearchPlants(type,
                familie, geslacht, soort, variant, TextInputPlantName);
                
            Plants.Clear();
            
            foreach (var plant in list)
            {
                Plants.Add(plant);
            }
        }
    }
}
