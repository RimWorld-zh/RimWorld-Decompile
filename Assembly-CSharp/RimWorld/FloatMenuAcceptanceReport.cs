using System;

namespace RimWorld
{
	// Token: 0x020005D0 RID: 1488
	public struct FloatMenuAcceptanceReport
	{
		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06001D01 RID: 7425 RVA: 0x000F83C4 File Offset: 0x000F67C4
		public bool Accepted
		{
			get
			{
				return this.acceptedInt;
			}
		}

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x06001D02 RID: 7426 RVA: 0x000F83E0 File Offset: 0x000F67E0
		public string FailMessage
		{
			get
			{
				return this.failMessageInt;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x06001D03 RID: 7427 RVA: 0x000F83FC File Offset: 0x000F67FC
		public string FailReason
		{
			get
			{
				return this.failReasonInt;
			}
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06001D04 RID: 7428 RVA: 0x000F8418 File Offset: 0x000F6818
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
		// (get) Token: 0x06001D05 RID: 7429 RVA: 0x000F8440 File Offset: 0x000F6840
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

		// Token: 0x06001D06 RID: 7430 RVA: 0x000F8468 File Offset: 0x000F6868
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

		// Token: 0x06001D07 RID: 7431 RVA: 0x000F8494 File Offset: 0x000F6894
		public static implicit operator bool(FloatMenuAcceptanceReport rep)
		{
			return rep.Accepted;
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000F84B0 File Offset: 0x000F68B0
		public static FloatMenuAcceptanceReport WithFailReason(string failReason)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failReasonInt = failReason
			};
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000F84E0 File Offset: 0x000F68E0
		public static FloatMenuAcceptanceReport WithFailMessage(string failMessage)
		{
			return new FloatMenuAcceptanceReport
			{
				acceptedInt = false,
				failMessageInt = failMessage
			};
		}

		// Token: 0x04001157 RID: 4439
		private string failMessageInt;

		// Token: 0x04001158 RID: 4440
		private string failReasonInt;

		// Token: 0x04001159 RID: 4441
		private bool acceptedInt;
	}
}
