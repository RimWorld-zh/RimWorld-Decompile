using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x02000FA6 RID: 4006
	public static class PerfLogger
	{
		// Token: 0x04003F5B RID: 16219
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x04003F5C RID: 16220
		private static long start;

		// Token: 0x04003F5D RID: 16221
		private static long current;

		// Token: 0x04003F5E RID: 16222
		private static int indent;

		// Token: 0x060060BC RID: 24764 RVA: 0x0031029C File Offset: 0x0030E69C
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x060060BD RID: 24765 RVA: 0x003102B9 File Offset: 0x0030E6B9
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog == null) ? "" : PerfLogger.currentLog.ToString(), false);
			PerfLogger.Reset();
		}

		// Token: 0x060060BE RID: 24766 RVA: 0x003102E8 File Offset: 0x0030E6E8
		public static void Record(string label)
		{
			long timestamp = Stopwatch.GetTimestamp();
			if (PerfLogger.currentLog == null)
			{
				PerfLogger.currentLog = new StringBuilder();
			}
			PerfLogger.currentLog.AppendLine(string.Format("{0}: {3}{1} ({2})", new object[]
			{
				(timestamp - PerfLogger.start) * 1000L / Stopwatch.Frequency,
				label,
				(timestamp - PerfLogger.current) * 1000L / Stopwatch.Frequency,
				new string(' ', PerfLogger.indent * 2)
			}));
			PerfLogger.current = timestamp;
		}

		// Token: 0x060060BF RID: 24767 RVA: 0x0031037E File Offset: 0x0030E77E
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x060060C0 RID: 24768 RVA: 0x0031038D File Offset: 0x0030E78D
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x0031039C File Offset: 0x0030E79C
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}
	}
}
