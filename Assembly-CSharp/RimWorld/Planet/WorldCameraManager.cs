using System;
using UnityEngine;

namespace RimWorld.Planet
{
	public static class WorldCameraManager
	{
		private static Camera worldCameraInt;

		private static Camera worldSkyboxCameraInt;

		private static WorldCameraDriver worldCameraDriverInt;

		public static readonly string WorldLayerName = "World";

		public static int WorldLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldLayerName
		});

		public static int WorldLayer = LayerMask.NameToLayer(WorldCameraManager.WorldLayerName);

		public static readonly string WorldSkyboxLayerName = "WorldSkybox";

		public static int WorldSkyboxLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldSkyboxLayerName
		});

		public static int WorldSkyboxLayer = LayerMask.NameToLayer(WorldCameraManager.WorldSkyboxLayerName);

		private static readonly Color SkyColor = new Color(0.0627451f, 0.09019608f, 0.117647059f);

		static WorldCameraManager()
		{
			WorldCameraManager.worldCameraInt = WorldCameraManager.CreateWorldCamera();
			WorldCameraManager.worldSkyboxCameraInt = WorldCameraManager.CreateWorldSkyboxCamera(WorldCameraManager.worldCameraInt);
			WorldCameraManager.worldCameraDriverInt = WorldCameraManager.worldCameraInt.GetComponent<WorldCameraDriver>();
		}

		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.worldCameraInt;
			}
		}

		public static Camera WorldSkyboxCamera
		{
			get
			{
				return WorldCameraManager.worldSkyboxCameraInt;
			}
		}

		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.worldCameraDriverInt;
			}
		}

		private static Camera CreateWorldCamera()
		{
			GameObject gameObject = new GameObject("WorldCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<WorldCameraDriver>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldLayerMask;
			component.clearFlags = CameraClearFlags.Depth;
			component.useOcclusionCulling = true;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 1200f;
			component.fieldOfView = 20f;
			component.depth = 1f;
			return component;
		}

		private static Camera CreateWorldSkyboxCamera(Camera parent)
		{
			GameObject gameObject = new GameObject("WorldSkyboxCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(true);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.SetParent(parent.transform);
			component.orthographic = false;
			component.cullingMask = WorldCameraManager.WorldSkyboxLayerMask;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = WorldCameraManager.SkyColor;
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			component.nearClipPlane = 2f;
			component.farClipPlane = 1200f;
			component.fieldOfView = 60f;
			component.depth = 0f;
			return component;
		}
	}
}
