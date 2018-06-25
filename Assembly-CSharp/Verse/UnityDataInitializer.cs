using System;

namespace Verse
{
	// Token: 0x02000BE0 RID: 3040
	public static class UnityDataInitializer
	{
		// Token: 0x04002D66 RID: 11622
		public static bool initializing;

		// Token: 0x0600425E RID: 16990 RVA: 0x0022EAF0 File Offset: 0x0022CEF0
		public static void CopyUnityData()
		{
			UnityDataInitializer.initializing = true;
			try
			{
				UnityData.CopyUnityData();
			}
			finally
			{
				UnityDataInitializer.initializing = false;
			}
		}
	}
}
