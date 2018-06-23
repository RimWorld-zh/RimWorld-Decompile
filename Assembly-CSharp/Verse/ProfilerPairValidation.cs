using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine.Profiling;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000F18 RID: 3864
	public static class ProfilerPairValidation
	{
		// Token: 0x04003D8E RID: 15758
		public static Stack<StackTrace> profilerSignatures = new Stack<StackTrace>();

		// Token: 0x06005CBD RID: 23741 RVA: 0x002F0C43 File Offset: 0x002EF043
		public static void BeginSample(string token)
		{
			Profiler.BeginSample(token);
			ProfilerPairValidation.profilerSignatures.Push(new StackTrace(1, true));
		}

		// Token: 0x06005CBE RID: 23742 RVA: 0x002F0C60 File Offset: 0x002EF060
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
