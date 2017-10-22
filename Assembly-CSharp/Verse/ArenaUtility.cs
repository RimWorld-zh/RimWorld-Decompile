using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using Verse.AI.Group;

namespace Verse
{
	public static class ArenaUtility
	{
		public struct ArenaResult
		{
			public enum Winner
			{
				Other = 0,
				Lhs = 1,
				Rhs = 2
			}

			public Winner winner;

			public int tickDuration;
		}

		private class ArenaSetState
		{
			public int live = 0;
		}

		private const int liveSimultaneous = 15;

		public static void BeginArenaFight(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaResult> callback)
		{
			MapParent mapParent = (MapParent)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.Debug_Arena);
			mapParent.Tile = TileFinder.RandomFactionBaseTileFor(Faction.OfPlayer, true, (Predicate<int>)((int tile) => lhs.Concat(rhs).Any((Func<PawnKindDef, bool>)((PawnKindDef pawnkind) => Find.World.tileTemperatures.SeasonAndOutdoorTemperatureAcceptableFor(tile, pawnkind.race)))));
			mapParent.SetFaction(Faction.OfPlayer);
			Find.WorldObjects.Add(mapParent);
			Map orGenerateMap = GetOrGenerateMapUtility.GetOrGenerateMap(mapParent.Tile, new IntVec3(50, 1, 50), null);
			IntVec3 spot = default(IntVec3);
			IntVec3 spot2 = default(IntVec3);
			MultipleCaravansCellFinder.FindStartingCellsFor2Groups(orGenerateMap, out spot, out spot2);
			List<Pawn> lhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, lhs, spot, Faction.OfSpacer);
			List<Pawn> rhs2 = ArenaUtility.SpawnPawnSet(orGenerateMap, rhs, spot2, Faction.OfSpacerHostile);
			DebugArena component = ((WorldObject)mapParent).GetComponent<DebugArena>();
			component.lhs = lhs2;
			component.rhs = rhs2;
			component.callback = callback;
		}

		public static List<Pawn> SpawnPawnSet(Map map, List<PawnKindDef> kinds, IntVec3 spot, Faction faction)
		{
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i < kinds.Count; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(kinds[i], faction);
				IntVec3 loc = CellFinder.RandomClosewalkCellNear(spot, map, 12, null);
				GenSpawn.Spawn(pawn, loc, map, Rot4.Random, false);
				list.Add(pawn);
			}
			LordMaker.MakeNewLord(faction, new LordJob_DefendPoint(map.Center), map, list);
			return list;
		}

		private static bool ArenaFightQueue(List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaResult> callback, ArenaSetState state)
		{
			bool result2;
			if (state.live < 15)
			{
				ArenaUtility.BeginArenaFight(lhs, rhs, (Action<ArenaResult>)delegate(ArenaResult result)
				{
					state.live--;
					callback(result);
				});
				state.live++;
				result2 = true;
			}
			else
			{
				result2 = false;
			}
			return result2;
		}

		public static void BeginArenaFightSet(int count, List<PawnKindDef> lhs, List<PawnKindDef> rhs, Action<ArenaResult> callback, Action report)
		{
			ArenaSetState state = new ArenaSetState();
			for (int num = 0; num < count; num++)
			{
				Current.Game.GetComponent<GameComponent_DebugTools>().AddPerFrameCallback((Func<bool>)(() => ArenaUtility.ArenaFightQueue(lhs, rhs, (Action<ArenaResult>)delegate(ArenaResult result)
				{
					callback(result);
					count--;
					if (count % 10 == 0)
					{
						report();
					}
				}, state)));
			}
		}
	}
}
