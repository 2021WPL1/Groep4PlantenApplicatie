using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Davy)
    public class EditGebruikerViewModel : ViewModelBase
    {

        private readonly PlantenDao _dao;

        private string? _selectedRole;
        private string? _textInputNumber;
        private string? _textInputVoornaam;
        private string? _textInputAchternaam;
        private string? _textInputEmail;

        private string _passwordErrorMessage;

        private readonly Gebruiker _gebruiker;

        private bool _passwordsMatch = true;

        public ObservableCollection<string> Roles { get; }

        // button commands
        public ICommand EditUserCommand { get; }

        private Window _editGebruikerWindow;

        // edit a user
        public EditGebruikerViewModel(Window window, Gebruiker gebruiker)
        {
            _gebruiker = gebruiker;
            _editGebruikerWindow = window;    
            _dao = PlantenDao.Instance;
            Roles = new ObservableCollection<string>();

            EditUserCommand = new DelegateCommand<PasswordBox>(EditUser);
            LoadRoles();
            LoadData();
        }

        // toon geselecteerde gebruiker in textboxen, comboboxen
        private void LoadData()
        {
            TextInputNumber = _gebruiker.Vivesnr;
            TextInputVoornaam = _gebruiker.Voornaam;
            TextInputAchternaam = _gebruiker.Achternaam;
            TextInputEmail = _gebruiker.Emailadres;
            SelectedRole = _gebruiker.Rol;
        }
        
        public string? TextInputNumber
        {
            get => _textInputNumber;
            set
            {
                _textInputNumber = value;
                OnPropertyChanged();
            }
        }

        public string? TextInputVoornaam
        {
            get => _textInputVoornaam;
            set
            {
                _textInputVoornaam = value;
                OnPropertyChanged();
            }
        }
        
        public string? TextInputAchternaam
        {
            get => _textInputAchternaam;
            set
            {
                _textInputAchternaam = value;
                OnPropertyChanged();
            }
        }

        public string? TextInputEmail
        {
            get => _textInputEmail;
            set
            {
                _textInputEmail = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
            }
        }

        public string PasswordErrorMessage
        {
            get => _passwordErrorMessage;
            private set
            {
                _passwordErrorMessage = value;
                OnPropertyChanged();
            }
        }

        public void PasswordChecker(string password, string passwordConfirm)
        {
            _passwordsMatch = password == passwordConfirm;
            
            PasswordErrorMessage = _passwordsMatch
                ? string.Empty 
                : "Paswoorden zijn niet gelijk";
        }

        public void LoadRoles()
        {
            Roles.Add("manager");
            Roles.Add("data-collector");
            Roles.Add("gebruiker");
        }

        public void EditUser(PasswordBox passwordBox)
        {
            if (TextInputNumber is null || TextInputVoornaam is null || TextInputAchternaam is null 
                || SelectedRole is null || TextInputEmail is null)
            {
                MessageBox.Show("Niet alle velden zijn ingevuld");
                return;
            }
            if (!IsEmailAddressValid(TextInputNumber, TextInputVoornaam, TextInputAchternaam, 
                TextInputEmail))
            {
                MessageBox.Show("Email is ongeldig");
                return;
            }
            if (!_passwordsMatch)
            {
                MessageBox.Show("Wachtwoorden zijn niet gelijk");
                return;
            }

            var gebruiker = new Gebruiker
            {
                Vivesnr = TextInputNumber,
                Voornaam = TextInputVoornaam,
                Achternaam = TextInputAchternaam,
                Rol = SelectedRole,
                Emailadres = TextInputEmail,
                HashPaswoord = Encryptor.GenerateMD5Hash(passwordBox.Password)
            };
                
            _dao.UpdateUser(_gebruiker, gebruiker);
                
            MessageBox.Show("Gebruiker bewerkt");
        }
        
        private static bool IsEmailAddressValid(string? number, string? firstName, string? lastName, string? email)
        {
            return email is not null 
                   && Regex.IsMatch(
                       email, $@"^({number}|{firstName}\.{lastName})@(vives.be|student.vives.be)$");
        }
    }
}
