using RimWorld.Planet;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public static class PawnsFinder
	{
		public static IEnumerable<Pawn> AllMapsAndWorld_AliveOrDead
		{
			get
			{
				foreach (Pawn item in PawnsFinder.AllMapsAndWorld_Alive)
				{
					yield return item;
				}
				if (Find.World != null)
				{
					foreach (Pawn item2 in Find.WorldPawns.AllPawnsDead)
					{
						yield return item2;
					}
				}
				List<Pawn> makingPawns = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				if (makingPawns != null)
				{
					for (int j = 0; j < makingPawns.Count; j++)
					{
						if (makingPawns[j].Dead)
						{
							yield return makingPawns[j];
						}
					}
				}
				List<Thing> makingThings = ItemCollectionGenerator.thingsBeingGeneratedNow;
				if (makingThings != null)
				{
					for (int i = 0; i < makingThings.Count; i++)
					{
						Pawn p = makingThings[i] as Pawn;
						if (p != null && p.Dead)
						{
							yield return p;
						}
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMapsAndWorld_Alive
		{
			get
			{
				List<Pawn> makingPawns = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				if (makingPawns != null)
				{
					for (int k = 0; k < makingPawns.Count; k++)
					{
						if (!makingPawns[k].Dead)
						{
							yield return makingPawns[k];
						}
					}
				}
				List<Thing> makingThings = ItemCollectionGenerator.thingsBeingGeneratedNow;
				if (makingThings != null)
				{
					for (int j = 0; j < makingThings.Count; j++)
					{
						Pawn p3 = makingThings[j] as Pawn;
						if (p3 != null && !p3.Dead)
						{
							yield return p3;
						}
					}
				}
				if (Find.World != null)
				{
					foreach (Pawn item in Find.WorldPawns.AllPawnsAlive)
					{
						yield return item;
					}
					foreach (Pawn allMap in PawnsFinder.AllMaps)
					{
						yield return allMap;
					}
				}
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null && Find.GameInitData != null)
				{
					List<Pawn> startingPawns = Find.GameInitData.startingPawns;
					for (int i = 0; i < startingPawns.Count; i++)
					{
						if (startingPawns[i] != null)
						{
							yield return startingPawns[i];
						}
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMaps
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn allPawn in maps[i].mapPawns.AllPawns)
					{
						yield return allPawn;
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMaps_Spawned
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int j = 0; j < maps.Count; j++)
				{
					List<Pawn> spawned = maps[j].mapPawns.AllPawnsSpawned;
					for (int i = 0; i < spawned.Count; i++)
					{
						yield return spawned[i];
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods
		{
			get
			{
				foreach (Pawn allMap in PawnsFinder.AllMaps)
				{
					yield return allMap;
				}
				if (Find.World != null)
				{
					List<Caravan> caravans = Find.WorldObjects.Caravans;
					for (int k = 0; k < caravans.Count; k++)
					{
						List<Pawn> pawns = caravans[k].PawnsListForReading;
						for (int i = 0; i < pawns.Count; i++)
						{
							yield return pawns[i];
						}
					}
					List<TravelingTransportPods> travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
					for (int j = 0; j < travelingTransportPods.Count; j++)
					{
						foreach (Pawn pawn in travelingTransportPods[j].Pawns)
						{
							yield return pawn;
						}
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Colonists
		{
			get
			{
				foreach (Pawn allMapsCaravansAndTravelingTransportPod in PawnsFinder.AllMapsCaravansAndTravelingTransportPods)
				{
					if (allMapsCaravansAndTravelingTransportPod.IsColonist)
					{
						yield return allMapsCaravansAndTravelingTransportPod;
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_FreeColonists
		{
			get
			{
				foreach (Pawn allMapsCaravansAndTravelingTransportPod in PawnsFinder.AllMapsCaravansAndTravelingTransportPods)
				{
					if (allMapsCaravansAndTravelingTransportPod.IsColonist && allMapsCaravansAndTravelingTransportPod.HostFaction == null)
					{
						yield return allMapsCaravansAndTravelingTransportPod;
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_PrisonersOfColony
		{
			get
			{
				foreach (Pawn allMapsCaravansAndTravelingTransportPod in PawnsFinder.AllMapsCaravansAndTravelingTransportPods)
				{
					if (allMapsCaravansAndTravelingTransportPod.IsPrisonerOfColony)
					{
						yield return allMapsCaravansAndTravelingTransportPod;
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMaps_PrisonersOfColonySpawned
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int j = 0; j < maps.Count; j++)
				{
					List<Pawn> prisonersOfColonySpawned = maps[j].mapPawns.PrisonersOfColonySpawned;
					for (int i = 0; i < prisonersOfColonySpawned.Count; i++)
					{
						yield return prisonersOfColonySpawned[i];
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMaps_PrisonersOfColony
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn item in maps[i].mapPawns.PrisonersOfColony)
					{
						yield return item;
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonists
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn freeColonist in maps[i].mapPawns.FreeColonists)
					{
						yield return freeColonist;
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonistsSpawned
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn item in maps[i].mapPawns.FreeColonistsSpawned)
					{
						yield return item;
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonistsAndPrisonersSpawned
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn item in maps[i].mapPawns.FreeColonistsAndPrisonersSpawned)
					{
						yield return item;
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonistsAndPrisoners
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn freeColonistsAndPrisoner in maps[i].mapPawns.FreeColonistsAndPrisoners)
					{
						yield return freeColonistsAndPrisoner;
					}
				}
			}
		}

		public static IEnumerable<Pawn> AllMaps_SpawnedPawnsInFaction(Faction faction)
		{
			List<Map> maps = Find.Maps;
			for (int j = 0; j < maps.Count; j++)
			{
				List<Pawn> spawnedPawnsInFaction = maps[j].mapPawns.SpawnedPawnsInFaction(faction);
				for (int i = 0; i < spawnedPawnsInFaction.Count; i++)
				{
					yield return spawnedPawnsInFaction[i];
				}
			}
		}
	}
}
