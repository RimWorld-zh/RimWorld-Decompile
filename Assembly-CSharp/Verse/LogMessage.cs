using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F16 RID: 3862
	public class LogMessage
	{
		// Token: 0x06005C86 RID: 23686 RVA: 0x002EE6F5 File Offset: 0x002ECAF5
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		// Token: 0x06005C87 RID: 23687 RVA: 0x002EE728 File Offset: 0x002ECB28
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06005C88 RID: 23688 RVA: 0x002EE75C File Offset: 0x002ECB5C
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

		// Token: 0x17000EE0 RID: 3808
		// (get) Token: 0x06005C89 RID: 23689 RVA: 0x002EE7B8 File Offset: 0x002ECBB8
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

		// Token: 0x06005C8A RID: 23690 RVA: 0x002EE7EC File Offset: 0x002ECBEC
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

		// Token: 0x06005C8B RID: 23691 RVA: 0x002EE840 File Offset: 0x002ECC40
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}

		// Token: 0x04003D74 RID: 15732
		public string text;

		// Token: 0x04003D75 RID: 15733
		public LogMessageType type = LogMessageType.Message;

		// Token: 0x04003D76 RID: 15734
		public int repeats = 1;

		// Token: 0x04003D77 RID: 15735
		private string stackTrace = null;
	}
}
