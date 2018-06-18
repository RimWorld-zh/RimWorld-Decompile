using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F15 RID: 3861
	public class LogMessage
	{
		// Token: 0x06005C84 RID: 23684 RVA: 0x002EE7D1 File Offset: 0x002ECBD1
		public LogMessage(string text)
		{
			this.text = text;
			this.type = LogMessageType.Message;
			this.stackTrace = null;
		}

		// Token: 0x06005C85 RID: 23685 RVA: 0x002EE804 File Offset: 0x002ECC04
		public LogMessage(LogMessageType type, string text, string stackTrace)
		{
			this.text = text;
			this.type = type;
			this.stackTrace = stackTrace;
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06005C86 RID: 23686 RVA: 0x002EE838 File Offset: 0x002ECC38
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

		// Token: 0x17000EDF RID: 3807
		// (get) Token: 0x06005C87 RID: 23687 RVA: 0x002EE894 File Offset: 0x002ECC94
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

		// Token: 0x06005C88 RID: 23688 RVA: 0x002EE8C8 File Offset: 0x002ECCC8
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

		// Token: 0x06005C89 RID: 23689 RVA: 0x002EE91C File Offset: 0x002ECD1C
		public bool CanCombineWith(LogMessage other)
		{
			return this.text == other.text && this.type == other.type;
		}

		// Token: 0x04003D73 RID: 15731
		public string text;

		// Token: 0x04003D74 RID: 15732
		public LogMessageType type = LogMessageType.Message;

		// Token: 0x04003D75 RID: 15733
		public int repeats = 1;

		// Token: 0x04003D76 RID: 15734
		private string stackTrace = null;
	}
}
