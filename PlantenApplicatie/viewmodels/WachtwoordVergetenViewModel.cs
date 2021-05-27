using PlantenApplicatie.Data;
using PlantenApplicatie.Domain;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace PlantenApplicatie.viewmodels
{
    public class WachtwoordVergetenViewModel : ViewModelBase
    {
        // button commando
        public ICommand ForgotPasswordCommand { get; set; }

        private Window _forgotPasswordWindow;

        private PlantenDao _dao;

        private string _textInputEmail;

        public WachtwoordVergetenViewModel(Window window)
        {
            _forgotPasswordWindow = window;
            _dao = PlantenDao.Instance;
            ForgotPasswordCommand = new DelegateCommand(ForgotPassword);
        }


        private static string mailFrom = "groepvierplantjes@gmail.com";
        private static string mailFromPassword = "Test1234Test1234";


        public void ForgotPassword()
        {
            Gebruiker gebruiker = _dao.GetGebruiker(TextInputEmail);

            string newPassword = Encoding.ASCII.GetString(gebruiker.HashPaswoord);
           

            try
            {
                                

                using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587))
                {
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(mailFrom, mailFromPassword);
                    client.EnableSsl = true;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(mailFrom);
                    string password = newPassword;
                    mail.Body = "Wachtwoord : " + password;
                    mail.Subject = "Wachtwoord";
                   
                    mail.To.Add(gebruiker.Emailadres);

                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            Inlogscherm inlogscherm = new Inlogscherm();
            inlogscherm.Show();

            _forgotPasswordWindow.Hide();
        }

        public string TextInputEmail
        {
            get => _textInputEmail;

            set
            {
                _textInputEmail = value;
                OnPropertyChanged(_textInputEmail);
            }
        }
    }
}
