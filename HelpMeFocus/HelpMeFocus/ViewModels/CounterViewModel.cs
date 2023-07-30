using HelpMeFocus.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace HelpMeFocus.ViewModels
{
    class CounterViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public string TimerDisplayValue { get; set; }

        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        private bool IsTimerStarted { get; set; } = false;

        private TimeSpan _timeSpan;

        public CounterViewModel()
        {
            TimerDisplayValue = _timeSpan.ToString();
            ButtonText = "Start";

            // TODO: Get the numbers from text block
            _timeSpan = new TimeSpan(0, 1, 6); // hh,mm,ss

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
        }

        private void dispatcherTimer_Tick(object? sender, EventArgs e)
        {
            TimerDisplayValue = _timeSpan.ToString();
            _timeSpan -= TimeSpan.FromSeconds(1);
            OnPropertyChanged(nameof(TimerDisplayValue));
        }

        private string _buttonText;
        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public void StartStopTimer()
        {
            if (IsTimerStarted)
            {
                dispatcherTimer.Stop();
                ButtonText = "Start";
            }
            else
            {
                dispatcherTimer.Start();
                ButtonText = "Stop";
            }
            IsTimerStarted = !IsTimerStarted;
            OnPropertyChanged(nameof(IsTimerStarted));
        }

        private RelayCommand _startStopTimerCommand;
        public RelayCommand StartStopTimerCommand
        {
            get 
            { 
                if (_startStopTimerCommand == null)
                {
                    _startStopTimerCommand = new RelayCommand(o => StartStopTimer());
                }
                return _startStopTimerCommand;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
