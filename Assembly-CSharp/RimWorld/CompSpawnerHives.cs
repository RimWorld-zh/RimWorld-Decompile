using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompSpawnerHives : ThingComp
	{
		private int nextHiveSpawnTick = -1;

		public bool canSpawnHives = true;

		public const int MaxHivesPerMap = 30;

		private CompProperties_SpawnerHives Props
		{
			get
			{
				return (CompProperties_SpawnerHives)base.props;
			}
		}

		private bool CanSpawnChildHive
		{
			get
			{
				return this.canSpawnHives && HivesUtility.TotalSpawnedHivesCount(base.parent.Map) < 30;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.CalculateNextHiveSpawnTick();
			}
		}

		public override void CompTickRare()
		{
			Hive hive = base.parent as Hive;
			if (hive != null && !hive.active)
				return;
			if (Find.TickManager.TicksGame >= this.nextHiveSpawnTick)
			{
				Hive hive2 = default(Hive);
				if (this.TrySpawnChildHive(false, out hive2))
				{
					hive2.nextPawnSpawnTick = Find.TickManager.TicksGame + Rand.Range(150, 350);
					Messages.Message("MessageHiveReproduced".Translate(), (Thing)hive2, MessageTypeDefOf.NegativeEvent);
				}
				else
				{
					this.CalculateNextHiveSpawnTick();
				}
			}
		}

		public override string CompInspectStringExtra()
		{
			return this.canSpawnHives ? ((!this.CanSpawnChildHive) ? null : ("HiveReproducesIn".Translate() + ": " + (this.nextHiveSpawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true))) : "DormantHiveNotReproducing".Translate();
		}

		public void CalculateNextHiveSpawnTick()
		{
			Room room = base.parent.GetRoom(RegionType.Set_Passable);
			int num = 0;
			int num2 = GenRadial.NumCellsInRadius(9f);
			for (int num3 = 0; num3 < num2; num3++)
			{
				IntVec3 intVec = base.parent.Position + GenRadial.RadialPattern[num3];
				if (intVec.InBounds(base.parent.Map) && intVec.GetRoom(base.parent.Map, RegionType.Set_Passable) == room && intVec.GetThingList(base.parent.Map).Any((Predicate<Thing>)((Thing t) => t is Hive)))
				{
					num++;
				}
			}
			float num4 = GenMath.LerpDouble(0f, 7f, 1f, 0.35f, (float)Mathf.Clamp(num, 0, 7));
			this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)(this.Props.HiveSpawnIntervalDays.RandomInRange * 60000.0 / (num4 * Find.Storyteller.difficulty.enemyReproductionRateFactor));
		}

		public bool TrySpawnChildHive(bool ignoreRoofedRequirement, out Hive newHive)
		{
			bool result;
			if (!this.CanSpawnChildHive)
			{
				newHive = null;
				result = false;
			}
			else
			{
				IntVec3 invalid = IntVec3.Invalid;
				int num = 0;
				while (num < 3)
				{
					float minDist = this.Props.HiveSpawnPreferredMinDist;
					switch (num)
					{
					case 2:
						goto IL_0079;
					case 1:
					{
						minDist = 0f;
						break;
					}
					}
					if (!CellFinder.TryFindRandomReachableCellNear(base.parent.Position, base.parent.Map, this.Props.HiveSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (Predicate<IntVec3>)((IntVec3 c) => this.CanSpawnHiveAt(c, minDist, ignoreRoofedRequirement)), (Predicate<Region>)null, out invalid, 999999))
					{
						num++;
						continue;
					}
					break;
				}
				newHive = (Hive)GenSpawn.Spawn(base.parent.def, invalid, base.parent.Map);
				if (newHive.Faction != base.parent.Faction)
				{
					newHive.SetFaction(base.parent.Faction, null);
				}
				Hive hive = base.parent as Hive;
				if (hive != null)
				{
					newHive.active = hive.active;
				}
				this.CalculateNextHiveSpawnTick();
				result = true;
			}
			goto IL_015a;
			IL_0079:
			newHive = null;
			result = false;
			goto IL_015a;
			IL_015a:
			return result;
		}

		private bool CanSpawnHiveAt(IntVec3 c, float minDist, bool ignoreRoofedRequirement)
		{
			bool result;
			if ((ignoreRoofedRequirement || c.Roofed(base.parent.Map)) && c.Standable(base.parent.Map) && (minDist == 0.0 || (float)c.DistanceToSquared(base.parent.Position) >= minDist * minDist))
			{
				for (int i = 0; i < 8; i++)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCells[i];
					if (c2.InBounds(base.parent.Map))
					{
						List<Thing> thingList = c2.GetThingList(base.parent.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j] is Hive)
								goto IL_00c7;
						}
					}
				}
				List<Thing> thingList2 = c.GetThingList(base.parent.Map);
				for (int k = 0; k < thingList2.Count; k++)
				{
					Thing thing = thingList2[k];
					if ((thing.def.category == ThingCategory.Item || thing.def.category == ThingCategory.Building) && GenSpawn.SpawningWipes(base.parent.def, thing.def))
						goto IL_0155;
				}
				result = true;
				goto IL_0179;
			}
			result = false;
			goto IL_0179;
			IL_0155:
			result = false;
			goto IL_0179;
			IL_0179:
			return result;
			IL_00c7:
			result = false;
			goto IL_0179;
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "DEBUG: Reproduce",
				icon = TexCommand.GatherSpotActive,
				action = (Action)delegate
				{
					Hive hive = default(Hive);
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_005e: stateMachine*/)._0024this.TrySpawnChildHive(false, out hive);
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}

		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextHiveSpawnTick, "nextHiveSpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canSpawnHives, "canSpawnHives", true, false);
		}
	}
}
