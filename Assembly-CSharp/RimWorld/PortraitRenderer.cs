using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A2 RID: 1186
	public class PortraitRenderer : MonoBehaviour
	{
		// Token: 0x04000C9F RID: 3231
		private Pawn pawn;

		// Token: 0x06001544 RID: 5444 RVA: 0x000BD860 File Offset: 0x000BBC60
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

		// Token: 0x06001545 RID: 5445 RVA: 0x000BD8E1 File Offset: 0x000BBCE1
		public void OnPostRender()
		{
			this.pawn.Drawer.renderer.RenderPortrait();
		}
	}
}
