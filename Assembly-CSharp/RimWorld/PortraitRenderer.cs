using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PortraitRenderer : MonoBehaviour
	{
		private Pawn pawn;

		public void RenderPortrait(Pawn pawn, RenderTexture renderTexture, Vector3 cameraOffset, float cameraZoom)
		{
			Camera portraitCamera = Find.PortraitCamera;
			portraitCamera.targetTexture = renderTexture;
			Vector3 position = portraitCamera.transform.position;
			float orthographicSize = portraitCamera.orthographicSize;
			Transform transform = portraitCamera.transform;
			transform.position += cameraOffset;
			portraitCamera.orthographicSize = (float)(1.0 / cameraZoom);
			this.pawn = pawn;
			portraitCamera.Render();
			portraitCamera.transform.position = position;
			portraitCamera.orthographicSize = orthographicSize;
			portraitCamera.targetTexture = null;
		}

		public void OnPostRender()
		{
			this.pawn.Drawer.renderer.RenderPortait();
		}
	}
}
