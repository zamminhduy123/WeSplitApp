using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media.Imaging;
using WeSplitApp.Model;
using WeSplitApp.Models;

namespace WeSplitApp.ViewModels
{
    public class DetailUCViewModel : BaseViewModel
    {
        //Properties
        private Journey _detailJourney;
        public Journey DetailJourney
        {
            get => _detailJourney; set
            {
                _detailJourney = value;

                // get member list who don't paticipate journey
                MemberList = new AsyncObservableCollection<Member>();
                foreach (var member in DataProvider.Ins.DB.Members)
                {
                    MemberList.Add(member);
                }
                foreach (var member in DetailJourney.Members)
                {
                    MemberList.Remove(member);
                }

                OnPropertyChanged();
            }
        }

        private AsyncObservableCollection<dynamic> _detailRouteList;
        public AsyncObservableCollection<dynamic> DetailRouteList { get => _detailRouteList; set { _detailRouteList = value; OnPropertyChanged(); } }

        private AsyncObservableCollection<dynamic> _detailMemberList;
        public AsyncObservableCollection<dynamic> DetailMemberList { get => _detailMemberList; set { _detailMemberList = value; OnPropertyChanged(); } }

        private AsyncObservableCollection<Member> _memberList;
        public AsyncObservableCollection<Member> MemberList { get => _memberList; set { _memberList = value; OnPropertyChanged(); } }

        private Member _selectedMember;
        public Member SelectedMember
        {
            get => _selectedMember;
            set
            {
                _selectedMember = value;
                if (SelectedMember != null)
                {
                    IsEnabledAddParticipantButton = true;
                }
                OnPropertyChanged();
            }
        }

        private string _startDate;
        public string StartDate
        {
            get => _startDate; set
            {
                _startDate = value;
                if (StartDate != null && StartDate != "")
                {
                    DataProvider.Ins.DB.Journeys.Find(DetailJourney.Id).Departure = Convert.ToDateTime(StartDate);
                    DataProvider.Ins.DB.SaveChanges();
                }
                OnPropertyChanged();
            }
        }

        private string _endDate;
        public string EndDate
        {
            get => _endDate; set
            {
                _endDate = value;
                if (EndDate != null && EndDate != "")
                {
                    DataProvider.Ins.DB.Journeys.Find(DetailJourney.Id).Arrival = Convert.ToDateTime(EndDate);
                    DataProvider.Ins.DB.SaveChanges();
                }
                OnPropertyChanged();
            }
        }

        private string _routeName;
        public string RouteName
        {
            get => _routeName;
            set
            {
                _routeName = value;
                if (RouteName != "" && RouteName != null)
                {
                    IsEnabledAddOrEditRouteButton = true;
                }
                OnPropertyChanged();
            }
        }

        private string _routeDescription;
        public string RouteDescription
        {
            get => _routeDescription;
            set
            {
                _routeDescription = value;
                OnPropertyChanged();
            }
        }

        private string _addOrUpdateRouteContent;
        public string AddOrUpdateRouteContent { get => _addOrUpdateRouteContent; set { _addOrUpdateRouteContent = value; OnPropertyChanged(); } }

        private dynamic _selectedRoute;
        public dynamic SelectedRoute
        {
            get => _selectedRoute;
            set
            {
                _selectedRoute = value;
                if (SelectedRoute == null)
                {
                    RouteName = "";
                    RouteDescription = "";
                    AddOrUpdateRouteContent = "THÊM";
                }
                else
                {
                    RouteName = SelectedRoute.Name;
                    RouteDescription = SelectedRoute.Description;
                    AddOrUpdateRouteContent = "CẬP NHẬT";
                }
                OnPropertyChanged();
            }
        }

