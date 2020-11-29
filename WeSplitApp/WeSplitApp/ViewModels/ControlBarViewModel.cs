using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WeSplitApp.ViewModels
{
    class ControlBarViewModel : BaseViewModel
    {
        #region Commands
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }

        public ICommand MaximizeWindowCommand { get; set; }
        public ICommand MouseMoveWindowCommand { get; set; }
        #endregion

        public ControlBarViewModel()
        {
            MouseMoveWindowCommand = new RelayCommand<UserControl>((param) => { return param == null ? false : true; }, (param) =>
            {
                FrameworkElement ucParent = GetWindowParent(param);
                var window = (ucParent as Window);
                if (window != null)
                {
                    window.DragMove();
                }
            });

            CloseWindowCommand = new RelayCommand<UserControl>((param) => { return param == null ? false : true; }, (param) =>
            {
                FrameworkElement ucParent = GetWindowParent(param);
                var window = (ucParent as Window);
                if (window != null)
                {
                    window.Close();
                }
            });

            MaximizeWindowCommand = new RelayCommand<UserControl>((param) => { return param == null ? false : true; }, (param) =>
            {
                FrameworkElement ucParent = GetWindowParent(param);
                var window = (ucParent as Window);
                if (window != null)
                {
                    if (window.WindowState != WindowState.Maximized)
                    {
                        window.WindowState = WindowState.Maximized;
                    } else
                    {
                        window.WindowState = WindowState.Normal;
                    }
                }
            });

            MinimizeWindowCommand = new RelayCommand<UserControl>((param) => { return param == null ? false : true; }, (param) =>
            {
                FrameworkElement ucParent = GetWindowParent(param);
                var window = (ucParent as Window);
                if (window != null)
                {
                    if (window.WindowState != WindowState.Minimized)
                    {
                        window.WindowState = WindowState.Minimized;
                    } else {
                        window.WindowState = WindowState.Maximized;
                    }
                }
            });
        }

        FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;

            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }

            return parent;
        } 
    }
}
