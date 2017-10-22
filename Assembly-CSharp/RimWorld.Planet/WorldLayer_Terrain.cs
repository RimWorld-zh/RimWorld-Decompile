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
			IEnumerator enumerator = this._003CRegenerate_003E__BaseCallProxy0().GetEnumerator();
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
			for (int num = 0; num < tilesCount; num++)
			{
				Tile tile = tiles[num];
				BiomeDef biome = tile.biome;
				int num2 = default(int);
				LayerSubMesh subMesh = base.GetSubMesh(biome.DrawMaterial, out num2);
				while (num2 >= this.triangleIndexToTileID.Count)
				{
					this.triangleIndexToTileID.Add(new List<int>());
				}
				int count = subMesh.verts.Count;
				int num3 = 0;
				int num4 = (num + 1 >= tileIDToVerts_offsets.Count) ? verts.Count : tileIDToVerts_offsets[num + 1];
				for (int num5 = tileIDToVerts_offsets[num]; num5 < num4; num5++)
				{
					subMesh.verts.Add(verts[num5]);
					subMesh.uvs.Add(this.elevationValues[colorsAndUVsIndex]);
					colorsAndUVsIndex++;
					if (num5 < num4 - 2)
					{
						subMesh.tris.Add(count + num3 + 2);
						subMesh.tris.Add(count + num3 + 1);
						subMesh.tris.Add(count);
						this.triangleIndexToTileID[num2].Add(num);
					}
					num3++;
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
			IL_043c:
			/*Error near IL_043d: Unexpected return in MoveNext()*/;
		}

		public int GetTileIDFromRayHit(RaycastHit hit)
		{
			int num = 0;
			int count = this.meshCollidersInOrder.Count;
			int result;
			while (true)
			{
				if (num < count)
				{
					if ((UnityEngine.Object)this.meshCollidersInOrder[num] == (UnityEngine.Object)hit.collider)
					{
						result = this.triangleIndexToTileID[num][hit.triangleIndex];
						break;
					}
					num++;
					continue;
				}
				result = -1;
				break;
			}
			return result;
		}

		private IEnumerable RegenerateMeshColliders()
		{
			this.meshCollidersInOrder.Clear();
			GameObject gameObject = WorldTerrainColliderManager.GameObject;
			MeshCollider[] components = gameObject.GetComponents<MeshCollider>();
			for (int j = 0; j < components.Length; j++)
			{
				MeshCollider obj = components[j];
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
					for (int num = tileIDToVerts_offsets[i]; num < oneAfterLastVert; num++)
					{
						Vector3 item = new Vector3
						{
							x = elevation
						};
						bool flag = false;
						for (int num2 = tileIDToNeighbors_offsets[i]; num2 < oneAfterLastNeighbor; num2++)
						{
							int num3 = (tileIDToNeighbors_values[num2] + 1 >= tileIDToVerts_offsets.Count) ? verts.Count : tileIDToVerts_offsets[tileIDToNeighbors_values[num2] + 1];
							int num4 = tileIDToVerts_offsets[tileIDToNeighbors_values[num2]];
							while (num4 < num3)
							{
								if (!(verts[num4] == verts[num]))
								{
									num4++;
									continue;
								}
								Tile tile2 = tiles[tileIDToNeighbors_values[num2]];
								if (!flag)
								{
									if (tile2.elevation >= 0.0 && elevation <= 0.0)
									{
										goto IL_02a0;
									}
									if (tile2.elevation <= 0.0 && elevation >= 0.0)
										goto IL_02a0;
									if (tile2.elevation > item.x)
									{
										item.x = tile2.elevation;
									}
								}
								break;
								IL_02a0:
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
