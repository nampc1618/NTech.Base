﻿using NTech.Base.Commons.BaseCommand;
using NTech.Xm.Station.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NTech.Xm.Station.Command
{
    public class ReprintCmd : CommandBase
    {
        private readonly MainViewModel _mainViewModel;
        public ReprintCmd(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }
        public override void Execute(object parameter)
        {
            _mainViewModel.IsReprint = true;
            _mainViewModel.MainView.popupDG.IsOpen = false;
        }
    }
}
