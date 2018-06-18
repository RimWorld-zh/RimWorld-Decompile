using System;
using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	// Token: 0x02000F10 RID: 3856
	public static class DeepProfiler
	{
		// Token: 0x06005C6A RID: 23658 RVA: 0x002EE1A0 File Offset: 0x002EC5A0
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

		// Token: 0x06005C6B RID: 23659 RVA: 0x002EE214 File Offset: 0x002EC614
		public static void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().Start(label);
			}
		}

		// Token: 0x06005C6C RID: 23660 RVA: 0x002EE231 File Offset: 0x002EC631
		public static void End()
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().End();
			}
		}

		// Token: 0x04003D61 RID: 15713
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		// Token: 0x04003D62 RID: 15714
		private static readonly object DeepProfilersLock = new object();
	}
}
