using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    // class and GUI (Davy)
    public class TabManagmentActsViewModel : ViewModelBase
    {
        // button  commands (Davy)
        public ICommand AddManagementActCommand { get; set; }
        public ICommand EditManagementActCommand { get; set; }
        public ICommand RemoveManagementActCommand { get; set; }

        //private dao to use its methods (Davy)
        private readonly PlantenDao _plantenDao;

        // ObservableCollections to show the current plant managements (Davy)
        public ObservableCollection<BeheerMaand> ManagementActs { get; set; }

        // private variables (Davy)
        private Plant _selectedPlant;
        private BeheerMaand _selectedManagementAct;
        private string _textInputManagementAct;
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

        private Gebruiker _selectedUser;
        private bool _IsManager;

        // constructor (Davy)
        public TabManagmentActsViewModel(Plant selectedPlant, Gebruiker user)
        {
            SelectedUser = user;
            SelectedPlant = selectedPlant;
            _plantenDao = PlantenDao.Instance;

            //buttoncommands
            AddManagementActCommand = new DelegateCommand(AddManagementAct);
            EditManagementActCommand = new DelegateCommand(EditManagementAct);
            RemoveManagementActCommand = new DelegateCommand(RemoveManagementAct);

            ManagementActs = new ObservableCollection<BeheerMaand>();
            //load in the values (Davy)

            LoadManagmentMonths();
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
            IsManager = SelectedUser.Rol.ToLower() == "manager";
        }
        //the selected user is the account with which you login. This getter setter is given at the start and passes to all other viewmodels (Davy)

        public Gebruiker SelectedUser
        { 
            private get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }

        // Getters and setters selected values (Davy)
        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }

        public BeheerMaand SelectedManagementMonth
        {
        private get => _selectedManagementAct;
            set
            {
                _selectedManagementAct = value;
                OnPropertyChanged();
            }
        }

        public string TextInputManagementAct
        {
            get { return _textInputManagementAct; }
            set
            {
                _textInputManagementAct = value;
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
                if (_isCheckedJanuary == value) return;

                _isCheckedJanuary = value;
                OnPropertyChanged();

            }
        }

        public bool IsCheckedFebruary
        {
            get { return _isCheckedFebruary; }
            set
            {
                if (_isCheckedFebruary == value) return;

                _isCheckedFebruary = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedMarch
        {
            get { return _isCheckedMarch; }
            set
            {
                if (_isCheckedMarch == value) return;

                _isCheckedMarch = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedApril
        {
            get { return _isCheckedApril; }
            set
            {
                if (_isCheckedApril == value) return;

                _isCheckedApril = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedMay
        {
            get { return _isCheckedMay; }
            set
            {
                if (_isCheckedMay == value) return;

                _isCheckedMay = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedJune
        {
            get { return _isCheckedJune; }
            set
            {
                if (_isCheckedJune == value) return;

                _isCheckedJune = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedJuly
        {
            get { return _isCheckedJuly; }
            set
            {
                if (_isCheckedJuly == value) return;

                _isCheckedJuly = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedAugust
        {
            get { return _isCheckedAugust; }
            set
            {
                if (_isCheckedAugust == value) return;

                _isCheckedAugust = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedSeptember
        {
            get { return _isCheckedSeptember; }
            set
            {
                if (_isCheckedSeptember == value) return;

                _isCheckedSeptember = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedOctober
        {
            get { return _isCheckedOctober; }
            set
            {
                if (_isCheckedOctober == value) return;

                _isCheckedOctober = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedNovember
        {
            get { return _isCheckedNovember; }
            set
            {
                if (_isCheckedNovember == value) return;

                _isCheckedNovember = value;
                OnPropertyChanged();

            }
        }
        public bool IsCheckedDecember
        {
            get { return _isCheckedDecember; }
            set
            {
                if (_isCheckedDecember == value) return;

                _isCheckedDecember = value;
                OnPropertyChanged();
            }
        }

        // show the different managements for the selected plants (Davy)
        public void LoadManagmentMonths()
        {
            var managementActs = _plantenDao.GetManagementActs(SelectedPlant);

            ManagementActs.Clear();

            //if object is not the selectedManagement then delete out of the list.
            foreach (var managementAct in managementActs)
            {
                if (managementAct != SelectedManagementMonth)
                {
                    ManagementActs.Remove(managementAct);
                }
            }

            // indien object beheermaand niet gelijk is aan SelectedBeheermaand, voeg object toe aan lijst
            foreach (var managementAct in managementActs)
            {
                ManagementActs.Add(managementAct);
            }
        }

        //make a managemant act for the current plant (Davy)
        private void AddManagementAct()
        {
            BeheerMaand managementMonth = new BeheerMaand();
            managementMonth.PlantId = SelectedPlant.PlantId;
            managementMonth.Beheerdaad = TextInputManagementAct;
            managementMonth.Omschrijving = TextInputDescription;
            managementMonth.Jan = IsCheckedJanuary;
            managementMonth.Feb = IsCheckedFebruary;
            managementMonth.Mrt = IsCheckedMarch;
            managementMonth.Apr = IsCheckedApril;
            managementMonth.Mei = IsCheckedMay;
            managementMonth.Jun = IsCheckedJune;
            managementMonth.Jul = IsCheckedJuly;
            managementMonth.Aug = IsCheckedAugust;
            managementMonth.Sept = IsCheckedSeptember;
            managementMonth.Okt = IsCheckedOctober;
            managementMonth.Nov = IsCheckedNovember;
            managementMonth.Dec = IsCheckedDecember;

            _plantenDao.CreateManagementAct(managementMonth);




    //reload the listview
    LoadManagmentMonths();
        }

        //Edit a selected management (Davy)
        private void EditManagementAct()
        {
            BeheerMaand managementMonth = SelectedManagementMonth; 

            if (managementMonth is not null)
            {
                managementMonth.PlantId = SelectedManagementMonth.PlantId;
                managementMonth.Beheerdaad = TextInputManagementAct;
                managementMonth.Omschrijving = TextInputDescription;
                managementMonth.Jan = IsCheckedJanuary;
                managementMonth.Feb = IsCheckedFebruary;
                managementMonth.Mrt = IsCheckedMarch;
                managementMonth.Apr = IsCheckedApril;
                managementMonth.Mei = IsCheckedMay;
                managementMonth.Jun = IsCheckedJune;
                managementMonth.Jul = IsCheckedJuly;
                managementMonth.Aug = IsCheckedAugust;
                managementMonth.Sept = IsCheckedSeptember;
                managementMonth.Okt = IsCheckedOctober;
                managementMonth.Nov = IsCheckedNovember;
                managementMonth.Dec = IsCheckedDecember;

                _plantenDao.EditManagementAct(managementMonth);
            } else
            {
                MessageBox.Show("Gelieve eerst een beheersdaad te selecteren.");
            }


            LoadManagmentMonths();
        }

        //delete management act (Davy)
        private void RemoveManagementAct()
        {
            BeheerMaand managementAct = SelectedManagementMonth;

    //delete the selected management act otherwise if there is none user gets a notification to select one       
             if (SelectedManagementMonth != null)
            {
                _plantenDao.RemoveManagementAct(managementAct);
            } else
            {
                MessageBox.Show("Gelieve eerst een beheersdaad te selecteren uit de lijst.");
            }
    //reload the list

    LoadManagmentMonths(); 
        }
    }
}
