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
    public class TabCommensalismeViewModel : ViewModelBase
    {
        //    private Plant _selectedPlant;
        //    private readonly PlantenDao _dao;
        //    private string _selectedOntwikkelingssnelheid;
        //    private string _selectedStrategie;
        //    private string _selectedLevensvorm;
        //    private string _selectedSociabiliteit;
        //    private Commensalisme _selectedCommensalisme;
        //    private ComboBoxItem _selectedCommen;
        //    private string _selectedCommenMulti;


        //    private string _selectedCommensalismeMulti;


        //    public ObservableCollection<string> CommenSociabiliteit { get; set; }
        //    public ObservableCollection<string> Commen { get; set; }
        //    public ObservableCollection<string> CommenOntwikkelsnelheid { get; set; }
        //    public ObservableCollection<string> CommenStrategie { get; set; }
        //    public ObservableCollection<string> CommenLevensvorm { get; set; }
        //    public ObservableCollection<string> CommenMultis { get; set; }


        //    public ObservableCollection<Commensalisme> Commensalismes { get; set; }

        //    public ObservableCollection<CommensalismeMulti> CommensalismesMulti { get; set; }

        //    public ICommand EditCommensalismeCommand { get; set; }

        //    public ICommand RemoveCommMultiCommand { get; set; }




        //    public TabCommensalismeViewModel(Plant selectedPlant)
        //    {
        //        SelectedPlant = selectedPlant;
        //        _dao = PlantenDao.Instance;
        //        EditCommensalismeCommand = new DelegateCommand(EditCommensalisme);
        //        RemoveCommMultiCommand = new DelegateCommand(RemoveCommMulti);
        //        //SelectMultiCommand = new DelegateCommand(ChangeCombobox);
        //        CommenOntwikkelsnelheid = new ObservableCollection<string>();
        //        CommenStrategie = new ObservableCollection<string>();
        //        Commensalismes = new ObservableCollection<Commensalisme>();
        //        CommensalismesMulti = new ObservableCollection<CommensalismeMulti>();
        //        Commen = new ObservableCollection<string>();
        //        CommenSociabiliteit = new ObservableCollection<string>();
        //        CommenLevensvorm = new ObservableCollection<string>();
        //        CommenMultis = new ObservableCollection<string>();


        //        LoadCommenOntwikkelsnelheid();
        //        LoadCommenStrategie();
        //        LoadSelectedValue();
        //        LoadCommenLevensvorm();
        //        LoadCommenSociabiliteit();
        //        //LoadCommensalismesMulti();
        //        LoadMultis();


        //    }

        //    //private void ChangeCombobox()
        //    //{
        //    //    ComboBoxItem str = SelectedCommen;

        //    //    if (SelectedCommen.Content == "Levensvorm")
        //    //    {
        //    //        LoadCommLevensvorm();
        //    //    }
        //    //    else if(SelectedCommen.Content == "Sociabiliteit")
        //    //    {
        //    //        LoadSociabiliteit();
        //    //    }
        //    //}





        //    public Plant SelectedPlant
        //    {
        //        private get => _selectedPlant;
        //        set
        //        {
        //            _selectedPlant = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public CommensalismeMulti SelectedCommensalismeMulti
        //    {
        //        private get => _selectedCommensalismeMulti;
        //        set
        //        {
        //            _selectedCommensalismeMulti = value;
        //            OnPropertyChanged();
        //        }

        //    }

        //    public string SelectedOntwikkelingssnelheid
        //    {
        //        private get => _selectedOntwikkelingssnelheid;
        //        set
        //        {
        //            _selectedOntwikkelingssnelheid = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public string SelectedStrategie
        //    {
        //        private get => _selectedStrategie;
        //        set
        //        {
        //            _selectedStrategie = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public Commensalisme SelectedCommensalisme
        //    {
        //        private get => _selectedCommensalisme;
        //        set
        //        {
        //            _selectedCommensalisme = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public string SelectedCommensalismeMultis
        //    {
        //        private get => _selectedCommensalismeMulti;
        //        set
        //        {
        //            _selectedCommensalismeMulti = value;
        //            EditCommensalismesMulti();
        //            OnPropertyChanged();
        //        }
        //    }

        //    public ComboBoxItem SelectedCommen
        //    {
        //        private get => _selectedCommen;
        //        set
        //        {
        //            _selectedCommen = value;
        //            OnPropertyChanged();
        //        }
        //    }



        //    public string SelectedCommenMulti
        //    {
        //        private get => _selectedCommenMulti;
        //        set
        //        {
        //            _selectedCommenMulti = value;
        //            OnPropertyChanged();
        //        }
        //    }



        //    private void EditCommensalisme()
        //    {
        //        var commensialismePlant = _dao.GetCommensialisme(SelectedPlant);
        //        if (commensialismePlant is null)
        //        {
        //            _dao.AddCommensalisme(SelectedPlant, SelectedOntwikkelingssnelheid, SelectedStrategie);
        //        }
        //        else
        //        {
        //            _dao.ChangeCommensalisme(SelectedPlant, SelectedOntwikkelingssnelheid, SelectedStrategie);
        //        }
        //        }

        //    private void LoadCommenOntwikkelsnelheid()
        //    {
        //        var ontwikkelsnelheden = _dao.GetCommOntwikkelsnelheid();

        //        CommenOntwikkelsnelheid.Clear();

        //        foreach (var ontwikkelsnelheid in ontwikkelsnelheden)
        //        {
        //            CommenOntwikkelsnelheid.Add(ontwikkelsnelheid);
        //        }
        //    }

        //    private void LoadCommenLevensvorm()
        //    {
        //        var levensvormen = _dao.GetCommLevensvorm();

        //        CommenLevensvorm.Clear();

        //        foreach (var levensvorm in levensvormen)
        //        {
        //            CommenLevensvorm.Add(levensvorm);
        //        }
        //    }
        //    private void LoadCommenSociabiliteit()
        //    {
        //        var sociabiliteiten = _dao.GetCommSociabiliteit();

        //        CommenSociabiliteit.Clear();

        //        foreach (var sociabiliteit in sociabiliteiten)
        //        {
        //            CommenSociabiliteit.Add(sociabiliteit);
        //        }
        //    }

        //    private void LoadCommenStrategie()
        //    {
        //        var strategien = _dao.GetCommStrategie();

        //        CommenStrategie.Clear();

        //        foreach (var strategie in strategien)
        //        {
        //            CommenStrategie.Add(strategie);
        //        }
        //    }

        //    private void EditCommensalismesMulti()
        //    {


        //        switch (SelectedCommenMulti.ToLower())
        //        {
        //            case "Sociabiliteit":
        //                LoadCommenSociabiliteit();
        //                break;
        //            case "Levensvorm":
        //                LoadCommenLevensvorm();
        //                break;
        //            default:
        //                break;
        //        }
        //    }





        //    private void LoadSelectedValue()
        //    {
        //        var commensialisme = _selectedPlant.Commensalisme.SingleOrDefault();
        //        if(commensialisme is null)
        //        {
        //            return;
        //        }

        //        SelectedOntwikkelingssnelheid = commensialisme.Ontwikkelsnelheid;
        //        SelectedStrategie = commensialisme.Strategie;
        //    }

        //    private void LoadMultis()
        //    {
        //        CommenMultis.Clear();

        //        CommenMultis.Add("Sociabiliteit");
        //        CommenMultis.Add("Levensvorm");
        //    }

        //    private void RemoveCommMulti()
        //    {
        //        if (SelectedCommensalismeMulti is not null)
        //        {
        //            _dao.DeleteCommensalismeMulti(SelectedCommensalismeMulti);           
        //        }
        //        else
        //        {
        //            MessageBox.Show("Gelieve eerst een Eigenschap te selecteren om te verwijderen uit de listview",
        //                "Fout", MessageBoxButton.OK, MessageBoxImage.Information) ;
        //        }

        //    }

        private Plant _selectedPlant;
        private readonly PlantenDao _dao;
        private string _selectedOntwikkelingssnelheid;
        private string _selectedStrategie;
        private string _selectedCommenEigenschappen;
        private string _selectedCommensalismeMulti;
        private CommensalismeMulti _selectedCommenMulti;

        public ObservableCollection<string> CommenStrategien { get; set; }
        public ObservableCollection<string> CommenOntwikkelsnelheden { get; set; }
        public ObservableCollection<string> CommenEigenschappen { get; set; }
        public ObservableCollection<CommensalismeMulti> CommensalismeMulti { get; set; }
        public ObservableCollection<string> CommenMulti { get; set; }

        public ICommand EditCommensalismeCommand { get; set; }
        public ICommand AddCommensalismeMultiCommand { get; set; }
        public ICommand EditCommensalismeMultiCommand { get; set; }
        public ICommand RemoveCommensalismeMultiCommand { get; set; }

        public TabCommensalismeViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _dao = PlantenDao.Instance;
            EditCommensalismeCommand = new DelegateCommand(EditCommensalisme);
            CommenOntwikkelsnelheden = new ObservableCollection<string>();
            CommenStrategien = new ObservableCollection<string>();
            AddCommensalismeMultiCommand = new DelegateCommand(AddCommenMulti);
            EditCommensalismeMultiCommand = new DelegateCommand(EditCommenMulti);
            RemoveCommensalismeMultiCommand = new DelegateCommand(RemoveCommenMulti);
            CommenEigenschappen = new ObservableCollection<string>();
            CommenMulti = new ObservableCollection<string>();
            CommensalismeMulti = new ObservableCollection<CommensalismeMulti>();

            LoadCommenOntwikkelingssnelheid();
            LoadCommenStrategie();
            LoadCommenMulti();
            LoadCommenEigenschappen();
            LoadLevensvorm();
            LoadSociabiliteit();
            LoadSelectedValues();
        }

        public void Reset()
        {
            LoadCommenStrategie();
            LoadCommenOntwikkelingssnelheid();
            LoadCommenMulti();
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

            foreach(var sociabiliteit in sociabiliteiten)
            {
                CommenMulti.Add(sociabiliteit.Sociabiliteit);
            }
        }

        private void LoadLevensvorm()
        {
            var levensvormen = _dao.GetCommLevensvorm();

            CommenMulti.Clear();

            foreach(var levensvorm in levensvormen)
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

        private void AddCommenMulti()
        {
            _dao.AddCommensalismeMulti(SelectedPlant, SelectedCommenEigenschappen, SelectedCommensalismeMulti);
            LoadCommenMulti();
        }

        private void EditCommenMulti()
        {
            _dao.ChangeCommensalismeMulti(SelectedCommenMulti, SelectedCommenEigenschappen, SelectedCommensalismeMulti);
            LoadCommenMulti();
        }
        
        

        private void EditCommensalismeMulti()
        {
        }

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
