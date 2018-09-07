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
	public class WorldLayer_Hills : WorldLayer
	{
		private static readonly FloatRange BaseSizeRange = new FloatRange(0.9f, 1.1f);

		private static readonly IntVec2 TexturesInAtlas = new IntVec2(2, 2);

		private static readonly FloatRange BasePosOffsetRange_SmallHills = new FloatRange(0f, 0.37f);

		private static readonly FloatRange BasePosOffsetRange_LargeHills = new FloatRange(0f, 0.2f);

		private static readonly FloatRange BasePosOffsetRange_Mountains = new FloatRange(0f, 0.08f);

		private static readonly FloatRange BasePosOffsetRange_ImpassableMountains = new FloatRange(0f, 0.08f);

		public WorldLayer_Hills()
		{
		}

		public override IEnumerable Regenerate()
		{
			IEnumerator enumerator = base.Regenerate().GetEnumerator();
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
			Rand.PushState();
			Rand.Seed = Find.World.info.Seed;
			WorldGrid grid = Find.WorldGrid;
			int tilesCount = grid.TilesCount;
			int i = 0;
			while (i < tilesCount)
			{
				Tile tile = grid[i];
				Material material;
				FloatRange floatRange;
				switch (tile.hilliness)
				{
				case Hilliness.SmallHills:
					material = WorldMaterials.SmallHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_SmallHills;
					goto IL_180;
				case Hilliness.LargeHills:
					material = WorldMaterials.LargeHills;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_LargeHills;
					goto IL_180;
				case Hilliness.Mountainous:
					material = WorldMaterials.Mountains;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_Mountains;
					goto IL_180;
				case Hilliness.Impassable:
					material = WorldMaterials.ImpassableMountains;
					floatRange = WorldLayer_Hills.BasePosOffsetRange_ImpassableMountains;
					goto IL_180;
				}
				IL_25C:
				i++;
				continue;
				IL_180:
				LayerSubMesh subMesh = base.GetSubMesh(material);
				Vector3 vector = grid.GetTileCenter(i);
				Vector3 posForTangents = vector;
				float magnitude = vector.magnitude;
				vector = (vector + Rand.UnitVector3 * floatRange.RandomInRange * grid.averageTileSize).normalized * magnitude;
				WorldRendererUtility.PrintQuadTangentialToPlanet(vector, posForTangents, WorldLayer_Hills.BaseSizeRange.RandomInRange * grid.averageTileSize, 0.005f, subMesh, false, true, false);
				WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, WorldLayer_Hills.TexturesInAtlas.x), Rand.Range(0, WorldLayer_Hills.TexturesInAtlas.z), WorldLayer_Hills.TexturesInAtlas.x, WorldLayer_Hills.TexturesInAtlas.z, subMesh);
				goto IL_25C;
			}
			Rand.PopState();
			base.FinalizeMesh(MeshParts.All);
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static WorldLayer_Hills()
		{
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

			internal WorldGrid <grid>__0;

			internal int <tilesCount>__0;

			internal WorldLayer_Hills $this;

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
				Rand.PushState();
				Rand.Seed = Find.World.info.Seed;
				grid = Find.WorldGrid;
				tilesCount = grid.TilesCount;
				int i = 0;
				while (i < tilesCount)
				{
					Tile tile = grid[i];
					Material material;
					FloatRange floatRange;
					switch (tile.hilliness)
					{
					case Hilliness.SmallHills:
						material = WorldMaterials.SmallHills;
						floatRange = WorldLayer_Hills.BasePosOffsetRange_SmallHills;
						goto IL_180;
					case Hilliness.LargeHills:
						material = WorldMaterials.LargeHills;
						floatRange = WorldLayer_Hills.BasePosOffsetRange_LargeHills;
						goto IL_180;
					case Hilliness.Mountainous:
						material = WorldMaterials.Mountains;
						floatRange = WorldLayer_Hills.BasePosOffsetRange_Mountains;
						goto IL_180;
					case Hilliness.Impassable:
						material = WorldMaterials.ImpassableMountains;
						floatRange = WorldLayer_Hills.BasePosOffsetRange_ImpassableMountains;
						goto IL_180;
					}
					IL_25C:
					i++;
					continue;
					IL_180:
					LayerSubMesh subMesh = base.GetSubMesh(material);
					Vector3 vector = grid.GetTileCenter(i);
					Vector3 posForTangents = vector;
					float magnitude = vector.magnitude;
					vector = (vector + Rand.UnitVector3 * floatRange.RandomInRange * grid.averageTileSize).normalized * magnitude;
					WorldRendererUtility.PrintQuadTangentialToPlanet(vector, posForTangents, WorldLayer_Hills.BaseSizeRange.RandomInRange * grid.averageTileSize, 0.005f, subMesh, false, true, false);
					WorldRendererUtility.PrintTextureAtlasUVs(Rand.Range(0, WorldLayer_Hills.TexturesInAtlas.x), Rand.Range(0, WorldLayer_Hills.TexturesInAtlas.z), WorldLayer_Hills.TexturesInAtlas.x, WorldLayer_Hills.TexturesInAtlas.z, subMesh);
					goto IL_25C;
				}
				Rand.PopState();
				base.FinalizeMesh(MeshParts.All);
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
				WorldLayer_Hills.<Regenerate>c__Iterator0 <Regenerate>c__Iterator = new WorldLayer_Hills.<Regenerate>c__Iterator0();
				<Regenerate>c__Iterator.$this = this;
				return <Regenerate>c__Iterator;
			}
		}
	}
}
