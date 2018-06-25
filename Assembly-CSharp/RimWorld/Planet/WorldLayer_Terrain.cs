using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldLayer_Terrain : WorldLayer
	{
		private List<MeshCollider> meshCollidersInOrder = new List<MeshCollider>();

		private List<List<int>> triangleIndexToTileID = new List<List<int>>();

		private List<Vector3> elevationValues = new List<Vector3>();

		public WorldLayer_Terrain()
		{
		}

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

		[DebuggerHidden]
		[CompilerGenerated]
		private IEnumerable <Regenerate>__BaseCallProxy0()
		{
			return base.Regenerate();
		}

		[CompilerGenerated]
		private sealed class <Regenerate>c__Iterator0 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal IEnumerator $locvar0;

			internal object <result>__1;

			internal IDisposable $locvar1;

			internal World <world>__0;

			internal WorldGrid <grid>__0;

			internal int <tilesCount>__0;

			internal List<Tile> <tiles>__0;

			internal List<int> <tileIDToVerts_offsets>__0;

			internal List<Vector3> <verts>__0;

			internal IEnumerator $locvar2;

			internal object <result>__2;

			internal IDisposable $locvar3;

			internal int <colorsAndUVsIndex>__0;

			internal IEnumerator $locvar4;

			internal object <result>__3;

			internal IDisposable $locvar5;

			internal WorldLayer_Terrain $this;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <Regenerate>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = base.<Regenerate>__BaseCallProxy0().GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_154;
				case 3u:
					goto IL_38F;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						result = enumerator.Current;
						this.$current = result;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				world = Find.World;
				grid = world.grid;
				tilesCount = grid.TilesCount;
				tiles = grid.tiles;
				tileIDToVerts_offsets = grid.tileIDToVerts_offsets;
				verts = grid.verts;
				this.triangleIndexToTileID.Clear();
				enumerator2 = base.CalculateInterpolatedVerticesParams().GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_154:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						result2 = enumerator2.Current;
						this.$current = result2;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
				}
				colorsAndUVsIndex = 0;
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
					int num2 = 0;
					int num3 = (i + 1 >= tileIDToVerts_offsets.Count) ? verts.Count : tileIDToVerts_offsets[i + 1];
					for (int k = tileIDToVerts_offsets[i]; k < num3; k++)
					{
						subMesh.verts.Add(verts[k]);
						subMesh.uvs.Add(this.elevationValues[colorsAndUVsIndex]);
						colorsAndUVsIndex++;
						if (k < num3 - 2)
						{
							subMesh.tris.Add(count + num2 + 2);
							subMesh.tris.Add(count + num2 + 1);
							subMesh.tris.Add(count);
							this.triangleIndexToTileID[j].Add(i);
						}
						num2++;
					}
				}
				base.FinalizeMesh(MeshParts.All);
				enumerator3 = base.RegenerateMeshColliders().GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_38F:
					switch (num)
					{
					}
					if (enumerator3.MoveNext())
					{
						result3 = enumerator3.Current;
						this.$current = result3;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						if ((disposable3 = (enumerator3 as IDisposable)) != null)
						{
							disposable3.Dispose();
						}
					}
				}
				this.elevationValues.Clear();
				this.elevationValues.TrimExcess();
				this.$PC = -1;
				return false;
			}

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					break;
				case 2u:
					try
					{
					}
					finally
					{
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
					break;
				case 3u:
					try
					{
					}
					finally
					{
						if ((disposable3 = (enumerator3 as IDisposable)) != null)
						{
							disposable3.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldLayer_Terrain.<Regenerate>c__Iterator0 <Regenerate>c__Iterator = new WorldLayer_Terrain.<Regenerate>c__Iterator0();
				<Regenerate>c__Iterator.$this = this;
				return <Regenerate>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <RegenerateMeshColliders>c__Iterator1 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal GameObject <gameObject>__0;

			internal MeshCollider[] $locvar0;

			internal int $locvar1;

			internal int <i>__1;

			internal MeshCollider <comp>__2;

			internal WorldLayer_Terrain $this;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <RegenerateMeshColliders>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.meshCollidersInOrder.Clear();
					gameObject = WorldTerrainColliderManager.GameObject;
					components = gameObject.GetComponents<MeshCollider>();
					for (j = 0; j < components.Length; j++)
					{
						MeshCollider obj = components[j];
						UnityEngine.Object.Destroy(obj);
					}
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < this.subMeshes.Count)
				{
					comp = gameObject.AddComponent<MeshCollider>();
					comp.sharedMesh = this.subMeshes[i].mesh;
					this.meshCollidersInOrder.Add(comp);
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldLayer_Terrain.<RegenerateMeshColliders>c__Iterator1 <RegenerateMeshColliders>c__Iterator = new WorldLayer_Terrain.<RegenerateMeshColliders>c__Iterator1();
				<RegenerateMeshColliders>c__Iterator.$this = this;
				return <RegenerateMeshColliders>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <CalculateInterpolatedVerticesParams>c__Iterator2 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal World <world>__0;

			internal WorldGrid <grid>__0;

			internal int <tilesCount>__0;

			internal List<Vector3> <verts>__0;

			internal List<int> <tileIDToVerts_offsets>__0;

			internal List<int> <tileIDToNeighbors_offsets>__0;

			internal List<int> <tileIDToNeighbors_values>__0;

			internal List<Tile> <tiles>__0;

			internal int <i>__1;

			internal Tile <tile>__2;

			internal float <elevation>__2;

			internal int <oneAfterLastNeighbor>__2;

			internal int <oneAfterLastVert>__2;

			internal WorldLayer_Terrain $this;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <CalculateInterpolatedVerticesParams>c__Iterator2()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.elevationValues.Clear();
					world = Find.World;
					grid = world.grid;
					tilesCount = grid.TilesCount;
					verts = grid.verts;
					tileIDToVerts_offsets = grid.tileIDToVerts_offsets;
					tileIDToNeighbors_offsets = grid.tileIDToNeighbors_offsets;
					tileIDToNeighbors_values = grid.tileIDToNeighbors_values;
					tiles = grid.tiles;
					i = 0;
					break;
				case 1u:
					IL_395:
					i++;
					break;
				default:
					return false;
				}
				if (i >= tilesCount)
				{
					this.$PC = -1;
				}
				else
				{
					tile = tiles[i];
					elevation = tile.elevation;
					oneAfterLastNeighbor = ((i + 1 >= tileIDToNeighbors_offsets.Count) ? tileIDToNeighbors_values.Count : tileIDToNeighbors_offsets[i + 1]);
					oneAfterLastVert = ((i + 1 >= tilesCount) ? verts.Count : tileIDToVerts_offsets[i + 1]);
					for (int j = tileIDToVerts_offsets[i]; j < oneAfterLastVert; j++)
					{
						Vector3 item = default(Vector3);
						item.x = elevation;
						bool flag = false;
						for (int k = tileIDToNeighbors_offsets[i]; k < oneAfterLastNeighbor; k++)
						{
							int num2 = (tileIDToNeighbors_values[k] + 1 >= tileIDToVerts_offsets.Count) ? verts.Count : tileIDToVerts_offsets[tileIDToNeighbors_values[k] + 1];
							for (int l = tileIDToVerts_offsets[tileIDToNeighbors_values[k]]; l < num2; l++)
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
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_395;
				}
				return false;
			}

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldLayer_Terrain.<CalculateInterpolatedVerticesParams>c__Iterator2 <CalculateInterpolatedVerticesParams>c__Iterator = new WorldLayer_Terrain.<CalculateInterpolatedVerticesParams>c__Iterator2();
				<CalculateInterpolatedVerticesParams>c__Iterator.$this = this;
				return <CalculateInterpolatedVerticesParams>c__Iterator;
			}
		}
	}
}
