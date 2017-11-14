using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	public class WorldLayer_Roads : WorldLayer_Paths
	{
		private ModuleBase roadDisplacementX = new Perlin(1.0, 2.0, 0.5, 3, 74173887, QualityMode.Medium);

		private ModuleBase roadDisplacementY = new Perlin(1.0, 2.0, 0.5, 3, 67515931, QualityMode.Medium);

		private ModuleBase roadDisplacementZ = new Perlin(1.0, 2.0, 0.5, 3, 87116801, QualityMode.Medium);

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
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Roads);
			WorldGrid grid = Find.WorldGrid;
			List<RoadWorldLayerDef> roadLayerDefs = (from rwld in DefDatabase<RoadWorldLayerDef>.AllDefs
			orderby rwld.order
			select rwld).ToList();
			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (i % 1000 == 0)
				{
					yield return (object)null;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				if (subMesh.verts.Count > 60000)
				{
					subMesh = base.GetSubMesh(WorldMaterials.Roads);
				}
				Tile tile = grid[i];
				if (!tile.WaterCovered)
				{
					List<OutputDirection> outputs = new List<OutputDirection>();
					if (tile.roads != null)
					{
						bool allowSmoothTransition = true;
						for (int j = 0; j < tile.roads.Count - 1; j++)
						{
							Tile.RoadLink roadLink = tile.roads[j];
							string worldTransitionGroup = roadLink.road.worldTransitionGroup;
							Tile.RoadLink roadLink2 = tile.roads[j + 1];
							if (worldTransitionGroup != roadLink2.road.worldTransitionGroup)
							{
								allowSmoothTransition = false;
							}
						}
						for (int k = 0; k < roadLayerDefs.Count; k++)
						{
							bool flag = false;
							outputs.Clear();
							for (int l = 0; l < tile.roads.Count; l++)
							{
								Tile.RoadLink roadLink3 = tile.roads[l];
								RoadDef road = roadLink3.road;
								float layerWidth = road.GetLayerWidth(roadLayerDefs[k]);
								if (layerWidth > 0.0)
								{
									flag = true;
								}
								List<OutputDirection> list = outputs;
								OutputDirection item = default(OutputDirection);
								Tile.RoadLink roadLink4 = tile.roads[l];
								item.neighbor = roadLink4.neighbor;
								item.width = layerWidth;
								item.distortionFrequency = road.distortionFrequency;
								item.distortionIntensity = road.distortionIntensity;
								list.Add(item);
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
			IL_03a2:
			/*Error near IL_03a3: Unexpected return in MoveNext()*/;
		}

		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			Vector3 coordinate = inp * distortionFrequency;
			float magnitude = inp.magnitude;
			inp = (inp + new Vector3(this.roadDisplacementX.GetValue(coordinate), this.roadDisplacementY.GetValue(coordinate), this.roadDisplacementZ.GetValue(coordinate)) * distortionIntensity).normalized * magnitude;
			return inp + inp.normalized * 0.012f;
		}
	}
}
