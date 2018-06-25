using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200073A RID: 1850
	public class CompSpawnerHives : ThingComp
	{
		// Token: 0x0400165A RID: 5722
		private int nextHiveSpawnTick = -1;

		// Token: 0x0400165B RID: 5723
		public bool canSpawnHives = true;

		// Token: 0x0400165C RID: 5724
		public const int MaxHivesPerMap = 30;

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x060028D8 RID: 10456 RVA: 0x0015C58C File Offset: 0x0015A98C
		private CompProperties_SpawnerHives Props
		{
			get
			{
				return (CompProperties_SpawnerHives)this.props;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x060028D9 RID: 10457 RVA: 0x0015C5AC File Offset: 0x0015A9AC
		private bool CanSpawnChildHive
		{
			get
			{
				return this.canSpawnHives && HivesUtility.TotalSpawnedHivesCount(this.parent.Map) < 30;
			}
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x0015C5E3 File Offset: 0x0015A9E3
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!respawningAfterLoad)
			{
				this.CalculateNextHiveSpawnTick();
			}
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x0015C5F4 File Offset: 0x0015A9F4
		public override void CompTick()
		{
			base.CompTick();
			Hive hive = this.parent as Hive;
			if (hive == null || hive.active)
			{
				if (Find.TickManager.TicksGame >= this.nextHiveSpawnTick)
				{
					Hive t;
					if (this.TrySpawnChildHive(false, out t))
					{
						Messages.Message("MessageHiveReproduced".Translate(), t, MessageTypeDefOf.NegativeEvent, true);
					}
					else
					{
						this.CalculateNextHiveSpawnTick();
					}
				}
			}
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x0015C674 File Offset: 0x0015AA74
		public override string CompInspectStringExtra()
		{
			string result;
			if (!this.canSpawnHives)
			{
				result = "DormantHiveNotReproducing".Translate();
			}
			else if (this.CanSpawnChildHive)
			{
				result = "HiveReproducesIn".Translate() + ": " + (this.nextHiveSpawnTick - Find.TickManager.TicksGame).ToStringTicksToPeriod();
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x0015C6E4 File Offset: 0x0015AAE4
		public void CalculateNextHiveSpawnTick()
		{
			Room room = this.parent.GetRoom(RegionType.Set_Passable);
			int num = 0;
			int num2 = GenRadial.NumCellsInRadius(9f);
			for (int i = 0; i < num2; i++)
			{
				IntVec3 intVec = this.parent.Position + GenRadial.RadialPattern[i];
				if (intVec.InBounds(this.parent.Map))
				{
					if (intVec.GetRoom(this.parent.Map, RegionType.Set_Passable) == room)
					{
						if (intVec.GetThingList(this.parent.Map).Any((Thing t) => t is Hive))
						{
							num++;
						}
					}
				}
			}
			float num3 = GenMath.LerpDouble(0f, 7f, 1f, 0.35f, (float)Mathf.Clamp(num, 0, 7));
			this.nextHiveSpawnTick = Find.TickManager.TicksGame + (int)(this.Props.HiveSpawnIntervalDays.RandomInRange * 60000f / (num3 * Find.Storyteller.difficulty.enemyReproductionRateFactor));
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x0015C814 File Offset: 0x0015AC14
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
				IntVec3 loc = CompSpawnerHives.FindChildHiveLocation(this.parent.Position, this.parent.Map, this.parent.def, this.Props, ignoreRoofedRequirement);
				if (!loc.IsValid)
				{
					newHive = null;
					result = false;
				}
				else
				{
					newHive = (Hive)GenSpawn.Spawn(this.parent.def, loc, this.parent.Map, WipeMode.FullRefund);
					if (newHive.Faction != this.parent.Faction)
					{
						newHive.SetFaction(this.parent.Faction, null);
					}
					Hive hive = this.parent as Hive;
					if (hive != null)
					{
						newHive.active = hive.active;
					}
					this.CalculateNextHiveSpawnTick();
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x0015C8F8 File Offset: 0x0015ACF8
		public static IntVec3 FindChildHiveLocation(IntVec3 pos, Map map, ThingDef parentDef, CompProperties_SpawnerHives props, bool ignoreRoofedRequirement)
		{
			IntVec3 intVec = IntVec3.Invalid;
			for (int i = 0; i < 2; i++)
			{
				float minDist = props.HiveSpawnPreferredMinDist;
				if (i == 1)
				{
					minDist = 0f;
				}
				if (CellFinder.TryFindRandomReachableCellNear(pos, map, props.HiveSpawnRadius, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement), null, out intVec, 999999))
				{
					intVec = CellFinder.FindNoWipeSpawnLocNear(intVec, map, parentDef, Rot4.North, 2, (IntVec3 c) => CompSpawnerHives.CanSpawnHiveAt(c, map, pos, parentDef, minDist, ignoreRoofedRequirement));
					break;
				}
			}
			return intVec;
		}

		// Token: 0x060028E0 RID: 10464 RVA: 0x0015C9DC File Offset: 0x0015ADDC
		private static bool CanSpawnHiveAt(IntVec3 c, Map map, IntVec3 parentPos, ThingDef parentDef, float minDist, bool ignoreRoofedRequirement)
		{
			bool result;
			if ((!ignoreRoofedRequirement && !c.Roofed(map)) || (!c.Walkable(map) || (minDist != 0f && (float)c.DistanceToSquared(parentPos) < minDist * minDist)) || c.GetFirstThing(map, ThingDefOf.InsectJelly) != null || c.GetFirstThing(map, ThingDefOf.GlowPod) != null)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < 9; i++)
				{
					IntVec3 c2 = c + GenAdj.AdjacentCellsAndInside[i];
					if (c2.InBounds(map))
					{
						List<Thing> thingList = c2.GetThingList(map);
						for (int j = 0; j < thingList.Count; j++)
						{
							if (thingList[j] is Hive || thingList[j] is TunnelHiveSpawner)
							{
								return false;
							}
						}
					}
				}
				List<Thing> thingList2 = c.GetThingList(map);
				for (int k = 0; k < thingList2.Count; k++)
				{
					Thing thing = thingList2[k];
					bool flag = thing.def.category == ThingCategory.Building && thing.def.passability == Traversability.Impassable;
					if (flag && GenSpawn.SpawningWipes(parentDef, thing.def))
					{
						return true;
					}
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x0015CB60 File Offset: 0x0015AF60
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Reproduce",
					icon = TexCommand.GatherSpotActive,
					action = delegate()
					{
						Hive hive;
						this.TrySpawnChildHive(false, out hive);
					}
				};
			}
			yield break;
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x0015CB8A File Offset: 0x0015AF8A
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.nextHiveSpawnTick, "nextHiveSpawnTick", 0, false);
			Scribe_Values.Look<bool>(ref this.canSpawnHives, "canSpawnHives", true, false);
		}
	}
}
