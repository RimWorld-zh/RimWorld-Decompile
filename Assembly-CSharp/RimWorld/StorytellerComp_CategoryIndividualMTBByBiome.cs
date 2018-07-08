using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public class StorytellerComp_CategoryIndividualMTBByBiome : StorytellerComp
	{
		public StorytellerComp_CategoryIndividualMTBByBiome()
		{
		}

		protected StorytellerCompProperties_CategoryIndividualMTBByBiome Props
		{
			get
			{
				return (StorytellerCompProperties_CategoryIndividualMTBByBiome)this.props;
			}
		}

		public override IEnumerable<FiringIncident> MakeIntervalIncidents(IIncidentTarget target)
		{
			if (target is World)
			{
				yield break;
			}
			List<IncidentDef> allIncidents = DefDatabase<IncidentDef>.AllDefsListForReading;
			int i = 0;
			while (i < allIncidents.Count)
			{
				IncidentDef inc = allIncidents[i];
				if (inc.category == this.Props.category)
				{
					BiomeDef biome = Find.WorldGrid[target.Tile].biome;
					if (inc.mtbDaysByBiome != null)
					{
						MTBByBiome entry = inc.mtbDaysByBiome.Find((MTBByBiome x) => x.biome == biome);
						if (entry != null)
						{
							float mtb = entry.mtbDays;
							if (this.Props.applyCaravanVisibility)
							{
								Caravan caravan = target as Caravan;
								if (caravan != null)
								{
									mtb /= caravan.Visibility;
								}
								else
								{
									Map map = target as Map;
									if (map != null && map.Parent.def.isTempIncidentMapOwner)
									{
										IEnumerable<Pawn> pawns = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).Concat(map.mapPawns.PrisonersOfColonySpawned);
										mtb /= CaravanVisibilityCalculator.Visibility(pawns, false, null);
									}
								}
							}
							if (Rand.MTBEventOccurs(mtb, 60000f, 1000f))
							{
								IncidentParms parms = this.GenerateParms(inc.category, target);
								if (inc.Worker.CanFireNow(parms, false))
								{
									yield return new FiringIncident(inc, this, parms);
								}
							}
						}
					}
				}
				IL_24F:
				i++;
				continue;
				goto IL_24F;
			}
			yield break;
		}

		public override string ToString()
		{
			return base.ToString() + " " + this.Props.category;
		}

		[CompilerGenerated]
		private sealed class <MakeIntervalIncidents>c__Iterator0 : IEnumerable, IEnumerable<FiringIncident>, IEnumerator, IDisposable, IEnumerator<FiringIncident>
		{
			internal IIncidentTarget target;

			internal List<IncidentDef> <allIncidents>__0;

			internal int <i>__1;

			internal IncidentDef <inc>__2;

			internal MTBByBiome <entry>__3;

			internal float <mtb>__3;

			internal IncidentParms <parms>__4;

			internal StorytellerComp_CategoryIndividualMTBByBiome $this;

			internal FiringIncident $current;

			internal bool $disposing;

			internal int $PC;

			private StorytellerComp_CategoryIndividualMTBByBiome.<MakeIntervalIncidents>c__Iterator0.<MakeIntervalIncidents>c__AnonStorey1 $locvar0;

			[DebuggerHidden]
			public <MakeIntervalIncidents>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (target is World)
					{
						return false;
					}
					allIncidents = DefDatabase<IncidentDef>.AllDefsListForReading;
					i = 0;
					goto IL_25D;
				case 1u:
					IL_24C:
					break;
				default:
					return false;
				}
				IL_24D:
				IL_24E:
				IL_24F:
				i++;
				IL_25D:
				if (i >= allIncidents.Count)
				{
					this.$PC = -1;
				}
				else
				{
					inc = allIncidents[i];
					if (inc.category != base.Props.category)
					{
						goto IL_24E;
					}
					BiomeDef biome = Find.WorldGrid[target.Tile].biome;
					if (inc.mtbDaysByBiome == null)
					{
						goto IL_24F;
					}
					entry = inc.mtbDaysByBiome.Find((MTBByBiome x) => x.biome == biome);
					if (entry == null)
					{
						goto IL_24F;
					}
					mtb = entry.mtbDays;
					if (base.Props.applyCaravanVisibility)
					{
						Caravan caravan = target as Caravan;
						if (caravan != null)
						{
							mtb /= caravan.Visibility;
						}
						else
						{
							Map map = target as Map;
							if (map != null && map.Parent.def.isTempIncidentMapOwner)
							{
								IEnumerable<Pawn> pawns = map.mapPawns.SpawnedPawnsInFaction(Faction.OfPlayer).Concat(map.mapPawns.PrisonersOfColonySpawned);
								mtb /= CaravanVisibilityCalculator.Visibility(pawns, false, null);
							}
						}
					}
					if (!Rand.MTBEventOccurs(mtb, 60000f, 1000f))
					{
						goto IL_24D;
					}
					parms = this.GenerateParms(inc.category, target);
					if (inc.Worker.CanFireNow(parms, false))
					{
						this.$current = new FiringIncident(inc, this, parms);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_24C;
				}
				return false;
			}

			FiringIncident IEnumerator<FiringIncident>.Current
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
				return this.System.Collections.Generic.IEnumerable<RimWorld.FiringIncident>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<FiringIncident> IEnumerable<FiringIncident>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				StorytellerComp_CategoryIndividualMTBByBiome.<MakeIntervalIncidents>c__Iterator0 <MakeIntervalIncidents>c__Iterator = new StorytellerComp_CategoryIndividualMTBByBiome.<MakeIntervalIncidents>c__Iterator0();
				<MakeIntervalIncidents>c__Iterator.$this = this;
				<MakeIntervalIncidents>c__Iterator.target = target;
				return <MakeIntervalIncidents>c__Iterator;
			}

			private sealed class <MakeIntervalIncidents>c__AnonStorey1
			{
				internal BiomeDef biome;

				internal StorytellerComp_CategoryIndividualMTBByBiome.<MakeIntervalIncidents>c__Iterator0 <>f__ref$0;

				public <MakeIntervalIncidents>c__AnonStorey1()
				{
				}

				internal bool <>m__0(MTBByBiome x)
				{
					return x.biome == this.biome;
				}
			}
		}
	}
}
