using System;
using System.Diagnostics;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000FAB RID: 4011
	public static class ProfilerThreadCheck
	{
		// Token: 0x060060D5 RID: 24789 RVA: 0x0030E3DD File Offset: 0x0030C7DD
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.BeginSample(name);
			}
		}

		// Token: 0x060060D6 RID: 24790 RVA: 0x0030E3F0 File Offset: 0x0030C7F0
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
