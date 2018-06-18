using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02000594 RID: 1428
	public class WorldLayer_Rivers : WorldLayer_Paths
	{
		// Token: 0x06001B3A RID: 6970 RVA: 0x000E9FC8 File Offset: 0x000E83C8
		public WorldLayer_Rivers()
		{
			this.pointyEnds = true;
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x000EA080 File Offset: 0x000E8480
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
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Rivers);
			LayerSubMesh subMeshBorder = base.GetSubMesh(WorldMaterials.RiversBorder);
			WorldGrid grid = Find.WorldGrid;
			List<WorldLayer_Paths.OutputDirection> outputs = new List<WorldLayer_Paths.OutputDirection>();
			List<WorldLayer_Paths.OutputDirection> outputsBorder = new List<WorldLayer_Paths.OutputDirection>();
			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (i % 1000 == 0)
				{
					yield return null;
				}
				if (subMesh.verts.Count > 60000)
				{
					subMesh = base.GetSubMesh(WorldMaterials.Rivers);
					subMeshBorder = base.GetSubMesh(WorldMaterials.RiversBorder);
				}
				Tile tile = grid[i];
				if (tile.potentialRivers != null)
				{
					outputs.Clear();
					outputsBorder.Clear();
					for (int j = 0; j < tile.potentialRivers.Count; j++)
					{
						outputs.Add(new WorldLayer_Paths.OutputDirection
						{
							neighbor = tile.potentialRivers[j].neighbor,
							width = tile.potentialRivers[j].river.widthOnWorld - 0.2f
						});
						outputsBorder.Add(new WorldLayer_Paths.OutputDirection
						{
							neighbor = tile.potentialRivers[j].neighbor,
							width = tile.potentialRivers[j].river.widthOnWorld
						});
					}
					base.GeneratePaths(subMesh, i, outputs, this.riverColor, true);
					base.GeneratePaths(subMeshBorder, i, outputsBorder, this.riverColor, true);
				}
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x000EA0AC File Offset: 0x000E84AC
		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			float magnitude = inp.magnitude;
			inp = (inp + new Vector3(this.riverDisplacementX.GetValue(inp), this.riverDisplacementY.GetValue(inp), this.riverDisplacementZ.GetValue(inp)) * 0.1f).normalized * magnitude;
			return inp + inp.normalized * 0.008f;
		}

		// Token: 0x04001010 RID: 4112
		private Color32 riverColor = new Color32(73, 82, 100, byte.MaxValue);

		// Token: 0x04001011 RID: 4113
		private const float PerlinFrequency = 0.6f;

		// Token: 0x04001012 RID: 4114
		private const float PerlinMagnitude = 0.1f;

		// Token: 0x04001013 RID: 4115
		private ModuleBase riverDisplacementX = new Perlin(0.60000002384185791, 2.0, 0.5, 3, 84905524, QualityMode.Medium);

		// Token: 0x04001014 RID: 4116
		private ModuleBase riverDisplacementY = new Perlin(0.60000002384185791, 2.0, 0.5, 3, 37971116, QualityMode.Medium);

		// Token: 0x04001015 RID: 4117
		private ModuleBase riverDisplacementZ = new Perlin(0.60000002384185791, 2.0, 0.5, 3, 91572032, QualityMode.Medium);
	}
}
