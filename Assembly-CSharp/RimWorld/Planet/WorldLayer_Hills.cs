using System;
using System.Collections;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200058C RID: 1420
	public class WorldLayer_Hills : WorldLayer
	{
		// Token: 0x04000FFC RID: 4092
		private static readonly FloatRange BaseSizeRange = new FloatRange(0.9f, 1.1f);

		// Token: 0x04000FFD RID: 4093
		private static readonly IntVec2 TexturesInAtlas = new IntVec2(2, 2);

		// Token: 0x04000FFE RID: 4094
		private static readonly FloatRange BasePosOffsetRange_SmallHills = new FloatRange(0f, 0.37f);

		// Token: 0x04000FFF RID: 4095
		private static readonly FloatRange BasePosOffsetRange_LargeHills = new FloatRange(0f, 0.2f);

		// Token: 0x04001000 RID: 4096
		private static readonly FloatRange BasePosOffsetRange_Mountains = new FloatRange(0f, 0.08f);

		// Token: 0x04001001 RID: 4097
		private static readonly FloatRange BasePosOffsetRange_ImpassableMountains = new FloatRange(0f, 0.08f);

		// Token: 0x06001B25 RID: 6949 RVA: 0x000E93D8 File Offset: 0x000E77D8
		public override IEnumerable Regenerate()
		{
			IEnumerator enumerator = this.<Regenerate>__BaseCallProxy0().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object result = enumerator.Current;
					yield return result;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			WorldGrid grid = Find.WorldGrid;
			int tilesCount = grid.TilesCount;
			int i = 0;
			while (i < tilesCount)
			{
				Tile tile = grid[i];
				Material material;
				FloatRange floatRange;
				switch (tile.hilliness)
				{
				case Hilliness.SmallHills:
					material = WorldMaterials.SmallHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_SmallHills;
					goto IL_185;
				case Hilliness.LargeHills:
					material = WorldMaterials.LargeHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_LargeHills;
					goto IL_185;
				case Hilliness.Mountainous:
					material = WorldMaterials.Mountains;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_Mountains;
					goto IL_185;
				case Hilliness.Impassable:
					material = WorldMaterials.ImpassableMountains;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_ImpassableMountains;
					goto IL_185;
				}
				IL_262:
				i++;
				continue;
				IL_185:
				LayerSubMesh subMesh = base.GetSubMesh(material);
				Vector3 vector = grid.GetTileCenter(i);
				Vector3 posForTangents = vector;
				float magnitude = vector.magnitude;
				vector = (vector + Rand.UnitVector3 * floatRange.RandomInRange * grid.averageTileSize).normalized * magnitude;
				WorldRendererUtility.PrintQuadTangentialToPlanet(vector, posForTangents, WorldLayer_Hills.BaseSizeRange.RandomInRange * grid.averageTileSize, 0.005f, subMesh, false, true, false);
				WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, WorldLayer_Hills.TexturesInAtlas.x), Rand.Range(0, WorldLayer_Hills.TexturesInAtlas.z), WorldLayer_Hills.TexturesInAtlas.x, WorldLayer_Hills.TexturesInAtlas.z, subMesh);
				goto IL_262;
			}
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
		}
	}
}
