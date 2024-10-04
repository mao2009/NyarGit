using System;
using System.Diagnostics;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Input;
using Avalonia.Interactivity;
using CommunityToolkit.Mvvm.ComponentModel;
using Avalonia.Logging;
using Avalonia.Threading;
using ReactiveUI;

namespace NyarGit.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string _greeting = "Welcome to Avalonia!";
        
        public ReactiveCommand<Unit, Unit> CloneCommand { get; }
        public MainViewModel() {
            CloneCommand = ReactiveCommand.CreateFromTask(ExecuteClone);
        }

        public async Task ExecuteClone()
        {
            
            await Task.Run(() => Thread.Sleep(1000));
            await Dispatcher.UIThread.InvokeAsync(() =>
            {

                Debug.WriteLine("test");
            });

        }
        private static readonly object _lock = new object();
        public void ExecuteCloneq()
        {
            lock (_lock)
            {


                Debug.WriteLine("test");
            }
        }
    }
}
