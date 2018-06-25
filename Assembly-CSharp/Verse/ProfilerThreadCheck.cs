using System;
using System.Diagnostics;
using UnityEngine.Profiling;

namespace Verse
{
	// Token: 0x02000FAE RID: 4014
	public static class ProfilerThreadCheck
	{
		// Token: 0x06006106 RID: 24838 RVA: 0x00310BDD File Offset: 0x0030EFDD
		[Conditional("UNITY_EDITOR")]
		[Conditional("BUILD_AND_RUN")]
		public static void BeginSample(string name)
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.BeginSample(name);
			}
		}

		// Token: 0x06006107 RID: 24839 RVA: 0x00310BF0 File Offset: 0x0030EFF0
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
