using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x02000F11 RID: 3857
	public static class DeepProfiler
	{
		// Token: 0x06005C6C RID: 23660 RVA: 0x002EE0C4 File Offset: 0x002EC4C4
		public static ThreadLocalDeepProfiler Get()
		{
			object deepProfilersLock = DeepProfiler.DeepProfilersLock;
			ThreadLocalDeepProfiler result;
			lock (deepProfilersLock)
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				ThreadLocalDeepProfiler threadLocalDeepProfiler;
				if (!DeepProfiler.deepProfilers.TryGetValue(managedThreadId, out threadLocalDeepProfiler))
				{
					threadLocalDeepProfiler = new ThreadLocalDeepProfiler();
					DeepProfiler.deepProfilers.Add(managedThreadId, threadLocalDeepProfiler);
					result = threadLocalDeepProfiler;
				}
				else
				{
					result = threadLocalDeepProfiler;
				}
			}
			return result;
		}

		// Token: 0x06005C6D RID: 23661 RVA: 0x002EE138 File Offset: 0x002EC538
		public static void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().Start(label);
			}
		}

		// Token: 0x06005C6E RID: 23662 RVA: 0x002EE155 File Offset: 0x002EC555
		public static void End()
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().End();
			}
		}

		// Token: 0x04003D62 RID: 15714
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x04003D63 RID: 15715
		private static readonly object DeepProfilersLock = new object();
	}
}
