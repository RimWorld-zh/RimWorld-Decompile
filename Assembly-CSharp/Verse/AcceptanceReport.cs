using System;

namespace Verse
{
	// Token: 0x02000EE1 RID: 3809
	public struct AcceptanceReport
	{
		// Token: 0x04003C74 RID: 15476
		private string reasonTextInt;

		// Token: 0x04003C75 RID: 15477
		private bool acceptedInt;

		// Token: 0x06005A2A RID: 23082 RVA: 0x002E468F File Offset: 0x002E2A8F
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x17000E29 RID: 3625
		// (get) Token: 0x06005A2B RID: 23083 RVA: 0x002E46A0 File Offset: 0x002E2AA0
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06005A2C RID: 23084 RVA: 0x002E46BC File Offset: 0x002E2ABC
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06005A2D RID: 23085 RVA: 0x002E46D8 File Offset: 0x002E2AD8
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
		// (get) Token: 0x06005A2E RID: 23086 RVA: 0x002E4704 File Offset: 0x002E2B04
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

		// Token: 0x06005A2F RID: 23087 RVA: 0x002E4730 File Offset: 0x002E2B30
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

		// Token: 0x06005A30 RID: 23088 RVA: 0x002E475C File Offset: 0x002E2B5C
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}
	}
}
