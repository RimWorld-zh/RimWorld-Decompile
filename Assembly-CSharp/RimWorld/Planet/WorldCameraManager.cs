using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02000585 RID: 1413
	public static class WorldCameraManager
	{
		// Token: 0x04000FDA RID: 4058
		private static Camera worldCameraInt;

		// Token: 0x04000FDB RID: 4059
		private static Camera worldSkyboxCameraInt;

		// Token: 0x04000FDC RID: 4060
		private static WorldCameraDriver worldCameraDriverInt;

		// Token: 0x04000FDD RID: 4061
		public static readonly string WorldLayerName = "World";

		// Token: 0x04000FDE RID: 4062
		public static int WorldLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldLayerName
		});

		// Token: 0x04000FDF RID: 4063
		public static int WorldLayer = LayerMask.NameToLayer(WorldCameraManager.WorldLayerName);

		// Token: 0x04000FE0 RID: 4064
		public static readonly string WorldSkyboxLayerName = "WorldSkybox";

		// Token: 0x04000FE1 RID: 4065
		public static int WorldSkyboxLayerMask = LayerMask.GetMask(new string[]
		{
			WorldCameraManager.WorldSkyboxLayerName
		});

		// Token: 0x04000FE2 RID: 4066
		public static int WorldSkyboxLayer = LayerMask.NameToLayer(WorldCameraManager.WorldSkyboxLayerName);

		// Token: 0x04000FE3 RID: 4067
		private static readonly Color SkyColor = new Color(0.0627451f, 0.09019608f, 0.117647059f);

		// Token: 0x06001AF2 RID: 6898 RVA: 0x000E807C File Offset: 0x000E647C
		static WorldCameraManager()
		{
			WorldCameraManager.worldCameraInt = WorldCameraManager.CreateWorldCamera();
			WorldCameraManager.worldSkyboxCameraInt = WorldCameraManager.CreateWorldSkyboxCamera(WorldCameraManager.worldCameraInt);
			WorldCameraManager.worldCameraDriverInt = WorldCameraManager.worldCameraInt.GetComponent<WorldCameraDriver>();
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001AF3 RID: 6899 RVA: 0x000E8130 File Offset: 0x000E6530
		public static Camera WorldCamera
		{
			get
			{
				return WorldCameraManager.worldCameraInt;
			}
		}

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001AF4 RID: 6900 RVA: 0x000E814C File Offset: 0x000E654C
		public static Camera WorldSkyboxCamera
		{
			get
			{
				return WorldCameraManager.worldSkyboxCameraInt;
			}
		}

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001AF5 RID: 6901 RVA: 0x000E8168 File Offset: 0x000E6568
		public static WorldCameraDriver WorldCameraDriver
		{
			get
			{
				return WorldCameraManager.worldCameraDriverInt;
			}
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x000E8184 File Offset: 0x000E6584
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

		// Token: 0x06001AF7 RID: 6903 RVA: 0x000E8228 File Offset: 0x000E6628
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
