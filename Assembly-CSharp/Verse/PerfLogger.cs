using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x02000FA1 RID: 4001
	public static class PerfLogger
	{
		// Token: 0x060060B2 RID: 24754 RVA: 0x0030F9D8 File Offset: 0x0030DDD8
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x060060B3 RID: 24755 RVA: 0x0030F9F5 File Offset: 0x0030DDF5
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog == null) ? "" : PerfLogger.currentLog.ToString(), false);
			PerfLogger.Reset();
		}

		// Token: 0x060060B4 RID: 24756 RVA: 0x0030FA24 File Offset: 0x0030DE24
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

		// Token: 0x060060B5 RID: 24757 RVA: 0x0030FABA File Offset: 0x0030DEBA
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x060060B6 RID: 24758 RVA: 0x0030FAC9 File Offset: 0x0030DEC9
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x060060B7 RID: 24759 RVA: 0x0030FAD8 File Offset: 0x0030DED8
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}

		// Token: 0x04003F50 RID: 16208
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x04003F51 RID: 16209
		private static long start;

		// Token: 0x04003F52 RID: 16210
		private static long current;

		// Token: 0x04003F53 RID: 16211
		private static int indent;
	}
}
