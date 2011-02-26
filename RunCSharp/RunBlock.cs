using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Mono.CSharp;

namespace RunCSharp
{
	public class RunBlock : INotifyPropertyChanged
	{
		/// <summary>
		/// Reports state changes
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		string _inputText = "";
		public string InputText
		{
			get { return _inputText; }
			set
			{
				if (_inputText != value) {
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
				OnPropertyChanged("OutputTypeName");
				OnPropertyChanged("OutputTime");
			}
		}

		DateTime _outputTime;
		public DateTime OutputTime { get { return _outputTime; } }

		bool _hasOutput = false;

		/// <summary>
		/// Whether the <see cref="OutputValue"/> is valid
		/// </summary>
		public bool HasOutput { get { return _hasOutput; } }

		/// <summary>
		/// Printing of <see cref="OutputValue"/> using ToString()
		/// </summary>
		public string OutputText
		{
			get
			{
				if (_outputValue != null) {
					return _outputValue.ToString();
				}
				else {
					return "null";
				}
			}
		}

		/// <summary>
		/// The name of the output type
		/// </summary>
		public string OutputTypeName
		{
			get
			{
				if (_outputValue != null) {
					return _outputValue.GetType().Name;
				}
				else {
					return typeof(object).Name;
				}
			}
		}

		/// <summary>
		/// List of errors encountered during the last run
		/// </summary>
		public ObservableCollection<AbstractMessage> Errors { get; private set; }

		/// <summary>
		/// Whether there are any errors in the <see cref="Errors"/> collection
		/// </summary>
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

		/// <summary>
		/// Executes the block using the given <see cref="Runner"/>
		/// </summary>
		/// <param name="runner">The <see cref="Runner"/> that will be used to run the code</param>
		/// <returns>Whether the <see cref="InputText"/> is complete</returns>
		public bool Run(Runner runner)
		{
			//
			// Run the code and track errors
			//
			Errors.Clear();
			var complete = runner.Run(InputText, msg => {
				Errors.Add(msg);
			});
			OnPropertyChanged("HasErrors");

			//
			// Set the output
			//
			if (runner.HasResult) {
				OutputValue = runner.Result;
			}
			else {
				OutputValue = null;
				_hasOutput = false;
				OnPropertyChanged("HasOutput");
			}

			return complete;
		}

		void OnPropertyChanged(string name)
		{
			var p = PropertyChanged;
			if (p != null) {
				p(this, new PropertyChangedEventArgs(name));
			}
		}
	}
}
