using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Verse;

namespace RimWorld
{
	internal static class TerrainDefGenerator_Stone
	{
		public static IEnumerable<TerrainDef> ImpliedTerrainDefs()
		{
			int i = 0;
			foreach (ThingDef rock in from def in DefDatabase<ThingDef>.AllDefs
			where def.building != null && def.building.isNaturalRock && !def.building.isResourceRock
			select def)
			{
				TerrainDef rough = new TerrainDef();
				TerrainDef hewn = new TerrainDef();
				TerrainDef smooth = new TerrainDef();
				rough.texturePath = "Terrain/Surfaces/RoughStone";
				rough.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
				rough.pathCost = 2;
				StatUtility.SetStatValueInList(ref rough.statBases, StatDefOf.Beauty, -1f);
				rough.scatterType = "Rocky";
				rough.affordances = new List<TerrainAffordanceDef>();
				rough.affordances.Add(TerrainAffordanceDefOf.Light);
				rough.affordances.Add(TerrainAffordanceDefOf.Medium);
				rough.affordances.Add(TerrainAffordanceDefOf.Heavy);
				rough.affordances.Add(TerrainAffordanceDefOf.SmoothableStone);
				rough.fertility = 0f;
				rough.acceptFilth = false;
				rough.acceptTerrainSourceFilth = false;
				rough.modContentPack = rock.modContentPack;
				rough.renderPrecedence = 190 + i;
				rough.defName = rock.defName + "_Rough";
				rough.label = "RoughStoneTerrainLabel".Translate(new object[]
				{
					rock.label
				});
				rough.description = "RoughStoneTerrainDesc".Translate(new object[]
				{
					rock.label
				});
				rough.color = rock.graphicData.color;
				rock.building.naturalTerrain = rough;
				hewn.texturePath = "Terrain/Surfaces/RoughHewnRock";
				hewn.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
				hewn.pathCost = 1;
				StatUtility.SetStatValueInList(ref hewn.statBases, StatDefOf.Beauty, -1f);
				hewn.scatterType = "Rocky";
				hewn.affordances = new List<TerrainAffordanceDef>();
				hewn.affordances.Add(TerrainAffordanceDefOf.Light);
				hewn.affordances.Add(TerrainAffordanceDefOf.Medium);
				hewn.affordances.Add(TerrainAffordanceDefOf.Heavy);
				hewn.affordances.Add(TerrainAffordanceDefOf.SmoothableStone);
				hewn.fertility = 0f;
				hewn.acceptFilth = true;
				hewn.acceptTerrainSourceFilth = true;
				hewn.modContentPack = rock.modContentPack;
				hewn.renderPrecedence = 50 + i;
				hewn.defName = rock.defName + "_RoughHewn";
				hewn.label = "RoughHewnStoneTerrainLabel".Translate(new object[]
				{
					rock.label
				});
				hewn.description = "RoughHewnStoneTerrainDesc".Translate(new object[]
				{
					rock.label
				});
				hewn.color = rock.graphicData.color;
				rock.building.leaveTerrain = hewn;
				smooth.texturePath = "Terrain/Surfaces/SmoothStone";
				smooth.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
				smooth.pathCost = 0;
				StatUtility.SetStatValueInList(ref smooth.statBases, StatDefOf.Beauty, 2f);
				StatUtility.SetStatValueInList(ref smooth.statBases, StatDefOf.MarketValue, 8f);
				smooth.scatterType = "Rocky";
				smooth.affordances = new List<TerrainAffordanceDef>();
				smooth.affordances.Add(TerrainAffordanceDefOf.Light);
				smooth.affordances.Add(TerrainAffordanceDefOf.Medium);
				smooth.affordances.Add(TerrainAffordanceDefOf.Heavy);
				smooth.fertility = 0f;
				smooth.acceptFilth = true;
				smooth.acceptTerrainSourceFilth = true;
				smooth.modContentPack = rock.modContentPack;
				smooth.renderPrecedence = 140 + i;
				smooth.defName = rock.defName + "_Smooth";
				smooth.label = "SmoothStoneTerrainLabel".Translate(new object[]
				{
					rock.label
				});
				smooth.description = "SmoothStoneTerrainDesc".Translate(new object[]
				{
					rock.label
				});
				smooth.color = rock.graphicData.color;
				rough.smoothedTerrain = smooth;
				hewn.smoothedTerrain = smooth;
				yield return rough;
				yield return hewn;
				yield return smooth;
				i++;
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <ImpliedTerrainDefs>c__Iterator0 : IEnumerable, IEnumerable<TerrainDef>, IEnumerator, IDisposable, IEnumerator<TerrainDef>
		{
			internal int <i>__0;

			internal IEnumerator<ThingDef> $locvar0;

			internal ThingDef <rock>__1;

			internal TerrainDef <rough>__2;

			internal TerrainDef <hewn>__2;

			internal TerrainDef <smooth>__2;

			internal TerrainDef $current;

			internal bool $disposing;

			internal int $PC;

			private static Func<ThingDef, bool> <>f__am$cache0;

			[DebuggerHidden]
			public <ImpliedTerrainDefs>c__Iterator0()
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
					i = 0;
					enumerator = (from def in DefDatabase<ThingDef>.AllDefs
					where def.building != null && def.building.isNaturalRock && !def.building.isResourceRock
					select def).GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
				case 2u:
				case 3u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						this.$current = hewn;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						flag = true;
						return true;
					case 2u:
						this.$current = smooth;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						flag = true;
						return true;
					case 3u:
						i++;
						break;
					}
					if (enumerator.MoveNext())
					{
						rock = enumerator.Current;
						rough = new TerrainDef();
						hewn = new TerrainDef();
						smooth = new TerrainDef();
						rough.texturePath = "Terrain/Surfaces/RoughStone";
						rough.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
						rough.pathCost = 2;
						StatUtility.SetStatValueInList(ref rough.statBases, StatDefOf.Beauty, -1f);
						rough.scatterType = "Rocky";
						rough.affordances = new List<TerrainAffordanceDef>();
						rough.affordances.Add(TerrainAffordanceDefOf.Light);
						rough.affordances.Add(TerrainAffordanceDefOf.Medium);
						rough.affordances.Add(TerrainAffordanceDefOf.Heavy);
						rough.affordances.Add(TerrainAffordanceDefOf.SmoothableStone);
						rough.fertility = 0f;
						rough.acceptFilth = false;
						rough.acceptTerrainSourceFilth = false;
						rough.modContentPack = rock.modContentPack;
						rough.renderPrecedence = 190 + i;
						rough.defName = rock.defName + "_Rough";
						rough.label = "RoughStoneTerrainLabel".Translate(new object[]
						{
							rock.label
						});
						rough.description = "RoughStoneTerrainDesc".Translate(new object[]
						{
							rock.label
						});
						rough.color = rock.graphicData.color;
						rock.building.naturalTerrain = rough;
						hewn.texturePath = "Terrain/Surfaces/RoughHewnRock";
						hewn.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
						hewn.pathCost = 1;
						StatUtility.SetStatValueInList(ref hewn.statBases, StatDefOf.Beauty, -1f);
						hewn.scatterType = "Rocky";
						hewn.affordances = new List<TerrainAffordanceDef>();
						hewn.affordances.Add(TerrainAffordanceDefOf.Light);
						hewn.affordances.Add(TerrainAffordanceDefOf.Medium);
						hewn.affordances.Add(TerrainAffordanceDefOf.Heavy);
						hewn.affordances.Add(TerrainAffordanceDefOf.SmoothableStone);
						hewn.fertility = 0f;
						hewn.acceptFilth = true;
						hewn.acceptTerrainSourceFilth = true;
						hewn.modContentPack = rock.modContentPack;
						hewn.renderPrecedence = 50 + i;
						hewn.defName = rock.defName + "_RoughHewn";
						hewn.label = "RoughHewnStoneTerrainLabel".Translate(new object[]
						{
							rock.label
						});
						hewn.description = "RoughHewnStoneTerrainDesc".Translate(new object[]
						{
							rock.label
						});
						hewn.color = rock.graphicData.color;
						rock.building.leaveTerrain = hewn;
						smooth.texturePath = "Terrain/Surfaces/SmoothStone";
						smooth.edgeType = TerrainDef.TerrainEdgeType.FadeRough;
						smooth.pathCost = 0;
						StatUtility.SetStatValueInList(ref smooth.statBases, StatDefOf.Beauty, 2f);
						StatUtility.SetStatValueInList(ref smooth.statBases, StatDefOf.MarketValue, 8f);
						smooth.scatterType = "Rocky";
						smooth.affordances = new List<TerrainAffordanceDef>();
						smooth.affordances.Add(TerrainAffordanceDefOf.Light);
						smooth.affordances.Add(TerrainAffordanceDefOf.Medium);
						smooth.affordances.Add(TerrainAffordanceDefOf.Heavy);
						smooth.fertility = 0f;
						smooth.acceptFilth = true;
						smooth.acceptTerrainSourceFilth = true;
						smooth.modContentPack = rock.modContentPack;
						smooth.renderPrecedence = 140 + i;
						smooth.defName = rock.defName + "_Smooth";
						smooth.label = "SmoothStoneTerrainLabel".Translate(new object[]
						{
							rock.label
						});
						smooth.description = "SmoothStoneTerrainDesc".Translate(new object[]
						{
							rock.label
						});
						smooth.color = rock.graphicData.color;
						rough.smoothedTerrain = smooth;
						hewn.smoothedTerrain = smooth;
						this.$current = rough;
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
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
				}
				return false;
			}

			TerrainDef IEnumerator<TerrainDef>.Current
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
				case 2u:
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<Verse.TerrainDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<TerrainDef> IEnumerable<TerrainDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new TerrainDefGenerator_Stone.<ImpliedTerrainDefs>c__Iterator0();
			}

			private static bool <>m__0(ThingDef def)
			{
				return def.building != null && def.building.isNaturalRock && !def.building.isResourceRock;
			}
		}
	}
}
