using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;

namespace Verse
{
	// Token: 0x02000C34 RID: 3124
	public sealed class MapPawns
	{
		// Token: 0x06004491 RID: 17553 RVA: 0x00240228 File Offset: 0x0023E628
		public MapPawns(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000AB9 RID: 2745
		// (get) Token: 0x06004492 RID: 17554 RVA: 0x0024027C File Offset: 0x0023E67C
		public IEnumerable<Pawn> AllPawns
		{
			get
			{
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					yield return this.pawnsSpawned[i];
				}
				foreach (Pawn p in this.AllPawnsUnspawned)
				{
					yield return p;
				}
				yield break;
			}
		}

		// Token: 0x17000ABA RID: 2746
		// (get) Token: 0x06004493 RID: 17555 RVA: 0x002402A8 File Offset: 0x0023E6A8
		public IEnumerable<Pawn> AllPawnsUnspawned
		{
			get
			{
				this.tmpUnspawnedPawns.Clear();
				ThingOwnerUtility.GetAllThingsRecursively<Pawn>(this.map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), this.tmpUnspawnedPawns, true, null, false);
				for (int j = this.tmpUnspawnedPawns.Count - 1; j >= 0; j--)
				{
					if (this.tmpUnspawnedPawns[j].Dead)
					{
						this.tmpUnspawnedPawns.RemoveAt(j);
					}
				}
				for (int i = this.tmpUnspawnedPawns.Count - 1; i >= 0; i--)
				{
					yield return this.tmpUnspawnedPawns[i];
				}
				yield break;
			}
		}

