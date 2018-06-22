using System;
using System.Diagnostics;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000FAA RID: 4010
	public static class ProfilerThreadCheck
	{
		// Token: 0x060060FC RID: 24828 RVA: 0x0031055D File Offset: 0x0030E95D
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.BeginSample(name);
			}
		}

		// Token: 0x060060FD RID: 24829 RVA: 0x00310570 File Offset: 0x0030E970
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
