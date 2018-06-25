using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using RimWorld;

namespace Verse.AI
{
	public static class GenAI
	{
		[CompilerGenerated]
		private static Func<Building, bool> <>f__am$cache0;

		public static bool MachinesLike(Faction machineFaction, Pawn p)
		{
			return (p.Faction != null || !p.NonHumanlikeOrWildMan() || (p.HostFaction == machineFaction && !p.IsPrisoner)) && (!p.IsPrisoner || p.HostFaction != machineFaction) && (p.Faction == null || !p.Faction.HostileTo(machineFaction));
		}

		public static bool CanUseItemForWork(Pawn p, Thing item)
		{
			return !item.IsForbidden(p) && p.CanReserveAndReach(item, PathEndMode.ClosestTouch, p.NormalMaxDanger(), 1, -1, null, false);
		}

		public static bool CanBeArrestedBy(this Pawn pawn, Pawn arrester)
		{
			return pawn.RaceProps.Humanlike && (!pawn.InAggroMentalState || !pawn.HostileTo(arrester)) && !pawn.HostileTo(Faction.OfPlayer) && (!pawn.IsPrisonerOfColony || !pawn.Position.IsInPrisonCell(pawn.Map));
		}

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

		[CompilerGenerated]
		private static bool <RandomRaidDest>m__0(Building b)
		{
			return !b.def.building.ai_combatDangerous && !b.def.building.isInert;
		}

		[CompilerGenerated]
		private sealed class <InDangerousCombat>c__AnonStorey0
		{
			internal Region root;

			internal Pawn pawn;

			internal bool found;

			public <InDangerousCombat>c__AnonStorey0()
			{
			}

			internal bool <>m__0(Region r1, Region r2)
			{
				return r2.Room == this.root.Room;
			}

			internal bool <>m__1(Region r)
			{
				return r.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn).Any(delegate(Thing t)
				{
					Pawn pawn = t as Pawn;
					bool result;
					if (pawn != null && !pawn.Downed && (float)(this.pawn.Position - pawn.Position).LengthHorizontalSquared < 144f && pawn.HostileTo(this.pawn.Faction))
					{
						this.found = true;
						result = true;
					}
					else
					{
						result = false;
					}
					return result;
				});
			}

			internal bool <>m__2(Thing t)
			{
				Pawn pawn = t as Pawn;
				bool result;
				if (pawn != null && !pawn.Downed && (float)(this.pawn.Position - pawn.Position).LengthHorizontalSquared < 144f && pawn.HostileTo(this.pawn.Faction))
				{
					this.found = true;
					result = true;
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		[CompilerGenerated]
		private sealed class <RandomRaidDest>c__AnonStorey1
		{
			internal Map map;

			internal IntVec3 raidSpawnLoc;

			public <RandomRaidDest>c__AnonStorey1()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return this.map.reachability.CanReach(this.raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly);
			}

			internal bool <>m__1(IntVec3 x)
			{
				return this.map.reachability.CanReach(this.raidSpawnLoc, x, PathEndMode.OnCell, TraverseMode.PassAllDestroyableThings, Danger.Deadly);
			}
		}
	}
}
