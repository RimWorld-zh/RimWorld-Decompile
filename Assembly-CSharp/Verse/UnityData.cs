using System;
using System.Threading;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BDD RID: 3037
	public static class UnityData
	{
		// Token: 0x06004258 RID: 16984 RVA: 0x0022E970 File Offset: 0x0022CD70
		static UnityData()
		{
			if (!UnityData.initialized && !UnityDataInitializer.initializing)
			{
				Log.Warning("Used UnityData before it's initialized.", false);
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x06004259 RID: 16985 RVA: 0x0022E994 File Offset: 0x0022CD94
		public static bool IsInMainThread
		{
			get
			{
				return UnityData.mainThreadId == Thread.CurrentThread.ManagedThreadId;
			}
		}

		// Token: 0x0600425A RID: 16986 RVA: 0x0022E9BC File Offset: 0x0022CDBC
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

		// Token: 0x04002D5F RID: 11615
		private static bool initialized;

		// Token: 0x04002D60 RID: 11616
		public static bool isDebugBuild;

		// Token: 0x04002D61 RID: 11617
		public static bool isEditor;

		// Token: 0x04002D62 RID: 11618
		public static string dataPath;

		// Token: 0x04002D63 RID: 11619
		public static RuntimePlatform platform;

		// Token: 0x04002D64 RID: 11620
		public static string persistentDataPath;

		// Token: 0x04002D65 RID: 11621
		private static int mainThreadId;
	}
}
