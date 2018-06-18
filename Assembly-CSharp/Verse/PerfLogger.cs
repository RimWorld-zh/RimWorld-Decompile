using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x02000FA1 RID: 4001
	public static class PerfLogger
	{
		// Token: 0x06006089 RID: 24713 RVA: 0x0030D934 File Offset: 0x0030BD34
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x0600608A RID: 24714 RVA: 0x0030D951 File Offset: 0x0030BD51
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog == null) ? "" : PerfLogger.currentLog.ToString(), false);
			PerfLogger.Reset();
		}

		// Token: 0x0600608B RID: 24715 RVA: 0x0030D980 File Offset: 0x0030BD80
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

		// Token: 0x0600608C RID: 24716 RVA: 0x0030DA16 File Offset: 0x0030BE16
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x0030DA25 File Offset: 0x0030BE25
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x0600608E RID: 24718 RVA: 0x0030DA34 File Offset: 0x0030BE34
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}

		// Token: 0x04003F3E RID: 16190
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x04003F3F RID: 16191
		private static long start;

		// Token: 0x04003F40 RID: 16192
		private static long current;

		// Token: 0x04003F41 RID: 16193
		private static int indent;
	}
}
