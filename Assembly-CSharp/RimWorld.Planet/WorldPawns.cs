#define ENABLE_PROFILER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld.Planet
{
	public class WorldPawns : IExposable
	{
		private HashSet<Pawn> pawnsAlive = new HashSet<Pawn>();

		private HashSet<Pawn> pawnsMothballed = new HashSet<Pawn>();

		private HashSet<Pawn> pawnsDead = new HashSet<Pawn>();

		private HashSet<Pawn> pawnsForcefullyKeptAsWorldPawns = new HashSet<Pawn>();

		public WorldPawnGC gc = new WorldPawnGC();

		private Stack<Pawn> pawnsBeingDiscarded = new Stack<Pawn>();

		private const float PctOfHumanlikesAlwaysKept = 0.1f;

		private const float PctOfUnnamedColonyAnimalsAlwaysKept = 0.05f;

		private const int TendIntervalTicks = 7500;

		private const int MothballUpdateInterval = 15000;

		private static List<Pawn> tmpPawnsToTick = new List<Pawn>();

		private static List<Pawn> tmpPawnsToRemove = new List<Pawn>();

		public IEnumerable<Pawn> AllPawnsAliveOrDead
		{
			get
			{
				return this.AllPawnsAlive.Concat(this.AllPawnsDead);
			}
		}

		public IEnumerable<Pawn> AllPawnsAlive
		{
			get
			{
				return this.pawnsAlive.Concat(this.pawnsMothballed);
			}
		}

		public IEnumerable<Pawn> AllPawnsDead
		{
			get
			{
				return this.pawnsDead;
			}
		}

		public HashSet<Pawn> ForcefullyKeptPawns
		{
			get
			{
				return this.pawnsForcefullyKeptAsWorldPawns;
			}
		}

		public void WorldPawnsTick()
		{
			WorldPawns.tmpPawnsToTick.Clear();
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsAlive);
			for (int i = 0; i < WorldPawns.tmpPawnsToTick.Count; i++)
			{
				WorldPawns.tmpPawnsToTick[i].Tick();
				if (this.ShouldAutoTendTo(WorldPawns.tmpPawnsToTick[i]))
				{
					TendUtility.DoTend(null, WorldPawns.tmpPawnsToTick[i], null);
				}
			}
			WorldPawns.tmpPawnsToTick.Clear();
			if (Find.TickManager.TicksGame % 15000 == 0)
			{
				this.DoMothballProcessing();
			}
			WorldPawns.tmpPawnsToRemove.Clear();
			foreach (Pawn item in this.pawnsDead)
			{
				if (item.Discarded)
				{
					Log.Error("World pawn " + item + " has been discarded while still being a world pawn. This should never happen, because discard destroy mode means that the pawn is no longer managed by anything. Pawn should have been removed from the world first.");
					WorldPawns.tmpPawnsToRemove.Add(item);
				}
			}
			for (int j = 0; j < WorldPawns.tmpPawnsToRemove.Count; j++)
			{
				this.pawnsDead.Remove(WorldPawns.tmpPawnsToRemove[j]);
			}
			WorldPawns.tmpPawnsToRemove.Clear();
			Profiler.BeginSample("WorldPawnGCTick");
			try
			{
				this.gc.WorldPawnGCTick();
			}
			catch (Exception arg)
			{
				Log.Error("Error in WorldPawnGCTick(): " + arg);
			}
			Profiler.EndSample();
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnsForcefullyKeptAsWorldPawns, true, "pawnsForcefullyKeptAsWorldPawns", LookMode.Reference);
			Scribe_Collections.Look<Pawn>(ref this.pawnsAlive, "pawnsAlive", LookMode.Deep);
			Scribe_Collections.Look<Pawn>(ref this.pawnsMothballed, "pawnsMothballed", LookMode.Deep);
			Scribe_Collections.Look<Pawn>(ref this.pawnsDead, true, "pawnsDead", LookMode.Deep);
			Scribe_Deep.Look<WorldPawnGC>(ref this.gc, "gc", new object[0]);
			if (this.pawnsMothballed == null)
			{
				this.pawnsMothballed = new HashSet<Pawn>();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.WorldPawnPostLoadInit(this);
			}
		}

		public bool Contains(Pawn p)
		{
			return this.pawnsAlive.Contains(p) || this.pawnsMothballed.Contains(p) || this.pawnsDead.Contains(p);
		}

		public void PassToWorld(Pawn pawn, PawnDiscardDecideMode discardMode = PawnDiscardDecideMode.Decide)
		{
			if (pawn.Spawned)
			{
				Log.Error("Tried to call PassToWorld with spawned pawn: " + pawn + ". Despawn him first.");
			}
			else if (this.Contains(pawn))
			{
				Log.Error("Tried to pass pawn " + pawn + " to world, but it's already here.");
			}
			else
			{
				if (discardMode == PawnDiscardDecideMode.KeepForever && pawn.Discarded)
				{
					Log.Error("Tried to pass a discarded pawn " + pawn + " to world with discardMode=Keep. Discarded pawns should never be stored in WorldPawns.");
					discardMode = PawnDiscardDecideMode.Decide;
				}
				if (pawn.pather != null)
				{
					PawnComponentsUtility.RemoveComponentsOnDespawned(pawn);
				}
				switch (discardMode)
				{
				case PawnDiscardDecideMode.Decide:
				{
					this.AddPawn(pawn);
					break;
				}
				case PawnDiscardDecideMode.KeepForever:
				{
					this.pawnsForcefullyKeptAsWorldPawns.Add(pawn);
					this.AddPawn(pawn);
					break;
				}
				case PawnDiscardDecideMode.Discard:
				{
					this.DiscardPawn(pawn, false);
					break;
				}
				}
			}
		}

		public void RemovePawn(Pawn p)
		{
			if (!this.Contains(p))
			{
				Log.Error("Tried to remove pawn " + p + " from " + base.GetType() + ", but it's not here.");
			}
			this.gc.CancelGCPass();
			if (this.pawnsMothballed.Contains(p))
			{
				p.TickMothballed(Find.TickManager.TicksGame % 15000);
			}
			this.pawnsAlive.Remove(p);
			this.pawnsMothballed.Remove(p);
			this.pawnsDead.Remove(p);
			this.pawnsForcefullyKeptAsWorldPawns.Remove(p);
		}

		public void RemoveAndDiscardPawnViaGC(Pawn p)
		{
			this.RemovePawn(p);
			this.DiscardPawn(p, true);
		}

		public WorldPawnSituation GetSituation(Pawn p)
		{
			return (WorldPawnSituation)(this.Contains(p) ? ((p.Dead || p.Destroyed) ? 2 : ((!PawnUtility.IsFactionLeader(p)) ? ((!PawnUtility.IsKidnappedPawn(p)) ? ((!p.IsCaravanMember()) ? ((!PawnUtility.IsTravelingInTransportPodWorldObject(p)) ? ((!PawnUtility.ForSaleBySettlement(p)) ? 1 : 7) : 6) : 5) : 4) : 3)) : 0);
		}

		public IEnumerable<Pawn> GetPawnsBySituation(WorldPawnSituation situation)
		{
			return from x in this.AllPawnsAliveOrDead
			where this.GetSituation(x) == situation
			select x;
		}

		public int GetPawnsBySituationCount(WorldPawnSituation situation)
		{
			int num = 0;
			foreach (Pawn item in this.pawnsAlive)
			{
				if (this.GetSituation(item) == situation)
				{
					num++;
				}
			}
			foreach (Pawn item2 in this.pawnsDead)
			{
				if (this.GetSituation(item2) == situation)
				{
					num++;
				}
			}
			return num;
		}

		private bool ShouldAutoTendTo(Pawn pawn)
		{
			return !pawn.Dead && !pawn.Destroyed && pawn.IsHashIntervalTick(7500) && !pawn.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(pawn);
		}

		public bool IsBeingDiscarded(Pawn p)
		{
			return this.pawnsBeingDiscarded.Contains(p);
		}

		public void Notify_PawnDestroyed(Pawn p)
		{
			if (!this.pawnsAlive.Contains(p) && !this.pawnsMothballed.Contains(p))
				return;
			this.pawnsAlive.Remove(p);
			this.pawnsMothballed.Remove(p);
			this.pawnsDead.Add(p);
		}

		private bool ShouldMothball(Pawn p)
		{
			return this.DefPreventingMothball(p) == null && !p.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(p);
		}

		private HediffDef DefPreventingMothball(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			int num = 0;
			HediffDef result;
			while (true)
			{
				if (num < hediffs.Count)
				{
					if (!hediffs[num].def.AlwaysAllowMothball && !hediffs[num].IsOld())
					{
						result = hediffs[num].def;
						break;
					}
					num++;
					continue;
				}
				result = null;
				break;
			}
			return result;
		}

		private void AddPawn(Pawn p)
		{
			this.gc.CancelGCPass();
			if (p.Dead || p.Destroyed)
			{
				this.pawnsDead.Add(p);
			}
			else
			{
				this.pawnsAlive.Add(p);
			}
			p.Notify_PassedToWorld();
		}

		private void DiscardPawn(Pawn p, bool silentlyRemoveReferences = false)
		{
			this.pawnsBeingDiscarded.Push(p);
			try
			{
				if (!p.Destroyed)
				{
					p.Destroy(DestroyMode.Vanish);
				}
				if (!p.Discarded)
				{
					p.Discard(silentlyRemoveReferences);
				}
			}
			finally
			{
				this.pawnsBeingDiscarded.Pop();
			}
		}

		private void DoMothballProcessing()
		{
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsMothballed);
			for (int i = 0; i < WorldPawns.tmpPawnsToTick.Count; i++)
			{
				WorldPawns.tmpPawnsToTick[i].TickMothballed(15000);
			}
			WorldPawns.tmpPawnsToTick.Clear();
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsAlive);
			for (int j = 0; j < WorldPawns.tmpPawnsToTick.Count; j++)
			{
				Pawn pawn = WorldPawns.tmpPawnsToTick[j];
				if (this.ShouldMothball(pawn))
				{
					this.pawnsAlive.Remove(pawn);
					this.pawnsMothballed.Add(pawn);
				}
			}
			WorldPawns.tmpPawnsToTick.Clear();
		}

		public void DebugRunMothballProcessing()
		{
			this.DoMothballProcessing();
			Log.Message(string.Format("World pawn mothball run complete"));
		}

		public void UnpinAllForcefullyKeptPawns()
		{
			this.pawnsForcefullyKeptAsWorldPawns.Clear();
		}

		public void LogWorldPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= World Pawns =======");
			stringBuilder.AppendLine("Count: " + this.AllPawnsAliveOrDead.Count());
			stringBuilder.AppendLine(string.Format("(Live: {0} - Mothballed: {1} - Dead: {2}; {3} forcefully kept)", this.pawnsAlive.Count, this.pawnsMothballed.Count, this.pawnsDead.Count, this.pawnsForcefullyKeptAsWorldPawns.Count));
			WorldPawnSituation[] array = (WorldPawnSituation[])Enum.GetValues(typeof(WorldPawnSituation));
			for (int i = 0; i < array.Length; i++)
			{
				WorldPawnSituation worldPawnSituation = array[i];
				if (worldPawnSituation != 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("== " + worldPawnSituation + " ==");
					foreach (Pawn item in from x in this.GetPawnsBySituation(worldPawnSituation)
					orderby (x.Faction != null) ? x.Faction.loadID : (-1)
					select x)
					{
						string text = (item.Name == null) ? item.LabelCap : item.Name.ToStringFull;
						stringBuilder.AppendLine(text + ", " + item.KindLabel + ", " + item.Faction);
					}
				}
			}
			stringBuilder.AppendLine("===========================");
			Log.Message(stringBuilder.ToString());
		}

		public void LogWorldPawnMothballPrevention()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= World Pawns Mothball Prevention =======");
			stringBuilder.AppendLine(string.Format("Count: {0}", this.pawnsAlive.Count()));
			int num = 0;
			Dictionary<HediffDef, int> dictionary = new Dictionary<HediffDef, int>();
			foreach (Pawn item in this.pawnsAlive)
			{
				HediffDef hediffDef = this.DefPreventingMothball(item);
				if (hediffDef == null)
				{
					num++;
				}
				else
				{
					if (!dictionary.ContainsKey(hediffDef))
					{
						dictionary[hediffDef] = 0;
					}
					Dictionary<HediffDef, int> dictionary2;
					HediffDef key;
					(dictionary2 = dictionary)[key = hediffDef] = dictionary2[key] + 1;
				}
			}
			stringBuilder.AppendLine(string.Format("Will be mothballed: {0}", num));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("Reasons to avoid mothballing:");
			foreach (KeyValuePair<HediffDef, int> item2 in from kvp in dictionary
			orderby kvp.Value descending
			select kvp)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", item2.Value, item2.Key));
			}
			Log.Message(stringBuilder.ToString());
		}
	}
}
