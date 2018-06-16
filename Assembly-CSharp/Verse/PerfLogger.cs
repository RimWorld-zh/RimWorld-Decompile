using System;
using System.Diagnostics;
using System.Text;

namespace Verse
{
	// Token: 0x02000FA2 RID: 4002
	public static class PerfLogger
	{
		// Token: 0x0600608B RID: 24715 RVA: 0x0030D858 File Offset: 0x0030BC58
		public static void Reset()
		{
			PerfLogger.currentLog = null;
			PerfLogger.start = Stopwatch.GetTimestamp();
			PerfLogger.current = PerfLogger.start;
		}

		// Token: 0x0600608C RID: 24716 RVA: 0x0030D875 File Offset: 0x0030BC75
		public static void Flush()
		{
			Log.Message((PerfLogger.currentLog == null) ? "" : PerfLogger.currentLog.ToString(), false);
			PerfLogger.Reset();
		}

		// Token: 0x0600608D RID: 24717 RVA: 0x0030D8A4 File Offset: 0x0030BCA4
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

		// Token: 0x0600608E RID: 24718 RVA: 0x0030D93A File Offset: 0x0030BD3A
		public static void Indent()
		{
			PerfLogger.indent++;
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x0030D949 File Offset: 0x0030BD49
		public static void Outdent()
		{
			PerfLogger.indent--;
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x0030D958 File Offset: 0x0030BD58
		public static float Duration()
		{
			return (float)(Stopwatch.GetTimestamp() - PerfLogger.start) / (float)Stopwatch.Frequency;
		}

		// Token: 0x04003F3F RID: 16191
		public static StringBuilder currentLog = new StringBuilder();

		// Token: 0x04003F40 RID: 16192
		private static long start;

		// Token: 0x04003F41 RID: 16193
		private static long current;

		// Token: 0x04003F42 RID: 16194
		private static int indent;
	}
}
