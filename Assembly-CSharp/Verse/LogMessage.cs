using UnityEngine;

namespace Verse
{
	public class LogMessage
	{
		public string text;

		public LogMessageType type = LogMessageType.Message;

		public int repeats = 1;

		private string stackTrace = (string)null;

		public Color Color
		{
			get
			{
				Color result;
				switch (this.type)
				{
				case LogMessageType.Message:
				{
					result = Color.white;
					break;
				}
				case LogMessageType.Warning:
				{
					result = Color.yellow;
					break;
				}
				case LogMessageType.Error:
				{
					result = Color.red;
					break;
				}
				default:
				{
					result = Color.white;
					break;
				}
				}
				return result;
			}
		}

		public string StackTrace
		{
			get
			{
				return (this.stackTrace == null) ? "No stack trace." : this.stackTrace;
			}
		}

		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = (string)null;
		}

		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		public override string ToString()
		{
			return (this.repeats <= 1) ? this.text : ("(" + this.repeats.ToString() + ") " + this.text);
		}

		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}
	}
}
