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
    public class TabCommensalismeViewModel : ViewModelBase
    {
       
        //private selecters en de dao (Liam)
        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private string _selectedOntwikkelingssnelheid;
        private string _selectedStrategie;
        private string _selectedCommenEigenschappen;
        private string _selectedCommensalismeMulti;
        private CommensalismeMulti _selectedCommenMulti;

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
        public TabCommensalismeViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
           
            CommenOntwikkelsnelheden = new ObservableCollection<string>();
            CommenStrategien = new ObservableCollection<string>();
            CommenEigenschappen = new ObservableCollection<string>();
            CommenMulti = new ObservableCollection<string>();
            CommensalismeMulti = new ObservableCollection<CommensalismeMulti>();

            //de delegate commands voor de knoppen (Liam)
            EditCommensalismeCommand = new DelegateCommand(EditCommensalisme);
            AddCommensalismeMultiCommand = new DelegateCommand(AddCommenMulti);
            EditCommensalismeMultiCommand = new DelegateCommand(EditCommenMulti);
            RemoveCommensalismeMultiCommand = new DelegateCommand(RemoveCommenMulti);

            //laad de verschillende lijsten in voor de comboboxes en listview (Liam)
            LoadCommenOntwikkelingssnelheid();
            LoadCommenStrategie();
            LoadCommenMulti();
            LoadCommenEigenschappen();
            LoadLevensvorm();
            LoadSociabiliteit();
            LoadSelectedValues();
        }

        //herlaad de comboboxes (Liam)
        public void Reset()
        {
            LoadCommenStrategie();
            LoadCommenOntwikkelingssnelheid();
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

        public string SelectedCommensalismeMulti
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

        public string SelectedCommenOntwikkelsnelheid
        {
            private get => _selectedOntwikkelingssnelheid;
            set
            {
                _selectedOntwikkelingssnelheid = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommenStrategie
        {
            private get => _selectedStrategie;
            set
            {
                _selectedStrategie = value;
                OnPropertyChanged();
            }
        }

        public string SelectedCommenEigenschappen
        {
            private get => _selectedCommenEigenschappen;
            set
            {
                _selectedCommenEigenschappen = value;
                ChangeCommEigenschappen();
                OnPropertyChanged();
            }
        }

        //laad de verschillende lijsten in (Liam)
        private void LoadCommenOntwikkelingssnelheid()
        {
            var ontwikkelingssnelheden = _dao.GetCommOntwikkelsnelheid();

            CommenOntwikkelsnelheden.Clear();

            foreach (var ontwikkelingssnelheid in ontwikkelingssnelheden)
            {
                CommenOntwikkelsnelheden.Add(ontwikkelingssnelheid);
            }
        }

        private void LoadCommenStrategie()
        {
            var strategien = _dao.GetCommStrategie();

            CommenStrategien.Clear();

            foreach (var strategie in strategien)
            {
                CommenStrategien.Add(strategie);
            }
        }

        private void LoadCommenMulti()
        {
            var commensalismeMultis = _dao.GetCommensalismeMulti(SelectedPlant);

            CommensalismeMulti.Clear();

            foreach (var commensalismeMulti in commensalismeMultis)
            {
                CommensalismeMulti.Add(commensalismeMulti);
            }
        }

        private void LoadCommenEigenschappen()
        {
            CommenEigenschappen.Clear();

            CommenEigenschappen.Add("Sociabiliteit");
            CommenEigenschappen.Add("Levensvorm");
        }

        private void LoadSociabiliteit()
        {
            var sociabiliteiten = _dao.GetCommSociabiliteit();

            CommenMulti.Clear();

            foreach (var sociabiliteit in sociabiliteiten)
            {
                CommenMulti.Add(sociabiliteit.Sociabiliteit);
            }
        }

        private void LoadLevensvorm()
        {
            var levensvormen = _dao.GetCommLevensvorm();

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

            SelectedCommenOntwikkelsnelheid = commensalisme.Ontwikkelsnelheid;
            SelectedCommenStrategie = commensalisme.Strategie;

        }



        //wijzig de commensialisme van de plant, als de plant er geen heeft word er eentje aangemaakt voor de plant (Liam)
        private void EditCommensalisme()
        {
            var commensalisme = _dao.GetCommensialisme(SelectedPlant);

            if(commensalisme == null)
            {
                _dao.AddCommensalisme(SelectedPlant, SelectedCommenOntwikkelsnelheid, SelectedCommenStrategie);
            }
            else
            {
                _dao.ChangeCommensalisme(SelectedPlant, SelectedCommenOntwikkelsnelheid, SelectedCommenStrategie);
            }
        }

        //voeg een Commensialisme multi toe (Liam)

        private void AddCommenMulti()
        {
            _dao.AddCommensalismeMulti(SelectedPlant, SelectedCommenEigenschappen, SelectedCommensalismeMulti);
            LoadCommenMulti();
        }

        //wijzig de geselecteerde commensialisme multi toe (Liam)
        private void EditCommenMulti()
        {
            _dao.ChangeCommensalismeMulti(SelectedCommenMulti, SelectedCommenEigenschappen, SelectedCommensalismeMulti);
            LoadCommenMulti();
        }



        //wijzig de eigenschap van de combobox, gebaseerd op welke er geselecteerd is word die ingeladen (Liam)
        private void ChangeCommEigenschappen()
        {
            switch (SelectedCommenEigenschappen.ToLower())
            {
                case "sociabiliteit":
                    LoadSociabiliteit();
                    break;
                case "levensvorm":
                    LoadLevensvorm();
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
                _dao.DeleteCommensalismeMulti(SelectedCommenMulti);
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
