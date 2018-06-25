using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005A3 RID: 1443
	[StaticConstructorOnStartup]
	public static class WorldTerrainColliderManager
	{
		// Token: 0x0400105C RID: 4188
		private static GameObject gameObjectInt = WorldTerrainColliderManager.CreateGameObject();

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06001B90 RID: 7056 RVA: 0x000EE430 File Offset: 0x000EC830
		public static GameObject GameObject
		{
			get
			{
				return WorldTerrainColliderManager.gameObjectInt;
			}
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x000EE44C File Offset: 0x000EC84C
		private static GameObject CreateGameObject()
		{
			GameObject gameObject = new GameObject("WorldTerrainCollider");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = WorldCameraManager.WorldLayer;
			return gameObject;
		}
	}
}
