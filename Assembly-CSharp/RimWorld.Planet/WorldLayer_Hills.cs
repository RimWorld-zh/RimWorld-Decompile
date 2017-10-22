using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_Hills : WorldLayer
	{
		private static readonly FloatRange BaseSizeRange = new FloatRange(0.9f, 1.1f);

		private static readonly IntVec2 TexturesInAtlas = new IntVec2(2, 2);

		private static readonly FloatRange BasePosOffsetRange_SmallHills = new FloatRange(0f, 0.37f);

		private static readonly FloatRange BasePosOffsetRange_LargeHills = new FloatRange(0f, 0.2f);

		private static readonly FloatRange BasePosOffsetRange_Mountains = new FloatRange(0f, 0.08f);

		private static readonly FloatRange BasePosOffsetRange_ImpassableMountains = new FloatRange(0f, 0.08f);

		public override IEnumerable Regenerate()
		{
			foreach (object item in base.Regenerate())
			{
				yield return item;
			}
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			WorldGrid grid = Find.WorldGrid;
			int tilesCount = grid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				Tile tile = grid[i];
				Material mat;
				FloatRange basePosOffsetRange;
				switch (tile.hilliness)
				{
				case Hilliness.SmallHills:
				{
					mat = WorldMaterials.SmallHills;
					basePosOffsetRange = WorldLayer_Hills.BasePosOffsetRange_SmallHills;
					goto IL_019a;
				}
				case Hilliness.LargeHills:
				{
					mat = WorldMaterials.LargeHills;
					basePosOffsetRange = WorldLayer_Hills.BasePosOffsetRange_LargeHills;
					goto IL_019a;
				}
				case Hilliness.Mountainous:
				{
					mat = WorldMaterials.Mountains;
					basePosOffsetRange = WorldLayer_Hills.BasePosOffsetRange_Mountains;
					goto IL_019a;
				}
				case Hilliness.Impassable:
				{
					mat = WorldMaterials.ImpassableMountains;
					basePosOffsetRange = WorldLayer_Hills.BasePosOffsetRange_ImpassableMountains;
					goto IL_019a;
				}
				}
				continue;
				IL_019a:
				LayerSubMesh subMesh = base.GetSubMesh(mat);
				Vector3 origPos;
				Vector3 pos = origPos = grid.GetTileCenter(i);
				float length = pos.magnitude;
				pos += Rand.PointOnSphere * basePosOffsetRange.RandomInRange * grid.averageTileSize;
				pos = pos.normalized * length;
				WorldRendererUtility.PrintQuadTangentialToPlanet(pos, origPos, WorldLayer_Hills.BaseSizeRange.RandomInRange * grid.averageTileSize, 0.005f, subMesh, false, true, false);
				IntVec2 texturesInAtlas = WorldLayer_Hills.TexturesInAtlas;
				int indexX = Rand.Range(0, texturesInAtlas.x);
				IntVec2 texturesInAtlas2 = WorldLayer_Hills.TexturesInAtlas;
				int indexY = Rand.Range(0, texturesInAtlas2.z);
				IntVec2 texturesInAtlas3 = WorldLayer_Hills.TexturesInAtlas;
				int x = texturesInAtlas3.x;
				IntVec2 texturesInAtlas4 = WorldLayer_Hills.TexturesInAtlas;
				WorldRendererUtility.PrintTextureAtlasUVs(indexX, indexY, x, texturesInAtlas4.z, subMesh);
			}
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All, true);
		}
	}
}
