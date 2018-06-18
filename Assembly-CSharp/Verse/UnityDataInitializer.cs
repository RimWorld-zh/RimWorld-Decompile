using System;

namespace Verse
{
	// Token: 0x02000BE2 RID: 3042
	public static class UnityDataInitializer
	{
		// Token: 0x06004259 RID: 16985 RVA: 0x0022E2F8 File Offset: 0x0022C6F8
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
