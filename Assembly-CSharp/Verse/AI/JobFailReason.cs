using System;

namespace Verse.AI
{
	// Token: 0x02000A54 RID: 2644
	public static class JobFailReason
	{
		// Token: 0x0400254C RID: 9548
		private static string lastReason;

		// Token: 0x0400254D RID: 9549
		private static string lastCustomJobString;

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x001F459C File Offset: 0x001F299C
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x001F45B8 File Offset: 0x001F29B8
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x001F45D8 File Offset: 0x001F29D8
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x001F45F2 File Offset: 0x001F29F2
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x001F4601 File Offset: 0x001F2A01
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
		}
	}
}
