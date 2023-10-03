using HelpMeFocus.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace HelpMeFocus.ViewModels
{
    class CounterViewModel : INotifyPropertyChanged
    {
        public Action CloseAction { get; set; }
        public event PropertyChangedEventHandler? PropertyChanged;
        private string _timerDisplayValue;
        private string _totalTimeText;


        private bool IsTimerStarted { get; set; } = false;

        private DispatcherTimer _countdownTimer = new();
        private TimeSpan _countdownTime;
        private TimeSpan _totalTime;
        private string _buttonText;
        private int _hours;
        private int _minutes;
        private int _seconds;
        private bool _oneRun;
        private bool _infiniteLoop = true;
        private bool _cycles;

        private int _numCycles;
        private int _completedCycles;

        private RelayCommand _startStopTimerCommand;
        private RelayCommand _setTimerCommand;
        private RelayCommand _openCompactViewCommand;
        private RelayCommand _openFullViewCommand;
        private RelayCommand _resetTimerCommand;

        public CounterViewModel()
        {
            _countdownTime = TimeSpan.FromSeconds(0);
            _totalTime = TimeSpan.FromSeconds(0);
            _completedCycles = 0;
            _numCycles = 2;

            TimerDisplayValue = _countdownTime.ToString();
            TotalTimeText = _totalTime.ToString();
            ButtonText = "Start";

            _countdownTimer.Tick += new EventHandler(countdownTimer_Tick);
            _countdownTimer.Interval = TimeSpan.FromSeconds(1);
        }

        private void countdownTimer_Tick(object? sender, EventArgs e)
        {
            _countdownTime -= _countdownTimer.Interval;
            _totalTime += _countdownTimer.Interval;
            TimerDisplayValue = _countdownTime.ToString();
            TotalTimeText = _totalTime.ToString();
            if (_countdownTime <= TimeSpan.FromSeconds(0))
            {
                if (_oneRun)
                {
                    StartStopTimer();
                }
                else if (_infiniteLoop)
                {
                    SetTimerValue();
                }
                else if (_cycles && _numCycles > 1)
                {
                    _numCycles -= 1;
                    SetTimerValue();
                }
                else // (_cycles && _numCycles <= 0) || (some other BS)
                {
                    StartStopTimer();
                }
                _completedCycles += 1;
                CompletedCyclesText = _completedCycles.ToString();
                SoundPlayer player = new SoundPlayer(@"kaching.wav");
                player.Play();
            }
        }

        public string CompletedCyclesText
        {
            get
            {
                return _completedCycles.ToString();
            }
            set
            {
                if (!int.TryParse(value, out _completedCycles))
                {
                    _completedCycles = 0;
                }
                OnPropertyChanged(nameof(CompletedCyclesText));
            }
        }

        public string NumCyclesText
        {
            get
            {
                return _numCycles.ToString();
            }
            set
            {
                if (!int.TryParse(value, out _numCycles))
                {
                    _numCycles = 2;
                }
                OnPropertyChanged(nameof(NumCyclesText));
            }
        }

        public string ButtonText
        {
            get { return _buttonText; }
            set
            {
                _buttonText = value;
                OnPropertyChanged(nameof(ButtonText));
            }
        }

        public string HoursText
        {
            get
            {
                return _hours.ToString("00");
            }
            set
            {
                if (int.TryParse(value, out _hours))
                {
                    if (_hours > 99) _hours = 99;
                    else if (_hours < 0) _hours = 0;
                }
                else
                {
                    _hours = 0;
                }
                OnPropertyChanged(nameof(HoursText));
            }
        }

        public string MinutesText
        {
            get
            {
                return _minutes.ToString("00");
            }
            set
            {
                if (int.TryParse(value, out _minutes))
                {
                    if (_minutes > 59) _minutes = 59;
                    else if (_minutes < 0) _minutes = 0;
                }
                else
                {
                    _minutes = 0;
                }
                OnPropertyChanged(nameof(MinutesText));
            }
        }

        public string SecondsText
        {
            get
            {
                return _seconds.ToString("00");
            }
            set
            {
                if (int.TryParse(value, out _seconds))
                {
                    if (_seconds > 59) _seconds = 59;
                    else if (_seconds < 0) _seconds = 0;
                }
                else
                {
                    _seconds = 0;
                }
                OnPropertyChanged(nameof(SecondsText));
            }
        }

        public bool OneRun
        {
            get { return _oneRun; }
            set
            {
                if (_oneRun == value)
                    return;
                _oneRun = value;
                OnPropertyChanged(nameof(OneRun));
            }
        }

        public bool InfiniteLoop
        {
            get { return _infiniteLoop; }
            set
            {
                if (_infiniteLoop == value)
                    return;
                _infiniteLoop = value;
                OnPropertyChanged(nameof(InfiniteLoop));
            }
        }

        public bool Cycles
        {
            get { return _cycles; }
            set
            {
                if (_cycles == value)
                    return;
                _cycles = value;
                OnPropertyChanged(nameof(Cycles));
            }
        }

        public string TimerDisplayValue
        {
            get { return _timerDisplayValue; }
            set
            {
                if (_timerDisplayValue == value)
                    return;
                _timerDisplayValue = value;
                OnPropertyChanged(nameof(TimerDisplayValue));
            }
        }

        public string TotalTimeText
        {
            get { return _totalTimeText; }
            set
            {
                if (_totalTimeText == value)
                    return;
                _totalTimeText = value;
                OnPropertyChanged(nameof(TotalTimeText));
            }
        }

        public bool CanStartStopTimer()
        {
            if (IsTimerStarted)
            {
                return true;
            }
            else // check if timer can be started
            {
                if (_countdownTime > TimeSpan.FromSeconds(0))
                    return true;
                else
                    return false;
            }
        }

        public void StartStopTimer()
        {
            if (IsTimerStarted)
            {
                _countdownTimer.Stop();
                ButtonText = "Start";
            }
            else
            {
                _countdownTimer.Start();
                ButtonText = "Pause";
            }
            IsTimerStarted = !IsTimerStarted;
            OnPropertyChanged(nameof(IsTimerStarted));
        }
        public RelayCommand StartStopTimerCommand
        {
            get 
            { 
                if (_startStopTimerCommand == null)
                    _startStopTimerCommand = new RelayCommand(o => StartStopTimer(), o => CanStartStopTimer());
                return _startStopTimerCommand;
            }
        }

        private bool CanSetTimerValue()
        {
            if (IsTimerStarted)
                return false;
            else
                return true;
        }

        public void SetTimerValue()
        {
            int hours = int.Parse(HoursText);
            int minutes = int.Parse(MinutesText);
            int seconds = int.Parse(SecondsText);
            TimeSpan hSpan = TimeSpan.FromHours(hours);
            TimeSpan mSpan = TimeSpan.FromMinutes(minutes);
            TimeSpan sSpan = TimeSpan.FromSeconds(seconds);
            _countdownTime = hSpan + mSpan + sSpan;
            if (_countdownTime <= TimeSpan.FromSeconds(0))
            {
                _countdownTime = TimeSpan.FromSeconds(0);
            }
            TimerDisplayValue = _countdownTime.ToString();
        }
        public RelayCommand SetTimerCommand
        {
            get
            {
                if (_setTimerCommand == null)
                    _setTimerCommand = new RelayCommand(o => SetTimerValue(), o => CanSetTimerValue());
                return _setTimerCommand;
            }
        }

        private void OpenCompactView()
        {
            Window compactViewWindow = new CompactWindow
            {
                DataContext = this
            };
            compactViewWindow.Show();
            CloseAction();
            this.CloseAction = compactViewWindow.Close;
        }

        public RelayCommand OpenCompactViewCommand
        {
            get
            {
                if (_openCompactViewCommand == null)
                    _openCompactViewCommand = new RelayCommand(o => OpenCompactView());
                return _openCompactViewCommand;
            }
        }

        private void OpenFullView()
        {
            Window fullView = new MainWindow
            {
                DataContext = this
            };
            fullView.Show();
            CloseAction();
            this.CloseAction = fullView.Close;
        }

        public RelayCommand OpenFullViewCommand
        {
            get
            {
                if (_openFullViewCommand == null)
                    _openFullViewCommand = new RelayCommand(o => OpenFullView());
                return _openFullViewCommand;
            }
        }

        public bool CanResetTimer()
        {
            return true; // TODO
        }
        public void ResetTimer()
        {
            // Stop timer
            _countdownTimer.Stop();
            ButtonText = "Start";
            IsTimerStarted = false;
            // Reset display value to zero
            _countdownTime = TimeSpan.FromSeconds(0);
            TimerDisplayValue = _countdownTime.ToString();
            // Reset completed cycles to zero
            CompletedCyclesText = "0";
            _totalTime = TimeSpan.FromSeconds(0);
            TotalTimeText = _totalTime.ToString();
            OnPropertyChanged(nameof(IsTimerStarted));
        }
        public RelayCommand ResetTimerCommand
        {
            get
            {
                if (_resetTimerCommand == null)
                    _resetTimerCommand = new RelayCommand(o => ResetTimer(), o => CanResetTimer());
                return _resetTimerCommand;
            }
        }


        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