        private dynamic _selectedTab;
        public dynamic SelectedTab
        {
            get => _selectedTab;
            set
            {
                _selectedTab = value;
                IsEnabledAddOrEditRouteButton = false;
                if (SelectedTab.Header == "Thời gian")
                {
                    NoImageVisibility = (DetailJourney.Photos.Count == 0) ? Visibility.Hidden : Visibility.Visible;

                    // get _imageList
                    _imageList = new List<dynamic>();
                    foreach (var photo in DetailJourney.Photos)
                    {
                        dynamic tmp = new {
                            Id = photo.OrderNumber,
                            ImageBytes = photo.ImageBytes,
                        };
                        _imageList.Add(tmp);
                    }

                    // get ImageHolder
                    _imageHolderNumber = (_imageList.Count == 0) ? 0 : _imageList[0].Id;
                    ImageHolder = (_imageList.Count == 0) ? System.Text.Encoding.Default.GetBytes(Global.GetInstance().NoImageStringSource) : _imageList[0].ImageBytes;

                    // get Start date
                    StartDate = DetailJourney.Departure.ToString().Split(' ').First();

                    // get end date
                    EndDate = DetailJourney.Arrival.ToString().Split(' ').First();

                }
                if (SelectedTab.Header == "Thành viên")
                {
                    //reset các biến kiểm tra dữ liệu nhập vào
                    SelectedMember = null;
                    IsEnabledAddParticipantButton = false;
                    // get Members List
                    DetailMemberList = new AsyncObservableCollection<dynamic>();
                    foreach (var member in DetailJourney.Members)
                    {
                        dynamic tmp = new
                        {
                            Id = member.Id,
                            Name = member.Name,
                            Phone = member.Phone,
                        };
                        DetailMemberList.Add(member);
                    }

                }
                if (SelectedTab.Header == "Lộ trình")
                {
                    //reset các biến kiểm tra dữ liệu nhập vào
                    RouteName = null;
                    RouteDescription = null;
                    IsEnabledAddOrEditRouteButton = false;
                    // get Routes list
                    DetailRouteList = new AsyncObservableCollection<dynamic>();
                    foreach (var route in DetailJourney.Routes.OrderBy(t => t.OrderNumber))
                    {
                        dynamic tmp = new
                        {
                            Id = route.OrderNumber,
                            Name = route.Name,
                            Description = route.Description,
                        };
                        DetailRouteList.Add(tmp);
                    }

                }
                if (SelectedTab.Header == "Thu chi")
                {
                    // reset lại các biến kiểm tra dữ liệu nhập vào
                    SelectedInFeeMember = null;
                    InFee = null;
                    OutFeeContent = null;
                    OutFee = null;
                    IsEnabledAddInFeeButton = false;
                    IsEnabledAddOutFeeButton = false;
                    // đọc danh sách thành viên tham gia chuyến đi
                    PaticipantsList = new AsyncObservableCollection<Member>();
                    foreach (var member in DetailJourney.Members)
                    {
                        PaticipantsList.Add(member);
                    }

                    // đọc danh sách các khoản thu
                    InFeesList = new AsyncObservableCollection<dynamic>();
                    foreach (var fee in DetailJourney.Expenses)
                    {
                        dynamic tmp = new
                        {
                            Id = fee.OrderNumber,
                            MemberId = fee.MemberId,
                            Name = fee.Member.Name,
                            Fees = ((fee.Fees == null) ? 0 : fee.Fees.Value).ToString(),
                        };
                        InFeesList.Add(tmp);
                    }

                    // đọc danh sách các khoản chi
                    OutFeesList = new AsyncObservableCollection<dynamic>();
                    foreach (var fee in DetailJourney.Costs)
                    {
                        dynamic tmp = new
                        {
                            Id = fee.OrderNumber,
                            Name = fee.Content,
                            Fees = ((fee.Fees == null) ? 0 : fee.Fees.Value).ToString(),
                        };
                        OutFeesList.Add(tmp);
                    }
                }
                if (SelectedTab.Header == "Biểu đồ")
                {
                    makePieChart(); 
                    makeCartestianChart();

                }
                if (SelectedTab.Header == "Tổng kết")
                {
                    List<int> memberId = new List<int>();
                    List<int> inFees = new List<int>();
                    int totalInFee = 0;
                    int totalOutFee = 0;
                    SummaryList = new List<dynamic>();

                    foreach (var member in DetailJourney.Members)
                    {
                        memberId.Add(member.Id);
                        inFees.Add(0);
                    }
                    foreach (var fee in DetailJourney.Expenses)
                    {
                        inFees[memberId.IndexOf(fee.MemberId)] += (fee.Fees == null) ? 0 : fee.Fees.Value;
                        totalInFee += (fee.Fees == null) ? 0 : fee.Fees.Value;
                    }
                    foreach (var fee in DetailJourney.Costs)
                    {
                        totalOutFee += (fee.Fees == null) ? 0 : fee.Fees.Value;
                    }
                    TotalInFee = totalInFee.ToString();
                    TotalOutFee = totalOutFee.ToString();
                    TotalFee = (totalInFee - totalOutFee).ToString();

                    for (int i = 0; i < memberId.Count(); i++)
                    {
                        SummaryList.Add(new
                        {
                            Name = DataProvider.Ins.DB.Members.Find(memberId[i]).Name,
                            Quantity = (inFees[i] - totalOutFee / memberId.Count).ToString(),
                        });
                    }
                }
                OnPropertyChanged();
            }
        }
        // Ảnh

