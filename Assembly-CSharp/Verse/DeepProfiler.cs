using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x02000F14 RID: 3860
	public static class DeepProfiler
	{
		// Token: 0x04003D76 RID: 15734
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x04003D77 RID: 15735
		private static readonly object DeepProfilersLock = new object();

		// Token: 0x06005C9C RID: 23708 RVA: 0x002F084C File Offset: 0x002EEC4C
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

		// Token: 0x06005C9D RID: 23709 RVA: 0x002F08C0 File Offset: 0x002EECC0
		public static void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().Start(label);
			}
		}

		// Token: 0x06005C9E RID: 23710 RVA: 0x002F08DD File Offset: 0x002EECDD
		public static void End()
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().End();
			}
		}
	}
}
