using System;

namespace Verse.AI
{
	// Token: 0x02000A51 RID: 2641
	public static class JobFailReason
	{
		// Token: 0x0400253B RID: 9531
		private static string lastReason;

		// Token: 0x0400253C RID: 9532
		private static string lastCustomJobString;

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x001F4144 File Offset: 0x001F2544
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003AD8 RID: 15064 RVA: 0x001F4160 File Offset: 0x001F2560
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06003AD9 RID: 15065 RVA: 0x001F4180 File Offset: 0x001F2580
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x001F419A File Offset: 0x001F259A
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x001F41A9 File Offset: 0x001F25A9
		public static void Clear()
		{
			JobFailReason.lastReason = null;
			JobFailReason.lastCustomJobString = null;
		}
	}
}
