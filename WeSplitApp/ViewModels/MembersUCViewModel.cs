using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeSplitApp.ViewModels
{
    class MembersUCViewModel : BaseViewModel
    {
        //Properties 
        private string _locationName;
        public string LocationName { get => _locationName; set { _locationName = value; OnPropertyChanged(); } }

        private string _locationAddress;
        public string LocationAddress { get => _locationAddress; set { _locationAddress = value; OnPropertyChanged(); } }

        private string _locationDescription;
        public string LocationDescription { get => _locationDescription; set { _locationDescription = value; OnPropertyChanged(); } }

        private byte[] _imageCover;
        public byte[] ImageCover { get => _imageCover; set { _imageCover = value; OnPropertyChanged(); } }

        private string _updateOrAddContent;
        public string UpdateOrAddContent { get => _updateOrAddContent; set { _updateOrAddContent = value; OnPropertyChanged(); } }


        //Commands
        public ICommand AddImageCommand { get; set; }
        public ICommand AddLocationCommand { get; set; }
        public ICommand DeletePlaceCommand { get; set; }

        public MembersUCViewModel()
        {
            UpdateOrAddContent = "THÊM";

            AddImageCommand = new RelayCommand<object>((param) => { return true; }, (param) => {


            });

            AddLocationCommand = new RelayCommand<object>((param) => { return true; }, (param) => {

               
            });

            DeletePlaceCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
               
            });
        }
    }
}

