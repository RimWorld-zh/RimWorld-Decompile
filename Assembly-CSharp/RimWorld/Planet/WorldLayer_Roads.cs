using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
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

		public WorldLayer_Roads()
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

			internal LayerSubMesh <subMesh>__0;

			internal WorldGrid <grid>__0;

			internal List<RoadWorldLayerDef> <roadLayerDefs>__0;

			internal int <i>__2;

			internal Tile <tile>__3;

			internal List<WorldLayer_Paths.OutputDirection> <outputs>__3;

			internal WorldLayer_Roads $this;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<RoadWorldLayerDef, int> <>f__am$cache0;

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
					IL_152:
					if (subMesh.verts.Count > 60000)
					{
						subMesh = base.GetSubMesh(WorldMaterials.Roads);
					}
					tile = grid[i];
					if (!tile.WaterCovered)
					{
						outputs = new List<WorldLayer_Paths.OutputDirection>();
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
								bool flag2 = false;
								outputs.Clear();
								for (int l = 0; l < tile.potentialRoads.Count; l++)
								{
									RoadDef road = tile.potentialRoads[l].road;
									float layerWidth = road.GetLayerWidth(roadLayerDefs[k]);
									if (layerWidth > 0f)
									{
										flag2 = true;
									}
									outputs.Add(new WorldLayer_Paths.OutputDirection
									{
										neighbor = tile.potentialRoads[l].neighbor,
										width = layerWidth,
										distortionFrequency = road.distortionFrequency,
										distortionIntensity = road.distortionIntensity
									});
								}
								if (flag2)
								{
									base.GeneratePaths(subMesh, i, outputs, roadLayerDefs[k].color, allowSmoothTransition);
								}
							}
						}
					}
					i++;
					goto IL_384;
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
				subMesh = base.GetSubMesh(WorldMaterials.Roads);
				grid = Find.WorldGrid;
				roadLayerDefs = (from rwld in DefDatabase<RoadWorldLayerDef>.AllDefs
				orderby rwld.order
				select rwld).ToList<RoadWorldLayerDef>();
				i = 0;
				IL_384:
				if (i >= grid.TilesCount)
				{
					base.FinalizeMesh(MeshParts.All);
					this.$PC = -1;
				}
				else
				{
					if (i % 1000 == 0)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_152;
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
				WorldLayer_Roads.<Regenerate>c__Iterator0 <Regenerate>c__Iterator = new WorldLayer_Roads.<Regenerate>c__Iterator0();
				<Regenerate>c__Iterator.$this = this;
				return <Regenerate>c__Iterator;
			}

			private static int <>m__0(RoadWorldLayerDef rwld)
			{
				return rwld.order;
			}
		}
	}
}
