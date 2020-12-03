using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeSplitApp.Models;

namespace WeSplitApp.Model
{
    public class DataProvider
    {
        private static DataProvider _ins;

        public static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                {
                    _ins = new DataProvider();
                }
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }

        public WeSplitAppDBEntities DB { get; set; }

        public DataProvider()
        {
            DB = new WeSplitAppDBEntities();
        }

    }
}
