using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WeSplitApp.ViewModels;

namespace WeSplitApp.UserControls
{
    /// <summary>
    /// Interaction logic for AboutUC.xaml
    /// </summary>
    public partial class AboutUC : UserControl
    {
        private AboutUCViewModel Viewmodel { get; set; }

        public AboutUC()
        {
            InitializeComponent();
            this.DataContext = Viewmodel = new AboutUCViewModel();
        }

    }
}
