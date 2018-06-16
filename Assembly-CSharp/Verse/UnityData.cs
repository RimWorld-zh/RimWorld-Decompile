using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BE1 RID: 3041
	public static class UnityData
	{
		// Token: 0x06004254 RID: 16980 RVA: 0x0022E1DC File Offset: 0x0022C5DC
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.", false);
			}
		}

		// Token: 0x17000A6D RID: 2669
		// (get) Token: 0x06004255 RID: 16981 RVA: 0x0022E200 File Offset: 0x0022C600
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x06004256 RID: 16982 RVA: 0x0022E228 File Offset: 0x0022C628
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

		// Token: 0x04002D5A RID: 11610
		private static bool initialized;

		// Token: 0x04002D5B RID: 11611
		public static bool isDebugBuild;

		// Token: 0x04002D5C RID: 11612
		public static bool isEditor;

		// Token: 0x04002D5D RID: 11613
		public static string dataPath;

		// Token: 0x04002D5E RID: 11614
		public static RuntimePlatform platform;

		// Token: 0x04002D5F RID: 11615
		public static string persistentDataPath;

		// Token: 0x04002D60 RID: 11616
		private static int mainThreadId;
	}
}
