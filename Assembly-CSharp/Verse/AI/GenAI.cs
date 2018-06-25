using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AE4 RID: 2788
	public static class GenAI
	{
		// Token: 0x06003DBD RID: 15805 RVA: 0x002094A8 File Offset: 0x002078A8
		public static bool MachinesLike(Faction machineFaction, Pawn p)
		{
			return (p.Faction != null || !p.NonHumanlikeOrWildMan() || (p.HostFaction == machineFaction && !p.IsPrisoner)) && (!p.IsPrisoner || p.HostFaction != machineFaction) && (p.Faction == null || !p.Faction.HostileTo(machineFaction));
		}

		// Token: 0x06003DBE RID: 15806 RVA: 0x00209534 File Offset: 0x00207934
		public static bool CanUseItemForWork(Pawn p, Thing item)
		{
			return !item.IsForbidden(p) && p.CanReserveAndReach(item, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false);
		}

		// Token: 0x06003DBF RID: 15807 RVA: 0x00209580 File Offset: 0x00207980
		public static bool CanBeArrestedBy(this Pawn pawn, Pawn arrester)
		{
			return pawn.RaceProps.Humanlike && (!pawn.InAggroMentalState || !pawn.HostileTo(arrester)) && !pawn.HostileTo(Faction.OfPlayer) && (!pawn.IsPrisonerOfColony || !pawn.Position.IsInPrisonCell(pawn.Map));
		}

		// Token: 0x06003DC0 RID: 15808 RVA: 0x00209604 File Offset: 0x00207A04
		public static bool InDangerousCombat(Pawn pawn)
		{
			Region root = pawn.GetRegion(RegionType.Set_Passable);
			bool found = false;
			RegionTraverser.BreadthFirstTraverse(root, (Region r1, Region r2) => r2.Room == root.Room, (Region r) => r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn).Any(delegate(Thing t)
			{
				Pawn pawn2 = t as Pawn;
				bool result;
				if (pawn2 != null && !pawn2.Downed && (float)(pawn.Position - pawn2.Position).LengthHorizontalSquared < 144f && pawn2.HostileTo(pawn.Faction))
				{
					found = true;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}), 9, RegionType.Set_Passable);
			return found;
		}

		// Token: 0x06003DC1 RID: 15809 RVA: 0x0020966C File Offset: 0x00207A6C
		public static IntVec3 RandomRaidDest(IntVec3 raidSpawnLoc, Map map)
		{
			List<ThingDef> allBedDefBestToWorst = RestUtility.AllBedDefBestToWorst;
			List<Building> list = new List<Building>(map.mapPawns.FreeColonistsAndPrisonersSpawnedCount);
			for (int i = 0; i < allBedDefBestToWorst.Count; i++)
			{
				foreach (Building building in map.listerBuildings.AllBuildingsColonistOfDef(allBedDefBestToWorst[i]))
				{
					if (((Building_Bed)building).owners.Any<Pawn>() && map.reachability.CanReach(raidSpawnLoc, building, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly))
					{
						list.Add(building);
					}
				}
			}
			Building building2;
			IntVec3 result;
			if (list.TryRandomElement(out building2))
			{
				result = building2.Position;
			}
			else
			{
				IEnumerable<Building> source = from b in map.listerBuildings.allBuildingsColonist
				where !b.def.building.ai_combatDangerous && !b.def.building.isInert
				select b;
				if (source.Any<Building>())
				{
					for (int j = 0; j < 500; j++)
					{
						Building t = source.RandomElement<Building>();
						IntVec3 intVec = t.RandomAdjacentCell8Way();
						if (intVec.Walkable(map) && map.reachability.CanReach(raidSpawnLoc, intVec, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly))
						{
							return intVec;
						}
					}
				}
				Pawn pawn;
				IntVec3 intVec2;
				if ((from x in map.mapPawns.FreeColonistsSpawned
				where map.reachability.CanReach(raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly)
				select x).TryRandomElement(out pawn))
				{
					result = pawn.Position;
				}
				else if (CellFinderLoose.TryGetRandomCellWith((IntVec3 x) => map.reachability.CanReach(raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly), map, 1000, out intVec2))
				{
					result = intVec2;
				}
				else
				{
					result = map.Center;
				}
			}
			return result;
		}

		// Token: 0x06003DC2 RID: 15810 RVA: 0x002098B0 File Offset: 0x00207CB0
		public static bool EnemyIsNear(Pawn p, float radius)
		{
			bool result;
			if (!p.Spawned)
			{
				result = false;
			}
			else
			{
				bool flag = p.Position.Fogged(p.Map);
				List<IAttackTarget> potentialTargetsFor = p.Map.attackTargetsCache.GetPotentialTargetsFor(p);
				for (int i = 0; i < potentialTargetsFor.Count; i++)
				{
					IAttackTarget attackTarget = potentialTargetsFor[i];
					if (!attackTarget.ThreatDisabled(p))
					{
						if (flag || !attackTarget.Thing.Position.Fogged(attackTarget.Thing.Map))
						{
							if (p.Position.InHorDistOf(((Thing)attackTarget).Position, radius))
							{
								return true;
							}
						}
					}
				}
				result = false;
			}
			return result;
		}
	}
}
