using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000737 RID: 1847
	public class CompSpawner : ThingComp
	{
		// Token: 0x04001655 RID: 5717
		private int ticksUntilSpawn;

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x060028BF RID: 10431 RVA: 0x0015BD1C File Offset: 0x0015A11C
		public CompProperties_Spawner PropsSpawner
		{
			get
			{
				return (CompProperties_Spawner)this.props;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x060028C0 RID: 10432 RVA: 0x0015BD3C File Offset: 0x0015A13C
		private bool PowerOn
		{
			get
			{
				CompPowerTrader comp = this.parent.GetComp<CompPowerTrader>();
				return comp != null && comp.PowerOn;
			}
		}

		// Token: 0x060028C1 RID: 10433 RVA: 0x0015BD6C File Offset: 0x0015A16C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.ResetCountdown();
			}
		}

		// Token: 0x060028C2 RID: 10434 RVA: 0x0015BD7B File Offset: 0x0015A17B
		public override void CompTick()
		{
			this.TickInterval(1);
		}

		// Token: 0x060028C3 RID: 10435 RVA: 0x0015BD85 File Offset: 0x0015A185
		public override void CompTickRare()
		{
			this.TickInterval(250);
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x0015BD94 File Offset: 0x0015A194
		private void TickInterval(int interval)
		{
			if (this.parent.Spawned)
			{
				Hive hive = this.parent as Hive;
				if (hive != null)
				{
					if (!hive.active)
					{
						return;
					}
				}
				else if (this.parent.Position.Fogged(this.parent.Map))
				{
					return;
				}
				if (!this.PropsSpawner.requiresPower || this.PowerOn)
				{
					this.ticksUntilSpawn -= interval;
					this.CheckShouldSpawn();
				}
			}
		}

		// Token: 0x060028C5 RID: 10437 RVA: 0x0015BE39 File Offset: 0x0015A239
		private void CheckShouldSpawn()
		{
			if (this.ticksUntilSpawn <= 0)
			{
				this.TryDoSpawn();
				this.ResetCountdown();
			}
		}

		// Token: 0x060028C6 RID: 10438 RVA: 0x0015BE58 File Offset: 0x0015A258
		public bool TryDoSpawn()
		{
			bool result;
			if (!this.parent.Spawned)
			{
				result = false;
			}
			else
			{
				if (this.PropsSpawner.spawnMaxAdjacent >= 0)
				{
					int num = 0;
					for (int i = 0; i < 9; i++)
					{
						IntVec3 c = this.parent.Position + GenAdj.AdjacentCellsAndInside[i];
						if (c.InBounds(this.parent.Map))
						{
							List<Thing> thingList = c.GetThingList(this.parent.Map);
							for (int j = 0; j < thingList.Count; j++)
							{
								if (thingList[j].def == this.PropsSpawner.thingToSpawn)
								{
									num += thingList[j].stackCount;
									if (num >= this.PropsSpawner.spawnMaxAdjacent)
									{
										return false;
									}
								}
							}
						}
					}
				}
				IntVec3 center;
				if (this.TryFindSpawnCell(out center))
				{
					Thing thing = ThingMaker.MakeThing(this.PropsSpawner.thingToSpawn, null);
					thing.stackCount = this.PropsSpawner.spawnCount;
					Thing t;
					GenPlace.TryPlaceThing(thing, center, this.parent.Map, ThingPlaceMode.Direct, out t, null, null);
					if (this.PropsSpawner.spawnForbidden)
					{
						t.SetForbidden(true, true);
					}
					if (this.PropsSpawner.showMessageIfOwned && this.parent.Faction == Faction.OfPlayer)
					{
						Messages.Message("MessageCompSpawnerSpawnedItem".Translate(new object[]
						{
							this.PropsSpawner.thingToSpawn.LabelCap
						}).CapitalizeFirst(), thing, MessageTypeDefOf.PositiveEvent, true);
					}
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x0015C030 File Offset: 0x0015A430
		private bool TryFindSpawnCell(out IntVec3 result)
		{
			foreach (IntVec3 intVec in GenAdj.CellsAdjacent8Way(this.parent).InRandomOrder(null))
			{
				if (intVec.Walkable(this.parent.Map))
				{
					Building edifice = intVec.GetEdifice(this.parent.Map);
					if (edifice == null || !this.PropsSpawner.thingToSpawn.IsEdifice())
					{
						Building_Door building_Door = edifice as Building_Door;
						if (building_Door == null || building_Door.FreePassage)
						{
							if (this.parent.def.passability == Traversability.Impassable || GenSight.LineOfSight(this.parent.Position, intVec, this.parent.Map, false, null, 0, 0))
							{
								bool flag = false;
								List<Thing> thingList = intVec.GetThingList(this.parent.Map);
								for (int i = 0; i < thingList.Count; i++)
								{
									Thing thing = thingList[i];
									if (thing.def.category == ThingCategory.Item && (thing.def != this.PropsSpawner.thingToSpawn || thing.stackCount > this.PropsSpawner.thingToSpawn.stackLimit - this.PropsSpawner.spawnCount))
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									result = intVec;
									return true;
								}
							}
						}
					}
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		// Token: 0x060028C8 RID: 10440 RVA: 0x0015C210 File Offset: 0x0015A610
		private void ResetCountdown()
		{
			this.ticksUntilSpawn = this.PropsSpawner.spawnIntervalRange.RandomInRange;
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x0015C22C File Offset: 0x0015A62C
		public override void PostExposeData()
		{
			string str = (!this.PropsSpawner.saveKeysPrefix.NullOrEmpty()) ? (this.PropsSpawner.saveKeysPrefix + "_") : null;
			Scribe_Values.Look<int>(ref this.ticksUntilSpawn, str + "ticksUntilSpawn", 0, false);
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x0015C284 File Offset: 0x0015A684
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "DEBUG: Spawn " + this.PropsSpawner.thingToSpawn.label,
					icon = TexCommand.DesirePower,
					action = delegate()
					{
						this.TryDoSpawn();
						this.ResetCountdown();
					}
				};
			}
			yield break;
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x0015C2B0 File Offset: 0x0015A6B0
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.PropsSpawner.writeTimeLeftToSpawn && (!this.PropsSpawner.requiresPower || this.PowerOn))
			{
				result = "NextSpawnedItemIn".Translate(new object[]
				{
					GenLabel.ThingLabel(this.PropsSpawner.thingToSpawn, null, this.PropsSpawner.spawnCount)
				}) + ": " + this.ticksUntilSpawn.ToStringTicksToPeriod();
			}
			else
			{
				result = null;
			}
			return result;
		}
	}
}
