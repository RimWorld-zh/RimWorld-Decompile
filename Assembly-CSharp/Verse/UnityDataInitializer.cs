using System;

namespace Verse
{
	// Token: 0x02000BE2 RID: 3042
	public static class UnityDataInitializer
	{
		// Token: 0x06004257 RID: 16983 RVA: 0x0022E280 File Offset: 0x0022C680
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

		// Token: 0x04002D61 RID: 11617
		public static bool initializing;
	}
}
