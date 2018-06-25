using System;

namespace Verse
{
	// Token: 0x02000EE2 RID: 3810
	public struct AcceptanceReport
	{
		// Token: 0x04003C7C RID: 15484
		private string reasonTextInt;

		// Token: 0x04003C7D RID: 15485
		private bool acceptedInt;

		// Token: 0x06005A2A RID: 23082 RVA: 0x002E48AF File Offset: 0x002E2CAF
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06005A2B RID: 23083 RVA: 0x002E48C0 File Offset: 0x002E2CC0
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06005A2C RID: 23084 RVA: 0x002E48DC File Offset: 0x002E2CDC
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06005A2D RID: 23085 RVA: 0x002E48F8 File Offset: 0x002E2CF8
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

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06005A2E RID: 23086 RVA: 0x002E4924 File Offset: 0x002E2D24
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

		// Token: 0x06005A2F RID: 23087 RVA: 0x002E4950 File Offset: 0x002E2D50
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

		// Token: 0x06005A30 RID: 23088 RVA: 0x002E497C File Offset: 0x002E2D7C
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}
	}
}
