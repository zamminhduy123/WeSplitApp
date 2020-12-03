using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WeSplitApp.ViewModels
{
    class DetailUCViewModel : BaseViewModel
    {
        public ICommand CloseWindowCommand { get; set; }
        public DetailUCViewModel()
        {

        }
    }
}