        private int _imageHolderNumber;

        private byte[] _imageHolder;
        public byte[] ImageHolder { get => _imageHolder; set { _imageHolder = value; OnPropertyChanged(); } }

        private List<dynamic> _imageList;

        // Thu chi
        //danh sách thành viên tham gia chuyến đi
        private AsyncObservableCollection<Member> _paticipantsList;
        public AsyncObservableCollection<Member> PaticipantsList { get => _paticipantsList; set { _paticipantsList = value; OnPropertyChanged(); } }

        // thành viên đc chọn để thu tiền
        private Member _selectedInFeeMember;
        public Member SelectedInFeeMember
        {
            get => _selectedInFeeMember;
            set
            {
                _selectedInFeeMember = value;
                if (SelectedInFeeMember != null
                    && InFee != null
                    && IsInFeeValid == true)
                {
                    IsEnabledAddInFeeButton = true;
                }
                OnPropertyChanged();
            }
        }

        // Danh sách thành viên đã đóng tiền và số tiền đã đóng
        private AsyncObservableCollection<dynamic> _inFeesList;
        public AsyncObservableCollection<dynamic> InFeesList { get => _inFeesList; set { _inFeesList = value; OnPropertyChanged(); } }

        // Số tiền thu vào
        private string _inFee;
        public string InFee
        {
            get => _inFee;
            set
            {
                _inFee = value;
                if (SelectedInFeeMember != null && InFee != null)
                {
                    IsEnabledAddInFeeButton = true;
                }
                IsInFeeValid = true;
                OnPropertyChanged();
            }
        }

        // danh sách khoản chi
        private AsyncObservableCollection<dynamic> _outFeesList;
        public AsyncObservableCollection<dynamic> OutFeesList { get => _outFeesList; set { _outFeesList = value; OnPropertyChanged(); } }

        // Tên khoản chi mới
        private string _outFeeContent;
        public string OutFeeContent
        {
            get => _outFeeContent;
            set
            {
                _outFeeContent = value;
                if (OutFeeContent != null && OutFee != null && IsOutFeeValid == true)
                {
                    IsEnabledAddOutFeeButton = true;
                }
                IsOutFeeContentValid = true;
                OnPropertyChanged();
            }
        }

        // Số tiền chi ra
        private string _outFee;
        public string OutFee
        {
            get => _outFee;
            set
            {
                _outFee = value;
                if (OutFeeContent != null && OutFee != null && IsOutFeeContentValid == true)
                {
                    IsEnabledAddOutFeeButton = true;
                }
                IsOutFeeValid = true;
                OnPropertyChanged();
            }
        }

        // Khoản chi đã chọn dưới danh sách
        private dynamic _selectedOutFee;
        public dynamic SelectedOutFee
        {
            get => _selectedOutFee; set
            {
                _selectedOutFee = value;
                OutFeeContent = (SelectedOutFee == null) ? null : SelectedOutFee.Name;
                OnPropertyChanged();
            }
        }

        private SeriesCollection _pieChartSeriesCollection = new SeriesCollection();
        public SeriesCollection PieChartSeriesCollection { get => _pieChartSeriesCollection; set { _pieChartSeriesCollection = value; OnPropertyChanged(); } }

        public Func<ChartPoint,string> PointLabel { get; set; }

