using System;

namespace Verse
{
	// Token: 0x02000BDE RID: 3038
	public static class UnityDataInitializer
	{
		// Token: 0x0600425B RID: 16987 RVA: 0x0022EA14 File Offset: 0x0022CE14
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

		// Token: 0x04002D66 RID: 11622
		public static bool initializing;
	}
}
