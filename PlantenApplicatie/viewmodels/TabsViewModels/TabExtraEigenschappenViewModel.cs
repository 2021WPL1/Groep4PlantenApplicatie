using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Davy)
    public class TabExtraEigenschappenViewModel : ViewModelBase
    {
        //private variabels (Davy)
        private Plant _selectedPlant;
        private ExtraEigenschap _selectedExtraEigenschap;


        // button commando's
        public ICommand AddExtraCommand { get; set; }
        public ICommand EditExtraCommand { get; set; }
        public ICommand RemoveExtraCommand { get; set; }

        // Hiermee kunnen we de data opvragen aan de databank. (Davy)
        private readonly PlantenDao _plantenDao;

        private string _selectedNectarValue;
        private string _selectedPollenValue;

        private bool _isCheckedBeeFriendly;
        private bool _isCheckedButterflyFriendly;
        private bool _isCheckedEdible;
        private bool _isCheckedHerbUse;
        private bool _isCheckedFragrant;
        private bool _isCheckedFrostSensitive;


        //de observable collections(Davy)
        public ObservableCollection<ExtraEigenschap> BeheerExtraEigenschappen { get; set; }

        public ObservableCollection<string> Nectars { get; set; }

        public ObservableCollection<string> Pollen { get; set; }

        //constructor (Davy)
        public TabExtraEigenschappenViewModel(Plant selectedPlant)
        {
            SelectedPlant = selectedPlant;
            _plantenDao = PlantenDao.Instance;

            BeheerExtraEigenschappen = new ObservableCollection<ExtraEigenschap>();
            Nectars = new ObservableCollection<string>();
            Pollen = new ObservableCollection<string>();

            AddExtraCommand = new DelegateCommand(AddExtra);
            EditExtraCommand = new DelegateCommand(EditExtra);
            RemoveExtraCommand = new DelegateCommand(RemoveExtra);

            //laad de gegevens in (Davy)
            LoadBeheerExtraEigenschappen();
            LoadNectars();
            LoadPollen();
        }

        //getters setters (Davy)
        public Plant SelectedPlant
        {
            private get => _selectedPlant;
            set
            {
                _selectedPlant = value;
                OnPropertyChanged();
            }
        }
        public ExtraEigenschap SelectedExtraEigenschap
        {
            private get => _selectedExtraEigenschap;
            set
            {
                _selectedExtraEigenschap = value;
                OnPropertyChanged();
            }
        }
        public bool IsCheckedBeeFriendly
        {
            get { return _isCheckedBeeFriendly; }
            set
            {
                if (_isCheckedBeeFriendly == value)
                {
                    return;
                }

                _isCheckedBeeFriendly = value;
            }
        }

        public bool IsCheckedButterflyFriendly
        {
            get { return _isCheckedButterflyFriendly; }
            set
            {
                if (_isCheckedButterflyFriendly == value)
                {
                    return;
                }

                _isCheckedButterflyFriendly = value;
            }
        }

        public bool IsCheckedEdible
        {
            get { return _isCheckedEdible; }
            set
            {
                if (_isCheckedEdible == value)
                {
                    return;
                }

                _isCheckedEdible = value;
            }
        }

        public bool IsCheckedFragrant
        {
            get { return _isCheckedFragrant; }
            set
            {
                if (_isCheckedFragrant == value)
                {
                    return;
                }

                _isCheckedFragrant = value;
            }
        }

        public bool IsCheckedFrostSensitive
        {
            get { return _isCheckedFrostSensitive; }
            set
            {
                if (_isCheckedFrostSensitive == value)
                {
                    return;
                }

                _isCheckedFrostSensitive = value;
            }
        }

        public bool IsCheckedHerbUse
        {
            get { return _isCheckedHerbUse; }
            set
            {
                if (_isCheckedHerbUse == value)
                {
                    return;
                }

                _isCheckedHerbUse = value;
            }
        }

        public string SelectedNectarValue
        {
            private get => _selectedNectarValue;
            set
            {
                _selectedNectarValue = value;
                OnPropertyChanged();
            }
        }

        public string SelectedPollenValue
        {
            private get => _selectedPollenValue;
            set
            {
                _selectedPollenValue = value;
                OnPropertyChanged();
            }
        }

        //laad de gegevens in de comboboxes (Davy)

        private void LoadBeheerExtraEigenschappen()
        {
            var eigenschappen = _plantenDao.getExtraEigenschappen(SelectedPlant);

            BeheerExtraEigenschappen.Clear();

            foreach (var eigenschap in eigenschappen)
            {
                BeheerExtraEigenschappen.Add(eigenschap);
            }
        }

        private void LoadNectars()
        {
            var nectars = _plantenDao.GetExtraNectarwaarde();

            Nectars.Clear();

            foreach (var nectar in nectars)
            {
                Nectars.Add(nectar);
            }
        }

        private void LoadPollen()
        {
            var pollen = _plantenDao.GetExtraPollenwaarde();

            Pollen.Clear();

            foreach (var pol in pollen)
            {
                Pollen.Add(pol);
            }
        }

      
        //voeg een extra eigenschap toe aan de plant, plant kan maar 1 hebben (Davy)
        public void AddExtra()
        {
            ExtraEigenschap extraEigenschap = new ExtraEigenschap();
            extraEigenschap.PlantId = SelectedPlant.PlantId;
            extraEigenschap.Nectarwaarde = SelectedNectarValue;
            extraEigenschap.Pollenwaarde = SelectedPollenValue;
            extraEigenschap.Bijvriendelijke = IsCheckedBeeFriendly;
            extraEigenschap.Vlindervriendelijk = IsCheckedButterflyFriendly;
            extraEigenschap.Eetbaar = IsCheckedEdible;
            extraEigenschap.Kruidgebruik = IsCheckedHerbUse;
            extraEigenschap.Geurend = IsCheckedFragrant;
            extraEigenschap.Vorstgevoelig = IsCheckedFrostSensitive;

            string message = _plantenDao.CreateExtraEigenschap(extraEigenschap);

            if (message != String.Empty)
            {
                MessageBox.Show(message);
            }


            // weergeef de aangepaste lijst
            LoadBeheerExtraEigenschappen();
        }
        //wijzig de extra eigenschap van een plant (Davy)
        public void EditExtra()
        {
            ExtraEigenschap extraEigenschap = SelectedExtraEigenschap;

            if (extraEigenschap != null)
            {
                extraEigenschap.Nectarwaarde = SelectedNectarValue;
                extraEigenschap.Pollenwaarde = SelectedPollenValue;
                extraEigenschap.Bijvriendelijke = IsCheckedBeeFriendly;
                extraEigenschap.Vlindervriendelijk = IsCheckedButterflyFriendly;
                extraEigenschap.Eetbaar = IsCheckedEdible;
                extraEigenschap.Kruidgebruik = IsCheckedHerbUse;
                extraEigenschap.Geurend = IsCheckedFragrant;
                extraEigenschap.Vorstgevoelig = IsCheckedFrostSensitive;

                _plantenDao.EditExtraEigenschap(extraEigenschap);
            }
            else
            {
                MessageBox.Show("Gelieve eerst een extra eigenschap te selecteren.");
            }


            LoadBeheerExtraEigenschappen();

        }
        //verwijder de extra eigenschap van een plant (Davy)
        public void RemoveExtra()
        {
            // toewijzen object ExtraEigenschap aan geselecteerd object ExtraEigenschap uit listview
            ExtraEigenschap extraEigenschap = SelectedExtraEigenschap;

            // ken een string waarde toe uit methode verwijder BeheerMaand uit database            
            if (SelectedExtraEigenschap != null)
            {
                _plantenDao.RemoveExtraEigenschap(extraEigenschap);
            }
            else
            {
                MessageBox.Show("Gelieve eerst een extra eigenschap te selecteren uit de lijst.");
            }

            // toon opnieuw de listview met lijst ExtraEigenschappen
            LoadBeheerExtraEigenschappen();
        }

      
    }
}
