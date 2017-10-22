using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class Caravan_GotoMoteRenderer
	{
		private int tile;

		private float lastOrderedToTileTime = -0.51f;

		private static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

		private static Material cachedMaterial;

		public static readonly Material FeedbackGoto = MaterialPool.MatFrom("Things/Mote/FeedbackGoto", ShaderDatabase.WorldOverlayTransparent, WorldMaterials.DynamicObjectRenderQueue);

		private const float Duration = 0.5f;

		private const float BaseSize = 0.8f;

		public void RenderMote()
		{
			float num = (float)((Time.time - this.lastOrderedToTileTime) / 0.5);
			if (!(num > 1.0))
			{
				if ((Object)Caravan_GotoMoteRenderer.cachedMaterial == (Object)null)
				{
					Caravan_GotoMoteRenderer.cachedMaterial = MaterialPool.MatFrom((Texture2D)Caravan_GotoMoteRenderer.FeedbackGoto.mainTexture, Caravan_GotoMoteRenderer.FeedbackGoto.shader, Color.white, WorldMaterials.DynamicObjectRenderQueue);
				}
				WorldGrid worldGrid = Find.WorldGrid;
				Vector3 tileCenter = worldGrid.GetTileCenter(this.tile);
				Color value = new Color(1f, 1f, 1f, (float)(1.0 - num));
				Caravan_GotoMoteRenderer.propertyBlock.SetColor(ShaderPropertyIDs.Color, value);
				Vector3 pos = tileCenter;
				float size = (float)(0.800000011920929 * worldGrid.averageTileSize);
				float altOffset = 0.018f;
				Material material = Caravan_GotoMoteRenderer.cachedMaterial;
				MaterialPropertyBlock materialPropertyBlock = Caravan_GotoMoteRenderer.propertyBlock;
				WorldRendererUtility.DrawQuadTangentialToPlanet(pos, size, altOffset, material, false, false, materialPropertyBlock);
			}
		}

		public void OrderedToTile(int tile)
		{
			this.tile = tile;
			this.lastOrderedToTileTime = Time.time;
		}
	}
}
