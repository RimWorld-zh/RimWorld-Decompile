using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F15 RID: 3861
	public class LogMessage
	{
		// Token: 0x04003D85 RID: 15749
		public string text;

		// Token: 0x04003D86 RID: 15750
		public LogMessageType type = LogMessageType.Message;

		// Token: 0x04003D87 RID: 15751
		public int repeats = 1;

		// Token: 0x04003D88 RID: 15752
		private string stackTrace = null;

		// Token: 0x06005CAC RID: 23724 RVA: 0x002F07FD File Offset: 0x002EEBFD
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		// Token: 0x06005CAD RID: 23725 RVA: 0x002F0830 File Offset: 0x002EEC30
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		// Token: 0x17000EE2 RID: 3810
		// (get) Token: 0x06005CAE RID: 23726 RVA: 0x002F0864 File Offset: 0x002EEC64
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

		// Token: 0x17000EE3 RID: 3811
		// (get) Token: 0x06005CAF RID: 23727 RVA: 0x002F08C0 File Offset: 0x002EECC0
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

		// Token: 0x06005CB0 RID: 23728 RVA: 0x002F08F4 File Offset: 0x002EECF4
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

		// Token: 0x06005CB1 RID: 23729 RVA: 0x002F0948 File Offset: 0x002EED48
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}
	}
}
