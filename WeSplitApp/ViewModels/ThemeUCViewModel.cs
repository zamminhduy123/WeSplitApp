using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WeSplitApp.ViewModels;

namespace WeSplitApp.ViewModels
{
    class ThemeUCViewModel : BaseViewModel
    {
        private Brush[] _color = { Brushes.Red, Brushes.Blue, Brushes.DarkSalmon, Brushes.Green, Brushes.DarkOrange, Brushes.DarkMagenta, Brushes.HotPink, Brushes.Brown, Brushes.Chocolate, Brushes.Orange, Brushes.YellowGreen, Brushes.DarkBlue, Brushes.DarkCyan };

        public Brush[]  Colors { get => _color; set { _color = value; OnPropertyChanged(); } }

        public ICommand ThemeButtonCommand { get; set; }

        public Global globalTheme = Global.GetInstance();

        public ThemeUCViewModel()
        {
            ThemeButtonCommand = new RelayCommand<Brush>((prop) => { return true; }, (prop) =>
            {
                globalTheme.ThemeColor = prop.ToString();
            });
        }
    }
}
