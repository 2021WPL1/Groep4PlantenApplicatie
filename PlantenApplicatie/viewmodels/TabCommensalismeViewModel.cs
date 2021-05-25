using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows;


namespace PlantenApplicatie.viewmodels
{
    public class TabCommensalismeViewModel : ViewModelBase
    {
        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private string _selectedOntwikkelingssnelheid;
        private string _selectedStrategie;
        private Commensalisme _selectedCommensalisme;
        private string _selectedCommen;

        private CommensalismeMulti _selectedCommensalismeMulti;
        private CommensalismeMulti _selectedCommenMultiToAdd;
        private CommensalismeMulti _selectedCommenMultiToRemove;

        public ObservableCollection<string> CommenSociabiliteit { get; set; }
        public ObservableCollection<string> Commen { get; set; }
        public ObservableCollection<string> CommenOntwikkelsnelheid { get; set; }
        public ObservableCollection<string> CommenStrategie { get; set; }

        public ObservableCollection<Commensalisme> Commensalismes { get; set; }

        public ObservableCollection<CommensalismeMulti> CommensalismesMulti { get; set; }

        public ICommand EditCommensalismeCommand { get; set; }

        public ICommand AddCommenMultiCommand { get; set; }

        public ICommand RemoveCommenMultiCommand { get; set; }

        public ICommand SelectMultiCommand { get; set; }


        public TabCommensalismeViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            EditCommensalismeCommand = new DelegateCommand(EditCommensalisme);
            AddCommenMultiCommand = new DelegateCommand(AddCommenMulti);
            RemoveCommenMultiCommand = new DelegateCommand(RemoveCommenMulti);
            SelectMultiCommand = new DelegateCommand(ChangeCombobox);
            CommenOntwikkelsnelheid = new ObservableCollection<string>();
            CommenStrategie = new ObservableCollection<string>();
            Commensalismes = new ObservableCollection<Commensalisme>();
            CommensalismesMulti = new ObservableCollection<CommensalismeMulti>();
            Commen = new ObservableCollection<string>();
            CommenSociabiliteit = new ObservableCollection<string>();

            LoadCommenOntwikkelsnelheid();
            LoadCommenStrategie();

            LoadCommensalismesMulti();

            
        }

        private void ChangeCombobox()
        {
            string str = SelectedCommen;
            
            if (SelectedCommen == "Levensvorm")
            {
                LoadCommLevensvorm();
            }
            else if(SelectedCommen == "Sociabiliteit")
            {
                LoadSociabiliteit();
            }
        }

        private void LoadSociabiliteit()
        {
            var sociabiliteiten = _dao.GetCommSociabiliteit();

            Commen.Clear();

            foreach (var sociabiliteit in sociabiliteiten)
            {
                Commen.Add(sociabiliteit);
            }
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

        public CommensalismeMulti SelectedCommensalismeMulti
        {
            private get => _selectedCommensalismeMulti;
            set
            {
                _selectedCommensalismeMulti = value;
                OnPropertyChanged();
            }
            
        }

        public string SelectedOntwikkelingssnelheid
        {
            private get => _selectedOntwikkelingssnelheid;
            set
            {
                _selectedOntwikkelingssnelheid = value;
                OnPropertyChanged();
            }
        }

        public string SelectedStrategie
        {
            private get => _selectedStrategie;
            set
            {
                _selectedStrategie = value;
                OnPropertyChanged();
            }
        }

        public Commensalisme SelectedCommensalisme
        {
            private get => _selectedCommensalisme;
            set
            {
                _selectedCommensalisme = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommen
        {
            private get => _selectedCommen;
            set
            {
                _selectedCommen = value;
                OnPropertyChanged();
            }
        }

        public CommensalismeMulti SelectedCommenMultiToAdd
        {
            private get => _selectedCommenMultiToAdd;
            set
            {
                _selectedCommenMultiToAdd = value;
                OnPropertyChanged();
            }
        }

        public CommensalismeMulti SelectedCommenMultiToRemove
        {
            private get => _selectedCommenMultiToRemove;
            set
            {
                _selectedCommenMultiToRemove = value;
                OnPropertyChanged();
            }
        }

        private void EditCommensalisme()
        {
            SelectedCommensalisme = _dao.ChangeCommensalisme(SelectedCommensalisme, SelectedOntwikkelingssnelheid, SelectedStrategie);
        }

        private void LoadCommenOntwikkelsnelheid()
        {
            var ontwikkelsnelheden = _dao.GetCommOntwikkelsnelheid();

            CommenOntwikkelsnelheid.Clear();

            foreach (var ontwikkelsnelheid in ontwikkelsnelheden)
            {
                CommenOntwikkelsnelheid.Add(ontwikkelsnelheid);
            }
        }

        private void LoadCommLevensvorm()
        {
            var levensvormen = _dao.GetCommLevensvorm();

            Commen.Clear();

            foreach (var levensvorm in levensvormen)
            {
                Commen.Add(levensvorm);
            }
        }

        private void LoadCommenStrategie()
        {
            var strategien = _dao.GetCommStrategie();

            CommenStrategie.Clear();

            foreach (var strategie in strategien)
            {
                CommenStrategie.Add(strategie);
            }
        }

        private void LoadCommensalismesMulti()
        {
            var commensalismesMulti = _dao.GetCommensalismeMulti(SelectedPlant);
            CommensalismesMulti.Clear();

            foreach (var commensalismeMulti in commensalismesMulti)
            {
                CommensalismesMulti.Add(commensalismeMulti);
            }
        }

        private void AddCommenMulti()
        {
            if(SelectedCommenMultiToAdd is not null)
            {
                MessageBox.Show("Niets",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Gelieve eerst een selectie te maken om toe te voegen",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void RemoveCommenMulti()
        {
            if (SelectedCommenMultiToRemove is not null)
            {
                MessageBox.Show("Niets",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Gelieve eerst een selectie te maken om toe te voegen",
                    "Fout", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
