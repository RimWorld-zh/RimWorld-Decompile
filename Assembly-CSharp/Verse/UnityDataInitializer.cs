using System;

namespace Verse
{
	// Token: 0x02000BE1 RID: 3041
	public static class UnityDataInitializer
	{
		// Token: 0x04002D6D RID: 11629
		public static bool initializing;

		// Token: 0x0600425E RID: 16990 RVA: 0x0022EDD0 File Offset: 0x0022D1D0
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
