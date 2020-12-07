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
        private String[] _color = { "#F44336","#e91e63","#ab47bc","#7e57c2","#5c6bc0","#2196f3","#03a9f4","#00bcd4","#009688", "#4caf50" ,"#7cb342", "#9e9d24" ,"#ef6c00" ,"#e64a19", "#8d6e83", "#607d8b" };

        public String[]  Colors { get => _color; set { _color = value; OnPropertyChanged(); } }

        public ICommand ThemeButtonCommand { get; set; }

        public Global globalTheme = Global.GetInstance();

        public ThemeUCViewModel()
        {
            ThemeButtonCommand = new RelayCommand<String>((prop) => { return true; }, (prop) =>
            {

                globalTheme.ThemeColor = prop;
                globalTheme.OnPropertyChanged("ThemeColor");

            });
        }
    }
}
