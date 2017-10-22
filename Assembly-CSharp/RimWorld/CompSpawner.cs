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

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.ResetCountdown();
			}
		}

		public override void CompTick()
		{
			if (!base.parent.Position.Fogged(base.parent.Map))
			{
				this.ticksUntilSpawn--;
				this.CheckShouldSpawn();
			}
		}

		public override void CompTickRare()
		{
			if (!base.parent.Position.Fogged(base.parent.Map))
			{
				this.ticksUntilSpawn -= 250;
				this.CheckShouldSpawn();
			}
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
							{
								return false;
							}
						}
					}
				}
			}
			IntVec3 center = default(IntVec3);
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
				return true;
			}
			return false;
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
						if ((building_Door == null || building_Door.FreePassage) && GenSight.LineOfSight(base.parent.Position, item, base.parent.Map, false, null, 0, 0))
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
			if (Prefs.DevMode)
			{
				yield return (Gizmo)new Command_Action
				{
					defaultLabel = "DEBUG: Spawn " + this.PropsSpawner.thingToSpawn.label,
					icon = TexCommand.DesirePower,
					action = (Action)delegate
					{
						((_003CCompGetGizmosExtra_003Ec__Iterator16B)/*Error near IL_0076: stateMachine*/)._003C_003Ef__this.TryDoSpawn();
						((_003CCompGetGizmosExtra_003Ec__Iterator16B)/*Error near IL_0076: stateMachine*/)._003C_003Ef__this.ResetCountdown();
					}
				};
			}
		}
	}
}
