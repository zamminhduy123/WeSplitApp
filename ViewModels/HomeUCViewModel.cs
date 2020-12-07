using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeSplitApp.Model;
using WeSplitApp.Models;

namespace WeSplitApp.ViewModels
{
    class HomeUCViewModel : BaseViewModel
    {
        //Properties
        private AsyncObservableCollection<dynamic> _currentTripList;
        public AsyncObservableCollection<dynamic> CurrentTripList { get => _currentTripList; set { _currentTripList = value; OnPropertyChanged(); } }

        private AsyncObservableCollection<dynamic> _lastTripList;
        public AsyncObservableCollection<dynamic> LastTripList { get => _lastTripList; set { _lastTripList = value; OnPropertyChanged(); } }

        private dynamic _selectedTrip;
        public dynamic SelectedTrip { get => _selectedTrip; set { _selectedTrip = value;
                var config = ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);
                config.AppSettings.Settings["DetailTripId"].Value = (SelectedTrip.Id).ToString();
                config.Save(ConfigurationSaveMode.Minimal);

                ConfigurationManager.RefreshSection("appSettings");
                OnPropertyChanged(); } 
        }

        //Command
        public ICommand CloseWindowCommand { get; set; }
        public ICommand SelectTripCommand { get; set; }

        public HomeUCViewModel()
        {
            CurrentTripList = new AsyncObservableCollection<dynamic>();
            foreach (var trip in DataProvider.Ins.DB.Journeys.ToList())
            {
                if (CompareDateWithPresent(trip.Arrival) == 1)
                {
                    dynamic tmp = new {
                        Id = trip.Id,
                        Name = trip.Name,
                        Description = trip.Description,
                        State = (CompareDateWithPresent(trip.Departure) == -1) ? "Đang đi" : "Kế hoạch",
                    };
                    CurrentTripList.Add(tmp);
                }
            }

            LastTripList = new AsyncObservableCollection<dynamic>();
            foreach (var trip in DataProvider.Ins.DB.Journeys.ToList())
            {
                if (CompareDateWithPresent(trip.Arrival) < 1)
                {
                    dynamic tmp = new
                    {
                        Id = trip.Id,
                        Name = trip.Name,
                        Description = trip.Description,
                    };
                    LastTripList.Add(tmp);
                }
            }

            SelectTripCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
              Global.GetInstance().CurrentPageViewModel = new DetailUCViewModel();
            });
        }


        public int CompareDateWithPresent(DateTime? x)// return 1/0/-1 if x >/=/< NOW
        {
            if (x == null) return 1;
            int result = 0;
            DateTime y = DateTime.Now;
            if (x.Value.Year > y.Year)
            {
                result = 1;
            }
            else if (x.Value.Year < y.Year)
            {
                result = -1;
            }
            else
            {
                if (x.Value.DayOfYear > y.DayOfYear)
                {
                    result = 1;
                }
                else if (x.Value.DayOfYear < y.DayOfYear)
                {
                    result = -1;
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }
    }
}
