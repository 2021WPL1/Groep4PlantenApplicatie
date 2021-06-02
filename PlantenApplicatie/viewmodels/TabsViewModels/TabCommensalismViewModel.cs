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
    //class and GUI Liam
    public class TabCommensalismViewModel : ViewModelBase
    {

        //private selecters and the dao (Liam)
        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private string _selectedOntwikkelingssnelheid;
        private string _selectedStrategie;
        private string _selectedCommenEigenschappen;
        private string _selectedCommensalismeMulti;
        private CommensalismeMulti _selectedCommenMulti;

        // private variables (Davy)
        private Gebruiker _selectedGebruiker;
        private bool _IsManager;

        //observable collections for the list and comboboxes (Liam)
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
        public TabCommensalismViewModel(Plant selectedPlant, Gebruiker gebruiker)
        {
            SelectedGebruiker = gebruiker;
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;

            CommenOntwikkelsnelheden = new ObservableCollection<string>();
            CommenStrategien = new ObservableCollection<string>();
            CommenEigenschappen = new ObservableCollection<string>();
            CommenMulti = new ObservableCollection<string>();
            CommensalismeMulti = new ObservableCollection<CommensalismeMulti>();

            //the delegate commands for the buttons (Liam)
            EditCommensalismeCommand = new DelegateCommand(EditCommensalisme);
            AddCommensalismeMultiCommand = new DelegateCommand(AddCommenMulti);
            EditCommensalismeMultiCommand = new DelegateCommand(EditCommenMulti);
            RemoveCommensalismeMultiCommand = new DelegateCommand(RemoveCommenMulti);

            //load the different lists in for the comboboxes and listview (Liam)
            LoadCommenDevelopmentSpeed();
            LoadCommenStrategy();
            LoadCommenMulti();
            LoadCommenProperties();
            LoadLifeform();
            LoadSociability();
            LoadSelectedValues();
            UserRole();
        }

        //boolean to check which functions the user can perform on the application (Davy)
        public bool IsManager
        {
            get => _IsManager;
            set
            {
                _IsManager = value;
                OnPropertyChanged("IsManager");
            }
        }


        //check which roles the user has. and if the user is an old student(Gebruiker)
        //He can only observe the selected values of the plant (Davy,Jim)
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

        //the selected user is the account with which you login. This getter setter is given at the start and passes to all other viewmodels (Davy)
        public Gebruiker SelectedGebruiker
        {
            private get => _selectedGebruiker;
            set
            {
                _selectedGebruiker = value;
                OnPropertyChanged();
            }
        }

        //reload the comboboxes (Liam)
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

        //load the different lists into the comboboxes + listview (Liam)
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

        //load the selected values into the different lists (Liam)
        private void LoadSelectedValues()
        {
            var commensalisme = _selectedPlant.Commensalisme.SingleOrDefault();

            if (commensalisme is null) return;

            SelectedCommenDevelopmentSpeed = commensalisme.Ontwikkelsnelheid;
            SelectedCommenStrategy = commensalisme.Strategie;

        }



        //Edit the commensalisme of a plant, if there is none for the current plant a new one will be made with the selected values(Liam)
        private void EditCommensalisme()
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

        //add a commensalisme multi to the plant(Liam)

        private void AddCommenMulti()
        {
            _dao.AddCommensalismMulti(SelectedPlant, SelectedCommenProperties, SelectedCommensalismMulti);
            LoadCommenMulti();
        }

        //edit the selected commensalisme multi of the plant (Liam)
        private void EditCommenMulti()
        {
            _dao.ChangeCommensalismMulti(SelectedCommenMulti, SelectedCommenProperties, SelectedCommensalismMulti);
            LoadCommenMulti();
        }



        //edit the properties of the combobox depending on which head property is selected (Liam)
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

        //delete the selected commensialisme multi (Liam)
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
