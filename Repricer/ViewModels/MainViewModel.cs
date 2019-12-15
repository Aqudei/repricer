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
using System.Windows.Media.Animation;
using MahApps.Metro.Controls.Dialogs;

namespace Repricer.ViewModels
{
    sealed class MainViewModel : Conductor<object>.Collection.OneActive
    {
        private readonly IDialogCoordinator _dialogCoordinator;

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            _dialogCoordinator = dialogCoordinator;
            ActivateItem(IoC.Get<InventoryListViewModel>());

            Task.Run(() => CheckExistingDb());
        }

        private void CheckExistingDb()
        {
            Execute.OnUIThread(async () =>
            {
                var sdf = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.sdf",
                    SearchOption.TopDirectoryOnly);

                if (sdf.Length > 0)
                {

                    var result =
                        await _dialogCoordinator.ShowMessageAsync(this, "Please Confirm Action", $"I found existing database file at {sdf[0]}.\nDo you want to delete it ? ",
                            MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MessageDialogResult.Affirmative)
                    {
                        File.Delete(sdf[0]);
                    }
                }
            });
        }
    }
}
