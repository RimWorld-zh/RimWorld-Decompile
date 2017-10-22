using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_Terrain : WorldLayer
	{
		private List<MeshCollider> meshCollidersInOrder = new List<MeshCollider>();

		private List<List<int>> triangleIndexToTileID = new List<List<int>>();

		private List<Vector3> elevationValues = new List<Vector3>();

		public override IEnumerable Regenerate()
		{
			foreach (object item in base.Regenerate())
			{
				yield return item;
			}
			World world = Find.World;
			WorldGrid grid = world.grid;
			int tilesCount = grid.TilesCount;
			List<Tile> tiles = grid.tiles;
			List<int> tileIDToVerts_offsets = grid.tileIDToVerts_offsets;
			List<Vector3> verts = grid.verts;
			this.triangleIndexToTileID.Clear();
			foreach (object item2 in this.CalculateInterpolatedVerticesParams())
			{
				yield return item2;
			}
			int colorsAndUVsIndex = 0;
			for (int j = 0; j < tilesCount; j++)
			{
				Tile tile = tiles[j];
				BiomeDef biome = tile.biome;
				int subMeshIndex;
				LayerSubMesh subMesh = base.GetSubMesh(biome.DrawMaterial, out subMeshIndex);
				while (subMeshIndex >= this.triangleIndexToTileID.Count)
				{
					this.triangleIndexToTileID.Add(new List<int>());
				}
				int startVertIndex = subMesh.verts.Count;
				int vertIndex = 0;
				int oneAfterLastVert = (j + 1 >= tileIDToVerts_offsets.Count) ? verts.Count : tileIDToVerts_offsets[j + 1];
				for (int i = tileIDToVerts_offsets[j]; i < oneAfterLastVert; i++)
				{
					subMesh.verts.Add(verts[i]);
					subMesh.uvs.Add(this.elevationValues[colorsAndUVsIndex]);
					colorsAndUVsIndex++;
					if (i < oneAfterLastVert - 2)
					{
						subMesh.tris.Add(startVertIndex + vertIndex + 2);
						subMesh.tris.Add(startVertIndex + vertIndex + 1);
						subMesh.tris.Add(startVertIndex);
						this.triangleIndexToTileID[subMeshIndex].Add(j);
					}
					vertIndex++;
				}
			}
			base.FinalizeMesh(MeshParts.All, true);
			foreach (object item3 in this.RegenerateMeshColliders())
			{
				yield return item3;
			}
			this.elevationValues.Clear();
			this.elevationValues.TrimExcess();
		}

		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int num = 0;
			int count = this.meshCollidersInOrder.Count;
			while (num < count)
			{
				if ((Object)this.meshCollidersInOrder[num] == (Object)hit.collider)
				{
					return this.triangleIndexToTileID[num][hit.triangleIndex];
				}
				num++;
			}
			return -1;
		}

		private IEnumerable RegenerateMeshColliders()
		{
			this.meshCollidersInOrder.Clear();
			GameObject gameObject = WorldTerrainColliderManager.GameObject;
			MeshCollider[] components = gameObject.GetComponents<MeshCollider>();
			for (int j = 0; j < components.Length; j++)
			{
				MeshCollider component = components[j];
				Object.Destroy(component);
			}
			for (int i = 0; i < base.subMeshes.Count; i++)
			{
				MeshCollider comp = gameObject.AddComponent<MeshCollider>();
				comp.sharedMesh = base.subMeshes[i].mesh;
				this.meshCollidersInOrder.Add(comp);
				yield return (object)null;
			}
		}

		private IEnumerable CalculateInterpolatedVerticesParams()
		{
			this.elevationValues.Clear();
			World world = Find.World;
			WorldGrid grid = world.grid;
			int tilesCount = grid.TilesCount;
			List<Vector3> verts = grid.verts;
			List<int> tileIDToVerts_offsets = grid.tileIDToVerts_offsets;
			List<int> tileIDToNeighbors_offsets = grid.tileIDToNeighbors_offsets;
			List<int> tileIDToNeighbors_values = grid.tileIDToNeighbors_values;
			List<Tile> tiles = grid.tiles;
			for (int l = 0; l < tilesCount; l++)
			{
				Tile tile = tiles[l];
				float elevation = tile.elevation;
				int oneAfterLastNeighbor = (l + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[l + 1];
				int oneAfterLastVert = (l + 1 >= tilesCount) ? verts.Count : tileIDToVerts_offsets[l + 1];
				for (int k = tileIDToVerts_offsets[l]; k < oneAfterLastVert; k++)
				{
					Vector3 elevationVal = new Vector3
					{
						x = elevation
					};
					bool isCoast = false;
					for (int j = tileIDToNeighbors_offsets[l]; j < oneAfterLastNeighbor; j++)
					{
						int oneAfterLastNeighVert = (tileIDToNeighbors_values[j] + 1 >= tileIDToVerts_offsets.Count) ? verts.Count : tileIDToVerts_offsets[tileIDToNeighbors_values[j] + 1];
						int i = tileIDToVerts_offsets[tileIDToNeighbors_values[j]];
						while (i < oneAfterLastNeighVert)
						{
							if (!(verts[i] == verts[k]))
							{
								i++;
								continue;
							}
							Tile neigh = tiles[tileIDToNeighbors_values[j]];
							if (!isCoast)
							{
								if (neigh.elevation >= 0.0 && elevation <= 0.0)
								{
									goto IL_02e4;
								}
								if (neigh.elevation <= 0.0 && elevation >= 0.0)
									goto IL_02e4;
								if (neigh.elevation > elevationVal.x)
								{
									elevationVal.x = neigh.elevation;
								}
							}
							break;
							IL_02e4:
							isCoast = true;
							break;
						}
					}
					if (isCoast)
					{
						elevationVal.x = 0f;
					}
					if ((Object)tile.biome.DrawMaterial.shader != (Object)ShaderDatabase.WorldOcean && elevationVal.x < 0.0)
					{
						elevationVal.x = 0f;
					}
					this.elevationValues.Add(elevationVal);
				}
				if (l % 1000 == 0)
				{
					yield return (object)null;
				}
			}
		}
	}
}
