using System;

namespace Verse.AI
{
	// Token: 0x02000A55 RID: 2645
	public static class JobFailReason
	{
		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06003ADA RID: 15066 RVA: 0x001F3D78 File Offset: 0x001F2178
		public static string Reason
		{
			get
			{
				return JobFailReason.lastReason;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06003ADB RID: 15067 RVA: 0x001F3D94 File Offset: 0x001F2194
		public static bool HaveReason
		{
			get
			{
				return JobFailReason.lastReason != null;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06003ADC RID: 15068 RVA: 0x001F3DB4 File Offset: 0x001F21B4
		public static string CustomJobString
		{
			get
			{
				return JobFailReason.lastCustomJobString;
			}
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x001F3DCE File Offset: 0x001F21CE
		public static void Is(string reason, string customJobString = null)
		{
			JobFailReason.lastReason = reason;
			JobFailReason.lastCustomJobString = customJobString;
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x001F3DDD File Offset: 0x001F21DD
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
