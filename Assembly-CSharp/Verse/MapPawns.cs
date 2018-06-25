using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using RimWorld;
using RimWorld.Planet;
using Verse.AI;

namespace Verse
{
	public sealed class MapPawns
	{
		private Map map;

		private List<Pawn> pawnsSpawned = new List<Pawn>();

		private Dictionary<Faction, List<Pawn>> pawnsInFactionSpawned = new Dictionary<Faction, List<Pawn>>();

		private List<Pawn> prisonersOfColonySpawned = new List<Pawn>();

		private List<Thing> tmpThings = new List<Thing>();

		private List<Pawn> tmpUnspawnedPawns = new List<Pawn>();

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<Pawn, bool> <>f__am$cache3;

		[CompilerGenerated]
		private static Comparison<Pawn> <>f__am$cache4;

		public MapPawns(Map map)
		{
			this.map = map;
		}

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

		public int AllPawnsCount
		{
			get
			{
				return this.AllPawns.Count<Pawn>();
			}
		}

		public int AllPawnsUnspawnedCount
		{
			get
			{
				return this.AllPawnsUnspawned.Count<Pawn>();
			}
		}

		public int FreeColonistsCount
		{
			get
			{
				return this.FreeColonists.Count<Pawn>();
			}
		}

		public int PrisonersOfColonyCount
		{
			get
			{
				return this.PrisonersOfColony.Count<Pawn>();
			}
		}

		public int FreeColonistsAndPrisonersCount
		{
			get
			{
				return this.PrisonersOfColony.Count<Pawn>();
			}
		}

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
				return this.FreeColonistsSpawned.Count<Pawn>();
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
				return this.FreeColonistsAndPrisonersSpawned.Count<Pawn>();
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
			MainTabWindowUtility.NotifyAllPawnTables_PawnsChanged();
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

		[CompilerGenerated]
		private static bool <get_PrisonersOfColony>m__0(Pawn x)
		{
			return x.IsPrisonerOfColony;
		}

		[CompilerGenerated]
		private static bool <get_ColonistCount>m__1(Pawn x)
		{
			return x.IsColonist;
		}

		[CompilerGenerated]
		private static bool <FreeHumanlikesOfFaction>m__2(Pawn p)
		{
			return p.HostFaction == null && p.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static bool <FreeHumanlikesSpawnedOfFaction>m__3(Pawn p)
		{
			return p.HostFaction == null && p.RaceProps.Humanlike;
		}

		[CompilerGenerated]
		private static int <RegisterPawn>m__4(Pawn a, Pawn b)
		{
			int num = (a.playerSettings == null) ? 0 : a.playerSettings.joinTick;
			int value = (b.playerSettings == null) ? 0 : b.playerSettings.joinTick;
			return num.CompareTo(value);
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal int <i>__1;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__2;

			internal MapPawns $this;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					i = 0;
					break;
				case 1u:
					i++;
					break;
				case 2u:
					Block_4:
					try
					{
						switch (num)
						{
						}
						if (enumerator.MoveNext())
						{
							p = enumerator.Current;
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator != null)
							{
								enumerator.Dispose();
							}
						}
					}
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				if (i >= this.pawnsSpawned.Count)
				{
					enumerator = base.AllPawnsUnspawned.GetEnumerator();
					num = 4294967293u;
					goto Block_4;
				}
				this.$current = this.pawnsSpawned[i];
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}

			Pawn IEnumerator<Pawn>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				MapPawns.<>c__Iterator0 <>c__Iterator = new MapPawns.<>c__Iterator0();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal int <i>__1;

			internal MapPawns $this;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator1()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.tmpUnspawnedPawns.Clear();
					ThingOwnerUtility.GetAllThingsRecursively<Pawn>(this.map, ThingRequest.ForGroup(ThingRequestGroup.Pawn), this.tmpUnspawnedPawns, true, null, false);
					for (int j = this.tmpUnspawnedPawns.Count - 1; j >= 0; j--)
					{
						if (this.tmpUnspawnedPawns[j].Dead)
						{
							this.tmpUnspawnedPawns.RemoveAt(j);
						}
					}
					i = this.tmpUnspawnedPawns.Count - 1;
					break;
				case 1u:
					i--;
					break;
				default:
					return false;
				}
				if (i >= 0)
				{
					this.$current = this.tmpUnspawnedPawns[i];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			Pawn IEnumerator<Pawn>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Pawn>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Pawn> IEnumerable<Pawn>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				MapPawns.<>c__Iterator1 <>c__Iterator = new MapPawns.<>c__Iterator1();
				<>c__Iterator.$this = this;
				return <>c__Iterator;
			}
		}

		[CompilerGenerated]
		private sealed class <PawnsInFaction>c__AnonStorey2
		{
			internal Faction faction;

			public <PawnsInFaction>c__AnonStorey2()
			{
			}

			internal bool <>m__0(Pawn x)
			{
				return x.Faction == this.faction;
			}
		}
	}
}
