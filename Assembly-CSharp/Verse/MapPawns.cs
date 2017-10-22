using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Verse
{
	public sealed class MapPawns
	{
		private Map map;

		private List<Pawn> pawnsSpawned = new List<Pawn>();

		private Dictionary<Faction, List<Pawn>> pawnsInFactionSpawned = new Dictionary<Faction, List<Pawn>>();

		private List<Pawn> prisonersOfColonySpawned = new List<Pawn>();

		private static List<Thing> tmpThings = new List<Thing>();

		private static List<IThingHolder> tmpMapChildHolders = new List<IThingHolder>();

		public IEnumerable<Pawn> AllPawns
		{
			get
			{
				int i = 0;
				if (i < this.pawnsSpawned.Count)
				{
					yield return this.pawnsSpawned[i];
					/*Error: Unable to find new state assignment for yield return*/;
				}
				using (IEnumerator<Pawn> enumerator = this.AllPawnsUnspawned.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						Pawn p = enumerator.Current;
						yield return p;
						/*Error: Unable to find new state assignment for yield return*/;
					}
				}
				yield break;
				IL_0128:
				/*Error near IL_0129: Unexpected return in MoveNext()*/;
			}
		}

		public IEnumerable<Pawn> AllPawnsUnspawned
		{
			get
			{
				List<Thing> holders = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
				for (int n = 0; n < holders.Count; n++)
				{
					IThingHolder holder = holders[n] as IThingHolder;
					if (holder != null)
					{
						ThingOwnerUtility.GetAllThingsRecursively(holder, MapPawns.tmpThings, true);
						for (int k = 0; k < MapPawns.tmpThings.Count; k++)
						{
							Pawn p2 = MapPawns.tmpThings[k] as Pawn;
							if (p2 != null && !p2.Dead)
							{
								yield return p2;
								/*Error: Unable to find new state assignment for yield return*/;
							}
						}
					}
					ThingWithComps twc = holders[n] as ThingWithComps;
					if (twc != null)
					{
						List<ThingComp> comps = twc.AllComps;
						for (int j = 0; j < comps.Count; j++)
						{
							IThingHolder compHolder = comps[j] as IThingHolder;
							if (compHolder != null)
							{
								ThingOwnerUtility.GetAllThingsRecursively(compHolder, MapPawns.tmpThings, true);
								for (int i = 0; i < MapPawns.tmpThings.Count; i++)
								{
									Pawn p = MapPawns.tmpThings[i] as Pawn;
									if (p != null && !p.Dead)
									{
										yield return p;
										/*Error: Unable to find new state assignment for yield return*/;
									}
								}
							}
						}
					}
				}
				MapPawns.tmpMapChildHolders.Clear();
				this.map.GetNonThingChildHolders(MapPawns.tmpMapChildHolders);
				for (int m = 0; m < MapPawns.tmpMapChildHolders.Count; m++)
				{
					ThingOwnerUtility.GetAllThingsRecursively(MapPawns.tmpMapChildHolders[m], MapPawns.tmpThings, true);
					for (int l = 0; l < MapPawns.tmpThings.Count; l++)
					{
						Pawn p3 = MapPawns.tmpThings[l] as Pawn;
						if (p3 != null && !p3.Dead)
						{
							yield return p3;
							/*Error: Unable to find new state assignment for yield return*/;
						}
					}
				}
				MapPawns.tmpMapChildHolders.Clear();
				MapPawns.tmpThings.Clear();
			}
		}

		public IEnumerable<Pawn> FreeColonists
		{
			get
			{
				return this.FreeHumanlikesOfFaction(Faction.OfPlayer);
			}
		}

		public IEnumerable<Pawn> PrisonersOfColony
		{
			get
			{
				return from x in this.AllPawns
				where x.IsPrisonerOfColony
				select x;
			}
		}

		public IEnumerable<Pawn> FreeColonistsAndPrisoners
		{
			get
			{
				return this.FreeColonists.Concat(this.PrisonersOfColony);
			}
		}

		public int ColonistCount
		{
			get
			{
				int result;
				if (Current.ProgramState != ProgramState.Playing)
				{
					Log.Error("ColonistCount while not playing. This should get the starting player pawn count.");
					result = 3;
				}
				else
				{
					result = this.AllPawns.Count((Func<Pawn, bool>)((Pawn x) => x.IsColonist));
				}
				return result;
			}
		}

		public int AllPawnsCount
		{
			get
			{
				return this.AllPawns.Count();
			}
		}

		public int AllPawnsUnspawnedCount
		{
			get
			{
				return this.AllPawnsUnspawned.Count();
			}
		}

		public int FreeColonistsCount
		{
			get
			{
				return this.FreeColonists.Count();
			}
		}

		public int PrisonersOfColonyCount
		{
			get
			{
				return this.PrisonersOfColony.Count();
			}
		}

		public int FreeColonistsAndPrisonersCount
		{
			get
			{
				return this.PrisonersOfColony.Count();
			}
		}

		public bool AnyPawnBlockingMapRemoval
		{
			get
			{
				Faction ofPlayer = Faction.OfPlayer;
				int num = 0;
				bool result;
				while (true)
				{
					if (num < this.pawnsSpawned.Count)
					{
						if (this.pawnsSpawned[num].Faction != ofPlayer && this.pawnsSpawned[num].HostFaction != ofPlayer)
						{
							if (this.pawnsSpawned[num].relations != null && this.pawnsSpawned[num].relations.relativeInvolvedInRescueQuest != null)
							{
								result = true;
								break;
							}
							num++;
							continue;
						}
						result = true;
					}
					else
					{
						List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.ThingHolder);
						for (int i = 0; i < list.Count; i++)
						{
							if (list[i] is IActiveDropPod || list[i].TryGetComp<CompTransporter>() != null)
							{
								IThingHolder holder = list[i].TryGetComp<CompTransporter>() ?? ((IThingHolder)list[i]);
								ThingOwnerUtility.GetAllThingsRecursively(holder, MapPawns.tmpThings, true);
								for (int j = 0; j < MapPawns.tmpThings.Count; j++)
								{
									Pawn pawn = MapPawns.tmpThings[j] as Pawn;
									if (pawn != null && (pawn.Faction == ofPlayer || pawn.HostFaction == ofPlayer))
									{
										goto IL_0140;
									}
								}
							}
						}
						MapPawns.tmpThings.Clear();
						result = false;
					}
					break;
					IL_0140:
					MapPawns.tmpThings.Clear();
					result = true;
					break;
				}
				return result;
			}
		}

		public List<Pawn> AllPawnsSpawned
		{
			get
			{
				return this.pawnsSpawned;
			}
		}

		public IEnumerable<Pawn> FreeColonistsSpawned
		{
			get
			{
				return this.FreeHumanlikesSpawnedOfFaction(Faction.OfPlayer);
			}
		}

		public List<Pawn> PrisonersOfColonySpawned
		{
			get
			{
				return this.prisonersOfColonySpawned;
			}
		}

		public IEnumerable<Pawn> FreeColonistsAndPrisonersSpawned
		{
			get
			{
				return this.FreeColonistsSpawned.Concat(this.PrisonersOfColonySpawned);
			}
		}

		public int AllPawnsSpawnedCount
		{
			get
			{
				return this.pawnsSpawned.Count;
			}
		}

		public int FreeColonistsSpawnedCount
		{
			get
			{
				return this.FreeColonistsSpawned.Count();
			}
		}

		public int PrisonersOfColonySpawnedCount
		{
			get
			{
				return this.PrisonersOfColonySpawned.Count;
			}
		}

		public int FreeColonistsAndPrisonersSpawnedCount
		{
			get
			{
				return this.FreeColonistsAndPrisonersSpawned.Count();
			}
		}

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
					if ((building_CryptosleepCasket == null || !building_CryptosleepCasket.def.building.isPlayerEjectable) && !(list[j] is IActiveDropPod) && list[j].TryGetComp<CompTransporter>() == null)
					{
						continue;
					}
					IThingHolder holder = list[j].TryGetComp<CompTransporter>() ?? ((IThingHolder)list[j]);
					ThingOwnerUtility.GetAllThingsRecursively(holder, MapPawns.tmpThings, true);
					for (int k = 0; k < MapPawns.tmpThings.Count; k++)
					{
						Pawn pawn = MapPawns.tmpThings[k] as Pawn;
						if (pawn != null && pawn.IsColonist && pawn.HostFaction == null)
						{
							num++;
						}
					}
				}
				MapPawns.tmpThings.Clear();
				return num;
			}
		}

		public bool AnyColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				int num = 0;
				bool result;
				while (true)
				{
					if (num < list.Count)
					{
						if (list[num].IsColonist)
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
			}
		}

		public bool AnyFreeColonistSpawned
		{
			get
			{
				List<Pawn> list = this.SpawnedPawnsInFaction(Faction.OfPlayer);
				int num = 0;
				bool result;
				while (true)
				{
					if (num < list.Count)
					{
						if (list[num].IsFreeColonist)
						{
							result = true;
							break;
						}
						num++;
						continue;
					}
					result = false;
					break;
				}
				return result;
			}
		}

		public MapPawns(Map map)
		{
			this.map = map;
		}

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

		public IEnumerable<Pawn> PawnsInFaction(Faction faction)
		{
			IEnumerable<Pawn> result;
			if (faction == null)
			{
				Log.Error("Called PawnsInFaction with null faction.");
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

		public List<Pawn> SpawnedPawnsInFaction(Faction faction)
		{
			this.EnsureFactionsListsInit();
			List<Pawn> result;
			if (faction == null)
			{
				Log.Error("Called SpawnedPawnsInFaction with null faction.");
				result = new List<Pawn>();
			}
			else
			{
				result = this.pawnsInFactionSpawned[faction];
			}
			return result;
		}

		public IEnumerable<Pawn> FreeHumanlikesOfFaction(Faction faction)
		{
			return from p in this.PawnsInFaction(faction)
			where p.HostFaction == null && p.RaceProps.Humanlike
			select p;
		}

		public IEnumerable<Pawn> FreeHumanlikesSpawnedOfFaction(Faction faction)
		{
			return from p in this.SpawnedPawnsInFaction(faction)
			where p.HostFaction == null && p.RaceProps.Humanlike
			select p;
		}

		public void RegisterPawn(Pawn p)
		{
			if (p.Dead)
			{
				Log.Warning("Tried to register dead pawn " + p + " in " + base.GetType() + ".");
			}
			else if (!p.Spawned)
			{
				Log.Warning("Tried to register despawned pawn " + p + " in " + base.GetType() + ".");
			}
			else if (p.Map != this.map)
			{
				Log.Warning("Tried to register pawn " + p + " but his Map is not this one.");
			}
			else if (p.mindState.Active)
			{
				this.EnsureFactionsListsInit();
				if (!this.pawnsSpawned.Contains(p))
				{
					this.pawnsSpawned.Add(p);
				}
				if (p.Faction != null && !this.pawnsInFactionSpawned[p.Faction].Contains(p))
				{
					this.pawnsInFactionSpawned[p.Faction].Add(p);
					if (p.Faction == Faction.OfPlayer)
					{
						this.pawnsInFactionSpawned[Faction.OfPlayer].InsertionSort((Comparison<Pawn>)delegate(Pawn a, Pawn b)
						{
							int num = (a.playerSettings != null) ? a.playerSettings.joinTick : 0;
							int value = (b.playerSettings != null) ? b.playerSettings.joinTick : 0;
							return num.CompareTo(value);
						});
					}
				}
				if (p.IsPrisonerOfColony && !this.prisonersOfColonySpawned.Contains(p))
				{
					this.prisonersOfColonySpawned.Add(p);
				}
				this.DoListChangedNotifications();
			}
		}

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

		public void UpdateRegistryForPawn(Pawn p)
		{
			this.DeRegisterPawn(p);
			if (p.Spawned && p.Map == this.map)
			{
				this.RegisterPawn(p);
			}
			this.DoListChangedNotifications();
		}

		private void DoListChangedNotifications()
		{
			if (Find.WindowStack != null)
			{
				WindowStack windowStack = Find.WindowStack;
				for (int i = 0; i < windowStack.Count; i++)
				{
					MainTabWindow_PawnTable mainTabWindow_PawnTable = windowStack[i] as MainTabWindow_PawnTable;
					if (mainTabWindow_PawnTable != null)
					{
						mainTabWindow_PawnTable.Notify_PawnsChanged();
					}
				}
			}
			if (Find.ColonistBar != null)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
		}

		public void LogListedPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("MapPawns:");
			stringBuilder.AppendLine("pawnsSpawned");
			foreach (Pawn item in this.pawnsSpawned)
			{
				stringBuilder.AppendLine("    " + item.ToString());
			}
			stringBuilder.AppendLine("AllPawnsUnspawned");
			foreach (Pawn item2 in this.AllPawnsUnspawned)
			{
				stringBuilder.AppendLine("    " + item2.ToString());
			}
			foreach (KeyValuePair<Faction, List<Pawn>> item3 in this.pawnsInFactionSpawned)
			{
				stringBuilder.AppendLine("pawnsInFactionSpawned[" + item3.Key.ToString() + "]");
				foreach (Pawn item4 in item3.Value)
				{
					stringBuilder.AppendLine("    " + item4.ToString());
				}
			}
			stringBuilder.AppendLine("prisonersOfColonySpawned");
			foreach (Pawn item5 in this.prisonersOfColonySpawned)
			{
				stringBuilder.AppendLine("    " + item5.ToString());
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
