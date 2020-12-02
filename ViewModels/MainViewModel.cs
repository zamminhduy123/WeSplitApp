﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WeSplitApp.ViewModels
{
    class MainViewModel : BaseViewModel
    {


        #region private variables
        private BaseViewModel _currentPageViewModel = null;

        private String _homeColor = "#2a9df4";
        private String _addPlaceColor = Brushes.White.ToString();
        private String _detailColor = Brushes.White.ToString();
        private String _addMemberColor = Brushes.White.ToString();
        private String _settingColor = Brushes.White.ToString();
        private String _aboutColor = Brushes.White.ToString();
        #endregion

        #region Commands

        public ICommand HomeCommand { get; set; }
        public ICommand DetailCommand { get; set; }
        public ICommand AddPlaceCommand { get; set; }
        public ICommand AddMemberCommand { get; set; }
        public ICommand SettingCommand { get; set; }
        public ICommand AboutCommand { get; set; }

        #endregion

        #region PanelColor

        public String HomeColor { get => _homeColor; set {_homeColor = value; OnPropertyChanged(); } }
        public String DetailColor { get => _detailColor; set { _detailColor = value; OnPropertyChanged(); } }

        public String AddPlaceColor { get => _addPlaceColor; set { _addPlaceColor = value; OnPropertyChanged(); } }
        public String AddMemberColor { get => _addMemberColor; set { _addMemberColor = value; OnPropertyChanged(); } }
        public String SettingColor { get => _settingColor; set { _settingColor = value; OnPropertyChanged(); } }
        public String AboutColor { get => _aboutColor; set { _aboutColor = value; OnPropertyChanged(); } }

        public BaseViewModel CurrentPageViewModel { get => _currentPageViewModel; set { _currentPageViewModel = value; OnPropertyChanged(); } }

        #endregion
        public MainViewModel()
        {
            HomeCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                HomeColor = "#2a9df4";
            });
            DetailCommand = new RelayCommand<ContentControl>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                DetailColor = "#2a9df4";
                param.Content = new HomeUCViewModel();
                CurrentPageViewModel = new HomeUCViewModel();
            });
            AddMemberCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                AddMemberColor = "#2a9df4";
            });
            AddPlaceCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                AddPlaceColor = "#2a9df4";
            });
            SettingCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                SettingColor = "#2a9df4";
            });
            AboutCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                ResetAllPanelColor();
                AboutColor = "#2a9df4";
            });

        }
        
        private void ResetAllPanelColor()
        {
            HomeColor = Brushes.White.ToString();
            DetailColor = Brushes.White.ToString();
            AddPlaceColor = Brushes.White.ToString();
            AddMemberColor = Brushes.White.ToString();
            SettingColor = Brushes.White.ToString();
            AboutColor = Brushes.White.ToString();
        }
    }
}
