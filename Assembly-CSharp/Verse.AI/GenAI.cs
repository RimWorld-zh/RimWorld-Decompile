using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse.AI
{
	public static class GenAI
	{
		public static bool CanInteractPawn(Pawn assister, Pawn assistee)
		{
			if (assistee.Spawned && assister.CanReach((Thing)assistee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				return true;
			}
			return false;
		}

		public static bool MachinesLike(Faction machineFaction, Pawn p)
		{
			if (p.Faction == null)
			{
				return false;
			}
			if (p.IsPrisoner && p.HostFaction == machineFaction)
			{
				return false;
			}
			if (p.Faction.HostileTo(machineFaction))
			{
				return false;
			}
			return true;
		}

		public static bool CanUseItemForWork(Pawn p, Thing item)
		{
			if (item.IsForbidden(p))
			{
				return false;
			}
			if (!p.CanReserveAndReach(item, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false))
			{
				return false;
			}
			return true;
		}

		public static bool CanBeArrested(this Pawn pawn)
		{
			if (!pawn.RaceProps.Humanlike)
			{
				return false;
			}
			if (!pawn.InAggroMentalState && !pawn.HostileTo(Faction.OfPlayer))
			{
				if (pawn.IsPrisonerOfColony && pawn.Position.IsInPrisonCell(pawn.Map))
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool InDangerousCombat(Pawn pawn)
		{
			Region root = pawn.GetRegion(RegionType.Set_Passable);
			bool found = false;
			RegionTraverser.BreadthFirstTraverse(root, (RegionEntryPredicate)((Region r1, Region r2) => r2.Room == root.Room), (RegionProcessor)((Region r) => r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn).Any((Predicate<Thing>)delegate(Thing t)
			{
				Pawn pawn2 = t as Pawn;
				if (pawn2 != null && !pawn2.Downed && (float)(pawn.Position - pawn2.Position).LengthHorizontalSquared < 144.0 && pawn2.HostileTo(pawn.Faction))
				{
					bool found2 = true;
					return true;
				}
				return false;
			})), 9, RegionType.Set_Passable);
			return found;
		}

		public static IntVec3 RandomRaidDest(IntVec3 raidSpawnLoc, Map map)
		{
			List<ThingDef> allBedDefBestToWorst = RestUtility.AllBedDefBestToWorst;
			List<Building> list = new List<Building>(map.mapPawns.FreeColonistsAndPrisonersSpawnedCount);
			for (int i = 0; i < allBedDefBestToWorst.Count; i++)
			{
				foreach (Building item in map.listerBuildings.AllBuildingsColonistOfDef(allBedDefBestToWorst[i]))
				{
					if (((Building_Bed)item).owners.Any() && map.reachability.CanReach(raidSpawnLoc, (Thing)item, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly))
					{
						list.Add(item);
					}
				}
			}
			Building building = default(Building);
			if (((IEnumerable<Building>)list).TryRandomElement<Building>(out building))
			{
				return building.Position;
			}
			IEnumerable<Building> source = from b in map.listerBuildings.allBuildingsColonist
			where !b.def.building.ai_combatDangerous && !b.def.building.isInert
			select b;
			if (source.Any())
			{
				for (int j = 0; j < 500; j++)
				{
					Building t = source.RandomElement();
					IntVec3 intVec = t.RandomAdjacentCell8Way();
					if (intVec.Walkable(map) && map.reachability.CanReach(raidSpawnLoc, intVec, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly))
					{
						return intVec;
					}
				}
			}
			Pawn pawn = default(Pawn);
			if ((from x in map.mapPawns.FreeColonistsSpawned
			where map.reachability.CanReach(raidSpawnLoc, (Thing)x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly)
			select x).TryRandomElement<Pawn>(out pawn))
			{
				return pawn.Position;
			}
			IntVec3 result = default(IntVec3);
			if (CellFinderLoose.TryGetRandomCellWith((Predicate<IntVec3>)((IntVec3 x) => map.reachability.CanReach(raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly)), map, 1000, out result))
			{
				return result;
			}
			return map.Center;
		}

		public static bool EnemyIsNear(Pawn p, float radius)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<IAttackTarget> potentialTargetsFor = p.Map.attackTargetsCache.GetPotentialTargetsFor(p);
			for (int i = 0; i < potentialTargetsFor.Count; i++)
			{
				IAttackTarget attackTarget = potentialTargetsFor[i];
				if (!attackTarget.ThreatDisabled() && p.Position.InHorDistOf(((Thing)attackTarget).Position, radius))
				{
					return true;
				}
			}
			return false;
		}
	}
}
