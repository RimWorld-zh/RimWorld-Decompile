using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Verse.AI;

namespace Verse
{
	public static class ProfilerPairValidation
	{
		public static Stack<StackTrace> profilerSignatures = new Stack<StackTrace>();

		public static void BeginSample(string token)
		{
			ProfilerPairValidation.profilerSignatures.Push(new StackTrace(1, true));
		}

		public static void EndSample()
		{
			StackTrace stackTrace = ProfilerPairValidation.profilerSignatures.Pop();
			StackTrace stackTrace2 = new StackTrace(1, true);
			if (stackTrace2.FrameCount != stackTrace.FrameCount)
			{
				Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()));
			}
			else
			{
				int num = 0;
				while (true)
				{
					if (num < stackTrace2.FrameCount)
					{
						if (stackTrace2.GetFrame(num).GetMethod() != stackTrace.GetFrame(num).GetMethod() && (stackTrace.GetFrame(num).GetMethod().DeclaringType != typeof(ProfilerThreadCheck) || stackTrace2.GetFrame(num).GetMethod().DeclaringType != typeof(ProfilerThreadCheck)))
						{
							if (stackTrace.GetFrame(num).GetMethod() != typeof(PathFinder).GetMethod("PfProfilerBeginSample", BindingFlags.Instance | BindingFlags.NonPublic))
								break;
							if (stackTrace2.GetFrame(num).GetMethod() != typeof(PathFinder).GetMethod("PfProfilerEndSample", BindingFlags.Instance | BindingFlags.NonPublic))
								break;
						}
						num++;
						continue;
					}
					return;
				}
				Log.Message(string.Format("Mismatch:\n{0}\n\n{1}", stackTrace.ToString(), stackTrace2.ToString()));
			}
		}
	}
}
