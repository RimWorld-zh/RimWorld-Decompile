using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000599 RID: 1433
	public class WorldLayer_Terrain : WorldLayer
	{
		// Token: 0x04001023 RID: 4131
		private List<MeshCollider> meshCollidersInOrder = new List<MeshCollider>();

		// Token: 0x04001024 RID: 4132
		private List<List<int>> triangleIndexToTileID = new List<List<int>>();

		// Token: 0x04001025 RID: 4133
		private List<Vector3> elevationValues = new List<Vector3>();

		// Token: 0x06001B57 RID: 6999 RVA: 0x000EB8DC File Offset: 0x000E9CDC
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
			World world = Find.World;
			WorldGrid grid = world.grid;
			int tilesCount = grid.TilesCount;
			List<Tile> tiles = grid.tiles;
			List<int> tileIDToVerts_offsets = grid.tileIDToVerts_offsets;
			List<Vector3> verts = grid.verts;
			this.triangleIndexToTileID.Clear();
			IEnumerator enumerator2 = this.CalculateInterpolatedVerticesParams().GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object result2 = enumerator2.Current;
					yield return result2;
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
			int colorsAndUVsIndex = 0;
			for (int i = 0; i < tilesCount; i++)
			{
				Tile tile = tiles[i];
				BiomeDef biome = tile.biome;
				int j;
				LayerSubMesh subMesh = base.GetSubMesh(biome.DrawMaterial, out j);
				while (j >= this.triangleIndexToTileID.Count)
				{
					this.triangleIndexToTileID.Add(new List<int>());
				}
				int count = subMesh.verts.Count;
				int num = 0;
				int num2 = (i + 1 >= tileIDToVerts_offsets.Count) ? verts.Count : tileIDToVerts_offsets[i + 1];
				for (int k = tileIDToVerts_offsets[i]; k < num2; k++)
				{
					subMesh.verts.Add(verts[k]);
					subMesh.uvs.Add(this.elevationValues[colorsAndUVsIndex]);
					colorsAndUVsIndex++;
					if (k < num2 - 2)
					{
						subMesh.tris.Add(count + num + 2);
						subMesh.tris.Add(count + num + 1);
						subMesh.tris.Add(count);
						this.triangleIndexToTileID[j].Add(i);
					}
					num++;
				}
			}
			base.FinalizeMesh(MeshParts.All);
			IEnumerator enumerator3 = this.RegenerateMeshColliders().GetEnumerator();
			try
			{
				while (enumerator3.MoveNext())
				{
					object result3 = enumerator3.Current;
					yield return result3;
				}
			}
			finally
			{
				IDisposable disposable3;
				if ((disposable3 = (enumerator3 as IDisposable)) != null)
				{
					disposable3.Dispose();
				}
			}
			this.elevationValues.Clear();
			this.elevationValues.TrimExcess();
			yield break;
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x000EB908 File Offset: 0x000E9D08
		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int i = 0;
			int count = this.meshCollidersInOrder.Count;
			while (i < count)
			{
				if (this.meshCollidersInOrder[i] == hit.collider)
				{
					return this.triangleIndexToTileID[i][hit.triangleIndex];
				}
				i++;
			}
			return -1;
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x000EB97C File Offset: 0x000E9D7C
		private IEnumerable RegenerateMeshColliders()
		{
			this.meshCollidersInOrder.Clear();
			GameObject gameObject = WorldTerrainColliderManager.GameObject;
			foreach (MeshCollider obj in gameObject.GetComponents<MeshCollider>())
			{
				UnityEngine.Object.Destroy(obj);
			}
			for (int i = 0; i < this.subMeshes.Count; i++)
			{
				MeshCollider comp = gameObject.AddComponent<MeshCollider>();
				comp.sharedMesh = this.subMeshes[i].mesh;
				this.meshCollidersInOrder.Add(comp);
				yield return null;
			}
			yield break;
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x000EB9A8 File Offset: 0x000E9DA8
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
			for (int i = 0; i < tilesCount; i++)
			{
				Tile tile = tiles[i];
				float elevation = tile.elevation;
				int oneAfterLastNeighbor = (i + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[i + 1];
				int oneAfterLastVert = (i + 1 >= tilesCount) ? verts.Count : tileIDToVerts_offsets[i + 1];
				for (int j = tileIDToVerts_offsets[i]; j < oneAfterLastVert; j++)
				{
					Vector3 item = default(Vector3);
					item.x = elevation;
					bool flag = false;
					for (int k = tileIDToNeighbors_offsets[i]; k < oneAfterLastNeighbor; k++)
					{
						int num = (tileIDToNeighbors_values[k] + 1 >= tileIDToVerts_offsets.Count) ? verts.Count : tileIDToVerts_offsets[tileIDToNeighbors_values[k] + 1];
						for (int l = tileIDToVerts_offsets[tileIDToNeighbors_values[k]]; l < num; l++)
						{
							if (verts[l] == verts[j])
							{
								Tile tile2 = tiles[tileIDToNeighbors_values[k]];
								if (!flag)
								{
									if ((tile2.elevation >= 0f && elevation <= 0f) || (tile2.elevation <= 0f && elevation >= 0f))
									{
										flag = true;
									}
									else if (tile2.elevation > item.x)
									{
										item.x = tile2.elevation;
									}
								}
								break;
							}
						}
					}
					if (flag)
					{
						item.x = 0f;
					}
					if (tile.biome.DrawMaterial.shader != ShaderDatabase.WorldOcean && item.x < 0f)
					{
						item.x = 0f;
					}
					this.elevationValues.Add(item);
				}
				if (i % 1000 == 0)
				{
					yield return null;
				}
			}
			yield break;
		}
	}
}
