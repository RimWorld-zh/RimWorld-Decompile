using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A1 RID: 1185
	public static class PortraitCameraManager
	{
		// Token: 0x04000C9D RID: 3229
		private static Camera portraitCameraInt = PortraitCameraManager.CreatePortraitCamera();

		// Token: 0x04000C9E RID: 3230
		private static PortraitRenderer portraitRendererInt = PortraitCameraManager.portraitCameraInt.GetComponent<PortraitRenderer>();

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x000BD728 File Offset: 0x000BBB28
		public static Camera PortraitCamera
		{
			get
			{
				return PortraitCameraManager.portraitCameraInt;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x000BD744 File Offset: 0x000BBB44
		public static PortraitRenderer PortraitRenderer
		{
			get
			{
				return PortraitCameraManager.portraitRendererInt;
			}
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x000BD760 File Offset: 0x000BBB60
		private static Camera CreatePortraitCamera()
		{
			GameObject gameObject = new GameObject("PortraitCamera", new Type[]
			{
				typeof(Camera)
			});
			gameObject.SetActive(false);
			gameObject.AddComponent<PortraitRenderer>();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Camera component = gameObject.GetComponent<Camera>();
			component.transform.position = new Vector3(0f, 15f, 0f);
			component.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
			component.orthographic = true;
			component.cullingMask = 0;
			component.orthographicSize = 1f;
			component.clearFlags = CameraClearFlags.Color;
			component.backgroundColor = new Color(0f, 0f, 0f, 0f);
			component.useOcclusionCulling = false;
			component.renderingPath = RenderingPath.Forward;
			Camera camera = Current.Camera;
			component.nearClipPlane = camera.nearClipPlane;
			component.farClipPlane = camera.farClipPlane;
			return component;
		}
	}
}
