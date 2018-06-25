using System;

namespace RimWorld
{
	// Token: 0x020005D2 RID: 1490
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x0400115B RID: 4443
		private string failMessageInt;

		// Token: 0x0400115C RID: 4444
		private string failReasonInt;

		// Token: 0x0400115D RID: 4445
		private bool acceptedInt;

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x000F877C File Offset: 0x000F6B7C
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x000F8798 File Offset: 0x000F6B98
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x000F87B4 File Offset: 0x000F6BB4
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x000F87D0 File Offset: 0x000F6BD0
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
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x000F87F8 File Offset: 0x000F6BF8
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

		// Token: 0x06001D09 RID: 7433 RVA: 0x000F8820 File Offset: 0x000F6C20
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

		// Token: 0x06001D0A RID: 7434 RVA: 0x000F884C File Offset: 0x000F6C4C
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x06001D0B RID: 7435 RVA: 0x000F8868 File Offset: 0x000F6C68
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000F8898 File Offset: 0x000F6C98
		public static FloatMenuAcceptanceReport WithFailMessage(string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failMessageInt = failMessage
			};
		}
	}
}
