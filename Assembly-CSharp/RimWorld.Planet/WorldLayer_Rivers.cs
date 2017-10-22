using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Noise;

namespace RimWorld.Planet
{
	public class WorldLayer_Rivers : WorldLayer_Paths
	{
		private const float PerlinFrequency = 0.6f;

		private const float PerlinMagnitude = 0.1f;

		private Color32 riverColor = new Color32((byte)73, (byte)82, (byte)100, (byte)255);

		private ModuleBase riverDisplacementX = new Perlin(0.60000002384185791, 2.0, 0.5, 3, 84905524, QualityMode.Medium);

		private ModuleBase riverDisplacementY = new Perlin(0.60000002384185791, 2.0, 0.5, 3, 37971116, QualityMode.Medium);

		private ModuleBase riverDisplacementZ = new Perlin(0.60000002384185791, 2.0, 0.5, 3, 91572032, QualityMode.Medium);

		public WorldLayer_Rivers()
		{
			base.pointyEnds = true;
		}

		public override IEnumerable Regenerate()
		{
			foreach (object item3 in base.Regenerate())
			{
				yield return item3;
			}
			LayerSubMesh subMesh = base.GetSubMesh(WorldMaterials.Rivers);
			LayerSubMesh subMeshBorder = base.GetSubMesh(WorldMaterials.RiversBorder);
			WorldGrid grid = Find.WorldGrid;
			List<OutputDirection> outputs = new List<OutputDirection>();
			List<OutputDirection> outputsBorder = new List<OutputDirection>();
			for (int i = 0; i < grid.TilesCount; i++)
			{
				if (i % 1000 == 0)
				{
					yield return (object)null;
				}
				if (subMesh.verts.Count > 60000)
				{
					subMesh = base.GetSubMesh(WorldMaterials.Rivers);
					subMeshBorder = base.GetSubMesh(WorldMaterials.RiversBorder);
				}
				Tile tile = grid[i];
				if (tile.rivers != null)
				{
					outputs.Clear();
					outputsBorder.Clear();
					for (int rc = 0; rc < tile.rivers.Count; rc++)
					{
						List<OutputDirection> obj = outputs;
						OutputDirection item = default(OutputDirection);
						Tile.RiverLink riverLink = tile.rivers[rc];
						item.neighbor = riverLink.neighbor;
						Tile.RiverLink riverLink2 = tile.rivers[rc];
						item.width = (float)(riverLink2.river.widthOnWorld - 0.20000000298023224);
						obj.Add(item);
						List<OutputDirection> obj2 = outputsBorder;
						OutputDirection item2 = default(OutputDirection);
						Tile.RiverLink riverLink3 = tile.rivers[rc];
						item2.neighbor = riverLink3.neighbor;
						Tile.RiverLink riverLink4 = tile.rivers[rc];
						item2.width = riverLink4.river.widthOnWorld;
						obj2.Add(item2);
					}
					base.GeneratePaths(subMesh, i, outputs, this.riverColor, true);
					base.GeneratePaths(subMeshBorder, i, outputsBorder, this.riverColor, true);
				}
			}
			base.FinalizeMesh(MeshParts.All, false);
		}

		public override Vector3 FinalizePoint(Vector3 inp, float distortionFrequency, float distortionIntensity)
		{
			float magnitude = inp.magnitude;
			inp = (inp + new Vector3(this.riverDisplacementX.GetValue(inp), this.riverDisplacementY.GetValue(inp), this.riverDisplacementZ.GetValue(inp)) * 0.1f).normalized * magnitude;
			return inp + inp.normalized * 0.008f;
		}
	}
}
