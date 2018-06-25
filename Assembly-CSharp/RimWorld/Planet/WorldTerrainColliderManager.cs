using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A3 RID: 1443
	[StaticConstructorOnStartup]
	public static class WorldTerrainColliderManager
	{
		// Token: 0x04001058 RID: 4184
		private static GameObject gameObjectInt = WorldTerrainColliderManager.CreateGameObject();

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001B91 RID: 7057 RVA: 0x000EE1C8 File Offset: 0x000EC5C8
		public static GameObject GameObject
		{
			get
			{
				return WorldTerrainColliderManager.gameObjectInt;
			}
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x000EE1E4 File Offset: 0x000EC5E4
		private static GameObject CreateGameObject()
		{
			GameObject gameObject = new GameObject("WorldTerrainCollider");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = WorldCameraManager.WorldLayer;
			return gameObject;
		}
	}
}
