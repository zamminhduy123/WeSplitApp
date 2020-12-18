using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WeSplitApp.Model;
using WeSplitApp.Models;

namespace WeSplitApp.ViewModels
{
    class HomeUCViewModel : BaseViewModel
    {
        #region Properties
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

        private bool _isPlaceSelected;
        public bool IsPlaceSelected
        {
            get => _isPlaceSelected;
            set
            {
                _isPlaceSelected = value;
                if (_isPlaceSelected == true && IsMemberSelected == true)
                {
                    IsMemberSelected = false;
                }
                else if (_isPlaceSelected == false && IsMemberSelected == false)
                {
                    IsMemberSelected = true;
                }
                CallSearch();
                OnPropertyChanged();
            }
        }

        private bool _isMemberSelected;
        public bool IsMemberSelected
        {
            get => _isMemberSelected;
            set
            {
                _isMemberSelected = value;
                if (_isMemberSelected == true && IsPlaceSelected == true)
                {
                    IsPlaceSelected = false;
                }
                else if (_isMemberSelected == false && IsPlaceSelected == false)
                {
                    IsPlaceSelected = true;
                }
                CallSearch();
                OnPropertyChanged();
            }
        }
        private string _search;
        public string Search
        {
            get => _search;
            set
            {
                _search = value;
                CallSearch();
                OnPropertyChanged();
            }
        }

        // New Journey

        private bool _isOpenAddJourneyDialog;
        public bool IsOpenAddJourneyDialog 
        { 
            get => _isOpenAddJourneyDialog; 
            set 
            { 
                _isOpenAddJourneyDialog = value;
                JourneyName = null;
                SelectedLocation = null;
                JourneyDescription = null;
                OnPropertyChanged();
            }
        }

        private string _journeyName;
        public string JourneyName 
        { 
            get => _journeyName; 
            set
            { 
                _journeyName = value;
                if (SelectedLocation != null && JourneyName != null)
                {
                    IsEnabledAddJourneyButton = true;
                }
                IsNameValid = true;
                OnPropertyChanged(); 
            } 
        }

        private bool _isNameValid;
        public bool IsNameValid { get => _isNameValid; set { _isNameValid = value; OnPropertyChanged(); } }

        private bool _isEnabledAddJourneyButton;
        public bool IsEnabledAddJourneyButton { get => _isEnabledAddJourneyButton; set { _isEnabledAddJourneyButton = value; OnPropertyChanged(); } }


        //
        private AsyncObservableCollection<Location> _locationsList;
        public AsyncObservableCollection<Location> LocationsList { get => _locationsList; set { _locationsList = value; OnPropertyChanged(); } }

        private Location _selectedLocation;
        public Location SelectedLocation 
        { 
            get => _selectedLocation; 
            set 
            { 
                _selectedLocation = value;
                if (SelectedLocation != null && JourneyName != null && IsNameValid == true)
                {
                    IsEnabledAddJourneyButton = true;
                }
                OnPropertyChanged();
            } 
        }

        private string _journeyDescription;
        public string JourneyDescription { get => _journeyDescription; set { _journeyDescription = value; OnPropertyChanged(); } }

