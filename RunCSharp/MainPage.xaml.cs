using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace RunCSharp
{
    public partial class MainPage : UserControl
    {
        public ObservableCollection<RunBlock> History { get; private set; }

        Runner _runner;

        public MainPage()
        {
            _runner = new Runner();

            History = new ObservableCollection<RunBlock>();

            History.Add(new RunBlock("Math.Abs (-42);"));
            History.Add(new RunBlock("Math.Sin (Math.Pi / 2);"));

            InitializeComponent();
            LayoutRoot.DataContext = this;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            var tb = (TextBox)sender;
            var block = (RunBlock)tb.DataContext;
            block.InputText = tb.Text;

            if (e.Key == Key.Enter)
            {
                if (block.Run(_runner))
                {
                    e.Handled = true;

                    CreateNewBlockIfNeeded();
                }
                else
                {
                    //
                    // More to edit
                    //
                }
            }
        }

        bool NewBlockNeeded
        {
            get
            {
                var last = History.LastOrDefault();
                if (last == null) return true;
                return !string.IsNullOrWhiteSpace(last.InputText);
            }
        }

        private void CreateNewBlockIfNeeded()
        {
            if (NewBlockNeeded)
            {
                var newBlock = new RunBlock();
                History.Add(newBlock);
                HistoryBox.SelectedItem = newBlock;
            }
        }
    }
}
