using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	public static class PawnsFinder
	{
		public static IEnumerable<Pawn> AllMapsWorldAndTemporary_AliveOrDead
		{
			get
			{
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMapsWorldAndTemporary_Alive.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Pawn p3 = enumerator.Current;
						yield return p3;
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
				using (IEnumerator<Pawn> enumerator3 = PawnsFinder.Temporary_Dead.GetEnumerator())
				{
					if (enumerator3.MoveNext())
					{
						Pawn p = enumerator3.Current;
						yield return p;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_01d8:
				/*Error near IL_01d9: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMapsWorldAndTemporary_Alive
		{
			get
			{
				using (IEnumerator<Pawn> enumerator = PawnsFinder.AllMaps.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Pawn p3 = enumerator.Current;
						yield return p3;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				if (Find.World != null)
				{
					using (IEnumerator<Pawn> enumerator2 = Find.WorldPawns.AllPawnsAlive.GetEnumerator())
					{
						if (enumerator2.MoveNext())
						{
							Pawn p2 = enumerator2.Current;
							yield return p2;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				using (IEnumerator<Pawn> enumerator3 = PawnsFinder.Temporary_Alive.GetEnumerator())
				{
					if (enumerator3.MoveNext())
					{
						Pawn p = enumerator3.Current;
						yield return p;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_01d8:
				/*Error near IL_01d9: Unexpected return in MoveNext()*/;
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
				IL_0104:
				/*Error near IL_0105: Unexpected return in MoveNext()*/;
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

		public static IEnumerable<Pawn> Temporary
		{
			get
			{
				List<List<Pawn>> makingPawnsList = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				for (int m = 0; m < makingPawnsList.Count; m++)
				{
					List<Pawn> makingPawns = makingPawnsList[m];
					int i = 0;
					if (i < makingPawns.Count)
					{
						yield return makingPawns[i];
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				List<List<Thing>> makingThingsList = ItemCollectionGenerator.thingsBeingGeneratedNow;
				for (int l = 0; l < makingThingsList.Count; l++)
				{
					List<Thing> makingThings = makingThingsList[l];
					for (int j = 0; j < makingThings.Count; j++)
					{
						Pawn p = makingThings[j] as Pawn;
						if (p != null)
						{
							yield return p;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				if (Current.ProgramState == ProgramState.Playing)
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
			}
		}

		public static IEnumerable<Pawn> Temporary_Alive
		{
			get
			{
				foreach (Pawn item in PawnsFinder.Temporary)
				{
					if (!item.Dead)
					{
						yield return item;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00c3:
				/*Error near IL_00c4: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> Temporary_Dead
		{
			get
			{
				foreach (Pawn item in PawnsFinder.Temporary)
				{
					if (item.Dead)
					{
						yield return item;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00c3:
				/*Error near IL_00c4: Unexpected return in MoveNext()*/;
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
				IL_0254:
				/*Error near IL_0255: Unexpected return in MoveNext()*/;
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
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00c3:
				/*Error near IL_00c4: Unexpected return in MoveNext()*/;
			}
		}

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_FreeColonists
		{
			get
			{
				foreach (Pawn allMapsCaravansAndTravelingTransportPod in PawnsFinder.AllMapsCaravansAndTravelingTransportPods)
				{
					if (allMapsCaravansAndTravelingTransportPod.IsFreeColonist)
					{
						yield return allMapsCaravansAndTravelingTransportPod;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00c3:
				/*Error near IL_00c4: Unexpected return in MoveNext()*/;
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
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_00c3:
				/*Error near IL_00c4: Unexpected return in MoveNext()*/;
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
				IL_0104:
				/*Error near IL_0105: Unexpected return in MoveNext()*/;
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
				IL_0104:
				/*Error near IL_0105: Unexpected return in MoveNext()*/;
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
				IL_0104:
				/*Error near IL_0105: Unexpected return in MoveNext()*/;
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
				IL_0104:
				/*Error near IL_0105: Unexpected return in MoveNext()*/;
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
				IL_0104:
				/*Error near IL_0105: Unexpected return in MoveNext()*/;
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
