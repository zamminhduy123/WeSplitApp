using WeSplitApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace WeSplitApp.ViewModels
{
    public class SplashScreenViewModel : BaseViewModel
    {
        #region private 
        private string _tip;
        private const int _time_SplashScreen = 3000;
        private const int Interval = 3000 / (24 * 3); //24 fps
        private readonly Timer dT = new Timer(Interval);

        private int _progressBarValue = 0;

        private string[] _tipList = {
        "Cất sạc pin điện thoại hay tai nghe vào hộp đựng kính, điều này giúp bạn rất dễ dàng tìm và bảo quản chúng.",
        "Cho kem dưỡng da vào ống hút vừa giúp tiết kiệm diện tích lại rất tiện dụng",
        "Dùng dây lò xo từ ruột bút bi để quấn quanh dây sạc điện thoại. Cách này giúp dây sạc không bị rách hay đứt.",
        "Kẹp đầu dao cạo bằng kẹp giấy, vừa giúp bảo vệ đầu dao, vừa giúp an toàn lúc bạn tìm kiếm chúng.",
        "Trong trường hợp bạn để quên sạc điện thoại ở nhà, hãy tận dụng bất cứ chiếc tivi, laptop hay máy tính nào để sạc",
        "Bạn nên cuận tròn quần áo khi xếp vào vali thay vì cách ta hay gấp hàng ngày. Bạn sẽ tiết kiệm được kha khá chỗ cho những vật dụng khác đó.",
        "Trong trường hợp, bạn vẫn muốn gấp quấn áo, hãy dùng khăn giấy lót quanh chúng để giữ quần áo không bị nhăn.",
        "Hãy tận dụng hộp đựng thuốc để đựng đồ trang sức.",
        "Bạn nên scan những thông tin cá nhân quan trọng rồi lưu vào máy tính, điện thoại. Điều này sẽ giúp ích cho bạn trong nhiều trường hợp.",
        "Hãy để ví, chìa khóa và điện thoại của bạn vào trong vali hoặc túi xách trước khi đặt chúng lên băng chuyền tại cửa an ninh. Bạn sẽ không phải mất thời gian xếp đồ vào thùng riêng nữa.",
        "Nên ngồi ở gần cánh máy bay để tránh rung lắc một cách tối đa.",
        "Tận dụng túi chụp đầu để đựng giày dép, bạn sẽ không lo đất bẩn dây lên quần áo.",
        "Đặt chế độ máy bay cho điện thoại giúp tiết kiệm pin và sạc nhanh hơn gần gấp 2 lần.",
        "Lại một công dụng khác nữa của kẹp giấy, bạn có thể tận dụng chúng để cuốn gọn các loại dây sạc hay tai nghe.",
        "Dùng hộp kẹo để đựng những chiếc kẹp tóc, vô cùng tiện dụng và hữu ích cho phái nữ.",
        "Giữ lại những đồ dùng như kem đánh răng, sửa rửa mặt hay kem dưỡng dạng nhỏ và đổ đầy khi dùng hết.",
        "Để sử dụng Google Maps offline, gõ “OK Maps”, vùng hiển thị sẽ được lưu cho việc truy cập trong tương lai.",
        "Thông thường vào 3h chiều thứ 3 là thời điểm mà những hãng máy bay lớn giảm giá bán để cạnh tranh với những hãng máy bay giá rẻ. Vậy tại sao không chớp thời cơ “ngàn năm có một” này nhỉ.",
        "Nếu không có ai ngồi ở giữa, mặc nhiên 2 bạn sẽ có cả 3 ghế ngồi. Còn nếu có người ngồi, hãy xin phép để được ngồi cạnh bạn của mình.",
        "Trên máy bay, bạn sẽ không được phép xếp hàng để dài để đi vệ sinh. Vậy nên thời điểm thích hợp nhất cho việc này là vào lúc ngay sau khi máy bay cất cánh hoặc 15-20 phút trước khi cất cánh.",
        "Trên mạng xã hội FourSquare có hàng tá pass wifi mà có thể bạn sẽ cần đến.",
        "Nếu như bạn đi du lịch bụi ở nước ngoài, đặt điện thoại ở chế độ máy bay, tắt 3G và sử dụng GPS. Tìm kiếm địa điểm cần đến trên Google Maps trước khi đến khách sạn, bạn sẽ có một bản đồ bao quát toàn khu vực.",
        "Nếu chẳng may quên sạc điện thoại ở nhà, đừng lo bởi quầy lễ tân khách sạn có một rổ đựng sạc mà những vị khách trước để quên.",
        "Dán nhãn “Hàng dễ vỡ” trên hành lý của bạn. Bằng cách này, nhân viên sân bay sẽ nhẹ tay với hành lý của bạn, chúng sẽ được đặt trên những hành lý của khách khác và thường ra đầu tiên.",
        "Rút tiền ở bốt ATM sẽ mất ít phí giao dịch hơn trạm đổi tiền ở sân bay.",
        "Khi ở sân bay, thêm đuôi “?.jpg” vào sau đường link là bạn có thể bắt wifi và lướt web thỏa thích.",
        "Trở thành “người hùng” ở sân bay khi mang theo ổ cắm điện.",
        "“Tự sướng” bằng cách gửi cho chính bạn những bức bưu thiếp tại chính nơi bạn đang đi du lịch. Tự tạo cho mình một sự bất ngờ nho nhỏ, tại sao không?",
        "Vào ngày cuối cùng của chuyến đi, hãy gom tất cả số tiền ngoại tệ còn lại và tặng cho những người vô gia cư.",
        };

        #endregion

        #region Properties

        public string Tip
        {
            get { return _tip; }
            set { _tip = value; }
        }

        public Global globalTheme = Global.GetInstance();

        public ICommand ChangeShow { get; set; }
        public ICommand LoadedWindowCommand { get; set; }
        public int ProgressBarValue { get => _progressBarValue; set { _progressBarValue = value; OnPropertyChanged(); } }
        #endregion
        public SplashScreenViewModel()
        {
            Tip = _tipList[MyRandom.Ins.Next(_tipList.Count())];

            ChangeShow = new RelayCommand<object>((prop) => { return true; }, (prop) =>
            {
                var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
                bool showSplash = bool.Parse(value);
                var config = ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);
                config.AppSettings.Settings["ShowSplashScreen"].Value = (!showSplash).ToString();
                config.Save(ConfigurationSaveMode.Minimal);

                ConfigurationManager.RefreshSection("appSettings");
            });

            LoadedWindowCommand = new RelayCommand<Window>((prop) => { return true; }, (splash) =>
            {
                
                splash.Hide();
                var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
                bool showSplash = bool.Parse(value);
                if (showSplash == false)
                {
                    if (MainViewModel.IsShowed == false)
                    {
                        MainWindow mW = new MainWindow();
                        mW.Show();
                        MainViewModel.IsShowed = true;
                    }
                    splash.Close();
                }
                else
                {
                    splash.Show();
                    dT.Elapsed += dt_Tick;
                    dT.Start();
                }

            });
        }

        void dt_Tick(object sender, EventArgs e)
        {
            ProgressBarValue += Interval;
            if (ProgressBarValue >= _time_SplashScreen)
            {
                dT.Dispose();
                //thisWindow.Close();
                //MainWindow mW = new MainWindow();
                //mW.Show();
            }
            else
            {
                dT.Start();
            }

        }



    }
}