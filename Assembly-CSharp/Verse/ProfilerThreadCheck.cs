#define ENABLE_PROFILER
using System.Diagnostics;
using UnityEngine.Profiling;

namespace Verse
{
	public static class ProfilerThreadCheck
	{
		[Conditional("UNITY_EDITOR")]
		public static void BeginSample(string name)
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.BeginSample(name);
			}
		}

		[Conditional("UNITY_EDITOR")]
		public static void EndSample()
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.EndSample();
			}
		}
	}
}
