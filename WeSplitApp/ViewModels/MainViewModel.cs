using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WeSplitApp.Model;
using System.Configuration;

namespace WeSplitApp.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        #region private variables
        //private BaseViewModel _currentPageViewModel = null;

        private String _homeColor;
        private String _addPlaceColor = Brushes.White.ToString();
        private String _detailColor = Brushes.White.ToString();
        private String _addMemberColor = Brushes.White.ToString();
        private String _settingColor = Brushes.White.ToString();
        private String _aboutColor = Brushes.White.ToString();
        #endregion

        #region Commands

        public ICommand HomeCommand { get; set; }
        public ICommand DetailCommand { get; set; }
        public ICommand PlacesCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand AboutCommand { get; set; }

        #endregion

        #region PanelColor

        public String HomeColor { get => _homeColor; set {_homeColor = value; OnPropertyChanged(); } }
        public String DetailColor { get => _detailColor; set { _detailColor = value; OnPropertyChanged(); } }

        public String PlacesColor { get => _addPlaceColor; set { _addPlaceColor = value; OnPropertyChanged(); } }
        public String AddMemberColor { get => _addMemberColor; set { _addMemberColor = value; OnPropertyChanged(); } }
        public String SettingColor { get => _settingColor; set { _settingColor = value; OnPropertyChanged(); } }
        public String AboutColor { get => _aboutColor; set { _aboutColor = value; OnPropertyChanged(); } }

        public Global global =  Global.GetInstance();

        #endregion

        #region static variable

        public static bool IsShowed = false;

        #endregion
        public MainViewModel()
        {
            global.CurrentPageViewModel = new HomeUCViewModel();
            HomeColor = global.ThemeColor;
            HomeCommand = new RelayCommand<ContentControl>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                HomeColor = global.ThemeColor;
                //param.Content = new HomeUCViewModel();
                global.CurrentPageViewModel = new HomeUCViewModel();
            });
            DetailCommand = new RelayCommand<ContentControl>((param) => { return true; }, (param) =>
            {
                var value = ConfigurationManager.AppSettings["DetailTripId"];
                int DetailTripId = int.Parse(value);
                if (DetailTripId == -1 || DataProvider.Ins.DB.Journeys.Where(x => x.Id == DetailTripId).Count() == 0)
                {
                    MessageBox.Show("Please select a trip to see detail !");
                }
                else
                {
                    ResetAllPanelColor();
                    DetailColor = global.ThemeColor;
                    //param.Content = new DetailUCViewModel();
                    global.CurrentPageViewModel = new DetailUCViewModel();
                }
            });
            PlacesCommand = new RelayCommand<ContentControl>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                PlacesColor = global.ThemeColor;
                //param.Content = new PlacesUCViewModel();
                global.CurrentPageViewModel = new PlacesUCViewModel();
            });
            AddMemberCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                AddMemberColor = global.ThemeColor;
                global.CurrentPageViewModel = new MembersUCViewModel();

            });
            SettingCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                SettingColor = global.ThemeColor;
                global.CurrentPageViewModel = new ThemeUCViewModel();
            });
            AboutCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                AboutColor = global.ThemeColor;
                global.CurrentPageViewModel = new AboutUCViewModel();
            });

        }
        
        private void ResetAllPanelColor()
        {
            HomeColor = Brushes.White.ToString();
            DetailColor = Brushes.White.ToString();
            PlacesColor = Brushes.White.ToString();
            AddMemberColor = Brushes.White.ToString();
            SettingColor = Brushes.White.ToString();
            AboutColor = Brushes.White.ToString();
        }
    }
}
