using System;
using PlantenApplicatie.Domain;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PlantenApplicatie.Data;
using System.Windows.Media;
using Prism.Commands;

namespace PlantenApplicatie.viewmodels
{
    //class and GUI (Lily)
    public class TabPhotoViewModel : ViewModelBase
    {
        //button commands
        public ICommand ChangePhotoCommand { get; }
        public ICommand DeletePhotoCommand { get; }
        public List<string> Properties { get; }
        
        // private variabelen (Davy)
        private Gebruiker _selectedUser;
        private bool _IsManager;
        private readonly PlantenDao _plantenDao;
        private  Plant _selectedPlant;
        private Foto? _selectedFoto;
        private ImageSource? _selectedImage;
        private string? _selectedProperty;
        private string? _selectedUrl;
        
        //constructor
        public TabPhotoViewModel(Plant selectedplant,Gebruiker user)
        {
            SelectedUser = user;

            _plantenDao = PlantenDao.Instance;
            _selectedPlant = selectedplant;

            Properties = _plantenDao.GetImageProperties();

            ChangePhotoCommand = new DelegateCommand(ChangePhoto);
            DeletePhotoCommand = new DelegateCommand(DeletePhoto);
            UserRole();
        }

        public bool IsManager
        {
            get => _IsManager;
            set
            {
                _IsManager = value;
                OnPropertyChanged("IsManager");
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

        public Gebruiker SelectedUser
        {
            private get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged();
            }
        }


        //getters and setters 
        public ImageSource? SelectedImage
        {
            get
            {
                return _selectedImage;
            }
            private set
            {
                _selectedImage = value;
                OnPropertyChanged();
            }
        }

        public string? SelectedProperty
        {
            get => _selectedProperty; 
            set
            {
                _selectedProperty = value;
                _selectedFoto = _selectedPlant.Foto
                    .SingleOrDefault(f => f.Eigenschap == SelectedProperty);
                SelectedUrl = _selectedFoto?.UrlLocatie;
                SelectedImage = GenerateBitmapImageFromByteArray(_selectedFoto?.Tumbnail);
                OnPropertyChanged();
            }
        }

        public string? SelectedUrl
        {
            get => _selectedUrl;
            set
            {
                _selectedUrl = value;
                OnPropertyChanged();
            }
        }

        //edit the current photo, when a photo gets changed through the url the image will change and be saved depending on the selected property
        private void ChangePhoto()
        {
            if (SelectedProperty is null) return;
            
            var imageBytes = DownloadImage(SelectedUrl);

            if (imageBytes is null) return;

            if (_selectedFoto is null)
            {
                _selectedFoto = _plantenDao.AddPhoto(SelectedProperty, _selectedPlant, SelectedUrl, imageBytes);
            }
            else {
                _selectedFoto = _plantenDao.ChangePhoto(_selectedFoto, SelectedProperty, SelectedUrl, imageBytes);
            }
            
            SelectedImage = GenerateBitmapImageFromByteArray(imageBytes);
        }

        //delete the selected photo 
        private void DeletePhoto()
        {
            if (SelectedProperty is null || SelectedImage is null) return;
            
            _plantenDao.DeletePhoto(_selectedFoto);

            SelectedProperty = SelectedUrl = null;
            SelectedImage = null;
            _selectedFoto = null;
        }

        private static BitmapImage? GenerateBitmapImageFromByteArray(byte[]? imageBytes)
        {
            /*
             * We create a BitmapImage from the byte array by creating a memory
             * stream (a stream is a flow of data, a sequence, generally with a
             * pointer pointing to the current data) to read the byte array
             * into a BitmapImage, we set the CacheOption to OnLoad so the image
             * gets loaded fully into memory on load time, this way the image
             * is rendered correctly, making it display in the GUI without issues
             */
            if (imageBytes is null) return null;

            using var stream = new MemoryStream(imageBytes);

            stream.Seek(0, SeekOrigin.Begin);
            
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapImage.StreamSource = stream;
            bitmapImage.EndInit();

            return bitmapImage;
        }
        
        //download the image from the online url and set it as the selected detail when you click on edit
        private static byte[]? DownloadImage(string url)
        {
            if (!IsUrlImage(url))
            {
                MessageBox.Show("Please enter an url that points to a valid image");
                return null;
            }
            
            using WebClient client = new();
            
            byte[] imageByteData = client.DownloadData(url!);
            
            return imageByteData;
        }

        private static bool IsUrlImage(string url)
        {
            try
            {
                /*
                 * Send a web request and check whether the content type of the
                 * response is an image type by checking whether the content type
                 * in the response starts with "image/", culture-invariant makes
                 * sure to ignore special characters and read the string raw,
                 * the request method we use is HEAD, which requests the headers
                 * that would be the headers if the request method were GET, which
                 * contains the information we need to know whether it is an image
                 * (the content type) without having to request any additional data
                 * Return false if any exception occured, since this mean something
                 * went wrong with creating or sending the request, indicating the
                 * URL was either invalid or did not work
                 */
                var request = (HttpWebRequest) WebRequest.Create(url);
                request.Method = "HEAD";

                using var response = request.GetResponse();
                
                return response.ContentType.ToLower(CultureInfo.InvariantCulture)
                    .StartsWith("image/");
            }
            catch (Exception)
            {
                return false;
            }
        }

        //controleer welke rol de gebruiker heeft
        private void UserRole()
        {
            switch (SelectedUser.Rol.ToLower())
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
    }
}
