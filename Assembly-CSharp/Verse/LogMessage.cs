using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F19 RID: 3865
	public class LogMessage
	{
		// Token: 0x04003D88 RID: 15752
		public string text;

		// Token: 0x04003D89 RID: 15753
		public LogMessageType type = LogMessageType.Message;

		// Token: 0x04003D8A RID: 15754
		public int repeats = 1;

		// Token: 0x04003D8B RID: 15755
		private string stackTrace = null;

		// Token: 0x06005CB6 RID: 23734 RVA: 0x002F0E7D File Offset: 0x002EF27D
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		// Token: 0x06005CB7 RID: 23735 RVA: 0x002F0EB0 File Offset: 0x002EF2B0
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		// Token: 0x17000EE1 RID: 3809
		// (get) Token: 0x06005CB8 RID: 23736 RVA: 0x002F0EE4 File Offset: 0x002EF2E4
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
		// (get) Token: 0x06005CB9 RID: 23737 RVA: 0x002F0F40 File Offset: 0x002EF340
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

		// Token: 0x06005CBA RID: 23738 RVA: 0x002F0F74 File Offset: 0x002EF374
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

		// Token: 0x06005CBB RID: 23739 RVA: 0x002F0FC8 File Offset: 0x002EF3C8
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}
	}
}
