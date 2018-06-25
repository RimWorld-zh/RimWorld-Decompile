using System;

namespace Verse.AI
{
	// Token: 0x02000A53 RID: 2643
	public static class JobFailReason
	{
		// Token: 0x0400253C RID: 9532
		private static string lastReason;

		// Token: 0x0400253D RID: 9533
		private static string lastCustomJobString;

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x001F4270 File Offset: 0x001F2670
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x001F428C File Offset: 0x001F268C
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x001F42AC File Offset: 0x001F26AC
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x001F42C6 File Offset: 0x001F26C6
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x001F42D5 File Offset: 0x001F26D5
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
		}
	}
}
