using System;
using System.Diagnostics;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000FAA RID: 4010
	public static class ProfilerThreadCheck
	{
		// Token: 0x060060D3 RID: 24787 RVA: 0x0030E4B9 File Offset: 0x0030C8B9
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.BeginSample(name);
			}
		}

		// Token: 0x060060D4 RID: 24788 RVA: 0x0030E4CC File Offset: 0x0030C8CC
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void EndSample()
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.EndSample();
			}
		}
	}
}
