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
			IEnumerator enumerator = base.Regenerate().GetEnumerator();
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
				LayerSubMesh subMesh;
				Vector3 vector;
				Vector3 posForTangents;
				float magnitude;
				IntVec2 texturesInAtlas;
				int indexX;
				IntVec2 texturesInAtlas2;
				int indexY;
				IntVec2 texturesInAtlas3;
				int x;
				IntVec2 texturesInAtlas4;
				switch (tile.hilliness)
				{
				case Hilliness.SmallHills:
					material = WorldMaterials.SmallHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_SmallHills;
					goto IL_0180;
				case Hilliness.LargeHills:
					material = WorldMaterials.LargeHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_LargeHills;
					goto IL_0180;
				case Hilliness.Mountainous:
					material = WorldMaterials.Mountains;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_Mountains;
					goto IL_0180;
				case Hilliness.Impassable:
					{
						material = WorldMaterials.ImpassableMountains;
						floatRange = WorldLayer_Hills.BasePosOffsetRange_ImpassableMountains;
						goto IL_0180;
					}
					IL_0180:
					subMesh = base.GetSubMesh(material);
					vector = grid.GetTileCenter(i);
					posForTangents = vector;
					magnitude = vector.magnitude;
					vector += Rand.UnitVector3 * floatRange.RandomInRange * grid.averageTileSize;
					vector = vector.normalized * magnitude;
					WorldRendererUtility.PrintQuadTangentialToPlanet(vector, posForTangents, WorldLayer_Hills.BaseSizeRange.RandomInRange * grid.averageTileSize, 0.005f, subMesh, false, true, false);
					texturesInAtlas = WorldLayer_Hills.TexturesInAtlas;
					indexX = Rand.Range(0, texturesInAtlas.x);
					texturesInAtlas2 = WorldLayer_Hills.TexturesInAtlas;
					indexY = Rand.Range(0, texturesInAtlas2.z);
					texturesInAtlas3 = WorldLayer_Hills.TexturesInAtlas;
					x = texturesInAtlas3.x;
					texturesInAtlas4 = WorldLayer_Hills.TexturesInAtlas;
					WorldRendererUtility.PrintTextureAtlasUVs(indexX, indexY, x, texturesInAtlas4.z, subMesh);
					break;
				}
			}
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
			IL_0287:
			/*Error near IL_0288: Unexpected return in MoveNext()*/;
		}
	}
}
