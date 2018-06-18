using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020004A2 RID: 1186
	public static class PawnsFinder
	{
		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600152B RID: 5419 RVA: 0x000B9B00 File Offset: 0x000B7F00
		public static IEnumerable<Pawn> AllMapsWorldAndTemporary_AliveOrDead
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsWorldAndTemporary_Alive)
				{
					yield return p;
				}
				if (Find.World != null)
				{
					foreach (Pawn p2 in Find.WorldPawns.AllPawnsDead)
					{
						yield return p2;
					}
				}
				foreach (Pawn p3 in PawnsFinder.Temporary_Dead)
				{
					yield return p3;
				}
				yield break;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600152C RID: 5420 RVA: 0x000B9B24 File Offset: 0x000B7F24
		public static IEnumerable<Pawn> AllMapsWorldAndTemporary_Alive
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMaps)
				{
					yield return p;
				}
				if (Find.World != null)
				{
					foreach (Pawn p2 in Find.WorldPawns.AllPawnsAlive)
					{
						yield return p2;
					}
				}
				foreach (Pawn p3 in PawnsFinder.Temporary_Alive)
				{
					yield return p3;
				}
				yield break;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600152D RID: 5421 RVA: 0x000B9B48 File Offset: 0x000B7F48
		public static IEnumerable<Pawn> AllMaps
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						foreach (Pawn p in maps[i].mapPawns.AllPawns)
						{
							yield return p;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600152E RID: 5422 RVA: 0x000B9B6C File Offset: 0x000B7F6C
		public static IEnumerable<Pawn> AllMaps_Spawned
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						List<Pawn> spawned = maps[i].mapPawns.AllPawnsSpawned;
						for (int j = 0; j < spawned.Count; j++)
						{
							yield return spawned[j];
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600152F RID: 5423 RVA: 0x000B9B90 File Offset: 0x000B7F90
		public static IEnumerable<Pawn> All_AliveOrDead
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
				{
					yield return p;
				}
				foreach (Pawn p2 in PawnsFinder.AllCaravansAndTravelingTransportPods_AliveOrDead)
				{
					yield return p2;
				}
				yield break;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06001530 RID: 5424 RVA: 0x000B9BB4 File Offset: 0x000B7FB4
		public static IEnumerable<Pawn> Temporary
		{
			get
			{
				List<List<Pawn>> makingPawnsList = PawnGroupKindWorker.pawnsBeingGeneratedNow;
				for (int i = 0; i < makingPawnsList.Count; i++)
				{
					List<Pawn> makingPawns = makingPawnsList[i];
					for (int j = 0; j < makingPawns.Count; j++)
					{
						yield return makingPawns[j];
					}
				}
				List<List<Thing>> makingThingsList = ThingSetMaker.thingsBeingGeneratedNow;
				for (int k = 0; k < makingThingsList.Count; k++)
				{
					List<Thing> makingThings = makingThingsList[k];
					for (int l = 0; l < makingThings.Count; l++)
					{
						Pawn p = makingThings[l] as Pawn;
						if (p != null)
						{
							yield return p;
						}
					}
				}
				if (Current.ProgramState != ProgramState.Playing && Find.GameInitData != null)
				{
					List<Pawn> startingAndOptionalPawns = Find.GameInitData.startingAndOptionalPawns;
					for (int m = 0; m < startingAndOptionalPawns.Count; m++)
					{
						if (startingAndOptionalPawns[m] != null)
						{
							yield return startingAndOptionalPawns[m];
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001531 RID: 5425 RVA: 0x000B9BD8 File Offset: 0x000B7FD8
		public static IEnumerable<Pawn> Temporary_Alive
		{
			get
			{
				foreach (Pawn p in PawnsFinder.Temporary)
				{
					if (!p.Dead)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001532 RID: 5426 RVA: 0x000B9BFC File Offset: 0x000B7FFC
		public static IEnumerable<Pawn> Temporary_Dead
		{
			get
			{
				foreach (Pawn p in PawnsFinder.Temporary)
				{
					if (p.Dead)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06001533 RID: 5427 RVA: 0x000B9C20 File Offset: 0x000B8020
		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMaps)
				{
					yield return p;
				}
				foreach (Pawn p2 in PawnsFinder.AllCaravansAndTravelingTransportPods_Alive)
				{
					yield return p2;
				}
				yield break;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06001534 RID: 5428 RVA: 0x000B9C44 File Offset: 0x000B8044
		public static IEnumerable<Pawn> AllCaravansAndTravelingTransportPods_Alive
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllCaravansAndTravelingTransportPods_AliveOrDead)
				{
					if (!p.Dead)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001535 RID: 5429 RVA: 0x000B9C68 File Offset: 0x000B8068
		public static IEnumerable<Pawn> AllCaravansAndTravelingTransportPods_AliveOrDead
		{
			get
			{
				if (Find.World != null)
				{
					List<Caravan> caravans = Find.WorldObjects.Caravans;
					for (int i = 0; i < caravans.Count; i++)
					{
						List<Pawn> pawns = caravans[i].PawnsListForReading;
						for (int j = 0; j < pawns.Count; j++)
						{
							yield return pawns[j];
						}
					}
					List<TravelingTransportPods> travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
					for (int k = 0; k < travelingTransportPods.Count; k++)
					{
						foreach (Pawn p in travelingTransportPods[k].Pawns)
						{
							yield return p;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001536 RID: 5430 RVA: 0x000B9C8C File Offset: 0x000B808C
		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_Colonists
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
				{
					if (p.IsColonist)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001537 RID: 5431 RVA: 0x000B9CB0 File Offset: 0x000B80B0
		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
				{
					if (p.IsFreeColonist)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001538 RID: 5432 RVA: 0x000B9CD4 File Offset: 0x000B80D4
		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists_NoCryptosleep
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
				{
					if (p.IsFreeColonist && !p.Suspended)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001539 RID: 5433 RVA: 0x000B9CF8 File Offset: 0x000B80F8
		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction
		{
			get
			{
				Faction playerFaction = Faction.OfPlayer;
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
				{
					if (p.Faction == playerFaction)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x0600153A RID: 5434 RVA: 0x000B9D1C File Offset: 0x000B811C
		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_OfPlayerFaction_NoCryptosleep
		{
			get
			{
				Faction playerFaction = Faction.OfPlayer;
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
				{
					if (p.Faction == playerFaction && !p.Suspended)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600153B RID: 5435 RVA: 0x000B9D40 File Offset: 0x000B8140
		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
				{
					if (p.IsPrisonerOfColony)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600153C RID: 5436 RVA: 0x000B9D64 File Offset: 0x000B8164
		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Concat(PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony);
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x0600153D RID: 5437 RVA: 0x000B9D88 File Offset: 0x000B8188
		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners_NoCryptosleep
		{
			get
			{
				foreach (Pawn p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners)
				{
					if (!p.Suspended)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600153E RID: 5438 RVA: 0x000B9DAC File Offset: 0x000B81AC
		public static IEnumerable<Pawn> AllMaps_PrisonersOfColonySpawned
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						List<Pawn> prisonersOfColonySpawned = maps[i].mapPawns.PrisonersOfColonySpawned;
						for (int j = 0; j < prisonersOfColonySpawned.Count; j++)
						{
							yield return prisonersOfColonySpawned[j];
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x0600153F RID: 5439 RVA: 0x000B9DD0 File Offset: 0x000B81D0
		public static IEnumerable<Pawn> AllMaps_PrisonersOfColony
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						foreach (Pawn p in maps[i].mapPawns.PrisonersOfColony)
						{
							yield return p;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06001540 RID: 5440 RVA: 0x000B9DF4 File Offset: 0x000B81F4
		public static IEnumerable<Pawn> AllMaps_FreeColonists
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						foreach (Pawn p in maps[i].mapPawns.FreeColonists)
						{
							yield return p;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001541 RID: 5441 RVA: 0x000B9E18 File Offset: 0x000B8218
		public static IEnumerable<Pawn> AllMaps_FreeColonistsSpawned
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						foreach (Pawn p in maps[i].mapPawns.FreeColonistsSpawned)
						{
							yield return p;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001542 RID: 5442 RVA: 0x000B9E3C File Offset: 0x000B823C
		public static IEnumerable<Pawn> AllMaps_FreeColonistsAndPrisonersSpawned
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					List<Map> maps = Find.Maps;
					for (int i = 0; i < maps.Count; i++)
					{
						foreach (Pawn p in maps[i].mapPawns.FreeColonistsAndPrisonersSpawned)
						{
							yield return p;
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001543 RID: 5443 RVA: 0x000B9E60 File Offset: 0x000B8260
		public static IEnumerable<Pawn> AllMaps_FreeColonistsAndPrisoners
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					foreach (Pawn p in maps[i].mapPawns.FreeColonistsAndPrisoners)
					{
						yield return p;
					}
				}
				yield break;
			}
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x000B9E84 File Offset: 0x000B8284
		public static IEnumerable<Pawn> AllMaps_SpawnedPawnsInFaction(Faction faction)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Pawn> spawnedPawnsInFaction = maps[i].mapPawns.SpawnedPawnsInFaction(faction);
				for (int j = 0; j < spawnedPawnsInFaction.Count; j++)
				{
					yield return spawnedPawnsInFaction[j];
				}
			}
			yield break;
		}
	}
}
