using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Mono.CSharp;

namespace RunCSharp
{
    public class RunBlock : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        string _inputText = "";
        public string InputText
        {
            get { return _inputText; }
            set
            {
                if (_inputText != value)
                {
                    _inputText = value;
                    _inputTime = DateTime.UtcNow;
                    OnPropertyChanged("InputText");
                    OnPropertyChanged("InputTime");
                }
            }
        }

        DateTime _inputTime;
        public DateTime InputTime { get { return _inputTime; } }

        object _outputValue = null;
        public object OutputValue
        {
            get { return _outputValue; }
            private set
            {
                _hasOutput = true;
                _outputValue = value;
                _outputTime = DateTime.UtcNow;
                OnPropertyChanged("HasOutput");
                OnPropertyChanged("OutputValue");
                OnPropertyChanged("OutputText");
                OnPropertyChanged("OutputTime");
            }
        }

        DateTime _outputTime;
        public DateTime OutputTime { get { return _outputTime; } }

        bool _hasOutput = false;
        public bool HasOutput { get { return _hasOutput; } }

        public string OutputText
        {
            get
            {
                if (_outputValue != null)
                {
                    return _outputValue.ToString();
                }
                else
                {
                    return "null";
                }
            }
        }

        public ObservableCollection<AbstractMessage> Errors { get; private set; }

        public bool HasErrors { get { return Errors.Count > 0; } }

        public RunBlock()
        {
            Errors = new ObservableCollection<AbstractMessage>();
        }

        public RunBlock(string inputText)
            : this()
        {
            _inputText = inputText;
        }

        public bool Run(Runner runner)
        {
            Errors.Clear();
            var r = runner.Run(InputText, msg =>
            {
                Errors.Add(msg);
            });
            if (runner.HasResult)
            {
                OutputValue = runner.Result;
            }
            else
            {
                OutputValue = null;
            }
            OnPropertyChanged("HasErrors");
            return true;
        }

        void OnPropertyChanged(string name)
        {
            var p = PropertyChanged;
            if (p != null)
            {
                p(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
