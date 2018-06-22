using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200049F RID: 1183
	public static class PortraitCameraManager
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x000BD3D8 File Offset: 0x000BB7D8
		public static Camera PortraitCamera
		{
			get
			{
				return PortraitCameraManager.portraitCameraInt;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x000BD3F4 File Offset: 0x000BB7F4
		public static PortraitRenderer PortraitRenderer
		{
			get
			{
				return PortraitCameraManager.portraitRendererInt;
			}
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x000BD410 File Offset: 0x000BB810
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

		// Token: 0x04000C9A RID: 3226
		private static Camera portraitCameraInt = PortraitCameraManager.CreatePortraitCamera();

		// Token: 0x04000C9B RID: 3227
		private static PortraitRenderer portraitRendererInt = PortraitCameraManager.portraitCameraInt.GetComponent<PortraitRenderer>();
	}
}
