using System;

namespace Verse
{
	// Token: 0x02000EE0 RID: 3808
	public struct AcceptanceReport
	{
		// Token: 0x06005A06 RID: 23046 RVA: 0x002E275B File Offset: 0x002E0B5B
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x17000E27 RID: 3623
		// (get) Token: 0x06005A07 RID: 23047 RVA: 0x002E276C File Offset: 0x002E0B6C
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06005A08 RID: 23048 RVA: 0x002E2788 File Offset: 0x002E0B88
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06005A09 RID: 23049 RVA: 0x002E27A4 File Offset: 0x002E0BA4
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

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06005A0A RID: 23050 RVA: 0x002E27D0 File Offset: 0x002E0BD0
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

		// Token: 0x06005A0B RID: 23051 RVA: 0x002E27FC File Offset: 0x002E0BFC
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

		// Token: 0x06005A0C RID: 23052 RVA: 0x002E2828 File Offset: 0x002E0C28
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}

		// Token: 0x04003C64 RID: 15460
		private string reasonTextInt;

		// Token: 0x04003C65 RID: 15461
		private bool acceptedInt;
	}
}
