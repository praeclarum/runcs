using System;
using Mono.CSharp;

namespace RunCSharp
{
	/// <summary>
	/// Runs C# code by wrapping the Mono.CSharp.Evaluator
	/// </summary>
    public class Runner
    {
        Report _report;
        CompilerSettings _settings;
        Evaluator _eval;
        Action<AbstractMessage> _printer;

		/// <summary>
		/// Initializes the evaluator and includes a few basic System libraries
		/// </summary>
        public Runner()
        {
            _report = new Report(new Printer(this));
            _settings = new CommandLineParser(_report).ParseArguments (new string[] {});
            _eval = new Evaluator(_settings, _report);

            _eval.Run("using System;");
            _eval.Run("using System.Collections.Generic;");
            _eval.Run("using System.Linq;");
        }

		/// <summary>
		/// The value of the last line of code executed
		/// </summary>
        public object Result { get; private set; }

		/// <summary>
		/// Whether the last line of code produced a result
		/// </summary>
        public bool HasResult { get; private set; }

		/// <summary>
		/// Run the given input and report any compiler messages to <paramref name="printer"/>
		/// </summary>
		/// <param name="input">The line of C# to execute</param>
		/// <param name="printer">Function to print compiler messages</param>
		/// <returns>Whether the input is complete or not</returns>
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

		/// <summary>
		/// Little printer object that forwards all calls to <see cref="Runner"/>.
		/// </summary>
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
