using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WeSplitApp.Model;
using WeSplitApp.Models;

namespace WeSplitApp.ViewModels
{
    class PlacesUCViewModel : BaseViewModel
    {
        //Properties 

        private AsyncObservableCollection<dynamic> _locationList;
        public AsyncObservableCollection<dynamic> LocationList { get => _locationList; set { _locationList = value; OnPropertyChanged(); } }

        private AsyncObservableCollection<Province> _provinceList;
        public AsyncObservableCollection<Province> ProvinceList { get => _provinceList; set { _provinceList = value; OnPropertyChanged(); } }

        private Province _selectedProvince;
        public Province SelectedProvince 
        { 
            get => _selectedProvince; 
            set 
            { 
                _selectedProvince = value;
                if (SelectedProvince != null && LocationName != null  && IsNameValid == true)
                {
                    IsEnabledAddLocationButton = true;
                }
                OnPropertyChanged(); 
            }
        }

        private dynamic _selectedLocation;

        public dynamic SelectedLocation { get => _selectedLocation; set { _selectedLocation = value;
                if (SelectedLocation != null)
                {
                    _selectedLocationId = SelectedLocation.Id;
                    Location location = DataProvider.Ins.DB.Locations.Where(x => x.Id == _selectedLocationId).FirstOrDefault();
                    LocationName = location.Name;
                    LocationAddress = location.Address;
                    LocationDescription = location.Description;
                    ImageCover = (location.ImageBytes != null) ? location.ImageBytes : System.Text.Encoding.Default.GetBytes(Global.GetInstance().NoImageStringSource);
                    SelectedProvince = DataProvider.Ins.DB.Provinces.Where(x => x.Id == location.ProvinceId).FirstOrDefault();
                    UpdateOrAddContent = "SỬA";
                }
                else
                {
                    LocationName = null;
                    LocationAddress = null;
                    LocationDescription = null;
                    ImageCover = null;
                    SelectedProvince = null;
                    UpdateOrAddContent = "THÊM";
                    
                }
                OnPropertyChanged(); 
            } 
        }


        private string _locationName;
        public string LocationName 
        { 
            get => _locationName;
            set
            { 
                _locationName = value;
                if (SelectedProvince != null && LocationName != null)
                {
                    IsEnabledAddLocationButton = true;
                }
                IsNameValid = true;
                OnPropertyChanged(); 
            } 
        }

        private string _locationAddress;
        public string LocationAddress { get => _locationAddress; set { _locationAddress = value; OnPropertyChanged(); } }

        private string _locationDescription;
        public string LocationDescription { get => _locationDescription; set { _locationDescription = value; OnPropertyChanged(); } }

        private byte[] _imageCover;
        public byte[] ImageCover { get => _imageCover; set { _imageCover = value; OnPropertyChanged(); } }

        private string _updateOrAddContent;
        public string UpdateOrAddContent { get => _updateOrAddContent; set { _updateOrAddContent = value; OnPropertyChanged(); } }

        // Variables
        private int _selectedLocationId;
        //

        private bool _isNameValid;
        public bool IsNameValid { get => _isNameValid; set { _isNameValid = value; OnPropertyChanged(); } }

        private bool _isEnabledAddLocationButton;
        public bool IsEnabledAddLocationButton { get => _isEnabledAddLocationButton; set { _isEnabledAddLocationButton = value; OnPropertyChanged(); } }


        #region Command
        public ICommand AddImageCommand { get; set; }
        public ICommand AddLocationCommand { get; set; }
        public ICommand DeletePlaceCommand { get; set; }
        public ICommand DisableName { get; set; }

        #endregion

        public PlacesUCViewModel()
        {
            UpdateOrAddContent = "THÊM";

            LocationList = new AsyncObservableCollection<dynamic>();
            foreach (var location in DataProvider.Ins.DB.Locations.OrderBy(x => x.Id))
            {
                AddLoctationToViewList(location);
            }

            ProvinceList = new AsyncObservableCollection<Province>();
            foreach (var province in DataProvider.Ins.DB.Provinces.OrderBy(x => x.Name))
            {
                ProvinceList.Add(province);
            }

            AddImageCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                string AbsoluteLink = "";
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = false;
                dialog.Filter = "JPG files (*.jpg)|*.jpg| PNG files (*.png)|*.png| All files (*.*)|*.*";
                if (dialog.ShowDialog() == true)
                {
                    AbsoluteLink = dialog.FileName;
                    ImageCover = BitMapImageTOBytes(new BitmapImage(
                    new Uri(AbsoluteLink,
                    UriKind.Absolute)
                    ));
                }

            });

            AddLocationCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                
                if (SelectedLocation != null) // update location
                {
                    var updatelocation = DataProvider.Ins.DB.Locations.Find(_selectedLocationId);
                    updatelocation.ProvinceId = SelectedProvince.Id;
                    updatelocation.Name = LocationName;
                    updatelocation.Address = LocationAddress;
                    updatelocation.Description = LocationDescription;
                    updatelocation.ImageBytes = ImageCover;
                        DataProvider.Ins.DB.SaveChanges();
                    LocationList = new AsyncObservableCollection<dynamic>();
                    foreach (var location in DataProvider.Ins.DB.Locations.OrderBy(x => x.Id))
                    {
                        AddLoctationToViewList(location);
                    }
                }
                else // Add new Location
                {
                    Location newLocation = new Location()
                    {
                        ProvinceId = SelectedProvince.Id,
                        Name = LocationName,
                        Address = LocationAddress,
                        Description = LocationDescription,
                        ImageBytes = ImageCover,
                    };
                    DataProvider.Ins.DB.Locations.Add(newLocation);
                    DataProvider.Ins.DB.SaveChanges();
                    AddLoctationToViewList(newLocation);
                }
                SelectedLocation = null;
                IsEnabledAddLocationButton = false;
            });

            DeletePlaceCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                int id = param.Id;
                Location deleteItem = DataProvider.Ins.DB.Locations.Find(id);
                if (deleteItem.Journeys.Count > 0)
                {
                    string listplace = "";
                    foreach (var journey in deleteItem.Journeys)
                    {
                        listplace += "- " + journey.Name + "\n";
                    }
                    MessageBox.Show("Hiện tại đang có những chuyến đi sau đến địa điểm này\n\n" + listplace + "\nBạn cần xóa những chuyến đi này để có thể xóa địa điểm.");
                }
                else
                {
                    if (Global.GetInstance().ConfirmMessageDelete() == true)
                    {
                        DataProvider.Ins.DB.Locations.Remove(deleteItem);
                        LocationList.Remove(param);
                    }
                }
            });
            DisableName = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsNameValid = false;
                IsEnabledAddLocationButton = false;
            });
        }


        public byte[] BitMapImageTOBytes(BitmapImage imageC)
        {
            if (imageC == null) return null;
            MemoryStream memStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(imageC));
            encoder.Save(memStream);
            return memStream.ToArray();
        }

        public void AddLoctationToViewList(Location location)
        {
            dynamic tmp = new
            {
                Id = location.Id,
                Name = location.Name,
                Province = DataProvider.Ins.DB.Provinces.Where(x => x.Id == location.ProvinceId).FirstOrDefault().Name,
                Address = location.Address,
                Description = location.Description,
                ImageBytes = (location.ImageBytes != null) ? location.ImageBytes : System.Text.Encoding.Default.GetBytes(Global.GetInstance().NoImageStringSource),
            };
            LocationList.Add(tmp);
        }
    }
}
