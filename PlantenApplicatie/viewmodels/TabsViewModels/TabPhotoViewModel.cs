using System;
using PlantenApplicatie.Domain;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PlantenApplicatie.Data;
using System.Windows.Media;
using Prism.Commands;

namespace PlantenApplicatie.viewmodels
{
    // klasse (Davy, Lily)
    public class TabPhotoViewModel : ViewModelBase
    {
        private readonly PlantenDao _plantenDao;
        private readonly Plant _selectedPlant;

        private Foto? _selectedFoto;

        private ImageSource? _selectedImage;
        private string? _selectedEigenschap;
        private string? _selectedUrl;

        public TabPhotoViewModel(Plant selectedplant,User gebruiker)
        {
            _plantenDao = PlantenDao.Instance;
            _selectedPlant = selectedplant;

            Eigenschappen = _plantenDao.GetFotoEigenschappen();

            ChangeFotoCommand = new DelegateCommand(ChangePhoto);
            DeleteFotoCommand = new DelegateCommand(DeletePhoto);
        }
        
        public ICommand ChangeFotoCommand { get; }
        public ICommand DeleteFotoCommand { get; }

        public List<string> Eigenschappen { get; }

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
            get => _selectedEigenschap; 
            set
            {
                _selectedEigenschap = value;
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

        private void ChangePhoto()
        {
            var imageBytes = DownloadImage(SelectedUrl);

            if (imageBytes is null)
            {
                return;
            }

            if (_selectedFoto is null)
            {
                _plantenDao.AddFoto(SelectedProperty, _selectedPlant, SelectedUrl, imageBytes);
            }
            else {
                _plantenDao.ChangeFoto(_selectedFoto,  SelectedProperty, SelectedUrl, imageBytes);
            }
            
            SelectedImage = GenerateBitmapImageFromByteArray(imageBytes);
        }

        private void DeletePhoto()
        {
            _plantenDao.DeleteFoto(_selectedFoto);

            SelectedProperty = SelectedUrl = null;
            SelectedImage = null;
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
    }
}
