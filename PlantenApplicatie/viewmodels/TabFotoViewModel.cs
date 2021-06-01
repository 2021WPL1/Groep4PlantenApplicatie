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
    public class TabFotoViewModel : ViewModelBase
    {
        private readonly PlantenDao _plantenDao;
        private readonly Plant _selectedPlant;

        private Foto? _selectedFoto;

        private ImageSource? _selectedImage;
        private string? _selectedEigenschap;
        private string? _selectedUrl;

        public TabFotoViewModel(Plant selectedplant)
        {
            _plantenDao = PlantenDao.Instance;
            _selectedPlant = selectedplant;

            Eigenschappen = _plantenDao.GetFotoEigenschappen();

            ChangeFotoCommand = new DelegateCommand(ChangeFoto);
            DeleteFotoCommand = new DelegateCommand(DeleteFoto);
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

        public string? SelectedEigenschap
        {
            get => _selectedEigenschap; 
            set
            {
                _selectedEigenschap = value;
                _selectedFoto = _selectedPlant.Foto
                    .SingleOrDefault(f => f.Eigenschap == SelectedEigenschap);
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

        private void ChangeFoto()
        {
            var imageBytes = DownloadImage(SelectedUrl);

            if (imageBytes is null)
            {
                return;
            }

            if (_selectedFoto is null)
            {
                _plantenDao.AddFoto(SelectedEigenschap, _selectedPlant, SelectedUrl, imageBytes);
            }
            else {
                _plantenDao.ChangeFoto(_selectedFoto,  SelectedEigenschap, SelectedUrl, imageBytes);
            }
            
            SelectedImage = GenerateBitmapImageFromByteArray(imageBytes);
        }

        private void DeleteFoto()
        {
            _plantenDao.DeleteFoto(_selectedFoto);

            SelectedEigenschap = SelectedUrl = null;
            SelectedImage = null;
        }

        private static BitmapImage? GenerateBitmapImageFromByteArray(byte[]? imageBytes)
        {
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
