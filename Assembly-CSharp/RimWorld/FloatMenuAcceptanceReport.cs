using System;

namespace RimWorld
{
	// Token: 0x020005D2 RID: 1490
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x04001157 RID: 4439
		private string failMessageInt;

		// Token: 0x04001158 RID: 4440
		private string failReasonInt;

		// Token: 0x04001159 RID: 4441
		private bool acceptedInt;

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x000F8514 File Offset: 0x000F6914
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001D06 RID: 7430 RVA: 0x000F8530 File Offset: 0x000F6930
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001D07 RID: 7431 RVA: 0x000F854C File Offset: 0x000F694C
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001D08 RID: 7432 RVA: 0x000F8568 File Offset: 0x000F6968
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
		// (get) Token: 0x06001D09 RID: 7433 RVA: 0x000F8590 File Offset: 0x000F6990
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

		// Token: 0x06001D0A RID: 7434 RVA: 0x000F85B8 File Offset: 0x000F69B8
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

		// Token: 0x06001D0B RID: 7435 RVA: 0x000F85E4 File Offset: 0x000F69E4
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x06001D0C RID: 7436 RVA: 0x000F8600 File Offset: 0x000F6A00
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x06001D0D RID: 7437 RVA: 0x000F8630 File Offset: 0x000F6A30
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
