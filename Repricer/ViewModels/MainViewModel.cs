using Caliburn.Micro;
using Microsoft.Win32;
using Repricer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repricer.ViewModels
{
    class MainViewModel : Conductor<object>.Collection.OneActive
    {
        public MainViewModel()
        {
            ActivateItem(IoC.Get<InventoryListViewModel>());
        }
    }
}
