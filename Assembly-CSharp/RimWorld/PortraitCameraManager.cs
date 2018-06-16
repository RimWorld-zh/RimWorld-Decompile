using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A3 RID: 1187
	public static class PortraitCameraManager
	{
		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001546 RID: 5446 RVA: 0x000BD3C0 File Offset: 0x000BB7C0
		public static Camera PortraitCamera
		{
			get
			{
				return PortraitCameraManager.portraitCameraInt;
			}
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06001547 RID: 5447 RVA: 0x000BD3DC File Offset: 0x000BB7DC
		public static PortraitRenderer PortraitRenderer
		{
			get
			{
				return PortraitCameraManager.portraitRendererInt;
			}
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x000BD3F8 File Offset: 0x000BB7F8
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

		// Token: 0x04000C9D RID: 3229
		private static Camera portraitCameraInt = PortraitCameraManager.CreatePortraitCamera();

		// Token: 0x04000C9E RID: 3230
		private static PortraitRenderer portraitRendererInt = PortraitCameraManager.portraitCameraInt.GetComponent<PortraitRenderer>();
	}
}
