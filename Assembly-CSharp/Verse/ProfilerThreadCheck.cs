using System;
using System.Diagnostics;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000FAF RID: 4015
	public static class ProfilerThreadCheck
	{
		// Token: 0x06006106 RID: 24838 RVA: 0x00310E21 File Offset: 0x0030F221
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.BeginSample(name);
			}
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x00310E34 File Offset: 0x0030F234
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
