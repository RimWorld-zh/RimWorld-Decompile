using System;
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
				if (enumerator2.MoveNext())
				{
					object result3 = enumerator2.Current;
					yield return result3;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			finally
			{
				IDisposable disposable;
				IDisposable disposable3 = disposable = (enumerator2 as IDisposable);
				if (disposable != null)
				{
					disposable3.Dispose();
				}
			}
			int colorsAndUVsIndex = 0;
			for (int i = 0; i < tilesCount; i++)
			{
				Tile tile = tiles[i];
				BiomeDef biome = tile.biome;
				int num = default(int);
				LayerSubMesh subMesh = base.GetSubMesh(biome.DrawMaterial, out num);
				while (num >= this.triangleIndexToTileID.Count)
				{
					this.triangleIndexToTileID.Add(new List<int>());
				}
				int count = subMesh.verts.Count;
				int num2 = 0;
				int num3 = (i + 1 >= tileIDToVerts_offsets.Count) ? verts.Count : tileIDToVerts_offsets[i + 1];
				for (int j = tileIDToVerts_offsets[i]; j < num3; j++)
				{
					subMesh.verts.Add(verts[j]);
					subMesh.uvs.Add(this.elevationValues[colorsAndUVsIndex]);
					colorsAndUVsIndex++;
					if (j < num3 - 2)
					{
						subMesh.tris.Add(count + num2 + 2);
						subMesh.tris.Add(count + num2 + 1);
						subMesh.tris.Add(count);
						this.triangleIndexToTileID[num].Add(i);
					}
					num2++;
				}
			}
			base.FinalizeMesh(MeshParts.All);
			IEnumerator enumerator3 = this.RegenerateMeshColliders().GetEnumerator();
			try
			{
				if (enumerator3.MoveNext())
				{
					object result2 = enumerator3.Current;
					yield return result2;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			finally
			{
				IDisposable disposable;
				IDisposable disposable4 = disposable = (enumerator3 as IDisposable);
				if (disposable != null)
				{
					disposable4.Dispose();
				}
			}
			this.elevationValues.Clear();
			this.elevationValues.TrimExcess();
			yield break;
			IL_042a:
			/*Error near IL_042b: Unexpected return in MoveNext()*/;
		}

		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int i = 0;
			int count = this.meshCollidersInOrder.Count;
			for (; i < count; i++)
			{
				if ((UnityEngine.Object)this.meshCollidersInOrder[i] == (UnityEngine.Object)hit.collider)
				{
					return this.triangleIndexToTileID[i][hit.triangleIndex];
				}
			}
			return -1;
		}

		private IEnumerable RegenerateMeshColliders()
		{
			this.meshCollidersInOrder.Clear();
			GameObject gameObject = WorldTerrainColliderManager.GameObject;
			MeshCollider[] components = gameObject.GetComponents<MeshCollider>();
			foreach (MeshCollider obj in components)
			{
				UnityEngine.Object.Destroy(obj);
			}
			int i = 0;
			if (i < base.subMeshes.Count)
			{
				MeshCollider comp = gameObject.AddComponent<MeshCollider>();
				comp.sharedMesh = base.subMeshes[i].mesh;
				this.meshCollidersInOrder.Add(comp);
				yield return (object)null;
				/*Error: Unable to find new state assignment for yield return*/;
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
			int i = 0;
			while (true)
			{
				if (i < tilesCount)
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
							int num2 = tileIDToVerts_offsets[tileIDToNeighbors_values[k]];
							while (num2 < num)
							{
								if (!(verts[num2] == verts[j]))
								{
									num2++;
									continue;
								}
								Tile tile2 = tiles[tileIDToNeighbors_values[k]];
								if (!flag)
								{
									if (tile2.elevation >= 0.0 && elevation <= 0.0)
									{
										goto IL_0299;
									}
									if (tile2.elevation <= 0.0 && elevation >= 0.0)
										goto IL_0299;
									if (tile2.elevation > item.x)
									{
										item.x = tile2.elevation;
									}
								}
								break;
								IL_0299:
								flag = true;
								break;
							}
						}
						if (flag)
						{
							item.x = 0f;
						}
						if ((UnityEngine.Object)tile.biome.DrawMaterial.shader != (UnityEngine.Object)ShaderDatabase.WorldOcean && item.x < 0.0)
						{
							item.x = 0f;
						}
						this.elevationValues.Add(item);
					}
					if (i % 1000 != 0)
					{
						i++;
						continue;
					}
					break;
				}
				yield break;
			}
			yield return (object)null;
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
