using System;
using System.Linq;
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
        private Visibility _leftPanelVisibility = Visibility.Visible;
        private String _addPlaceColor = Brushes.White.ToString();
        private String _addMemberColor = Brushes.White.ToString();
        private String _settingColor = Brushes.White.ToString();
        private String _aboutColor = Brushes.White.ToString();
        private String _versionTextBlock = Brushes.White.ToString();
        #endregion

        #region Commands

        public ICommand HomeCommand { get; set; }
        public ICommand DetailCommand { get; set; }
        public ICommand PlacesCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand AboutCommand { get; set; }

        public ICommand OpenPanelCommand { get; set; }

        #endregion

        #region PanelColor

        public String PlacesColor { get => _addPlaceColor; set { _addPlaceColor = value; OnPropertyChanged(); } }
        public String AddMemberColor { get => _addMemberColor; set { _addMemberColor = value; OnPropertyChanged(); } }
        public String SettingColor { get => _settingColor; set { _settingColor = value; OnPropertyChanged(); } }
        public String AboutColor { get => _aboutColor; set { _aboutColor = value; OnPropertyChanged(); } }

        public String VersionTextBlock { get => _versionTextBlock; set { _versionTextBlock = value; OnPropertyChanged(); } }

        public Global global =  Global.GetInstance();

        public Visibility LeftPanelVisibility { get => _leftPanelVisibility; set { _leftPanelVisibility = value; OnPropertyChanged(); } }

        #endregion

        private string GetPublishedVersion()
        {
            var version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            string appVersion = $"{version.Major}.{version.Minor}";
            return appVersion;
        }


        #region static variable

        public static bool IsShowed = false;

        #endregion
        public MainViewModel()
        {
              VersionTextBlock = GetPublishedVersion();
             if (VersionTextBlock == null || VersionTextBlock =="" )
                    VersionTextBlock = "not installed";
          
            global.CurrentPageViewModel = new HomeUCViewModel();
            global.HomeColor = global.ThemeColor;

            OpenPanelCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                if (LeftPanelVisibility == Visibility.Visible)
                    LeftPanelVisibility = Visibility.Collapsed;
                else
                    LeftPanelVisibility = Visibility.Visible;
            });
            HomeCommand = new RelayCommand<ContentControl>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                
                //param.Content = new HomeUCViewModel();
                global.CurrentPageViewModel = new HomeUCViewModel();
                global.HomeColor = global.ThemeColor;
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
                    global.DetailColor = global.ThemeColor;
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
            global.HomeColor =  Brushes.White.ToString();
            global.DetailColor = Brushes.White.ToString();
            PlacesColor = Brushes.White.ToString();
            AddMemberColor = Brushes.White.ToString();
            SettingColor = Brushes.White.ToString();
            AboutColor = Brushes.White.ToString();
        }
    }
}
