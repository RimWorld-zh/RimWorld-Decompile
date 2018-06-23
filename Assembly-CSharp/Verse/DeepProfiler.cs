using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x02000F10 RID: 3856
	public static class DeepProfiler
	{
		// Token: 0x04003D73 RID: 15731
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x04003D74 RID: 15732
		private static readonly object DeepProfilersLock = new object();

		// Token: 0x06005C92 RID: 23698 RVA: 0x002F01CC File Offset: 0x002EE5CC
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

		// Token: 0x06005C93 RID: 23699 RVA: 0x002F0240 File Offset: 0x002EE640
		public static void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().Start(label);
			}
		}

		// Token: 0x06005C94 RID: 23700 RVA: 0x002F025D File Offset: 0x002EE65D
		public static void End()
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().End();
			}
		}
	}
}
