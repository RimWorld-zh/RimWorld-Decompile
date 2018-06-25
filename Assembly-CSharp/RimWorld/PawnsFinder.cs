using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	public static class PawnsFinder
	{
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

		public static IEnumerable<Pawn> AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners
		{
			get
			{
				return PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Concat(PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_PrisonersOfColony);
			}
		}

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

		[CompilerGenerated]
		private sealed class <>c__Iterator0 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal IEnumerator<Pawn> $locvar1;

			internal Pawn <p>__2;

			internal IEnumerator<Pawn> $locvar2;

			internal Pawn <p>__3;

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
					enumerator = PawnsFinder.AllMapsWorldAndTemporary_Alive.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_DA;
				case 3u:
					Block_5:
					try
					{
						switch (num)
						{
						}
						if (enumerator3.MoveNext())
						{
							p3 = enumerator3.Current;
							this.$current = p3;
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator3 != null)
							{
								enumerator3.Dispose();
							}
						}
					}
					this.$PC = -1;
					return false;
				default:
					return false;
				}
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
							this.$PC = 1;
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
				if (Find.World == null)
				{
					goto IL_151;
				}
				enumerator2 = Find.WorldPawns.AllPawnsDead.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_DA:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						p2 = enumerator2.Current;
						this.$current = p2;
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
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				IL_151:
				enumerator3 = PawnsFinder.Temporary_Dead.GetEnumerator();
				num = 4294967293u;
				goto Block_5;
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
				case 1u:
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
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
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
				return new PawnsFinder.<>c__Iterator0();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator1 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal IEnumerator<Pawn> $locvar1;

			internal Pawn <p>__2;

			internal IEnumerator<Pawn> $locvar2;

			internal Pawn <p>__3;

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
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = PawnsFinder.AllMaps.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_DA;
				case 3u:
					Block_5:
					try
					{
						switch (num)
						{
						}
						if (enumerator3.MoveNext())
						{
							p3 = enumerator3.Current;
							this.$current = p3;
							if (!this.$disposing)
							{
								this.$PC = 3;
							}
							flag = true;
							return true;
						}
					}
					finally
					{
						if (!flag)
						{
							if (enumerator3 != null)
							{
								enumerator3.Dispose();
							}
						}
					}
					this.$PC = -1;
					return false;
				default:
					return false;
				}
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
							this.$PC = 1;
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
				if (Find.World == null)
				{
					goto IL_151;
				}
				enumerator2 = Find.WorldPawns.AllPawnsAlive.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_DA:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						p2 = enumerator2.Current;
						this.$current = p2;
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
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
				}
				IL_151:
				enumerator3 = PawnsFinder.Temporary_Alive.GetEnumerator();
				num = 4294967293u;
				goto Block_5;
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
				case 1u:
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
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					break;
				case 3u:
					try
					{
					}
					finally
					{
						if (enumerator3 != null)
						{
							enumerator3.Dispose();
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
				return new PawnsFinder.<>c__Iterator1();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator2 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__1;

			internal int <i>__2;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__3;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator2()
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
					if (Current.ProgramState == ProgramState.Entry)
					{
						goto IL_10D;
					}
					maps = Find.Maps;
					i = 0;
					break;
				case 1u:
					Block_3:
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
								this.$PC = 1;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < maps.Count)
				{
					enumerator = maps[i].mapPawns.AllPawns.GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
				IL_10D:
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
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
				return new PawnsFinder.<>c__Iterator2();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator3 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__1;

			internal int <i>__2;

			internal List<Pawn> <spawned>__3;

			internal int <j>__4;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator3()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (Current.ProgramState != ProgramState.Entry)
					{
						maps = Find.Maps;
						i = 0;
						goto IL_D2;
					}
					goto IL_E9;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_AD:
				if (j < spawned.Count)
				{
					this.$current = spawned[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_D2:
				if (i < maps.Count)
				{
					spawned = maps[i].mapPawns.AllPawnsSpawned;
					j = 0;
					goto IL_AD;
				}
				IL_E9:
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
				return new PawnsFinder.<>c__Iterator3();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator4 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal IEnumerator<Pawn> $locvar1;

			internal Pawn <p>__2;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator4()
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
					enumerator = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_C6;
				default:
					return false;
				}
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
							this.$PC = 1;
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
				enumerator2 = PawnsFinder.AllCaravansAndTravelingTransportPods_AliveOrDead.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_C6:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						p2 = enumerator2.Current;
						this.$current = p2;
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
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
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
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
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
				return new PawnsFinder.<>c__Iterator4();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator5 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<List<Pawn>> <makingPawnsList>__0;

			internal int <i>__1;

			internal List<Pawn> <makingPawns>__2;

			internal int <j>__3;

			internal List<List<Thing>> <makingThingsList>__0;

			internal int <i>__4;

			internal List<Thing> <makingThings>__5;

			internal int <j>__6;

			internal Pawn <p>__7;

			internal List<Pawn> <startingAndOptionalPawns>__8;

			internal int <i>__9;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator5()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					makingPawnsList = PawnGroupKindWorker.pawnsBeingGeneratedNow;
					i = 0;
					goto IL_C5;
				case 1u:
					j++;
					break;
				case 2u:
					IL_15E:
					l++;
					goto IL_16D;
				case 3u:
					IL_21C:
					m++;
					goto IL_22B;
				default:
					return false;
				}
				IL_A0:
				if (j < makingPawns.Count)
				{
					this.$current = makingPawns[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_C5:
				if (i >= makingPawnsList.Count)
				{
					makingThingsList = ThingSetMaker.thingsBeingGeneratedNow;
					k = 0;
					goto IL_192;
				}
				makingPawns = makingPawnsList[i];
				j = 0;
				goto IL_A0;
				IL_16D:
				if (l >= makingThings.Count)
				{
					k++;
				}
				else
				{
					p = (makingThings[l] as Pawn);
					if (p != null)
					{
						this.$current = p;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_15E;
				}
				IL_192:
				if (k < makingThingsList.Count)
				{
					makingThings = makingThingsList[k];
					l = 0;
					goto IL_16D;
				}
				if (Current.ProgramState == ProgramState.Playing || Find.GameInitData == null)
				{
					goto IL_242;
				}
				startingAndOptionalPawns = Find.GameInitData.startingAndOptionalPawns;
				m = 0;
				IL_22B:
				if (m < startingAndOptionalPawns.Count)
				{
					if (startingAndOptionalPawns[m] != null)
					{
						this.$current = startingAndOptionalPawns[m];
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
					goto IL_21C;
				}
				IL_242:
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
				return new PawnsFinder.<>c__Iterator5();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator6 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator6()
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
					enumerator = PawnsFinder.Temporary.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_8D:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (!p.Dead)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_8D;
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
				case 1u:
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
				return new PawnsFinder.<>c__Iterator6();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator7 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator7()
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
					enumerator = PawnsFinder.Temporary.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_8D:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (p.Dead)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_8D;
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
				case 1u:
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
				return new PawnsFinder.<>c__Iterator7();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator8 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal IEnumerator<Pawn> $locvar1;

			internal Pawn <p>__2;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator8()
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
					enumerator = PawnsFinder.AllMaps.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				case 2u:
					goto IL_C6;
				default:
					return false;
				}
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
							this.$PC = 1;
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
				enumerator2 = PawnsFinder.AllCaravansAndTravelingTransportPods_Alive.GetEnumerator();
				num = 4294967293u;
				try
				{
					IL_C6:
					switch (num)
					{
					}
					if (enumerator2.MoveNext())
					{
						p2 = enumerator2.Current;
						this.$current = p2;
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
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
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
				case 2u:
					try
					{
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
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
				return new PawnsFinder.<>c__Iterator8();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator9 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator9()
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
					enumerator = PawnsFinder.AllCaravansAndTravelingTransportPods_AliveOrDead.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_8D:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (!p.Dead)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_8D;
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
				case 1u:
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
				return new PawnsFinder.<>c__Iterator9();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__IteratorA : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Caravan> <caravans>__1;

			internal int <i>__2;

			internal List<Pawn> <pawns>__3;

			internal int <j>__4;

			internal List<TravelingTransportPods> <travelingTransportPods>__1;

			internal int <i>__5;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__6;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__IteratorA()
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
					if (Find.World != null)
					{
						caravans = Find.WorldObjects.Caravans;
						i = 0;
						goto IL_D8;
					}
					goto IL_1CC;
				case 1u:
					j++;
					break;
				case 2u:
					Block_6:
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
					k++;
					goto IL_1B5;
				default:
					return false;
				}
				IL_B3:
				if (j < pawns.Count)
				{
					this.$current = pawns[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_D8:
				if (i < caravans.Count)
				{
					pawns = caravans[i].PawnsListForReading;
					j = 0;
					goto IL_B3;
				}
				travelingTransportPods = Find.WorldObjects.TravelingTransportPods;
				k = 0;
				IL_1B5:
				if (k < travelingTransportPods.Count)
				{
					enumerator = travelingTransportPods[k].Pawns.GetEnumerator();
					num = 4294967293u;
					goto Block_6;
				}
				IL_1CC:
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
				return new PawnsFinder.<>c__IteratorA();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__IteratorB : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__IteratorB()
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
					enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_8D:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (p.IsColonist)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_8D;
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
				case 1u:
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
				return new PawnsFinder.<>c__IteratorB();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__IteratorC : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__IteratorC()
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
					enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_8D:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (p.IsFreeColonist)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_8D;
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
				case 1u:
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
				return new PawnsFinder.<>c__IteratorC();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__IteratorD : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__IteratorD()
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
					enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_9D:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (p.IsFreeColonist && !p.Suspended)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_9D;
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
				case 1u:
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
				return new PawnsFinder.<>c__IteratorD();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__IteratorE : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal Faction <playerFaction>__0;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__IteratorE()
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
					playerFaction = Faction.OfPlayer;
					enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_9E:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (p.Faction == playerFaction)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_9E;
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
				case 1u:
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
				return new PawnsFinder.<>c__IteratorE();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__IteratorF : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal Faction <playerFaction>__0;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__IteratorF()
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
					playerFaction = Faction.OfPlayer;
					enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_AE:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (p.Faction == playerFaction && !p.Suspended)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_AE;
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
				case 1u:
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
				return new PawnsFinder.<>c__IteratorF();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator10 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator10()
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
					enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_8D:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (p.IsPrisonerOfColony)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_8D;
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
				case 1u:
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
				return new PawnsFinder.<>c__Iterator10();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator11 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__1;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator11()
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
					enumerator = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonistsAndPrisoners.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					case 1u:
						IL_8D:
						break;
					}
					if (enumerator.MoveNext())
					{
						p = enumerator.Current;
						if (!p.Suspended)
						{
							this.$current = p;
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							flag = true;
							return true;
						}
						goto IL_8D;
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
				case 1u:
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
				return new PawnsFinder.<>c__Iterator11();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator12 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__1;

			internal int <i>__2;

			internal List<Pawn> <prisonersOfColonySpawned>__3;

			internal int <j>__4;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator12()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (Current.ProgramState != ProgramState.Entry)
					{
						maps = Find.Maps;
						i = 0;
						goto IL_D2;
					}
					goto IL_E9;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_AD:
				if (j < prisonersOfColonySpawned.Count)
				{
					this.$current = prisonersOfColonySpawned[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_D2:
				if (i < maps.Count)
				{
					prisonersOfColonySpawned = maps[i].mapPawns.PrisonersOfColonySpawned;
					j = 0;
					goto IL_AD;
				}
				IL_E9:
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
				return new PawnsFinder.<>c__Iterator12();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator13 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__1;

			internal int <i>__2;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__3;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator13()
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
					if (Current.ProgramState == ProgramState.Entry)
					{
						goto IL_10D;
					}
					maps = Find.Maps;
					i = 0;
					break;
				case 1u:
					Block_3:
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
								this.$PC = 1;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < maps.Count)
				{
					enumerator = maps[i].mapPawns.PrisonersOfColony.GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
				IL_10D:
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
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
				return new PawnsFinder.<>c__Iterator13();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator14 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__1;

			internal int <i>__2;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__3;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator14()
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
					if (Current.ProgramState == ProgramState.Entry)
					{
						goto IL_10D;
					}
					maps = Find.Maps;
					i = 0;
					break;
				case 1u:
					Block_3:
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
								this.$PC = 1;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < maps.Count)
				{
					enumerator = maps[i].mapPawns.FreeColonists.GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
				IL_10D:
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
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
				return new PawnsFinder.<>c__Iterator14();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator15 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__1;

			internal int <i>__2;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__3;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator15()
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
					if (Current.ProgramState == ProgramState.Entry)
					{
						goto IL_10D;
					}
					maps = Find.Maps;
					i = 0;
					break;
				case 1u:
					Block_3:
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
								this.$PC = 1;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < maps.Count)
				{
					enumerator = maps[i].mapPawns.FreeColonistsSpawned.GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
				IL_10D:
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
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
				return new PawnsFinder.<>c__Iterator15();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator16 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__1;

			internal int <i>__2;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__3;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator16()
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
					if (Current.ProgramState == ProgramState.Entry)
					{
						goto IL_10D;
					}
					maps = Find.Maps;
					i = 0;
					break;
				case 1u:
					Block_3:
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
								this.$PC = 1;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < maps.Count)
				{
					enumerator = maps[i].mapPawns.FreeColonistsAndPrisonersSpawned.GetEnumerator();
					num = 4294967293u;
					goto Block_3;
				}
				IL_10D:
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
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
				return new PawnsFinder.<>c__Iterator16();
			}
		}

		[CompilerGenerated]
		private sealed class <>c__Iterator17 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__0;

			internal int <i>__1;

			internal IEnumerator<Pawn> $locvar0;

			internal Pawn <p>__2;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <>c__Iterator17()
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
					maps = Find.Maps;
					i = 0;
					break;
				case 1u:
					Block_2:
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
								this.$PC = 1;
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
					i++;
					break;
				default:
					return false;
				}
				if (i < maps.Count)
				{
					enumerator = maps[i].mapPawns.FreeColonistsAndPrisoners.GetEnumerator();
					num = 4294967293u;
					goto Block_2;
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
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
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
				return new PawnsFinder.<>c__Iterator17();
			}
		}

		[CompilerGenerated]
		private sealed class <AllMaps_SpawnedPawnsInFaction>c__Iterator18 : IEnumerable, IEnumerable<Pawn>, IEnumerator, IDisposable, IEnumerator<Pawn>
		{
			internal List<Map> <maps>__0;

			internal int <i>__1;

			internal Faction faction;

			internal List<Pawn> <spawnedPawnsInFaction>__2;

			internal int <j>__3;

			internal Pawn $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <AllMaps_SpawnedPawnsInFaction>c__Iterator18()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					maps = Find.Maps;
					i = 0;
					goto IL_CD;
				case 1u:
					j++;
					break;
				default:
					return false;
				}
				IL_A8:
				if (j < spawnedPawnsInFaction.Count)
				{
					this.$current = spawnedPawnsInFaction[j];
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				i++;
				IL_CD:
				if (i < maps.Count)
				{
					spawnedPawnsInFaction = maps[i].mapPawns.SpawnedPawnsInFaction(faction);
					j = 0;
					goto IL_A8;
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
				PawnsFinder.<AllMaps_SpawnedPawnsInFaction>c__Iterator18 <AllMaps_SpawnedPawnsInFaction>c__Iterator = new PawnsFinder.<AllMaps_SpawnedPawnsInFaction>c__Iterator18();
				<AllMaps_SpawnedPawnsInFaction>c__Iterator.faction = faction;
				return <AllMaps_SpawnedPawnsInFaction>c__Iterator;
			}
		}
	}
}
