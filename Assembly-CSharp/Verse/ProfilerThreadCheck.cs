namespace Verse
{
	public static class ProfilerThreadCheck
	{
		public static void BeginSample(string name)
		{
			if (!UnityData.IsInMainThread)
				return;
		}

		public static void EndSample()
		{
			if (!UnityData.IsInMainThread)
				return;
		}
	}
}
