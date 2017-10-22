using System.Collections.Generic;
using System.Threading;

namespace Verse
{
	public static class DeepProfiler
	{
		private static Dictionary<int, ThreadLocalDeepProfiler> deepProfilers = new Dictionary<int, ThreadLocalDeepProfiler>();

		private static readonly object DeepProfilersLock = new object();

		public static ThreadLocalDeepProfiler Get()
		{
			object deepProfilersLock = DeepProfiler.DeepProfilersLock;
			Monitor.Enter(deepProfilersLock);
			try
			{
				int managedThreadId = Thread.CurrentThread.ManagedThreadId;
				ThreadLocalDeepProfiler threadLocalDeepProfiler = default(ThreadLocalDeepProfiler);
				if (!DeepProfiler.deepProfilers.TryGetValue(managedThreadId, out threadLocalDeepProfiler))
				{
					threadLocalDeepProfiler = new ThreadLocalDeepProfiler();
					DeepProfiler.deepProfilers.Add(managedThreadId, threadLocalDeepProfiler);
					return threadLocalDeepProfiler;
				}
				return threadLocalDeepProfiler;
				IL_0049:
				ThreadLocalDeepProfiler result;
				return result;
			}
			finally
			{
				Monitor.Exit(deepProfilersLock);
			}
		}

		public static void Start(string label = null)
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().Start(label);
			}
		}

		public static void End()
		{
			if (Prefs.LogVerbose)
			{
				DeepProfiler.Get().End();
			}
		}
	}
}
