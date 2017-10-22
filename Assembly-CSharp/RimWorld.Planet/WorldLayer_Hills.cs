using System;
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
			IEnumerator enumerator = this._003CRegenerate_003E__BaseCallProxy0().GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					object result = enumerator.Current;
					yield return result;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			finally
			{
				IDisposable disposable;
				IDisposable disposable2 = disposable = (enumerator as IDisposable);
				if (disposable != null)
				{
					disposable2.Dispose();
				}
			}
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			WorldGrid grid = Find.WorldGrid;
			int tilesCount = grid.TilesCount;
			for (int i = 0; i < tilesCount; i++)
			{
				Tile tile = grid[i];
				Material material;
				FloatRange floatRange;
				switch (tile.hilliness)
				{
				case Hilliness.SmallHills:
				{
					material = WorldMaterials.SmallHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_SmallHills;
					goto IL_0185;
				}
				case Hilliness.LargeHills:
				{
					material = WorldMaterials.LargeHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_LargeHills;
					goto IL_0185;
				}
				case Hilliness.Mountainous:
				{
					material = WorldMaterials.Mountains;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_Mountains;
					goto IL_0185;
				}
				case Hilliness.Impassable:
				{
					material = WorldMaterials.ImpassableMountains;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_ImpassableMountains;
					goto IL_0185;
				}
				}
				continue;
				IL_0185:
				LayerSubMesh subMesh = base.GetSubMesh(material);
				Vector3 tileCenter;
				Vector3 vector = tileCenter = grid.GetTileCenter(i);
				float magnitude = vector.magnitude;
				vector += Rand.UnitVector3 * floatRange.RandomInRange * grid.averageTileSize;
				vector = vector.normalized * magnitude;
				WorldRendererUtility.PrintQuadTangentialToPlanet(vector, tileCenter, WorldLayer_Hills.BaseSizeRange.RandomInRange * grid.averageTileSize, 0.005f, subMesh, false, true, false);
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
			base.FinalizeMesh(MeshParts.All);
			yield break;
			IL_028d:
			/*Error near IL_028e: Unexpected return in MoveNext()*/;
		}
	}
}
