using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class PawnsFinder
	{
		public static IEnumerable<Pawn> AllMapsAndWorld_AliveOrDead
		{
			get
			{
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMapsAndWorld_Alive.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Pawn alive = enumerator.Current;
						yield return alive;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				if (Find.World != null)
				{
					using (IEnumerator<Pawn> enumerator2 = Find.WorldPawns.AllPawnsDead.GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							Pawn p2 = enumerator2.Current;
							yield return p2;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				List<List<Pawn>> makingPawnsList = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				for (int l = 0; l < makingPawnsList.Count; l++)
				{
					List<Pawn> makingPawns = makingPawnsList[l];
					for (int i = 0; i < makingPawns.Count; i++)
					{
						if (makingPawns[i].Dead)
						{
							yield return makingPawns[i];
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				List<List<Thing>> makingThingsList = ItemCollectionGenerator.thingsBeingGeneratedNow;
				for (int k = 0; k < makingThingsList.Count; k++)
				{
					List<Thing> makingThings = makingThingsList[k];
					for (int j = 0; j < makingThings.Count; j++)
					{
						Pawn p = makingThings[j] as Pawn;
						if (p != null && p.Dead)
						{
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_0307:
				/*Error near IL_0308: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMapsAndWorld_Alive
		{
			get
			{
				List<List<Pawn>> makingPawnsList = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				for (int m = 0; m < makingPawnsList.Count; m++)
				{
					List<Pawn> makingPawns = makingPawnsList[m];
					for (int i = 0; i < makingPawns.Count; i++)
					{
						if (!makingPawns[i].Dead)
						{
							yield return makingPawns[i];
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				List<List<Thing>> makingThingsList = ItemCollectionGenerator.thingsBeingGeneratedNow;
				for (int l = 0; l < makingThingsList.Count; l++)
				{
					List<Thing> makingThings = makingThingsList[l];
					for (int j = 0; j < makingThings.Count; j++)
					{
						Pawn p = makingThings[j] as Pawn;
						if (p != null && !p.Dead)
						{
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				if (Find.World != null)
				{
					using (IEnumerator<Pawn> enumerator = Find.WorldPawns.AllPawnsAlive.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Pawn p3 = enumerator.Current;
							yield return p3;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
					using (IEnumerator<Pawn> enumerator2 = PawnsFinder.AllMaps.GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							Pawn p2 = enumerator2.Current;
							yield return p2;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				if (Current.ProgramState == ProgramState.Playing)
					yield break;
				if (Find.GameInitData == null)
					yield break;
				if (Find.GameInitData == null)
					yield break;
				List<Pawn> startingPawns = Find.GameInitData.startingPawns;
				int k = 0;
				while (true)
				{
					if (k < startingPawns.Count)
					{
						if (startingPawns[k] == null)
						{
							k++;
							continue;
						}
						break;
					}
					yield break;
				}
				yield return startingPawns[k];
				/*Error: Unable to find new state assignment for yield return*/;
				IL_03b1:
				/*Error near IL_03b2: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMaps
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					using (IEnumerator<Pawn> enumerator = maps[i].mapPawns.AllPawns.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Pawn p = enumerator.Current;
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_010a:
				/*Error near IL_010b: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMaps_Spawned
		{
			get
			{
				List<Map> maps = Find.Maps;
				int j = 0;
				List<Pawn> spawned;
				int i;
				while (true)
				{
					if (j < maps.Count)
					{
						spawned = maps[j].mapPawns.AllPawnsSpawned;
						i = 0;
						if (i < spawned.Count)
							break;
						j++;
						continue;
					}
					yield break;
				}
				yield return spawned[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods
		{
			get
			{
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMaps.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Pawn p2 = enumerator.Current;
						yield return p2;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				if (Find.World != null)
				{
					List<Caravan> caravans = Find.WorldObjects.Caravans;
					for (int k = 0; k < caravans.Count; k++)
					{
						List<Pawn> pawns = caravans[k].PawnsListForReading;
						int i = 0;
						if (i < pawns.Count)
						{
							yield return pawns[i];
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
					List<TravelingTransportPods> travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
					for (int j = 0; j < travelingTransportPods.Count; j++)
					{
						using (IEnumerator<Pawn> enumerator2 = travelingTransportPods[j].Pawns.GetEnumerator())
						{
							if (enumerator2.MoveNext())
							{
								Pawn p = enumerator2.Current;
								yield return p;
								/*Error: Unable to find new state assignment for yield return*/;
							}
						}
					}
				}
				yield break;
				IL_0263:
				/*Error near IL_0264: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Colonists
		{
			get
			{
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods.GetEnumerator())
				{
					Pawn p;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							p = enumerator.Current;
							if (p.IsColonist)
								break;
							continue;
						}
						yield break;
					}
					yield return p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_00c7:
				/*Error near IL_00c8: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_FreeColonists
		{
			get
			{
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods.GetEnumerator())
				{
					Pawn p;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							p = enumerator.Current;
							if (p.IsFreeColonist)
								break;
							continue;
						}
						yield break;
					}
					yield return p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_00c7:
				/*Error near IL_00c8: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_PrisonersOfColony
		{
			get
			{
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods.GetEnumerator())
				{
					Pawn p;
					while (true)
					{
						if (enumerator.MoveNext())
						{
							p = enumerator.Current;
							if (p.IsPrisonerOfColony)
								break;
							continue;
						}
						yield break;
					}
					yield return p;
					/*Error: Unable to find new state assignment for yield return*/;
				}
				IL_00c7:
				/*Error near IL_00c8: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_FreeColonistsAndPrisoners
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_FreeColonists.Concat(PawnsFinder.AllMapsCaravansAndTravelingTransportPods_PrisonersOfColony);
			}
		}

		public static IEnumerable<Pawn> AllMaps_PrisonersOfColonySpawned
		{
			get
			{
				List<Map> maps = Find.Maps;
				int j = 0;
				List<Pawn> prisonersOfColonySpawned;
				int i;
				while (true)
				{
					if (j < maps.Count)
					{
						prisonersOfColonySpawned = maps[j].mapPawns.PrisonersOfColonySpawned;
						i = 0;
						if (i < prisonersOfColonySpawned.Count)
							break;
						j++;
						continue;
					}
					yield break;
				}
				yield return prisonersOfColonySpawned[i];
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public static IEnumerable<Pawn> AllMaps_PrisonersOfColony
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					using (IEnumerator<Pawn> enumerator = maps[i].mapPawns.PrisonersOfColony.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Pawn p = enumerator.Current;
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_010a:
				/*Error near IL_010b: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonists
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					using (IEnumerator<Pawn> enumerator = maps[i].mapPawns.FreeColonists.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Pawn p = enumerator.Current;
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_010a:
				/*Error near IL_010b: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonistsSpawned
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					using (IEnumerator<Pawn> enumerator = maps[i].mapPawns.FreeColonistsSpawned.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Pawn p = enumerator.Current;
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_010a:
				/*Error near IL_010b: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonistsAndPrisonersSpawned
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					using (IEnumerator<Pawn> enumerator = maps[i].mapPawns.FreeColonistsAndPrisonersSpawned.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Pawn p = enumerator.Current;
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_010a:
				/*Error near IL_010b: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMaps_FreeColonistsAndPrisoners
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					using (IEnumerator<Pawn> enumerator = maps[i].mapPawns.FreeColonistsAndPrisoners.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							Pawn p = enumerator.Current;
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				yield break;
				IL_010a:
				/*Error near IL_010b: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMaps_SpawnedPawnsInFaction(Faction faction)
		{
			List<Map> maps = Find.Maps;
			int j = 0;
			List<Pawn> spawnedPawnsInFaction;
			int i;
			while (true)
			{
				if (j < maps.Count)
				{
					spawnedPawnsInFaction = maps[j].mapPawns.SpawnedPawnsInFaction(faction);
					i = 0;
					if (i < spawnedPawnsInFaction.Count)
						break;
					j++;
					continue;
				}
				yield break;
			}
			yield return spawnedPawnsInFaction[i];
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
