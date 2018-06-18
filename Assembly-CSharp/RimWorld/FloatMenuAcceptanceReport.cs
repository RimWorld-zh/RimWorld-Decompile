using System;

namespace RimWorld
{
	// Token: 0x020005D4 RID: 1492
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001D0A RID: 7434 RVA: 0x000F8370 File Offset: 0x000F6770
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001D0B RID: 7435 RVA: 0x000F838C File Offset: 0x000F678C
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001D0C RID: 7436 RVA: 0x000F83A8 File Offset: 0x000F67A8
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001D0D RID: 7437 RVA: 0x000F83C4 File Offset: 0x000F67C4
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
		// (get) Token: 0x06001D0E RID: 7438 RVA: 0x000F83EC File Offset: 0x000F67EC
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

		// Token: 0x06001D0F RID: 7439 RVA: 0x000F8414 File Offset: 0x000F6814
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

		// Token: 0x06001D10 RID: 7440 RVA: 0x000F8440 File Offset: 0x000F6840
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x06001D11 RID: 7441 RVA: 0x000F845C File Offset: 0x000F685C
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000F848C File Offset: 0x000F688C
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
