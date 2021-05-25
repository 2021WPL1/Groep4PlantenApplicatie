using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    // klasse BeheerDadenViewModel (Davy)
    public class TabBeheerDadenViewModel : ViewModelBase
    {
        // knop commando's
        public ICommand AddManagementActCommand { get; set; }
        public ICommand EditManagementActCommand { get; set; }
        public ICommand RemoveManagementActCommand { get; set; }

        // Hiermee kunnen we de data opvragen aan de databank.
        private readonly PlantenDao _plantenDao;

        // ObservableCollection om de beheermaanden weer te geven (Davy)
        public ObservableCollection<BeheerMaand> BeheerMaanden { get; set; }

        // private variabelen Davy
        private Plant _selectedPlant;
        private BeheerMaand _selectedBeheerMaand;
        private string _textInputBeheerdaad;
        private string _textInputDescription;
        private bool _isCheckedJanuary;
        private bool _isCheckedFebruary;
        private bool _isCheckedMarch;
        private bool _isCheckedApril;
        private bool _isCheckedMay;
        private bool _isCheckedJune;
        private bool _isCheckedJuly;
        private bool _isCheckedAugust;
        private bool _isCheckedSeptember;
        private bool _isCheckedOctober;
        private bool _isCheckedNovember;
        private bool _isCheckedDecember;

        // constructor (Davy)
        public TabBeheerDadenViewModel(Plant selectedPlant)
        {

            SelectedPlant = selectedPlant;
            _plantenDao = PlantenDao.Instance;

            AddManagementActCommand = new DelegateCommand(AddManagementAct);
            EditManagementActCommand = new DelegateCommand(EditManagementAct);
            RemoveManagementActCommand = new DelegateCommand(RemoveManagementAct);

            BeheerMaanden = new ObservableCollection<BeheerMaand>();

            LoadBeheerMaanden();
        }

        // Getters and setters selected waardes (Davy)
        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }   

        public BeheerMaand SelectedBeheerMaand
        {
            private get => _selectedBeheerMaand;
            set
            {
                _selectedBeheerMaand = value;
                OnPropertyChanged();
            }
        }

        public string TextInputBeheerdaad
        {
            get { return _textInputBeheerdaad; }
            set
            {
                _textInputBeheerdaad = value;
                OnPropertyChanged();
            }
        }
        public string TextInputDescription
        {
            get { return _textInputDescription; }
            set
            {
                _textInputDescription = value;
                OnPropertyChanged();
            }
        }

        public bool IsCheckedJanuary
        {
            get { return _isCheckedJanuary; }
            set
            {
                if (_isCheckedJanuary == value)
                {
                    return;
                }

                _isCheckedJanuary = value;
            }
        }

        public bool IsCheckedFebruary
        {
            get { return _isCheckedFebruary; }
            set
            {
                if (_isCheckedFebruary == value)
                {
                    return;
                }

                _isCheckedFebruary = value;
            }
        }
        public bool IsCheckedMarch
        {
            get { return _isCheckedMarch; }
            set
            {
                if (_isCheckedMarch == value)
                {
                    return;
                }

                _isCheckedMarch = value;
            }
        }
        public bool IsCheckedApril
        {
            get { return _isCheckedApril; }
            set
            {
                if (_isCheckedApril == value)
                {
                    return;
                }

                _isCheckedApril = value;
            }
        }
        public bool IsCheckedMay
        {
            get { return _isCheckedMay; }
            set
            {
                if (_isCheckedMay == value)
                {
                    return;
                }

                _isCheckedMay = value;
            }
        }
        public bool IsCheckedJune
        {
            get { return _isCheckedJune; }
            set
            {
                if (_isCheckedJune == value)
                {
                    return;
                }

                _isCheckedJune = value;
            }
        }
        public bool IsCheckedJuly
        {
            get { return _isCheckedJuly; }
            set
            {
                if (_isCheckedJuly == value)
                {
                    return;
                }

                _isCheckedJuly = value;
            }
        }
        public bool IsCheckedAugust
        {
            get { return _isCheckedAugust; }
            set
            {
                if (_isCheckedAugust == value)
                {
                    return;
                }

                _isCheckedAugust = value;
            }
        }
        public bool IsCheckedSeptember
        {
            get { return _isCheckedSeptember; }
            set
            {
                if (_isCheckedSeptember == value)
                {
                    return;
                }

                _isCheckedSeptember = value;
            }
        }
        public bool IsCheckedOctober
        {
            get { return _isCheckedOctober; }
            set
            {
                if (_isCheckedOctober == value)
                {
                    return;
                }

                _isCheckedOctober = value;
            }
        }
        public bool IsCheckedNovember
        {
            get { return _isCheckedNovember; }
            set
            {
                if (_isCheckedNovember == value)
                {
                    return;
                }

                _isCheckedNovember = value;
            }
        }
        public bool IsCheckedDecember
        {
            get { return _isCheckedDecember; }
            set
            {
                if (_isCheckedDecember == value)
                {
                    return;
                }

                _isCheckedDecember = value;
            }
        }

        // geef de BeheerMaanden weer in de listview
        public void LoadBeheerMaanden()
        {
            var beheermaanden = _plantenDao.GetBeheerMaanden(SelectedPlant);

            BeheerMaanden.Clear();

            foreach (var beheermaand in beheermaanden)
            {
                BeheerMaanden.Add(beheermaand);
            }
        }

        // maak een beheerdaad aan
        private void AddManagementAct()
        {
            BeheerMaand beheerMaand = new BeheerMaand();
            beheerMaand.PlantId = SelectedPlant.PlantId;
            beheerMaand.Beheerdaad = TextInputBeheerdaad;
            beheerMaand.Omschrijving = TextInputDescription;
            beheerMaand.Jan = IsCheckedJanuary;
            beheerMaand.Feb = IsCheckedFebruary;
            beheerMaand.Mrt = IsCheckedMarch;
            beheerMaand.Apr = IsCheckedApril;
            beheerMaand.Mei = IsCheckedMay;
            beheerMaand.Jun = IsCheckedJune;
            beheerMaand.Jul = IsCheckedJuly;
            beheerMaand.Aug = IsCheckedAugust;
            beheerMaand.Sept = IsCheckedSeptember;
            beheerMaand.Okt = IsCheckedOctober;
            beheerMaand.Nov = IsCheckedNovember;
            beheerMaand.Dec = IsCheckedDecember;

            _plantenDao.CreateBeheerMaand(beheerMaand);

            // weergeef de aangepaste lijst
            LoadBeheerMaanden();
        }

        // wijzig een beheerdaad
        private void EditManagementAct()
        {
            BeheerMaand beheerMaand = SelectedBeheerMaand; 
            beheerMaand.PlantId = SelectedPlant.PlantId;
            beheerMaand.Beheerdaad = TextInputBeheerdaad;
            beheerMaand.Omschrijving = TextInputDescription;
            beheerMaand.Jan = IsCheckedJanuary;
            beheerMaand.Feb = IsCheckedFebruary;
            beheerMaand.Mrt = IsCheckedMarch;
            beheerMaand.Apr = IsCheckedApril;
            beheerMaand.Mei = IsCheckedMay;
            beheerMaand.Jun = IsCheckedJune;
            beheerMaand.Jul = IsCheckedJuly;
            beheerMaand.Aug = IsCheckedAugust;
            beheerMaand.Sept = IsCheckedSeptember;
            beheerMaand.Okt = IsCheckedOctober;
            beheerMaand.Nov = IsCheckedNovember;
            beheerMaand.Dec = IsCheckedDecember;

            _plantenDao.EditBeheerMaand(beheerMaand);

            LoadBeheerMaanden();
        }

        // verwijder beheerdaad
        private void RemoveManagementAct()
        {
            // toewijzen object BeheerMaand aan geselecteerd object BeheerMaand uit listview
            BeheerMaand beheerMaand = SelectedBeheerMaand;

            // verwijder BeheerMaand uit database
            _plantenDao.RemoveBeheerMaand(beheerMaand);

            // toon opnieuw de listview met lijst BeheerMaanden
            LoadBeheerMaanden();
        }
    }
}
