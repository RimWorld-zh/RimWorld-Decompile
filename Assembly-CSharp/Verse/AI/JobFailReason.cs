using System;

namespace Verse.AI
{
	// Token: 0x02000A55 RID: 2645
	public static class JobFailReason
	{
		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x001F3E4C File Offset: 0x001F224C
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003ADD RID: 15069 RVA: 0x001F3E68 File Offset: 0x001F2268
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003ADE RID: 15070 RVA: 0x001F3E88 File Offset: 0x001F2288
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x001F3EA2 File Offset: 0x001F22A2
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x001F3EB1 File Offset: 0x001F22B1
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
		}

		// Token: 0x04002540 RID: 9536
		private static string lastReason;

		// Token: 0x04002541 RID: 9537
		private static string lastCustomJobString;
	}
}
