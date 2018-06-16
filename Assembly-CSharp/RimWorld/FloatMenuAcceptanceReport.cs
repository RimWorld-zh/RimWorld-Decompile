using System;

namespace RimWorld
{
	// Token: 0x020005D4 RID: 1492
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x000F82F8 File Offset: 0x000F66F8
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x000F8314 File Offset: 0x000F6714
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x000F8330 File Offset: 0x000F6730
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x000F834C File Offset: 0x000F674C
		public static FloatMenuAcceptanceReport WasAccepted
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = true
				};
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x06001D0C RID: 7436 RVA: 0x000F8374 File Offset: 0x000F6774
		public static FloatMenuAcceptanceReport WasRejected
		{
			get
			{
				return new FloatMenuAcceptanceReport
				{
					acceptedInt = false
				};
			}
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000F839C File Offset: 0x000F679C
		public static implicit operator FloatMenuAcceptanceReport(bool value)
		{
			FloatMenuAcceptanceReport result;
			if (value)
			{
				result = FloatMenuAcceptanceReport.WasAccepted;
			}
			else
			{
				result = FloatMenuAcceptanceReport.WasRejected;
			}
			return result;
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000F83C8 File Offset: 0x000F67C8
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000F83E4 File Offset: 0x000F67E4
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x06001D10 RID: 7440 RVA: 0x000F8414 File Offset: 0x000F6814
		public static FloatMenuAcceptanceReport WithFailMessage(string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failMessageInt = failMessage
			};
		}

		// Token: 0x0400115A RID: 4442
		private string failMessageInt;

		// Token: 0x0400115B RID: 4443
		private string failReasonInt;

		// Token: 0x0400115C RID: 4444
		private bool acceptedInt;
	}
}
