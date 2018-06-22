using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A0 RID: 1184
	public class PortraitRenderer : MonoBehaviour
	{
		// Token: 0x06001541 RID: 5441 RVA: 0x000BD510 File Offset: 0x000BB910
		public void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Camera portraitCamera = Find.PortraitCamera;
			portraitCamera.targetTexture = renderTexture;
			Vector3 position = portraitCamera.transform.position;
			float orthographicSize = portraitCamera.orthographicSize;
			portraitCamera.transform.position += cameraOffset;
			portraitCamera.orthographicSize = 1f / cameraZoom;
			this.pawn = pawn;
			portraitCamera.Render();
			this.pawn = null;
			portraitCamera.transform.position = position;
			portraitCamera.orthographicSize = orthographicSize;
			portraitCamera.targetTexture = null;
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x000BD591 File Offset: 0x000BB991
		public void OnPostRender()
		{
			this.pawn.Drawer.renderer.RenderPortrait();
		}

		// Token: 0x04000C9C RID: 3228
		private Pawn pawn;
	}
}