        #endregion
        #region Command
        public ICommand CloseWindowCommand { get; set; }
        public ICommand SelectTripCommand { get; set; }
        public ICommand AddJourneyCommand { get; set; }
        public ICommand DeleteJourneyCommand { get; set; }
        public ICommand DisableName { get; set; }
        #endregion
        public HomeUCViewModel()
        {
            IsOpenAddJourneyDialog = false;
            IsPlaceSelected = true;
            IsMemberSelected = false;
            LoadTripList();

            LocationsList = new AsyncObservableCollection<Location>();
            foreach (var location in DataProvider.Ins.DB.Locations)
            {
                LocationsList.Add(location);
            }

            SelectTripCommand = new RelayCommand<object>((param) => { return true; }, (param) =>
            {
                Global global = Global.GetInstance();

                global.CurrentPageViewModel = new DetailUCViewModel();
                global.HomeColor = Brushes.White.ToString();
                global.DetailColor = global.ThemeColor;
            });

            AddJourneyCommand = new RelayCommand<string>((param) => { return true; }, (param) =>
            {
                if (bool.Parse(param) == true)
                {
                    Journey newJourney = new Journey
                    {
                        Id = (DataProvider.Ins.DB.Journeys == null) ? 1 : DataProvider.Ins.DB.Journeys.Max(x => x.Id),
                        Name = JourneyName,
                        Description = JourneyDescription,
                        LocationId = SelectedLocation.Id,
                    };
                    DataProvider.Ins.DB.Journeys.Add(newJourney);
                    DataProvider.Ins.DB.SaveChanges();
                    if (CompareDateWithPresent(newJourney.Arrival) == 1)
                    {
                        dynamic tmp = new
                        {
                            Id = newJourney.Id,
                            Name = newJourney.Name,
                            Description = newJourney.Description,
                            State = (CompareDateWithPresent(newJourney.Departure) == -1) ? "Đang đi" : "Kế hoạch",
                        };
                        CurrentTripList.Add(tmp);
                    }
                    else
                    {
                        dynamic tmp = new
                        {
                            Id = newJourney.Id,
                            Name = newJourney.Name,
                            Description = newJourney.Description,
                        };
                        LastTripList.Add(tmp);
                    }
                }
                IsOpenAddJourneyDialog = false;
                IsEnabledAddJourneyButton = false;
            });

            DeleteJourneyCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (Global.GetInstance().ConfirmMessageDelete() == true)
                {
                    Journey deleteJourney = DataProvider.Ins.DB.Journeys.Find(param.Id);
                    DataProvider.Ins.DB.Locations.Find(deleteJourney.LocationId).Journeys.Remove(deleteJourney);
                    foreach (var cost in deleteJourney.Costs.ToList())
                    {
                        DataProvider.Ins.DB.Costs.Remove(cost);
                    }
                    foreach (var expense in deleteJourney.Expenses.ToList())
                    {
                        DataProvider.Ins.DB.Expenses.Remove(expense);
                    }
                    foreach (var member in deleteJourney.Members.ToList())
                    {
                        DataProvider.Ins.DB.Members.Find(member.Id).Journeys.Remove(deleteJourney);
                        deleteJourney.Members.Remove(DataProvider.Ins.DB.Members.Find(member.Id));
                    }
                    foreach (var photo in deleteJourney.Photos.ToList())
                    {
                        DataProvider.Ins.DB.Photos.Remove(photo);
                    }
                    foreach (var route in deleteJourney.Routes.ToList())
                    {
                        DataProvider.Ins.DB.Routes.Remove(route);
                    }
                    DataProvider.Ins.DB.Journeys.Remove(deleteJourney);
                    DataProvider.Ins.DB.SaveChanges();
                        CurrentTripList.Remove(param);
                        LastTripList.Remove(param);
                }
            });
            DisableName = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsNameValid = false;
                IsEnabledAddJourneyButton = false;
            });
        }
        public void CallSearch()
        {
            LoadTripList();
            if (Search != null && Search != "")
            {
                LastTripList = SearchJourney(Search, LastTripList, IsPlaceSelected);
                CurrentTripList = SearchJourney(Search, CurrentTripList, IsPlaceSelected);
            }
        }
        public void LoadTripList()
        {
            CurrentTripList = new AsyncObservableCollection<dynamic>();
            foreach (var trip in DataProvider.Ins.DB.Journeys.ToList())
            {
                if (CompareDateWithPresent(trip.Arrival) == 1)
                {
                    dynamic tmp = new
                    {
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

        public AsyncObservableCollection<dynamic> SearchJourney(string inputKeyWord, AsyncObservableCollection<dynamic> inputList, bool mode)//mode = true: search by journey, mode = false: search by member
        {
            AsyncObservableCollection<dynamic> outputList = new AsyncObservableCollection<dynamic>();
            foreach (var item in inputList)
            {
                outputList.Add(item);
            }    
            if (inputKeyWord != "" && inputKeyWord != null)
            {
                List<string> listWordsAND = new List<string>();
                List<string> listWordsOR = new List<string>();
                List<string> listWordsNOT = new List<string>();

                List<string> listWords = new List<string>();
                List<string> listOperators = new List<string>();
                string[] words = inputKeyWord.Trim().ToLower().Split(' ');
                string wordElement = "";
                foreach (string word in words)
                {
                    if (word == "and" || word == "or" || word == "not")
                    {
                        listWords.Add(wordElement);
                        wordElement = "";
                        if (listOperators.Count > 0)
                        {
                            foreach (string item in listWords)
                            {
                                if (listOperators[0] == "and")
                                {
                                    listWordsAND.Add(item);
                                }
                                else if (listOperators[0] == "or")
                                {
                                    listWordsOR.Add(item);
                                }
                                else if (listOperators[0] == "not")
                                {
                                    listWordsNOT.Add(item);
                                }
                            }
                            listWords.RemoveRange(0, listWords.Count);
                            string temp = listOperators[0];
                            listOperators.RemoveAt(0);
                            listOperators.Add(word);
                            listOperators.Add(temp);
                        }
                        else if (word == "not" && listWords.Count > 0)
                        {
                            listWordsOR.Add(listWords[0]);
                            listOperators.Add(word);
                            listWords.RemoveRange(0, listWords.Count);
                        }
                        else
                        {
                            listOperators.Add(word);
                        }
                    }
                    else
                    {
                        if (wordElement == "")
                        {
                            wordElement += word;
                        }
                        else
                        {
                            wordElement += " " + word;
                        }
                    }
                }
                if (wordElement != "")
                {
                    listWords.Add(wordElement);
                    wordElement = "";
                }
                if (listOperators.Count > 0)
                {
                    foreach (string item in listWords)
                    {
                        if (listOperators[0] == "and")
                        {
                            listWordsAND.Add(item);
                        }
                        else if (listOperators[0] == "or")
                        {
                            listWordsOR.Add(item);
                        }
                        else if (listOperators[0] == "not")
                        {
                            listWordsNOT.Add(item);
                        }
                    }
                    listWords.RemoveRange(0, listWords.Count);
                    listOperators.RemoveAt(0);
                }
                else if (listWords.Count > 0)
                {
                    foreach (string item in listWords)
                    {
                        listWordsOR.Add(item);
                    }
                }


                DataTable rawTable = new DataTable();
                rawTable.Columns.Add("Name", typeof(string));
                rawTable.Columns.Add("Order", typeof(int));
                rawTable.Columns.Add("Distance", typeof(int));
                rawTable.Columns.Add("Length", typeof(int));
                rawTable.Columns.Add("JourneyId", typeof(int));
                int ComparedColumn = 0;

                DataTable resultTable = new DataTable();
                resultTable = rawTable.Copy();
                if (mode == true)
                {
                    foreach (dynamic item in inputList)
                    {
                        rawTable.Rows.Add(item.Name, 0, 0, 0, 0);
                    }
                }
                else
                {
                    ComparedColumn = 4;
                    foreach (dynamic item in DataProvider.Ins.DB.Members.ToList())
                    {
                        foreach (var memberJourney in item.Journeys)
                        {
                            rawTable.Rows.Add(item.Name, 0, 0, 0, memberJourney.Id);
                        }
                    }
                    
                }
               


                DataTable notTable = new DataTable();
                if (listWordsNOT.Count > 0)
                {
                    notTable = rawTable.Copy();
                    foreach (string word in listWordsNOT)
                    {
                        notTable = SearchOneWord(word, notTable);
                    }
                }
                


                DataTable andTable = new DataTable();
                if (listWordsAND.Count > 0)
                {
                    andTable = rawTable.Copy();
                    foreach (string word in listWordsAND)
                    {
                        andTable = SearchOneWord(word, andTable);
                    }
                    foreach (DataRow rowData in andTable.Rows)
                    {
                        resultTable.Rows.Add(rowData.ItemArray);
                    }
                }

               
                

                foreach (string word in listWordsOR)
                {
                    DataTable orTable = SearchOneWord(word, rawTable);
                    
                    foreach (DataRow rowDataOrTable in orTable.Rows)
                    {
                        bool flag = true;
                        foreach (DataRow rowDataResultTable in resultTable.Rows)
                        {
                            if (rowDataOrTable["Name"] == rowDataResultTable["Name"] && rowDataOrTable["JourneyId"] == rowDataResultTable["JourneyId"])
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag == true)
                        {
                            resultTable.Rows.Add(rowDataOrTable.ItemArray);
                        }
                    }
                }
                
                for (int x = 0; x < notTable.Rows.Count; x++)
                {
                    for (int y = 0; y < resultTable.Rows.Count; y++)
                    {
                        if (notTable.Rows[x].Field<string>(0) == resultTable.Rows[y].Field<string>(0) && notTable.Rows[x].Field<int>(4) == resultTable.Rows[y].Field<int>(4))
                        {
                            resultTable.Rows.RemoveAt(y);
                        }
                    }
                }
                
                int tableRange = resultTable.Rows.Count;
                int listRange = inputList.Count;

                DataView DV = resultTable.DefaultView;
                DV.Sort = "Order DESC, Distance ASC, Length ASC";
                

                
                //create output
                outputList.Clear();
                for (int tableIndex = 0; tableIndex < tableRange; tableIndex++)
                {

                    for (int listIndex = 0; listIndex < listRange; listIndex++)
                    {
                        if (mode == true)
                        {
                            if (resultTable.Rows[tableIndex].Field<string>(ComparedColumn) == inputList[listIndex].Name)
                            {
                                outputList.Add(inputList[listIndex]);
                                break;
                            }
                        }
                        else
                        {
                            if (resultTable.Rows[tableIndex].Field<int>(ComparedColumn) == inputList[listIndex].Id && outputList.Contains(inputList[listIndex]) == false)
                            {
                                outputList.Add(inputList[listIndex]);
                                break;
                            }
                        }
                    }
                }
            }
            return outputList;
        }

        public DataTable SearchOneWord(string word, DataTable inputTable)
        {
            DataTable resultTable = new DataTable();
            resultTable.Columns.Add("Name", typeof(string));
            resultTable.Columns.Add("Order", typeof(int));
            resultTable.Columns.Add("Distance", typeof(int));
            resultTable.Columns.Add("Length", typeof(int));
            resultTable.Columns.Add("JourneyId", typeof(int));

            int tableRange = inputTable.Rows.Count;
            for (int tableIndex = 0; tableIndex < tableRange; tableIndex++)
            {
                string Name = inputTable.Rows[tableIndex].Field<string>(0);
                int Id = inputTable.Rows[tableIndex].Field<int>(4);

                int order = -1;
                int Distance = 0;
                int prevWordIndex = 0;
                int signedWordIndex = Name.IndexOf(word, StringComparison.OrdinalIgnoreCase);
                int unsignedWordIndex = RemoveSign(Name).IndexOf(RemoveSign(word), StringComparison.OrdinalIgnoreCase);


                if (signedWordIndex >= 0)
                {
                    if (IsSeparateWord(word, Name, signedWordIndex) == true)
                    {
                        order = word.Length + 2;
                    }
                    else if (IsBeginOfWord(word, Name, signedWordIndex) == true)
                    {
                        order = word.Length - 1;
                    }
                    else
                    {
                        continue;
                    }

                    if (signedWordIndex >= prevWordIndex)
                    {
                        Distance += signedWordIndex - prevWordIndex;
                    }
                    else
                    {
                        Distance += Name.Length;
                    }
                    prevWordIndex = signedWordIndex;
                }
                else if (unsignedWordIndex >= 0)
                {
                    if (IsSeparateWord(word, Name, unsignedWordIndex) == true)
                    {
                        order = word.Length + 1;
                    }
                    else if (IsBeginOfWord(word, Name, unsignedWordIndex) == true)
                    {
                        order = word.Length - 2;
                    }
                    else
                    {
                        continue;
                    }
                    if (unsignedWordIndex >= prevWordIndex)
                    {
                        Distance += unsignedWordIndex - prevWordIndex;
                    }
                    else
                    {
                        Distance += Name.Length;
                    }
                    prevWordIndex = unsignedWordIndex;
                }

                if (order > -1)
                {
                    resultTable.Rows.Add(Name, order, Distance, Name.Length, Id);
                }

            }
            return resultTable;
        }
        public static string RemoveSign(string inputString)
        {
            string[] VietNamChar = new string[]
            {
                "aAeEoOuUiIdDyY",
                "áàạảãâấầậẩẫăắằặẳẵ",
                "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                "éèẹẻẽêếềệểễ",
                "ÉÈẸẺẼÊẾỀỆỂỄ",
                "óòọỏõôốồộổỗơớờợởỡ",
                "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                "úùụủũưứừựửữ",
                "ÚÙỤỦŨƯỨỪỰỬỮ",
                "íìịỉĩ",
                "ÍÌỊỈĨ",
                "đ",
                "Đ",
                "ýỳỵỷỹ",
                "ÝỲỴỶỸ"
            };

            for (int i = 1; i < VietNamChar.Length; i++)
            {
                for (int j = 0; j < VietNamChar[i].Length; j++)
                    inputString = inputString.Replace(VietNamChar[i][j], VietNamChar[0][i - 1]);
            }
            return inputString;
        }

        private static bool IsSeparateWord(string word, string sentence, int index)
        {
            bool result = true;
            if (index > 0 && sentence[index - 1] != ' ')
            {
                result = false;
            }
            if (index + word.Length < sentence.Length && sentence[index + word.Length] != ' ')
            {
                result = false;
            }
            return result;
        }

        public static bool IsBeginOfWord(string word, string sentence, int index)
        {
            bool result = true;
            if (index > 0 && sentence[index - 1] != ' ')
            {
                result = false;
            }
            return result;
        }
    }
}
