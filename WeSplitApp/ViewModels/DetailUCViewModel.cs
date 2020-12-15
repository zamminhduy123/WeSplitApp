using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class DetailUCViewModel : BaseViewModel
    {
        //Properties
        private Journey _detailJourney;
        public Journey DetailJourney { get => _detailJourney; set { _detailJourney = value;

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

                OnPropertyChanged(); } }

        private  AsyncObservableCollection<dynamic> _detailRouteList;
        public AsyncObservableCollection<dynamic> DetailRouteList { get => _detailRouteList; set { _detailRouteList = value; OnPropertyChanged(); } }

        private AsyncObservableCollection<dynamic> _detailMemberList;
        public AsyncObservableCollection<dynamic> DetailMemberList { get => _detailMemberList; set { _detailMemberList = value; OnPropertyChanged(); } }

        private AsyncObservableCollection<Member> _memberList;
        public AsyncObservableCollection<Member> MemberList { get => _memberList; set { _memberList = value; OnPropertyChanged(); } }

        private Member _selectedMember;
        public Member SelectedMember { get => _selectedMember; set { _selectedMember = value; OnPropertyChanged(); } }

        private string _startDate;
        public string StartDate { get => _startDate; set { _startDate = value;
                DataProvider.Ins.DB.Journeys.Find(DetailJourney.Id).Departure = Convert.ToDateTime(StartDate);
                DataProvider.Ins.DB.SaveChanges();
                OnPropertyChanged(); } }

        private string _endDate;
        public string EndDate { get => _endDate; set { _endDate = value;
                DataProvider.Ins.DB.Journeys.Find(DetailJourney.Id).Arrival = Convert.ToDateTime(EndDate);
                DataProvider.Ins.DB.SaveChanges();
                OnPropertyChanged(); } }

        private BitmapImage _cover;
        public BitmapImage Cover { get => _cover; set { _cover = value; OnPropertyChanged(); } }

        private string _routeName;
        public string RouteName { get => _routeName; set { _routeName = value; OnPropertyChanged(); } }

        private string _routeDescription;
        public string RouteDescription { get => _routeDescription; set { _routeDescription = value; OnPropertyChanged(); } }

        private string _addOrUpdateRouteContent;
        public string AddOrUpdateRouteContent { get => _addOrUpdateRouteContent; set { _addOrUpdateRouteContent = value; OnPropertyChanged(); } }

        private dynamic _selectedRoute;
        public dynamic SelectedRoute { get => _selectedRoute; set { _selectedRoute = value;
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
                OnPropertyChanged(); } }

        private dynamic _selectedTab;
        public dynamic SelectedTab { get => _selectedTab; set { _selectedTab = value;
                if (SelectedTab.Header == "Thời gian")
                {

                    // get cover
                    byte[] bytearray = (DetailJourney.Photos.Count == 0) ? System.Text.Encoding.Default.GetBytes(Global.GetInstance().NoImageStringSource) : DetailJourney.Photos.ToList()[0].ImageBytes;
                    BitmapImage bitmapimage = ByteArrayToImage(bytearray);
                    Cover = bitmapimage;

                    // get Start date
                    StartDate = DetailJourney.Departure.ToString().Split(' ').First();

                    // get end date
                    EndDate = DetailJourney.Arrival.ToString().Split(' ').First();

                }
                if (SelectedTab.Header == "Thành viên")
                {

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
                            Fees = (fee.Fees == null) ? 0 : fee.Fees.Value,
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
                            Fees = (fee.Fees == null) ? 0 : fee.Fees.Value,
                        };
                        OutFeesList.Add(tmp);
                    }
                }
                if (SelectedTab.Header == "Tổng kết")
                {

                }
                OnPropertyChanged(); } }
           
        // Thu chi
            //danh sách thành viên tham gia chuyến đi
        private AsyncObservableCollection<Member> _paticipantsList;
        public AsyncObservableCollection<Member> PaticipantsList { get => _paticipantsList; set { _paticipantsList = value; OnPropertyChanged(); } }
            
            // thành viên đc chọn để thu tiền
        private Member _selectedInFeeMember;
        public Member SelectedInFeeMember { get => _selectedInFeeMember; set { _selectedInFeeMember = value; OnPropertyChanged(); } }

            // Danh sách thành viên đã đóng tiền và số tiền đã đóng
        private AsyncObservableCollection<dynamic> _inFeesList;
        public AsyncObservableCollection<dynamic> InFeesList { get => _inFeesList; set { _inFeesList = value; OnPropertyChanged(); } }

            // Số tiền thu vào
        private string _inFee;
        public string InFee { get => _inFee; set { _inFee = value; OnPropertyChanged(); } }

            // danh sách khoản chi
        private AsyncObservableCollection<dynamic> _outFeesList;
        public AsyncObservableCollection<dynamic> OutFeesList { get => _outFeesList; set { _outFeesList = value; OnPropertyChanged(); } }
            
            // Tên khoản chi mới
        private string _outFeeContent;
        public string OutFeeContent { get => _outFeeContent; set { _outFeeContent = value; OnPropertyChanged(); } }

            // Số tiền chi ra
        private string _outFee;
        public string OutFee { get => _outFee; set { _outFee = value; OnPropertyChanged(); } }

            // Khoản chi đã chọn dưới danh sách
        private dynamic _selectedOutFee;
        public dynamic SelectedOutFee { get => _selectedOutFee; set { _selectedOutFee = value;
                OutFeeContent = (SelectedOutFee == null) ? null : SelectedOutFee.Name;
                OnPropertyChanged(); } }
        //Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand DeleteParticipantCommand { get; set; }
        public ICommand AddParticipantCommand { get; set; }
        public ICommand AddRouteCommand { get; set; }
        public ICommand DeleteRouteCommand { get; set; }
        public ICommand AddInFeeCommand { get; set; }
        public ICommand DeleteInFeeCommand { get; set; }
        public ICommand AddOutFeeCommand { get; set; }
        public ICommand DeleteOutFeeCommand { get; set; }
        public DetailUCViewModel()
        {
            AddOrUpdateRouteContent = "THÊM";
            var value = ConfigurationManager.AppSettings["DetailTripId"];
            int DetailTripId = int.Parse(value);
            int JourneyId = DetailTripId;
            DetailJourney = DataProvider.Ins.DB.Journeys.Where(x => x.Id == JourneyId).FirstOrDefault();

            DeleteParticipantCommand = new RelayCommand<Member>((param) => { return true; }, (param) => {
                if (Global.GetInstance().ConfirmMessageDelete() == true)
                {
                    foreach (var expense in DataProvider.Ins.DB.Expenses.Where(x => x.JourneyId == DetailJourney.Id && x.MemberId == param.Id))
                    {
                        DataProvider.Ins.DB.Expenses.Remove(expense);
                    }
                    DataProvider.Ins.DB.Journeys.Find(DetailJourney.Id).Members.Remove(param);
                    DataProvider.Ins.DB.Members.Find(param.Id).Journeys.Remove(DetailJourney);
                    DataProvider.Ins.DB.SaveChanges();
                    MemberList.Add(param);
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
            });

            AddRouteCommand = new RelayCommand<object>((param) => { return true; }, (param) => {
                if (RouteName == null || RouteName == "")
                {
                    MessageBox.Show("Please enter route name");
                    return;
                }
                if (SelectedRoute == null)
                {
                    DataProvider.Ins.DB.Routes.Add(new Route { 
                        JourneyId = DetailJourney.Id,
                        OrderNumber = DetailJourney.Routes.Max(x => x.OrderNumber) + 1,
                        Name = RouteName,
                        Description=RouteDescription,
                    });
                    DataProvider.Ins.DB.SaveChanges();
                    dynamic tmp = new
                    {
                        Id = DetailJourney.Routes.Max(x => x.OrderNumber),
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
                    OrderNumber = DataProvider.Ins.DB.Expenses.Max(x => x.OrderNumber) + 1,
                    Fees = int.Parse(InFee),
                };
                DataProvider.Ins.DB.Expenses.Add(newexpense);
                DataProvider.Ins.DB.SaveChanges();
                dynamic tmp = new
                {
                    Id = newexpense.OrderNumber,
                    MemberId =  newexpense.MemberId,
                    Name = newexpense.Member.Name,
                    Fees = newexpense.Fees.Value,
                };
                InFeesList.Add(tmp);

                SelectedInFeeMember = null;
                InFee = null;
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
                            Fees = (fee.Fees == null) ? 0 : fee.Fees.Value,
                        };
                        OutFeesList.Add(tmp);
                    }
                }
                else
                {
                    Cost newCost = new Cost
                    {
                        JourneyId = DetailJourney.Id,
                        OrderNumber = DataProvider.Ins.DB.Costs.Max(x => x.OrderNumber) + 1,
                        Content = OutFeeContent,
                        Fees = int.Parse(OutFee),
                    };
                    DataProvider.Ins.DB.Costs.Add(newCost);
                    dynamic tmp = new
                    {
                        Id = newCost.OrderNumber,
                        Name = newCost.Content,
                        Fees = newCost.Fees.Value,
                    };
                    OutFeesList.Add(tmp);
                }
                DataProvider.Ins.DB.SaveChanges();
                OutFeeContent = null;
                OutFee = null;
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
        }



        public BitmapImage ByteArrayToImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
