using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F1A RID: 3866
	public class LogMessage
	{
		// Token: 0x04003D90 RID: 15760
		public string text;

		// Token: 0x04003D91 RID: 15761
		public LogMessageType type = LogMessageType.Message;

		// Token: 0x04003D92 RID: 15762
		public int repeats = 1;

		// Token: 0x04003D93 RID: 15763
		private string stackTrace = null;

		// Token: 0x06005CB6 RID: 23734 RVA: 0x002F109D File Offset: 0x002EF49D
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x002F10D0 File Offset: 0x002EF4D0
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06005CB8 RID: 23736 RVA: 0x002F1104 File Offset: 0x002EF504
		public Color Color
		{
			get
			{
				LogMessageType logMessageType = this.type;
				Color result;
				if (logMessageType != LogMessageType.Message)
				{
					if (logMessageType != LogMessageType.Warning)
					{
						if (logMessageType != LogMessageType.Error)
						{
							result = Color.white;
						}
						else
						{
							result = Color.red;
						}
					}
					else
					{
						result = Color.yellow;
					}
				}
				else
				{
					result = Color.white;
				}
				return result;
			}
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06005CB9 RID: 23737 RVA: 0x002F1160 File Offset: 0x002EF560
		public string StackTrace
		{
			get
			{
				string result;
				if (this.stackTrace != null)
				{
					result = this.stackTrace;
				}
				else
				{
					result = "No stack trace.";
				}
				return result;
			}
		}

		// Token: 0x06005CBA RID: 23738 RVA: 0x002F1194 File Offset: 0x002EF594
		public override string ToString()
		{
			string result;
			if (this.repeats > 1)
			{
				result = "(" + this.repeats.ToString() + ") " + this.text;
			}
			else
			{
				result = this.text;
			}
			return result;
		}

		// Token: 0x06005CBB RID: 23739 RVA: 0x002F11E8 File Offset: 0x002EF5E8
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}
	}
}
