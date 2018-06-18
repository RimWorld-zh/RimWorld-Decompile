using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005EC RID: 1516
	[StaticConstructorOnStartup]
	public class Caravan_GotoMoteRenderer
	{
		// Token: 0x06001DFC RID: 7676 RVA: 0x00101F2C File Offset: 0x0010032C
		public void RenderMote()
		{
			float num = (Time.time - this.lastOrderedToTileTime) / 0.5f;
			if (num <= 1f)
			{
				if (Caravan_GotoMoteRenderer.cachedMaterial == null)
				{
					Caravan_GotoMoteRenderer.cachedMaterial = MaterialPool.MatFrom((Texture2D)Caravan_GotoMoteRenderer.FeedbackGoto.mainTexture, Caravan_GotoMoteRenderer.FeedbackGoto.shader, Color.white, WorldMaterials.DynamicObjectRenderQueue);
				}
				WorldGrid worldGrid = Find.WorldGrid;
				Vector3 tileCenter = worldGrid.GetTileCenter(this.tile);
				Color value = new Color(1f, 1f, 1f, 1f - num);
				Caravan_GotoMoteRenderer.propertyBlock.SetColor(ShaderPropertyIDs.Color, value);
				Vector3 pos = tileCenter;
				float size = 0.8f * worldGrid.averageTileSize;
				float altOffset = 0.018f;
				Material material = Caravan_GotoMoteRenderer.cachedMaterial;
				MaterialPropertyBlock materialPropertyBlock = Caravan_GotoMoteRenderer.propertyBlock;
				WorldRendererUtility.DrawQuadTangentialToPlanet(pos, size, altOffset, material, false, false, materialPropertyBlock);
			}
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x00102011 File Offset: 0x00100411
		public void OrderedToTile(int tile)
		{
			this.tile = tile;
			this.lastOrderedToTileTime = Time.time;
		}

		// Token: 0x040011B7 RID: 4535
		private int tile;

		// Token: 0x040011B8 RID: 4536
		private float lastOrderedToTileTime = -0.51f;

		// Token: 0x040011B9 RID: 4537
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x040011BA RID: 4538
		private static Material cachedMaterial;

		// Token: 0x040011BB RID: 4539
		public static readonly Material FeedbackGoto = MaterialPool.MatFrom("Things/Mote/FeedbackGoto", ShaderDatabase.WorldOverlayTransparent, WorldMaterials.DynamicObjectRenderQueue);

		// Token: 0x040011BC RID: 4540
		private const float Duration = 0.5f;

		// Token: 0x040011BD RID: 4541
		private const float BaseSize = 0.8f;
	}
}
