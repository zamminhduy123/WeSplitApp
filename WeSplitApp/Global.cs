using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WeSplitApp.ViewModels;

namespace WeSplitApp
{
    public class Global : INotifyPropertyChanged
    {
        private static Global _instance = null;
        private BaseViewModel _currentPageViewModel = null; // Attribute - Backup fields
        //Helper for Thread Safety
        private static object m_lock = new object();

        public static Global GetInstance()
        {
            // DoubleLock
            if (_instance == null)
            {
                lock (m_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Global();
                    }
                }
            }
            return _instance;
        }

        public BaseViewModel CurrentPageViewModel { get => _currentPageViewModel; set { _currentPageViewModel = value; OnPropertyChanged("CurrentPageViewModel"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        Global()
        {
            CurrentPageViewModel = new HomeUCViewModel();
        }
    }
}
