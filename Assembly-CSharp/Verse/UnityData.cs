using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BE0 RID: 3040
	public static class UnityData
	{
		// Token: 0x04002D66 RID: 11622
		private static bool initialized;

		// Token: 0x04002D67 RID: 11623
		public static bool isDebugBuild;

		// Token: 0x04002D68 RID: 11624
		public static bool isEditor;

		// Token: 0x04002D69 RID: 11625
		public static string dataPath;

		// Token: 0x04002D6A RID: 11626
		public static RuntimePlatform platform;

		// Token: 0x04002D6B RID: 11627
		public static string persistentDataPath;

		// Token: 0x04002D6C RID: 11628
		private static int mainThreadId;

		// Token: 0x0600425B RID: 16987 RVA: 0x0022ED2C File Offset: 0x0022D12C
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.", false);
			}
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x0600425C RID: 16988 RVA: 0x0022ED50 File Offset: 0x0022D150
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x0022ED78 File Offset: 0x0022D178
		public static void CopyUnityData()
		{
			UnityData.mainThreadId = Thread.CurrentThread.ManagedThreadId;
			UnityData.isDebugBuild = Debug.isDebugBuild;
			UnityData.isEditor = Application.isEditor;
			UnityData.dataPath = Application.dataPath;
			UnityData.platform = Application.platform;
			UnityData.persistentDataPath = Application.persistentDataPath;
			UnityData.initialized = true;
		}
	}
}