		// Token: 0x17000ABB RID: 2747
		// (get) Token: 0x06004494 RID: 17556 RVA: 0x002402D4 File Offset: 0x0023E6D4
		public IEnumerable<Pawn> FreeColonists
		{
			get
			{
				return this.FreeHumanlikesOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000ABC RID: 2748
		// (get) Token: 0x06004495 RID: 17557 RVA: 0x002402F4 File Offset: 0x0023E6F4
		public IEnumerable<Pawn> PrisonersOfColony
		{
			get
			{
				return from x in this.AllPawns
				where x.IsPrisonerOfColony
				select x;
			}
		}

		// Token: 0x17000ABD RID: 2749
		// (get) Token: 0x06004496 RID: 17558 RVA: 0x00240334 File Offset: 0x0023E734
		public IEnumerable<Pawn> FreeColonistsAndPrisoners
		{
			get
			{
				return this.FreeColonists.Concat(this.PrisonersOfColony);
			}
		}

		// Token: 0x17000ABE RID: 2750
		// (get) Token: 0x06004497 RID: 17559 RVA: 0x0024035C File Offset: 0x0023E75C
		public int ColonistCount
		{
			get
			{
				int result;
				if (Current.ProgramState != ProgramState.Playing)
				{
					Log.Error("ColonistCount while not playing. This should get the starting player pawn count.", false);
					result = 3;
				}
				else
				{
					result = this.AllPawns.Count((Pawn x) => x.IsColonist);
				}
				return result;
			}
		}

		// Token: 0x17000ABF RID: 2751
		// (get) Token: 0x06004498 RID: 17560 RVA: 0x002403B8 File Offset: 0x0023E7B8
		public int AllPawnsCount
		{
			get
			{
				return this.AllPawns.Count<Pawn>();
			}
		}

		// Token: 0x17000AC0 RID: 2752
		// (get) Token: 0x06004499 RID: 17561 RVA: 0x002403D8 File Offset: 0x0023E7D8
		public int AllPawnsUnspawnedCount
		{
			get
			{
				return this.AllPawnsUnspawned.Count<Pawn>();
			}
		}

		// Token: 0x17000AC1 RID: 2753
		// (get) Token: 0x0600449A RID: 17562 RVA: 0x002403F8 File Offset: 0x0023E7F8
		public int FreeColonistsCount
		{
			get
			{
				return this.FreeColonists.Count<Pawn>();
			}
		}

		// Token: 0x17000AC2 RID: 2754
		// (get) Token: 0x0600449B RID: 17563 RVA: 0x00240418 File Offset: 0x0023E818
		public int PrisonersOfColonyCount
		{
			get
			{
				return this.PrisonersOfColony.Count<Pawn>();
			}
		}

		// Token: 0x17000AC3 RID: 2755
		// (get) Token: 0x0600449C RID: 17564 RVA: 0x00240438 File Offset: 0x0023E838
		public int FreeColonistsAndPrisonersCount
		{
			get
			{
				return this.PrisonersOfColony.Count<Pawn>();
			}
		}

		// Token: 0x17000AC4 RID: 2756
		// (get) Token: 0x0600449D RID: 17565 RVA: 0x00240458 File Offset: 0x0023E858
		public bool AnyPawnBlockingMapRemoval
		{
			get
			{
				Faction ofPlayer = Faction.OfPlayer;
				int i = 0;
				while (i < this.pawnsSpawned.Count)
				{
					bool result;
					if (!this.pawnsSpawned[i].Downed && this.pawnsSpawned[i].IsColonist)
					{
						result = true;
					}
					else if (this.pawnsSpawned[i].relations != null && this.pawnsSpawned[i].relations.relativeInvolvedInRescueQuest != null)
					{
						result = true;
					}
					else
					{
						if (this.pawnsSpawned[i].Faction == ofPlayer || this.pawnsSpawned[i].HostFaction == ofPlayer)
						{
							Job curJob = this.pawnsSpawned[i].CurJob;
							if (curJob != null && curJob.exitMapOnArrival)
							{
								return true;
							}
						}
						if (CaravanExitMapUtility.FindCaravanToJoinFor(this.pawnsSpawned[i]) == null || this.pawnsSpawned[i].Downed)
						{
							i++;
							continue;
						}
						result = true;
					}
					return result;
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j] is IActiveDropPod || list[j].TryGetComp<CompTransporter>() != null)
					{
						IThingHolder holder = list[j].TryGetComp<CompTransporter>() ?? ((IThingHolder)list[j]);
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(holder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && !pawn.Downed && pawn.IsColonist)
							{
								this.tmpThings.Clear();
								return true;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return false;
			}
		}

		// Token: 0x17000AC5 RID: 2757
		// (get) Token: 0x0600449E RID: 17566 RVA: 0x002406A4 File Offset: 0x0023EAA4
		public List<Pawn> AllPawnsSpawned
		{
			get
			{
				return this.pawnsSpawned;
			}
		}

		// Token: 0x17000AC6 RID: 2758
		// (get) Token: 0x0600449F RID: 17567 RVA: 0x002406C0 File Offset: 0x0023EAC0
		public IEnumerable<Pawn> FreeColonistsSpawned
		{
			get
			{
				return this.FreeHumanlikesSpawnedOfFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x17000AC7 RID: 2759
		// (get) Token: 0x060044A0 RID: 17568 RVA: 0x002406E0 File Offset: 0x0023EAE0
		public List<Pawn> PrisonersOfColonySpawned
		{
			get
			{
				return this.prisonersOfColonySpawned;
			}
		}

		// Token: 0x17000AC8 RID: 2760
		// (get) Token: 0x060044A1 RID: 17569 RVA: 0x002406FC File Offset: 0x0023EAFC
		public IEnumerable<Pawn> FreeColonistsAndPrisonersSpawned
		{
			get
			{
				return this.FreeColonistsSpawned.Concat(this.PrisonersOfColonySpawned);
			}
		}

		// Token: 0x17000AC9 RID: 2761
		// (get) Token: 0x060044A2 RID: 17570 RVA: 0x00240724 File Offset: 0x0023EB24
		public int AllPawnsSpawnedCount
		{
			get
			{
				return this.pawnsSpawned.Count;
			}
		}

		// Token: 0x17000ACA RID: 2762
		// (get) Token: 0x060044A3 RID: 17571 RVA: 0x00240744 File Offset: 0x0023EB44
		public int FreeColonistsSpawnedCount
		{
			get
			{
				return this.FreeColonistsSpawned.Count<Pawn>();
			}
		}

		// Token: 0x17000ACB RID: 2763
		// (get) Token: 0x060044A4 RID: 17572 RVA: 0x00240764 File Offset: 0x0023EB64
		public int PrisonersOfColonySpawnedCount
		{
			get
			{
				return this.PrisonersOfColonySpawned.Count;
			}
		}

		// Token: 0x17000ACC RID: 2764
		// (get) Token: 0x060044A5 RID: 17573 RVA: 0x00240784 File Offset: 0x0023EB84
		public int FreeColonistsAndPrisonersSpawnedCount
		{
			get
			{
				return this.FreeColonistsAndPrisonersSpawned.Count<Pawn>();
			}
		}

		// Token: 0x17000ACD RID: 2765
		// (get) Token: 0x060044A6 RID: 17574 RVA: 0x002407A4 File Offset: 0x0023EBA4
		public int ColonistsSpawnedCount
		{
			get
			{
				int num = 0;
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						num++;
					}
				}
				return num;
			}
		}

		// Token: 0x17000ACE RID: 2766
		// (get) Token: 0x060044A7 RID: 17575 RVA: 0x002407FC File Offset: 0x0023EBFC
		public int FreeColonistsSpawnedOrInPlayerEjectablePodsCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.pawnsSpawned.Count; i++)
				{
					if (this.pawnsSpawned[i].IsFreeColonist)
					{
						num++;
					}
				}
				List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int j = 0; j < list.Count; j++)
				{
					Building_CryptosleepCasket building_CryptosleepCasket = list[j] as Building_CryptosleepCasket;
					if ((building_CryptosleepCasket != null && building_CryptosleepCasket.def.building.isPlayerEjectable) || list[j] is IActiveDropPod || list[j].TryGetComp<CompTransporter>() != null)
					{
						IThingHolder holder = list[j].TryGetComp<CompTransporter>() ?? ((IThingHolder)list[j]);
						this.tmpThings.Clear();
						ThingOwnerUtility.GetAllThingsRecursively(holder, this.tmpThings, true, null);
						for (int k = 0; k < this.tmpThings.Count; k++)
						{
							Pawn pawn = this.tmpThings[k] as Pawn;
							if (pawn != null && !pawn.Dead && pawn.IsFreeColonist)
							{
								num++;
							}
						}
					}
				}
				this.tmpThings.Clear();
				return num;
			}
		}

		// Token: 0x17000ACF RID: 2767
		// (get) Token: 0x060044A8 RID: 17576 RVA: 0x0024096C File Offset: 0x0023ED6C
		public bool AnyColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17000AD0 RID: 2768
		// (get) Token: 0x060044A9 RID: 17577 RVA: 0x002409C0 File Offset: 0x0023EDC0
		public bool AnyFreeColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsFreeColonist)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x060044AA RID: 17578 RVA: 0x00240A14 File Offset: 0x0023EE14
		private void EnsureFactionsListsInit()
		{
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (!this.pawnsInFactionSpawned.ContainsKey(allFactionsListForReading[i]))
				{
					this.pawnsInFactionSpawned.Add(allFactionsListForReading[i], new List<Pawn>());
				}
			}
		}

		// Token: 0x060044AB RID: 17579 RVA: 0x00240A74 File Offset: 0x0023EE74
		public IEnumerable<Pawn> PawnsInFaction(Faction faction)
		{
			IEnumerable<Pawn> result;
			if (faction == null)
			{
				Log.Error("Called PawnsInFaction with null faction.", false);
				result = new List<Pawn>();
			}
			else
			{
				result = from x in this.AllPawns
				where x.Faction == faction
				select x;
			}
			return result;
		}

		// Token: 0x060044AC RID: 17580 RVA: 0x00240AD0 File Offset: 0x0023EED0
		public List<Pawn> SpawnedPawnsInFaction(Faction faction)
		{
			this.EnsureFactionsListsInit();
			List<Pawn> result;
			if (faction == null)
			{
				Log.Error("Called SpawnedPawnsInFaction with null faction.", false);
				result = new List<Pawn>();
			}
			else
			{
				result = this.pawnsInFactionSpawned[faction];
			}
			return result;
		}

		// Token: 0x060044AD RID: 17581 RVA: 0x00240B14 File Offset: 0x0023EF14
		public IEnumerable<Pawn> FreeHumanlikesOfFaction(Faction faction)
		{
			return from p in this.PawnsInFaction(faction)
			where p.HostFaction == null && p.RaceProps.Humanlike
			select p;
		}

		// Token: 0x060044AE RID: 17582 RVA: 0x00240B54 File Offset: 0x0023EF54
		public IEnumerable<Pawn> FreeHumanlikesSpawnedOfFaction(Faction faction)
		{
			return from p in this.SpawnedPawnsInFaction(faction)
			where p.HostFaction == null && p.RaceProps.Humanlike
			select p;
		}

		// Token: 0x060044AF RID: 17583 RVA: 0x00240B94 File Offset: 0x0023EF94
		public void RegisterPawn(Pawn p)
		{
			if (p.Dead)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register dead pawn ",
					p,
					" in ",
					base.GetType(),
					"."
				}), false);
			}
			else if (!p.Spawned)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register despawned pawn ",
					p,
					" in ",
					base.GetType(),
					"."
				}), false);
			}
			else if (p.Map != this.map)
			{
				Log.Warning("Tried to register pawn " + p + " but his Map is not this one.", false);
			}
			else if (p.mindState.Active)
			{
				this.EnsureFactionsListsInit();
				if (!this.pawnsSpawned.Contains(p))
				{
					this.pawnsSpawned.Add(p);
				}
				if (p.Faction != null)
				{
					if (!this.pawnsInFactionSpawned[p.Faction].Contains(p))
					{
						this.pawnsInFactionSpawned[p.Faction].Add(p);
						if (p.Faction == Faction.OfPlayer)
						{
							this.pawnsInFactionSpawned[Faction.OfPlayer].InsertionSort(delegate(Pawn a, Pawn b)
							{
								int num = (a.playerSettings == null) ? 0 : a.playerSettings.joinTick;
								int value = (b.playerSettings == null) ? 0 : b.playerSettings.joinTick;
								return num.CompareTo(value);
							});
						}
					}
				}
				if (p.IsPrisonerOfColony)
				{
					if (!this.prisonersOfColonySpawned.Contains(p))
					{
						this.prisonersOfColonySpawned.Add(p);
					}
				}
				this.DoListChangedNotifications();
			}
		}

		// Token: 0x060044B0 RID: 17584 RVA: 0x00240D4C File Offset: 0x0023F14C
		public void DeRegisterPawn(Pawn p)
		{
			this.EnsureFactionsListsInit();
			this.pawnsSpawned.Remove(p);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				Faction key = allFactionsListForReading[i];
				this.pawnsInFactionSpawned[key].Remove(p);
			}
			this.prisonersOfColonySpawned.Remove(p);
			this.DoListChangedNotifications();
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x00240DBF File Offset: 0x0023F1BF
		public void UpdateRegistryForPawn(Pawn p)
		{
			this.DeRegisterPawn(p);
			if (p.Spawned && p.Map == this.map)
			{
				this.RegisterPawn(p);
			}
			this.DoListChangedNotifications();
		}

		// Token: 0x060044B2 RID: 17586 RVA: 0x00240DF2 File Offset: 0x0023F1F2
		private void DoListChangedNotifications()
		{
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
			if (Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		// Token: 0x060044B3 RID: 17587 RVA: 0x00240E10 File Offset: 0x0023F210
		public void LogListedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MapPawns:");
			stringBuilder.AppendLine("pawnsSpawned");
			foreach (Pawn pawn in this.pawnsSpawned)
			{
				stringBuilder.AppendLine("    " + pawn.ToString());
			}
			stringBuilder.AppendLine("AllPawnsUnspawned");
			foreach (Pawn pawn2 in this.AllPawnsUnspawned)
			{
				stringBuilder.AppendLine("    " + pawn2.ToString());
			}
			foreach (KeyValuePair<Faction, List<Pawn>> keyValuePair in this.pawnsInFactionSpawned)
			{
				stringBuilder.AppendLine("pawnsInFactionSpawned[" + keyValuePair.Key.ToString() + "]");
				foreach (Pawn pawn3 in keyValuePair.Value)
				{
					stringBuilder.AppendLine("    " + pawn3.ToString());
				}
			}
			stringBuilder.AppendLine("prisonersOfColonySpawned");
			foreach (Pawn pawn4 in this.prisonersOfColonySpawned)
			{
				stringBuilder.AppendLine("    " + pawn4.ToString());
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x04002EAA RID: 11946
		private Map map;

		// Token: 0x04002EAB RID: 11947
		private List<Pawn> pawnsSpawned = new List<Pawn>();

		// Token: 0x04002EAC RID: 11948
		private Dictionary<Faction, List<Pawn>> pawnsInFactionSpawned = new Dictionary<Faction, List<Pawn>>();

		// Token: 0x04002EAD RID: 11949
		private List<Pawn> prisonersOfColonySpawned = new List<Pawn>();

		// Token: 0x04002EAE RID: 11950
		private List<Thing> tmpThings = new List<Thing>();

		// Token: 0x04002EAF RID: 11951
		private List<Pawn> tmpUnspawnedPawns = new List<Pawn>();
	}
}
