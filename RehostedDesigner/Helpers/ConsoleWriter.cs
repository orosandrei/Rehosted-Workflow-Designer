namespace RehostedWorkflowDesigner.Helpers
{
	using System;
	using System.IO;
	using System.Text;

	public sealed class ConsoleWriter : TextWriter
	{
		public override Encoding Encoding => Encoding.UTF8;

		public event EventHandler<ConsoleWriterEventArgs> WriteEvent;
		public event EventHandler<ConsoleWriterEventArgs> WriteLineEvent;

		public override void Write(string value)
		{
			WriteEvent?.Invoke(this, new ConsoleWriterEventArgs(value));
			base.Write(value);
		}

		public override void WriteLine(string value)
		{
			WriteLineEvent?.Invoke(this, new ConsoleWriterEventArgs(value));
			base.WriteLine(value);
		}
	}
}