        // take data and tranfer to pie chart
        void makePieChart()
        {
            if (PieChartSeriesCollection != null)
                PieChartSeriesCollection.Clear();
            int totalCost = 0;
            foreach (var fee in DetailJourney.Costs)
            {
                totalCost += (fee.Fees == null) ? 0 : fee.Fees.Value;
            }
            foreach (var fee in DetailJourney.Costs)
            {
                dynamic tmp = new
                {
                    Name = fee.Content,
                    Fees = (fee.Fees == null) ? 0 : fee.Fees.Value,
                };
                double x = (double)(tmp.Fees) / totalCost * 100;
                double percent = Math.Truncate(x * 100) / 100;
                PointLabel = chartPoint => (chartPoint.Participation > 0.05) ? string.Format("{0:P}", chartPoint.Participation) : null;
                PieChartSeriesCollection.Add(new PieSeries { Title = $"{tmp.Name}", Values = new ChartValues<int> { tmp.Fees }, DataLabels = true, LabelPoint = PointLabel});
            }
        }

        //Biểu đồ cột - Thu
        private List<string> _labels = new List<string>();
        public List<string> Labels { get => _labels; set { _labels = value; OnPropertyChanged(); } }
        public Func<double, string> Formatter { get; set; }

        private SeriesCollection _cartesianChartCollection = new SeriesCollection();
        public SeriesCollection CartesianChartCollection { get => _cartesianChartCollection; set { _cartesianChartCollection = value; OnPropertyChanged(); } }


        void makeCartestianChart()
        {
            if (CartesianChartCollection != null)
                CartesianChartCollection.Clear();
            ChartValues
                <Double> fees = new ChartValues<Double>();
            List<String> name = new List<String>();
            List<dynamic> counts = new List<dynamic>();
            foreach (var member in DetailJourney.Members)
            {
                counts.Add(new { 
                    Id = member.Id,
                    Name = member.Name,
                    Fee = 0,
                });
            }

            foreach (var fee in DetailJourney.Expenses)
            {
                dynamic member = counts.First(x => x.Id == fee.MemberId);
                dynamic newmember = new {
                    Id = member.Id,
                    Name = member.Name,
                    Fee = member.Fee + ((fee.Fees == null) ? 0 : fee.Fees.Value),
                };
                counts.Remove(member);
                counts.Add(newmember);
            }

            foreach (var count in counts)
            {
                name.Add(count.Name);
                fees.Add(count.Fee);
            }
            CartesianChartCollection = new SeriesCollection
                {
                    new ColumnSeries
                    {
                        Title = "Payment",
                        Values = fees
                    }
                };
            // collumn name
            Labels = name.ToList();
            Formatter = value => value.ToString("N");
        }

        // Tổng kết
            //Tổng tiền thu
        private string _totalInFee;
        public string TotalInFee { get => _totalInFee; set { _totalInFee = value; OnPropertyChanged(); } }

        //Tổng tiền chi
        private string _totalOutFee;
        public string TotalOutFee { get => _totalOutFee; set { _totalOutFee = value; OnPropertyChanged(); } }

        //Tổng tiền còn lại Quỹ
        private string _totalFee;
        public string TotalFee { get => _totalFee; set { _totalFee = value; OnPropertyChanged(); } }

        //Danh sách thành viên và khoản tiền còn lại
        private List<dynamic> _summaryList = new List<dynamic>();
        public List<dynamic> SummaryList { get => _summaryList; set { _summaryList = value; OnPropertyChanged(); } }

        #region EnableButtonCommand
        private Visibility _noImageVisibility; // hiden nếu ko có ảnh
        public Visibility NoImageVisibility { get => _noImageVisibility; set { _noImageVisibility = value; OnPropertyChanged(); } }
        private bool _isEnabledAddOrEditRouteButton;
        public bool IsEnabledAddOrEditRouteButton { get => _isEnabledAddOrEditRouteButton; set { _isEnabledAddOrEditRouteButton = value; OnPropertyChanged(); } }
        private bool _isEnabledAddParticipantButton;
        public bool IsEnabledAddParticipantButton { get => _isEnabledAddParticipantButton; set { _isEnabledAddParticipantButton = value; OnPropertyChanged(); } }
        private bool _isEnabledAddInFeeButton;
        public bool IsEnabledAddInFeeButton { get => _isEnabledAddInFeeButton; set { _isEnabledAddInFeeButton = value; OnPropertyChanged(); } }
        private bool _isEnabledAddOutFeeButton;
        public bool IsEnabledAddOutFeeButton { get => _isEnabledAddOutFeeButton; set { _isEnabledAddOutFeeButton = value; OnPropertyChanged(); } }
        #endregion

