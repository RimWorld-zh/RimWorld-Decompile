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
			foreach (object item2 in base.Regenerate())
			{
				yield return item2;
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
						bool allowSmoothTransitions = true;
						for (int rc2 = 0; rc2 < tile.roads.Count - 1; rc2++)
						{
							Tile.RoadLink roadLink = tile.roads[rc2];
							string worldTransitionGroup = roadLink.road.worldTransitionGroup;
							Tile.RoadLink roadLink2 = tile.roads[rc2 + 1];
							if (worldTransitionGroup != roadLink2.road.worldTransitionGroup)
							{
								allowSmoothTransitions = false;
							}
						}
						for (int roadLayer = 0; roadLayer < roadLayerDefs.Count; roadLayer++)
						{
							bool hasWidth = false;
							outputs.Clear();
							for (int rc = 0; rc < tile.roads.Count; rc++)
							{
								Tile.RoadLink roadLink3 = tile.roads[rc];
								RoadDef roadDef = roadLink3.road;
								float layerWidth = roadDef.GetLayerWidth(roadLayerDefs[roadLayer]);
								if (layerWidth > 0.0)
								{
									hasWidth = true;
								}
								List<OutputDirection> obj = outputs;
								OutputDirection item = default(OutputDirection);
								Tile.RoadLink roadLink4 = tile.roads[rc];
								item.neighbor = roadLink4.neighbor;
								item.width = layerWidth;
								item.distortionFrequency = roadDef.distortionFrequency;
								item.distortionIntensity = roadDef.distortionIntensity;
								obj.Add(item);
							}
							if (hasWidth)
							{
								base.GeneratePaths(subMesh, i, outputs, roadLayerDefs[roadLayer].color, allowSmoothTransitions);
							}
						}
					}
				}
			}
			base.FinalizeMesh(MeshParts.All, false);
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
