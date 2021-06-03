using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Davy)
    public class EditGebruikerViewModel : ViewModelBase
    {
        private readonly PlantenDao _dao;

        private string _SelectedRole;
        private string _textInputNumber;
        private string _TextInputVoornaam;
        private string _TextInputAchternaam;
        private string _TextInputEmail;
        private string _TextInputPaswoord;
        private string _TextInputPaswoordCheck;
        private string _Check;

        private Gebruiker _gebruiker;

        public ObservableCollection<string> Roles { get; set; }

        // button commando's
        public ICommand EditUserCommand { get; set; }

        public ICommand CloseWindowCommand { get; set; }

        private Window _editGebruikerWindow;

        private Brush _color;

        public EditGebruikerViewModel(Window window, Gebruiker gebruiker)
        {
            _gebruiker = gebruiker;
            _editGebruikerWindow = window;    
            _dao = PlantenDao.Instance;
            Roles = new ObservableCollection<string>();

            EditUserCommand = new DelegateCommand(EditUser);
            CloseWindowCommand = new DelegateCommand(CloseWindow);
            LoadRoles();
            LoadData();
        }

        public Brush ChangeColor
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

        // toon geselecteerde gebruiker in textboxen, comboboxen
        private void LoadData()
        {
            TextInputVoornaam = _gebruiker.Voornaam;
            TextInputAchternaam = _gebruiker.Achternaam;
            TextInputEmail = _gebruiker.Emailadres;
            SelectedRole = _gebruiker.Rol;
        }
        public string TextInputNumber
        {
            get => _textInputNumber;
            set
            {
                _textInputNumber = value;
            }
        }

        public string TextInputVoornaam
        {
            get => _TextInputVoornaam;
            set
            {
                _TextInputVoornaam = value;
                OnPropertyChanged();
            }
        }
        public string TextInputAchternaam
        {
            get => _TextInputAchternaam;
            set
            {
                _TextInputAchternaam = value;
                OnPropertyChanged();
            }
        }

        public string TextInputEmail
        {
            get => _TextInputEmail;
            set
            {
                _TextInputEmail = value;
                OnPropertyChanged();
            }
        }

        public string TextInputPaswoord
        {
            get => _TextInputPaswoord;
            set
            {
                _TextInputPaswoord = value;
                OnPropertyChanged();
            }
        }
        public string TextInputPaswoordCheck
        {
            get => _TextInputPaswoordCheck;
            set
            {
                _TextInputPaswoordCheck = value;
                OnPropertyChanged();
                PasswordChecker();
            }
        }

        public string SelectedRole
        {
            get => _SelectedRole;
            set
            {
                _SelectedRole = value;
                OnPropertyChanged();
            }
        }

        public string Check
        {
            get => _Check;
            set
            {
                _Check = value;
                OnPropertyChanged();
            }

        }

        public void LoadRoles()
        {
            Roles.Add("manager");
            Roles.Add("data-collector");
            Roles.Add("gebruiker");

        }

        public void PasswordChecker()
        {
            if (TextInputPaswoord != TextInputPaswoordCheck)
            {
                Check = "Paswoorden zijn niet gelijk";
                ChangeColor = Brushes.Red;
            }
            else
            {
                Check = "Paswoorden zijn gelijk";
                ChangeColor = Brushes.Green;
            }

        }


        public void EditUser()
        {
            string message = "";

            if (SelectedRole == null || TextInputVoornaam == null || _TextInputAchternaam == null ||
                TextInputEmail == null || TextInputPaswoord == null || TextInputPaswoordCheck == null)
            {
                MessageBox.Show("Niet alle velden zijn ingevuld");
            }
            else
            {
                if (TextInputEmail.Contains("@vives.be") || TextInputEmail.Contains("@student.vives.be"))
                {
                    if (TextInputPaswoord == TextInputPaswoordCheck)
                    {
                        var gebruiker = new Gebruiker
                        {
                            Voornaam = TextInputVoornaam,
                            Achternaam = TextInputAchternaam,
                            Rol = SelectedRole,
                            Emailadres = TextInputEmail,
                            HashPaswoord = Encryptor.GenerateMD5Hash(TextInputPaswoord)
                        };
                        message = _dao.UpdateUser(gebruiker.Emailadres, TextInputPaswoord);
                        MessageBox.Show(message);

                        // herladen Users door nieuw venster BeheerPlanten op te starten
                        BeheerPlanten beheerPlanten = new BeheerPlanten(_gebruiker);
                        beheerPlanten.Show();

                        _editGebruikerWindow.Close();
                    }

                }
                else
                {
                    MessageBox.Show("Email mag alleen van het Vives domein zijn.");
                }
            }
        }

        private void CloseWindow()
        {
            // nieuw venster BeheerPlanten opstarten
            BeheerPlanten beheerPlanten = new BeheerPlanten(_gebruiker);
            beheerPlanten.Show();

            _editGebruikerWindow.Close();
        }

    }
}
