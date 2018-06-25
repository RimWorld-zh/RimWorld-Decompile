using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x02000FA5 RID: 4005
	public static class PerfLogger
	{
		// Token: 0x04003F53 RID: 16211
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x04003F54 RID: 16212
		private static long start;

		// Token: 0x04003F55 RID: 16213
		private static long current;

		// Token: 0x04003F56 RID: 16214
		private static int indent;

		// Token: 0x060060BC RID: 24764 RVA: 0x00310058 File Offset: 0x0030E458
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x060060BD RID: 24765 RVA: 0x00310075 File Offset: 0x0030E475
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog == null) ? "" : PerfLogger.currentLog.ToString(), false);
			PerfLogger.Reset();
		}

		// Token: 0x060060BE RID: 24766 RVA: 0x003100A4 File Offset: 0x0030E4A4
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

		// Token: 0x060060BF RID: 24767 RVA: 0x0031013A File Offset: 0x0030E53A
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x060060C0 RID: 24768 RVA: 0x00310149 File Offset: 0x0030E549
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x060060C1 RID: 24769 RVA: 0x00310158 File Offset: 0x0030E558
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}
	}
}
