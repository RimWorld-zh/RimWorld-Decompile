using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A4 RID: 1188
	public class PortraitRenderer : MonoBehaviour
	{
		// Token: 0x0600154A RID: 5450 RVA: 0x000BD4F8 File Offset: 0x000BB8F8
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

		// Token: 0x0600154B RID: 5451 RVA: 0x000BD579 File Offset: 0x000BB979
		public void OnPostRender()
		{
			this.pawn.Drawer.renderer.RenderPortrait();
		}

		// Token: 0x04000C9F RID: 3231
		private Pawn pawn;
	}
}
