using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine.Profiling;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000F1D RID: 3869
	public static class ProfilerPairValidation
	{
		// Token: 0x04003D99 RID: 15769
		public static Stack<StackTrace> profilerSignatures = new Stack<StackTrace>();

		// Token: 0x06005CC7 RID: 23751 RVA: 0x002F14E3 File Offset: 0x002EF8E3
		public static void BeginSample(string token)
		{
			Profiler.BeginSample(token);
			ProfilerPairValidation.profilerSignatures.Push(new StackTrace(1, true));
		}

		// Token: 0x06005CC8 RID: 23752 RVA: 0x002F1500 File Offset: 0x002EF900
		public static void EndSample()
		{
			StackTrace stackTrace = ProfilerPairValidation.profilerSignatures.Pop();
			StackTrace stackTrace2 = new StackTrace(1, true);
			if (stackTrace2.FrameCount != stackTrace.FrameCount)
			{
				Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()), false);
			}
			else
			{
				for (int i = 0; i < stackTrace2.FrameCount; i++)
				{
					if (stackTrace2.GetFrame(i).GetMethod() != stackTrace.GetFrame(i).GetMethod())
					{
						if (stackTrace.GetFrame(i).GetMethod().DeclaringType != typeof(ProfilerThreadCheck) || stackTrace2.GetFrame(i).GetMethod().DeclaringType != typeof(ProfilerThreadCheck))
						{
							if (stackTrace.GetFrame(i).GetMethod() != typeof(PathFinder).GetMethod("PfProfilerBeginSample", BindingFlags.Instance | BindingFlags.NonPublic) || stackTrace2.GetFrame(i).GetMethod() != typeof(PathFinder).GetMethod("PfProfilerEndSample", BindingFlags.Instance | BindingFlags.NonPublic))
							{
								Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()), false);
								break;
							}
						}
					}
				}
			}
			Profiler.EndSample();
		}
	}
}