        #region IsValidValue
        private bool _isInFeeValid;
        public bool IsInFeeValid { get => _isInFeeValid; set { _isInFeeValid = value; OnPropertyChanged(); } }
        private bool _isOutFeeContentValid;
        public bool IsOutFeeContentValid { get => _isOutFeeContentValid; set { _isOutFeeContentValid = value; OnPropertyChanged(); } }
        private bool _isOutFeeValid;
        public bool IsOutFeeValid { get => _isOutFeeValid; set { _isOutFeeValid = value; OnPropertyChanged(); } }
        #endregion

        #region Command
        public ICommand CloseWindowCommand { get; set; }
        public ICommand DeleteParticipantCommand { get; set; }
        public ICommand AddParticipantCommand { get; set; }
        public ICommand AddOrEditRouteCommand { get; set; }
        public ICommand DeleteRouteCommand { get; set; }
        public ICommand AddInFeeCommand { get; set; }
        public ICommand DeleteInFeeCommand { get; set; }
        public ICommand AddOutFeeCommand { get; set; }
        public ICommand DeleteOutFeeCommand { get; set; }
        public ICommand DisableAddOrEditRouteButton { get; set; }
        public ICommand DisableAddInFeeButton { get; set; }
        public ICommand DisableAddOutFee { get; set; }
        public ICommand DisableAddOutFeeContent { get; set; }
        public ICommand PrevImageCommand { get; set; }
        public ICommand NextImageCommand { get; set; }
        public ICommand AddImageCommand { get; set; }
        public ICommand DeleteImageCommand { get; set; }

        #endregion
        public DetailUCViewModel()
        {
            AddOrUpdateRouteContent = "THÊM";
            var value = ConfigurationManager.AppSettings["DetailTripId"];
            int DetailTripId = int.Parse(value);
            int JourneyId = DetailTripId;
            DetailJourney = DataProvider.Ins.DB.Journeys.Where(x => x.Id == JourneyId).FirstOrDefault();

            DeleteParticipantCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (Global.GetInstance().ConfirmMessageDelete() == true)
                {
                    Member deleteMember = DataProvider.Ins.DB.Members.Find(param.Id);
                    foreach (var expense in DataProvider.Ins.DB.Expenses.Where(x => x.JourneyId == DetailJourney.Id && x.MemberId == deleteMember.Id))
                    {
                        DataProvider.Ins.DB.Expenses.Remove(expense);
                    }
                    DataProvider.Ins.DB.Journeys.Find(DetailJourney.Id).Members.Remove(deleteMember);
                    DataProvider.Ins.DB.Members.Find(deleteMember.Id).Journeys.Remove(DetailJourney);
                    DataProvider.Ins.DB.SaveChanges();
                    MemberList.Add(deleteMember);
                    DetailMemberList.Remove(param);
                }
            });

            AddParticipantCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (SelectedMember == null)
                {
                    MessageBox.Show("Please select a member to add to this journey");
                    return;
                }
                dynamic tmp = new
                {
                    Id = SelectedMember.Id,
                    Name = SelectedMember.Name,
                    Phone = SelectedMember.Phone,
                };
                DetailMemberList.Add(tmp);
                DataProvider.Ins.DB.Journeys.Find(DetailJourney.Id).Members.Add(SelectedMember);
                DataProvider.Ins.DB.Members.Find(SelectedMember.Id).Journeys.Add(DetailJourney);
                MemberList.Remove(SelectedMember);
                DataProvider.Ins.DB.SaveChanges();
                SelectedMember = null;
                IsEnabledAddParticipantButton = false;
            });

            AddOrEditRouteCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (RouteName == null || RouteName == "")
                {
                    MessageBox.Show("Please enter route name");
                    return;
                }
                if (SelectedRoute == null)
                {
                    DataProvider.Ins.DB.Routes.Add(new Route
                    {
                        JourneyId = DetailJourney.Id,
                        OrderNumber = (DetailJourney.Routes == null) ? 1 : DetailJourney.Routes.Max(x => x.OrderNumber) + 1,
                        Name = RouteName,
                        Description = RouteDescription,
                    });
                    DataProvider.Ins.DB.SaveChanges();
                    dynamic tmp = new
                    {
                        Id = (DetailJourney.Routes == null) ? 1 : DetailJourney.Routes.Max(x => x.OrderNumber),
                        Name = RouteName,
                        Description = RouteDescription,
                    };
                    DetailRouteList.Add(tmp);
                }
                else // update exist route
                {
                    int id = SelectedRoute.Id;
                    Route tmp = DataProvider.Ins.DB.Routes.Find(DetailJourney.Id, id);
                    tmp.Name = RouteName;
                    tmp.Description = RouteDescription;
                    DataProvider.Ins.DB.SaveChanges();
                    DetailRouteList = new AsyncObservableCollection<dynamic>();
                    foreach (var route in DetailJourney.Routes.OrderBy(t => t.OrderNumber))
                    {
                        dynamic tmp2 = new
                        {
                            Id = route.OrderNumber,
                            Name = route.Name,
                            Description = route.Description,
                        };
                        DetailRouteList.Add(tmp2);
                    }
                }
                SelectedRoute = null;
                RouteName = null;
                RouteDescription = null;
                IsEnabledAddOrEditRouteButton = false;
            });

            DeleteRouteCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (Global.GetInstance().ConfirmMessageDelete() == true)
                {
                    int id = param.Id;
                    Route deleteroute = DataProvider.Ins.DB.Routes.Find(DetailJourney.Id, id);
                    DataProvider.Ins.DB.Routes.Remove(deleteroute);
                    DataProvider.Ins.DB.SaveChanges();

                    DetailRouteList.Remove(param);
                }
            });

            AddInFeeCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (SelectedInFeeMember == null)
                {
                    MessageBox.Show("Hãy chọn thành viên trước");
                    return;
                }
                if (InFee == null || InFee == "")
                {
                    MessageBox.Show("Hãy nhập số tiền");
                    return;
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(InFee, "[^0-9]"))
                {
                    MessageBox.Show("Số tiền chỉ có thể nhập số");
                    InFee = null;
                    return;
                }
                Expense newexpense = new Expense
                {
                    JourneyId = DetailJourney.Id,
                    MemberId = SelectedInFeeMember.Id,
                    OrderNumber = (DataProvider.Ins.DB.Expenses == null) ? 1 : DataProvider.Ins.DB.Expenses.Max(x => x.OrderNumber) + 1,
                    Fees = int.Parse(InFee),
                };
                DataProvider.Ins.DB.Expenses.Add(newexpense);
                DataProvider.Ins.DB.SaveChanges();
                dynamic tmp = new
                {
                    Id = newexpense.OrderNumber,
                    MemberId = newexpense.MemberId,
                    Name = newexpense.Member.Name,
                    Fees = newexpense.Fees.Value.ToString(),
                };
                InFeesList.Add(tmp);

                SelectedInFeeMember = null;
                InFee = null;
                IsEnabledAddInFeeButton = false;
            });

            DeleteInFeeCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (Global.GetInstance().ConfirmMessageDelete() == true)
                {
                    Expense deleteExpense = DataProvider.Ins.DB.Expenses.Find(DetailJourney.Id, param.Id, param.MemberId);
                    DataProvider.Ins.DB.Expenses.Remove(deleteExpense);
                    DataProvider.Ins.DB.SaveChanges();
                    InFeesList.Remove(param);
                }
            });

            AddOutFeeCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (OutFeeContent == null || OutFeeContent == "")
                {
                    MessageBox.Show("Hãy nhập khoản chi trước");
                    OutFeeContent = null;
                    return;
                }
                if (OutFee == null || OutFee == "")
                {
                    MessageBox.Show("Hãy nhập số tiền");
                    return;
                }
                if (System.Text.RegularExpressions.Regex.IsMatch(OutFee, "[^0-9]"))
                {
                    MessageBox.Show("Số tiền chỉ có thể nhập số");
                    OutFee = null;
                    return;
                }
                if (DetailJourney.Costs.Where(x => x.Content.ToUpper() == OutFeeContent.ToUpper()).Count() != 0)
                {
                    int id = DetailJourney.Costs.First(x => x.Content.ToUpper() == OutFeeContent.ToUpper()).OrderNumber;
                    Cost tmpCost = DataProvider.Ins.DB.Costs.Find(DetailJourney.Id, id);
                    tmpCost.Fees = ((tmpCost.Fees == null) ? 0 : tmpCost.Fees.Value) + int.Parse(OutFee);
                    OutFeesList = new AsyncObservableCollection<dynamic>();
                    foreach (var fee in DetailJourney.Costs)
                    {
                        dynamic tmp = new
                        {
                            Id = fee.OrderNumber,
                            Name = fee.Content,
                            Fees = ((fee.Fees == null) ? 0 : fee.Fees.Value).ToString(),
                        };
                        OutFeesList.Add(tmp);
                    }
                }
                else
                {
                    Cost newCost = new Cost
                    {
                        JourneyId = DetailJourney.Id,
                        OrderNumber = (DataProvider.Ins.DB.Costs == null) ? 1 : DataProvider.Ins.DB.Costs.Max(x => x.OrderNumber) + 1,
                        Content = OutFeeContent.Substring(0, 1).ToUpper() + OutFeeContent.Substring(1, OutFeeContent.Length - 1).ToLower(),
                        Fees = int.Parse(OutFee),
                    };
                    DataProvider.Ins.DB.Costs.Add(newCost);
                    dynamic tmp = new
                    {
                        Id = newCost.OrderNumber,
                        Name = newCost.Content,
                        Fees = newCost.Fees.Value.ToString(),
                    };
                    OutFeesList.Add(tmp);
                }
                DataProvider.Ins.DB.SaveChanges();
                OutFeeContent = null;
                OutFee = null;
                IsEnabledAddOutFeeButton = false;
            });

            DeleteOutFeeCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (Global.GetInstance().ConfirmMessageDelete() == true)
                {
                    Cost deleteCost = DetailJourney.Costs.First(x => x.OrderNumber == param.Id);
                    DataProvider.Ins.DB.Costs.Remove(deleteCost);
                    DataProvider.Ins.DB.SaveChanges();
                    OutFeesList.Remove(param);
                }
            });

            DisableAddOrEditRouteButton = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsEnabledAddOrEditRouteButton = false;
            });
            DisableAddInFeeButton = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsInFeeValid = false;
                IsEnabledAddInFeeButton = false;
            });
            DisableAddOutFee = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOutFeeValid = false;
                IsEnabledAddOutFeeButton = false;
            });
            DisableAddOutFeeContent = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                IsOutFeeContentValid = false;
                IsEnabledAddOutFeeButton = false;
            });
            PrevImageCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (_imageList.Count < 2) return;
                if (_imageList[0].Id == _imageHolderNumber) // ảnh đầu list
                {
                    _imageHolderNumber = _imageList[_imageList.Count - 1].Id;
                    ImageHolder = _imageList[_imageList.Count - 1].ImageBytes;
                }
                else
                {
                    dynamic tmp = new
                    {
                        Id = _imageHolderNumber,
                        ImageBytes = ImageHolder,
                    };
                    int index = _imageList.IndexOf(tmp);
                    _imageHolderNumber = _imageList[index - 1].Id;
                    ImageHolder = _imageList[index - 1].ImageBytes;
                }
            });
            NextImageCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (_imageList.Count < 2) return;
                if (_imageList[_imageList.Count - 1].Id == _imageHolderNumber) // ảnh cuối list
                {
                    _imageHolderNumber = _imageList[0].Id;
                    ImageHolder = _imageList[0].ImageBytes;
                }
                else
                {
                    dynamic tmp = new
                    {
                        Id = _imageHolderNumber,
                        ImageBytes = ImageHolder,
                    };
                    int index = _imageList.IndexOf(tmp);
                    _imageHolderNumber = _imageList[index + 1].Id;
                    ImageHolder = _imageList[index + 1].ImageBytes;
                }
            });
            AddImageCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Multiselect = true;
                dialog.Filter = "JPG files (*.jpg)|*.jpg| PNG files (*.png)|*.png| All files (*.*)|*.*";
                if (dialog.ShowDialog() == true)
                {
                    foreach (var absoluteLink in dialog.FileNames)
                    {
                        byte[] newBytesImage = BitMapImageTOBytes(new BitmapImage(
                                                    new Uri(absoluteLink,
                                                    UriKind.Absolute)
                                                    ));
                        Photo newPhoto = new Photo {
                            JourneyId = DetailJourney.Id,
                            OrderNumber = (DetailJourney.Photos.Count == 0) ? 1 : DetailJourney.Photos.Max(x => x.OrderNumber) + 1,
                            ImageBytes = newBytesImage,
                        };
                        DataProvider.Ins.DB.Photos.Add(newPhoto);
                        DataProvider.Ins.DB.SaveChanges();
                        _imageList.Add(new { 
                            Id = newPhoto.OrderNumber,
                            ImageBytes = newPhoto.ImageBytes,
                        });
                    }
                    if (NoImageVisibility == Visibility.Hidden)
                    {
                        _imageHolderNumber = _imageList[0].Id;
                        ImageHolder = _imageList[0].ImageBytes;
                        NoImageVisibility = Visibility.Visible;
                    }
                }

            });
            DeleteImageCommand = new RelayCommand<dynamic>((param) => { return true; }, (param) => {
                if (Global.GetInstance().ConfirmMessageDelete() == true)
                {
                    Photo deletePhoto = DetailJourney.Photos.First(x => x.OrderNumber == _imageHolderNumber);
                    DataProvider.Ins.DB.Photos.Remove(deletePhoto);
                    DataProvider.Ins.DB.SaveChanges();
                    dynamic tmp = new
                    {
                        Id = _imageHolderNumber,
                        ImageBytes = ImageHolder,
                    };
                    if (_imageList.Count == 1) // rỗng sau khi xóa
                    {
                        _imageList.RemoveAt(0);
                        _imageHolderNumber = 0;
                        ImageHolder = System.Text.Encoding.Default.GetBytes(Global.GetInstance().NoImageStringSource);
                        NoImageVisibility = Visibility.Hidden;
                    }
                    else
                    {
                        if (_imageList[_imageList.Count - 1].Id == _imageHolderNumber) // ảnh cuối list
                        {
                            _imageHolderNumber = _imageList[0].Id;
                            ImageHolder = _imageList[0].ImageBytes;
                        }
                        else
                        {
                            int index = _imageList.IndexOf(tmp);
                            _imageHolderNumber = _imageList[index + 1].Id;
                            ImageHolder = _imageList[index + 1].ImageBytes;
                        }
                    }
                    _imageList.Remove(tmp);
                }
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
    }

    public class RoutedEventTrigger : EventTriggerBase<DependencyObject>
    {
        RoutedEvent _routedEvent;
        public RoutedEvent RoutedEvent
        {
            get => _routedEvent;
            set { _routedEvent = value; }
        }
        public RoutedEventTrigger() { }
        protected override void OnAttached()
        {
            Behavior behavior = base.AssociatedObject as Behavior;
            FrameworkElement associatedElement = base.AssociatedObject as FrameworkElement;
            if (behavior != null)
            {
                associatedElement = ((IAttachedObject)behavior).AssociatedObject as FrameworkElement;
            }
            if (associatedElement == null)
            {
                throw new ArgumentException("Routed Event Trigger can only be associated to framework elements");
            }
            if (RoutedEvent != null)
            {
                associatedElement.AddHandler(RoutedEvent, new RoutedEventHandler(this.OnRoutedEvent));
            }
        }
        void OnRoutedEvent(object sender, RoutedEventArgs args)
        {
            base.OnEvent(args);
        }
        protected override string GetEventName()
        {
            return RoutedEvent.Name;
        }
    }
    public class IsNotNullStringRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (((string)value).Length > 0)
                {
                    return ValidationResult.ValidResult;
                }
                else
                {
                    return new ValidationResult(false, "Vui lòng không bỏ trống trường này");
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

        }
    }
    public class IsOnlyContainNumberRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (((string)value).Length > 0)
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch((string)value, "^[0-9]+$"))
                    {
                        return ValidationResult.ValidResult;
                    }
                    else
                    {
                        return new ValidationResult(false, "Vui lòng nhập trường này chỉ bao gồm kí tự số");
                    }
                }
                else
                {
                    return new ValidationResult(false, "Vui lòng không bỏ trống trường này");
                }
            }
            catch (Exception e)
            {
                return new ValidationResult(false, e.Message);
            }

        }
    }
}
