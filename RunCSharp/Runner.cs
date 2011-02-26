using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Mono.CSharp;

namespace RunCSharp
{
    public class Runner
    {
        Report _report;
        CompilerSettings _settings;
        Evaluator _eval;
        Action<AbstractMessage> _printer;

        public Runner()
        {
            _report = new Report(new Printer(this));
            _settings = new CommandLineParser(_report).ParseArguments (new string[] {});
            _eval = new Evaluator(_settings, _report);

            _eval.Run("using System;");
            _eval.Run("using System.Collections.Generic;");
            _eval.Run("using System.Linq;");
        }

        public object Result { get; private set; }

        public bool HasResult { get; private set; }

        public bool Run(string input, Action<AbstractMessage> printer) {
            _printer = printer;
            object result = null;
            bool result_set = false;
            var success = _eval.Evaluate(input, out result, out result_set) == null;
            HasResult = result_set;
            Result = result;
            return success;
        }

        void OnMessage(AbstractMessage msg)
        {
            var m = _printer;
            if (m != null)
            {
                m(msg);
            }
        }

        class Printer : ReportPrinter
        {
            Runner _r;
            public Printer(Runner r)
            {
                _r = r;
            }
            public override void Print(AbstractMessage msg)
            {
                base.Print(msg);

                _r.OnMessage(msg);
            }
        }
    }
}
