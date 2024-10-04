using System;
using System.Collections.Generic;
using System.Reactive;
using Avalonia.Controls;
using System.Runtime.InteropServices;
using ReactiveUI;

namespace NyarGit.ViewModels.Controls
{
	public class CustomTitleBarViewModel : ViewModelBase
    {
		public ReactiveCommand<Unit, Unit> MinimizeCommand { get; }

        public ReactiveCommand<Unit, Unit> MaximizeRestoreCommand { get; }
        public ReactiveCommand<Unit, Unit> CloseCommand { get; }

        public CustomTitleBarViewModel()
        {

            MinimizeCommand = ReactiveCommand.Create(ExecuteMinimize);
            MaximizeRestoreCommand = ReactiveCommand.Create(ExecuteMaximize);
            CloseCommand = ReactiveCommand.Create(ExecuteClose);
        }

        public void ExecuteMinimize()
        {
            

        }

        public void ExecuteMaximize() { }

        public void ExecuteClose() { }
    }
}