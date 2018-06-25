using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public static class WorldTerrainColliderManager
	{
		private static GameObject gameObjectInt = WorldTerrainColliderManager.CreateGameObject();

		static WorldTerrainColliderManager()
		{
		}

		public static GameObject GameObject
		{
			get
			{
				return WorldTerrainColliderManager.gameObjectInt;
			}
		}

		private static GameObject CreateGameObject()
		{
			GameObject gameObject = new GameObject("WorldTerrainCollider");
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = WorldCameraManager.WorldLayer;
			return gameObject;
		}
	}
}
