using System;
using System.Windows.Controls;
using WeSplitApp.ViewModels;

namespace WeSplitApp.UserControls
{
    /// <summary>
    /// Interaction logic for ThemeViewUC.xaml
    /// </summary>
    public partial class ThemeUC : UserControl
    {
        private ThemeUCViewModel Viewmodel { get; set; }

        public ThemeUC()
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new ThemeUCViewModel();
        }
    }
}
