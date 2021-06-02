using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace PlantenApplicatie.viewmodels
{
    //MVVM Liam
    public class TabCommensalismViewModel : ViewModelBase
    {

        //private selecters en de dao (Liam)
        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private string _selectedOntwikkelingssnelheid;
        private string _selectedStrategie;
        private string _selectedCommenEigenschappen;
        private string _selectedCommensalismeMulti;
        private CommensalismeMulti _selectedCommenMulti;

        // private variabelen (Davy)
        private User _selectedGebruiker;
        private bool _IsManager;

        //observable collections voor de lijst en comboboxes (Liam)
        public ObservableCollection<string> CommenStrategien { get; set; }
        public ObservableCollection<string> CommenOntwikkelsnelheden { get; set; }
        public ObservableCollection<string> CommenEigenschappen { get; set; }
        public ObservableCollection<CommensalismeMulti> CommensalismeMulti { get; set; }
        public ObservableCollection<string> CommenMulti { get; set; }

        //button commands (Liam)
        public ICommand EditCommensalismeCommand { get; set; }
        public ICommand AddCommensalismeMultiCommand { get; set; }
        public ICommand EditCommensalismeMultiCommand { get; set; }
        public ICommand RemoveCommensalismeMultiCommand { get; set; }

        //constructor (Liam)
        public TabCommensalismViewModel(Plant selectedPlant, User gebruiker)
        {
            SelectedGebruiker = gebruiker;
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;

            CommenOntwikkelsnelheden = new ObservableCollection<string>();
            CommenStrategien = new ObservableCollection<string>();
            CommenEigenschappen = new ObservableCollection<string>();
            CommenMulti = new ObservableCollection<string>();
            CommensalismeMulti = new ObservableCollection<CommensalismeMulti>();

            //de delegate commands voor de knoppen (Liam)
            EditCommensalismeCommand = new DelegateCommand(EditCommensalism);
            AddCommensalismeMultiCommand = new DelegateCommand(AddCommenMulti);
            EditCommensalismeMultiCommand = new DelegateCommand(EditCommenMulti);
            RemoveCommensalismeMultiCommand = new DelegateCommand(RemoveCommenMulti);

            //laad de verschillende lijsten in voor de comboboxes en listview (Liam)
            LoadCommenDevelopmentSpeed();
            LoadCommenStrategy();
            LoadCommenMulti();
            LoadCommenProperties();
            LoadLifeform();
            LoadSociability();
            LoadSelectedValues();
            UserRole();
        }

        public bool IsManager
        {
            get => _IsManager;
            set
            {
                _IsManager = value;
                OnPropertyChanged("IsManager");
            }
        }


        //controleer welke rol de gebruiker heeft
        private void UserRole()
        {
            switch (SelectedGebruiker.Rol.ToLower())
            {
                case "manager":
                    IsManager = true;
                    break;
                case "data-collector":
                    IsManager = false;
                    break;
                case "gebruiker":
                    IsManager = false;
                    break;
            }
        }
        public User SelectedGebruiker
        {
            private get => _selectedGebruiker;
            set
            {
                _selectedGebruiker = value;
                OnPropertyChanged();
            }
        }

        //herlaad de comboboxes (Liam)
        public void Reset()
        {
            LoadCommenStrategy();
            LoadCommenDevelopmentSpeed();
            LoadCommenMulti();
        }

        //getters and setters (Liam)

        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommensalismMulti
        {
            private get => _selectedCommensalismeMulti;
            set
            {
                _selectedCommensalismeMulti = value;
                OnPropertyChanged();
            }
        }

        public CommensalismeMulti SelectedCommenMulti
        {
            private get => _selectedCommenMulti;
            set
            {
                _selectedCommenMulti = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommenDevelopmentSpeed
        {
            private get => _selectedOntwikkelingssnelheid;
            set
            {
                _selectedOntwikkelingssnelheid = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommenStrategy
        {
            private get => _selectedStrategie;
            set
            {
                _selectedStrategie = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommenProperties
        {
            private get => _selectedCommenEigenschappen;
            set
            {
                _selectedCommenEigenschappen = value;
                ChangeCommProperties();
                OnPropertyChanged();
            }
        }

        //laad de verschillende lijsten in (Liam)
        private void LoadCommenDevelopmentSpeed()
        {
            var ontwikkelingssnelheden = _dao.GetCommDevelopmentSpeed();

            CommenOntwikkelsnelheden.Clear();

            foreach (var ontwikkelingssnelheid in ontwikkelingssnelheden)
            {
                CommenOntwikkelsnelheden.Add(ontwikkelingssnelheid);
            }
        }

        private void LoadCommenStrategy()
        {
            var strategien = _dao.GetCommStrategy();

            CommenStrategien.Clear();

            foreach (var strategie in strategien)
            {
                CommenStrategien.Add(strategie);
            }
        }

        private void LoadCommenMulti()
        {
            var commensalismeMultis = _dao.GetCommensalismMulti(SelectedPlant);

            CommensalismeMulti.Clear();

            foreach (var commensalismeMulti in commensalismeMultis)
            {
                CommensalismeMulti.Add(commensalismeMulti);
            }
        }

        private void LoadCommenProperties()
        {
            CommenEigenschappen.Clear();

            CommenEigenschappen.Add("Sociabiliteit");
            CommenEigenschappen.Add("Levensvorm");
        }

        private void LoadSociability()
        {
            var sociabiliteiten = _dao.GetCommSociability();

            CommenMulti.Clear();

            foreach (var sociabiliteit in sociabiliteiten)
            {
                CommenMulti.Add(sociabiliteit.Sociabiliteit);
            }
        }

        private void LoadLifeform()
        {
            var levensvormen = _dao.GetCommLifeform();

            CommenMulti.Clear();

            foreach (var levensvorm in levensvormen)
            {
                CommenMulti.Add(levensvorm.Levensvorm);
            }
        }

        private void LoadSelectedValues()
        {
            var commensalisme = _selectedPlant.Commensalisme.SingleOrDefault();

            if (commensalisme is null) return;

            SelectedCommenDevelopmentSpeed = commensalisme.Ontwikkelsnelheid;
            SelectedCommenStrategy = commensalisme.Strategie;

        }



        //wijzig de commensialisme van de plant, als de plant er geen heeft word er eentje aangemaakt voor de plant (Liam)
        private void EditCommensalism()
        {
            var commensalisme = _dao.GetCommensialism(SelectedPlant);

            if (commensalisme == null)
            {
                _dao.AddCommensalism(SelectedPlant, SelectedCommenDevelopmentSpeed, SelectedCommenStrategy);
            }
            else
            {
                _dao.ChangeCommensalism(SelectedPlant, SelectedCommenDevelopmentSpeed, SelectedCommenStrategy);
            }
        }

        //voeg een Commensialisme multi toe (Liam)

        private void AddCommenMulti()
        {
            _dao.AddCommensalismMulti(SelectedPlant, SelectedCommenProperties, SelectedCommensalismMulti);
            LoadCommenMulti();
        }

        //wijzig de geselecteerde commensialisme multi toe (Liam)
        private void EditCommenMulti()
        {
            _dao.ChangeCommensalismMulti(SelectedCommenMulti, SelectedCommenProperties, SelectedCommensalismMulti);
            LoadCommenMulti();
        }



        //wijzig de eigenschap van de combobox, gebaseerd op welke er geselecteerd is word die ingeladen (Liam)
        private void ChangeCommProperties()
        {
            switch (SelectedCommenProperties.ToLower())
            {
                case "sociabiliteit":
                    LoadSociability();
                    break;
                case "levensvorm":
                    LoadLifeform();
                    break;
                default:
                    break;
            }
        }

        //verwijder de geselecteerde commensialisme multi (Liam)
        private void RemoveCommenMulti()
        {
            if (SelectedCommenMulti is not null)
            {
                _dao.DeleteCommensalismMulti(SelectedCommenMulti);
            }
            else
            {
                MessageBox.Show("Gelieve eerst een Eigenschap te selecteren om te verwijderen uit de listview",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            LoadCommenMulti();
        }

    }
}
