using System;

namespace Verse
{
	// Token: 0x02000EE1 RID: 3809
	public struct AcceptanceReport
	{
		// Token: 0x06005A08 RID: 23048 RVA: 0x002E2683 File Offset: 0x002E0A83
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06005A09 RID: 23049 RVA: 0x002E2694 File Offset: 0x002E0A94
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06005A0A RID: 23050 RVA: 0x002E26B0 File Offset: 0x002E0AB0
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06005A0B RID: 23051 RVA: 0x002E26CC File Offset: 0x002E0ACC
		public static AcceptanceReport WasAccepted
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06005A0C RID: 23052 RVA: 0x002E26F8 File Offset: 0x002E0AF8
		public static AcceptanceReport WasRejected
		{
			get
			{
				return new AcceptanceReport("")
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x06005A0D RID: 23053 RVA: 0x002E2724 File Offset: 0x002E0B24
		public static implicit operator AcceptanceReport(bool value)
		{
			AcceptanceReport result;
			if (value)
			{
				result = AcceptanceReport.WasAccepted;
			}
			else
			{
				result = AcceptanceReport.WasRejected;
			}
			return result;
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x002E2750 File Offset: 0x002E0B50
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x04003C65 RID: 15461
		private string reasonTextInt;

		// Token: 0x04003C66 RID: 15462
		private bool acceptedInt;
	}
}
