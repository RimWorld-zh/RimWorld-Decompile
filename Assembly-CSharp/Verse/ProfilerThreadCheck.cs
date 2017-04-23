using System;
using UnityEngine;

namespace Verse
{
	public static class ProfilerThreadCheck
	{
		public static void BeginSample(string name)
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.BeginSample(name);
			}
		}

		public static void EndSample()
		{
			if (UnityData.IsInMainThread)
			{
				Profiler.EndSample();
			}
		}
	}
}
