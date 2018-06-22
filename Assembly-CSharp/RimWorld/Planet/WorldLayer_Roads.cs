using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	// Token: 0x02000591 RID: 1425
	public class WorldLayer_Roads : WorldLayer_Paths
	{
		// Token: 0x06001B36 RID: 6966 RVA: 0x000EA6A4 File Offset: 0x000E8AA4
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
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Roads);
			WorldGrid grid = Find.WorldGrid;
			List<RoadWorldLayerDef> roadLayerDefs = (from rwld in DefDatabase<RoadWorldLayerDef>.AllDefs
			orderby rwld.order
			select rwld).ToList<RoadWorldLayerDef>();
			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (i % 1000 == 0)
				{
					yield return null;
				}
				if (subMesh.verts.Count > 60000)
				{
					subMesh = base.GetSubMesh(WorldMaterials.Roads);
				}
				Tile tile = grid[i];
				if (!tile.WaterCovered)
				{
					List<WorldLayer_Paths.OutputDirection> outputs = new List<WorldLayer_Paths.OutputDirection>();
					if (tile.potentialRoads != null)
					{
						bool allowSmoothTransition = true;
						for (int j = 0; j < tile.potentialRoads.Count - 1; j++)
						{
							if (tile.potentialRoads[j].road.worldTransitionGroup != tile.potentialRoads[j + 1].road.worldTransitionGroup)
							{
								allowSmoothTransition = false;
							}
						}
						for (int k = 0; k < roadLayerDefs.Count; k++)
						{
							bool flag = false;
							outputs.Clear();
							for (int l = 0; l < tile.potentialRoads.Count; l++)
							{
								RoadDef road = tile.potentialRoads[l].road;
								float layerWidth = road.GetLayerWidth(roadLayerDefs[k]);
								if (layerWidth > 0f)
								{
									flag = true;
								}
								outputs.Add(new WorldLayer_Paths.OutputDirection
								{
									neighbor = tile.potentialRoads[l].neighbor,
									width = layerWidth,
									distortionFrequency = road.distortionFrequency,
									distortionIntensity = road.distortionIntensity
								});
							}
							if (flag)
							{
								base.GeneratePaths(subMesh, i, outputs, roadLayerDefs[k].color, allowSmoothTransition);
							}
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All);
			yield break;
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x000EA6D0 File Offset: 0x000E8AD0
		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			Vector3 coordinate = inp * distortionFrequency;
			float magnitude = inp.magnitude;
			Vector3 a = new Vector3(this.roadDisplacementX.GetValue(coordinate), this.roadDisplacementY.GetValue(coordinate), this.roadDisplacementZ.GetValue(coordinate));
			if ((double)a.magnitude > 0.0001)
			{
				float d = (1f / (1f + Mathf.Exp(-a.magnitude / 1f * 2f)) * 2f - 1f) * 1f;
				a = a.normalized * d;
			}
			inp = (inp + a * distortionIntensity).normalized * magnitude;
			return inp + inp.normalized * 0.012f;
		}

		// Token: 0x04001013 RID: 4115
		private ModuleBase roadDisplacementX = new Perlin(1.0, 2.0, 0.5, 3, 74173887, QualityMode.Medium);

		// Token: 0x04001014 RID: 4116
		private ModuleBase roadDisplacementY = new Perlin(1.0, 2.0, 0.5, 3, 67515931, QualityMode.Medium);

		// Token: 0x04001015 RID: 4117
		private ModuleBase roadDisplacementZ = new Perlin(1.0, 2.0, 0.5, 3, 87116801, QualityMode.Medium);
	}
}
