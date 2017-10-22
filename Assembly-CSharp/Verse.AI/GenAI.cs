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
			return (byte)((assistee.Spawned && assister.CanReach((Thing)assistee, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn)) ? 1 : 0) != 0;
		}

		public static bool MachinesLike(Faction machineFaction, Pawn p)
		{
			return (byte)((p.Faction != null) ? ((!p.IsPrisoner || p.HostFaction != machineFaction) ? ((!p.Faction.HostileTo(machineFaction)) ? 1 : 0) : 0) : 0) != 0;
		}

		public static bool CanUseItemForWork(Pawn p, Thing item)
		{
			return (byte)((!item.IsForbidden(p)) ? (p.CanReserveAndReach(item, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false) ? 1 : 0) : 0) != 0;
		}

		public static bool CanBeArrestedBy(this Pawn pawn, Pawn arrester)
		{
			bool result;
			if (pawn.NonHumanlikeOrWildMan())
			{
				result = false;
			}
			else
			{
				if (pawn.InAggroMentalState && pawn.HostileTo(arrester))
				{
					goto IL_003a;
				}
				if (pawn.HostileTo(Faction.OfPlayer))
					goto IL_003a;
				result = ((byte)((!pawn.IsPrisonerOfColony || !pawn.Position.IsInPrisonCell(pawn.Map)) ? 1 : 0) != 0);
			}
			goto IL_0070;
			IL_003a:
			result = false;
			goto IL_0070;
			IL_0070:
			return result;
		}

		public static bool InDangerousCombat(Pawn pawn)
		{
			Region root = pawn.GetRegion(RegionType.Set_Passable);
			bool found = false;
			RegionTraverser.BreadthFirstTraverse(root, (RegionEntryPredicate)((Region r1, Region r2) => r2.Room == root.Room), (RegionProcessor)((Region r) => r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn).Any((Predicate<Thing>)delegate(Thing t)
			{
				Pawn pawn2 = t as Pawn;
				bool result;
				if (pawn2 != null && !pawn2.Downed && (float)(pawn.Position - pawn2.Position).LengthHorizontalSquared < 144.0 && pawn2.HostileTo(pawn.Faction))
				{
					bool found2 = true;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
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
			IntVec3 result;
			IntVec3 intVec;
			if (((IEnumerable<Building>)list).TryRandomElement<Building>(out building))
			{
				result = building.Position;
			}
			else
			{
				IEnumerable<Building> source = from b in map.listerBuildings.allBuildingsColonist
				where !b.def.building.ai_combatDangerous && !b.def.building.isInert
				select b;
				if (source.Any())
				{
					for (int j = 0; j < 500; j++)
					{
						Building t = source.RandomElement();
						intVec = t.RandomAdjacentCell8Way();
						if (intVec.Walkable(map) && map.reachability.CanReach(raidSpawnLoc, intVec, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly))
							goto IL_018c;
					}
				}
				Pawn pawn = default(Pawn);
				IntVec3 intVec2 = default(IntVec3);
				result = ((!(from x in map.mapPawns.FreeColonistsSpawned
				where map.reachability.CanReach(raidSpawnLoc, (Thing)x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly)
				select x).TryRandomElement<Pawn>(out pawn)) ? ((!CellFinderLoose.TryGetRandomCellWith((Predicate<IntVec3>)((IntVec3 x) => map.reachability.CanReach(raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly)), map, 1000, out intVec2)) ? map.Center : intVec2) : pawn.Position);
			}
			goto IL_0225;
			IL_0225:
			return result;
			IL_018c:
			result = intVec;
			goto IL_0225;
		}

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
					if (!attackTarget.ThreatDisabled() && (flag || !attackTarget.Thing.Position.Fogged(attackTarget.Thing.Map)) && p.Position.InHorDistOf(((Thing)attackTarget).Position, radius))
						goto IL_00a7;
				}
				result = false;
			}
			goto IL_00c6;
			IL_00c6:
			return result;
			IL_00a7:
			result = true;
			goto IL_00c6;
		}
	}
}
