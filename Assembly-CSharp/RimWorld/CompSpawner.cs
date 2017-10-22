using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class CompSpawner : ThingComp
	{
		private int ticksUntilSpawn;

		public CompProperties_Spawner PropsSpawner
		{
			get
			{
				return (CompProperties_Spawner)base.props;
			}
		}

		private bool PowerOn
		{
			get
			{
				CompPowerTrader comp = base.parent.GetComp<CompPowerTrader>();
				return comp != null && comp.PowerOn;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.ResetCountdown();
			}
		}

		public override void CompTick()
		{
			this.TickInterval(1);
		}

		public override void CompTickRare()
		{
			this.TickInterval(250);
		}

		private void TickInterval(int interval)
		{
			Hive hive = base.parent as Hive;
			if (hive != null)
			{
				if (!hive.active)
					return;
			}
			else if (base.parent.Position.Fogged(base.parent.Map))
				return;
			if (this.PropsSpawner.requiresPower && !this.PowerOn)
				return;
			this.ticksUntilSpawn -= interval;
			this.CheckShouldSpawn();
		}

		private void CheckShouldSpawn()
		{
			if (this.ticksUntilSpawn <= 0)
			{
				this.TryDoSpawn();
				this.ResetCountdown();
			}
		}

		public bool TryDoSpawn()
		{
			if (this.PropsSpawner.spawnMaxAdjacent >= 0)
			{
				int num = 0;
				for (int i = 0; i < 9; i++)
				{
					List<Thing> thingList = (base.parent.Position + GenAdj.AdjacentCellsAndInside[i]).GetThingList(base.parent.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						if (thingList[j].def == this.PropsSpawner.thingToSpawn)
						{
							num += thingList[j].stackCount;
							if (num >= this.PropsSpawner.spawnMaxAdjacent)
								goto IL_0093;
						}
					}
				}
			}
			IntVec3 center = default(IntVec3);
			bool result;
			if (this.TryFindSpawnCell(out center))
			{
				Thing thing = ThingMaker.MakeThing(this.PropsSpawner.thingToSpawn, null);
				thing.stackCount = this.PropsSpawner.spawnCount;
				Thing t = default(Thing);
				GenPlace.TryPlaceThing(thing, center, base.parent.Map, ThingPlaceMode.Direct, out t, (Action<Thing, int>)null);
				if (this.PropsSpawner.spawnForbidden)
				{
					t.SetForbidden(true, true);
				}
				if (this.PropsSpawner.showMessageIfOwned && base.parent.Faction == Faction.OfPlayer)
				{
					Messages.Message("MessageCompSpawnerSpawnedItem".Translate(this.PropsSpawner.thingToSpawn.label).CapitalizeFirst(), thing, MessageTypeDefOf.PositiveEvent);
				}
				result = true;
			}
			else
			{
				result = false;
			}
			goto IL_018e;
			IL_018e:
			return result;
			IL_0093:
			result = false;
			goto IL_018e;
		}

		private bool TryFindSpawnCell(out IntVec3 result)
		{
			foreach (IntVec3 item in GenAdj.CellsAdjacent8Way(base.parent).InRandomOrder(null))
			{
				if (item.Walkable(base.parent.Map))
				{
					Building edifice = item.GetEdifice(base.parent.Map);
					if (edifice == null || !this.PropsSpawner.thingToSpawn.IsEdifice())
					{
						Building_Door building_Door = edifice as Building_Door;
						if ((building_Door == null || building_Door.FreePassage) && (base.parent.def.passability == Traversability.Impassable || GenSight.LineOfSight(base.parent.Position, item, base.parent.Map, false, null, 0, 0)))
						{
							bool flag = false;
							List<Thing> thingList = item.GetThingList(base.parent.Map);
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
								result = item;
								return true;
							}
						}
					}
				}
			}
			result = IntVec3.Invalid;
			return false;
		}

		private void ResetCountdown()
		{
			this.ticksUntilSpawn = this.PropsSpawner.spawnIntervalRange.RandomInRange;
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksUntilSpawn, "ticksUntilSpawn", 0, false);
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "DEBUG: Spawn " + this.PropsSpawner.thingToSpawn.label,
				icon = TexCommand.DesirePower,
				action = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_0078: stateMachine*/)._0024this.TryDoSpawn();
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_0078: stateMachine*/)._0024this.ResetCountdown();
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override string CompInspectStringExtra()
		{
			return (!this.PropsSpawner.writeTimeLeftToSpawn || (this.PropsSpawner.requiresPower && !this.PowerOn)) ? null : ("NextSpawnedItemIn".Translate(GenLabel.ThingLabel(this.PropsSpawner.thingToSpawn, null, this.PropsSpawner.spawnCount)) + ": " + this.ticksUntilSpawn.ToStringTicksToPeriod(true, false, true));
		}
	}
}
