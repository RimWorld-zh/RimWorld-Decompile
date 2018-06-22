using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005E8 RID: 1512
	[StaticConstructorOnStartup]
	public class Caravan_GotoMoteRenderer
	{
		// Token: 0x06001DF3 RID: 7667 RVA: 0x00101F80 File Offset: 0x00100380
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

		// Token: 0x06001DF4 RID: 7668 RVA: 0x00102065 File Offset: 0x00100465
		public void OrderedToTile(int tile)
		{
			this.tile = tile;
			this.lastOrderedToTileTime = Time.time;
		}

		// Token: 0x040011B4 RID: 4532
		private int tile;

		// Token: 0x040011B5 RID: 4533
		private float lastOrderedToTileTime = -0.51f;

		// Token: 0x040011B6 RID: 4534
		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		// Token: 0x040011B7 RID: 4535
		private static Material cachedMaterial;

		// Token: 0x040011B8 RID: 4536
		public static readonly Material FeedbackGoto = MaterialPool.MatFrom("Things/Mote/FeedbackGoto", ShaderDatabase.WorldOverlayTransparent, WorldMaterials.DynamicObjectRenderQueue);

		// Token: 0x040011B9 RID: 4537
		private const float Duration = 0.5f;

		// Token: 0x040011BA RID: 4538
		private const float BaseSize = 0.8f;
	}
}
