using System;

namespace Verse
{
	// Token: 0x02000EDF RID: 3807
	public struct AcceptanceReport
	{
		// Token: 0x04003C74 RID: 15476
		private string reasonTextInt;

		// Token: 0x04003C75 RID: 15477
		private bool acceptedInt;

		// Token: 0x06005A27 RID: 23079 RVA: 0x002E456F File Offset: 0x002E296F
		public AcceptanceReport(string reasonText)
		{
			this.acceptedInt = false;
			this.reasonTextInt = reasonText;
		}

		// Token: 0x17000E2A RID: 3626
		// (get) Token: 0x06005A28 RID: 23080 RVA: 0x002E4580 File Offset: 0x002E2980
		public string Reason
		{
			get
			{
				return this.reasonTextInt;
			}
		}

		// Token: 0x17000E2B RID: 3627
		// (get) Token: 0x06005A29 RID: 23081 RVA: 0x002E459C File Offset: 0x002E299C
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x17000E2C RID: 3628
		// (get) Token: 0x06005A2A RID: 23082 RVA: 0x002E45B8 File Offset: 0x002E29B8
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

		// Token: 0x17000E2D RID: 3629
		// (get) Token: 0x06005A2B RID: 23083 RVA: 0x002E45E4 File Offset: 0x002E29E4
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

		// Token: 0x06005A2C RID: 23084 RVA: 0x002E4610 File Offset: 0x002E2A10
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

		// Token: 0x06005A2D RID: 23085 RVA: 0x002E463C File Offset: 0x002E2A3C
		public static implicit operator AcceptanceReport(string value)
		{
			return new AcceptanceReport(value);
		}
	}
}
