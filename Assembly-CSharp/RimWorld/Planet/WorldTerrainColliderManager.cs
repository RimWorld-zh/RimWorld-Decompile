using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A1 RID: 1441
	[StaticConstructorOnStartup]
	public static class WorldTerrainColliderManager
	{
		// Token: 0x04001058 RID: 4184
		private static GameObject gameObjectInt = WorldTerrainColliderManager.CreateGameObject();

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001B8D RID: 7053 RVA: 0x000EE078 File Offset: 0x000EC478
		public static GameObject GameObject
		{
			get
			{
				return WorldTerrainColliderManager.gameObjectInt;
			}
		}

		// Token: 0x06001B8E RID: 7054 RVA: 0x000EE094 File Offset: 0x000EC494
		private static GameObject CreateGameObject()
		{
			GameObject gameObject = new GameObject("WorldTerrainCollider");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = WorldCameraManager.WorldLayer;
			return gameObject;
		}
	}
}
