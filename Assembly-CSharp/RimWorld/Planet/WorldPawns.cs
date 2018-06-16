using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Profiling;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000630 RID: 1584
	public class WorldPawns : IExposable
	{
		// Token: 0x170004DE RID: 1246
		// (get) Token: 0x06002077 RID: 8311 RVA: 0x00115E18 File Offset: 0x00114218
		public IEnumerable<Pawn> AllPawnsAliveOrDead
		{
			get
			{
				return this.AllPawnsAlive.Concat(this.AllPawnsDead);
			}
		}

		// Token: 0x170004DF RID: 1247
		// (get) Token: 0x06002078 RID: 8312 RVA: 0x00115E40 File Offset: 0x00114240
		public IEnumerable<Pawn> AllPawnsAlive
		{
			get
			{
				return this.pawnsAlive.Concat(this.pawnsMothballed);
			}
		}

		// Token: 0x170004E0 RID: 1248
		// (get) Token: 0x06002079 RID: 8313 RVA: 0x00115E68 File Offset: 0x00114268
		public IEnumerable<Pawn> AllPawnsDead
		{
			get
			{
				return this.pawnsDead;
			}
		}

		// Token: 0x170004E1 RID: 1249
		// (get) Token: 0x0600207A RID: 8314 RVA: 0x00115E84 File Offset: 0x00114284
		public HashSet<Pawn> ForcefullyKeptPawns
		{
			get
			{
				return this.pawnsForcefullyKeptAsWorldPawns;
			}
		}

		// Token: 0x0600207B RID: 8315 RVA: 0x00115EA0 File Offset: 0x001142A0
		public void WorldPawnsTick()
		{
			WorldPawns.tmpPawnsToTick.Clear();
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsAlive);
			for (int i = 0; i < WorldPawns.tmpPawnsToTick.Count; i++)
			{
				try
				{
					WorldPawns.tmpPawnsToTick[i].Tick();
				}
				catch (Exception arg)
				{
					Log.ErrorOnce("Exception ticking world pawn: " + arg, WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 1148571423, false);
				}
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
			foreach (Pawn pawn in this.pawnsDead)
			{
				if (pawn == null)
				{
					Log.ErrorOnce("Dead null world pawn detected, discarding.", 94424128, false);
					WorldPawns.tmpPawnsToRemove.Add(pawn);
				}
				else if (pawn.Discarded)
				{
					Log.Error("World pawn " + pawn + " has been discarded while still being a world pawn. This should never happen, because discard destroy mode means that the pawn is no longer managed by anything. Pawn should have been removed from the world first.", false);
					WorldPawns.tmpPawnsToRemove.Add(pawn);
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
			catch (Exception arg2)
			{
				Log.Error("Error in WorldPawnGCTick(): " + arg2, false);
			}
			Profiler.EndSample();
		}

		// Token: 0x0600207C RID: 8316 RVA: 0x001160BC File Offset: 0x001144BC
		public void ExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnsForcefullyKeptAsWorldPawns, true, "pawnsForcefullyKeptAsWorldPawns", LookMode.Reference);
			Scribe_Collections.Look<Pawn>(ref this.pawnsAlive, "pawnsAlive", LookMode.Deep);
			Scribe_Collections.Look<Pawn>(ref this.pawnsMothballed, "pawnsMothballed", LookMode.Deep);
			Scribe_Collections.Look<Pawn>(ref this.pawnsDead, true, "pawnsDead", LookMode.Deep);
			Scribe_Deep.Look<WorldPawnGC>(ref this.gc, "gc", new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.WorldPawnPostLoadInit(this, ref this.pawnsMothballed);
				if (this.pawnsForcefullyKeptAsWorldPawns.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsForcefullyKeptAsWorldPawns were null after loading.", false);
				}
				if (this.pawnsAlive.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsAlive were null after loading.", false);
				}
				if (this.pawnsMothballed.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsMothballed were null after loading.", false);
				}
				if (this.pawnsDead.RemoveWhere((Pawn x) => x == null) != 0)
				{
					Log.Error("Some pawnsDead were null after loading.", false);
				}
			}
		}

		// Token: 0x0600207D RID: 8317 RVA: 0x00116220 File Offset: 0x00114620
		public bool Contains(Pawn p)
		{
			return this.pawnsAlive.Contains(p) || this.pawnsMothballed.Contains(p) || this.pawnsDead.Contains(p);
		}

		// Token: 0x0600207E RID: 8318 RVA: 0x00116268 File Offset: 0x00114668
		public void PassToWorld(Pawn pawn, PawnDiscardDecideMode discardMode = PawnDiscardDecideMode.Decide)
		{
			if (pawn.Spawned)
			{
				Log.Error("Tried to call PassToWorld with spawned pawn: " + pawn + ". Despawn him first.", false);
			}
			else if (this.Contains(pawn))
			{
				Log.Error("Tried to pass pawn " + pawn + " to world, but it's already here.", false);
			}
			else
			{
				if (discardMode == PawnDiscardDecideMode.KeepForever && pawn.Discarded)
				{
					Log.Error("Tried to pass a discarded pawn " + pawn + " to world with discardMode=Keep. Discarded pawns should never be stored in WorldPawns.", false);
					discardMode = PawnDiscardDecideMode.Decide;
				}
				if (PawnComponentsUtility.HasSpawnedComponents(pawn))
				{
					PawnComponentsUtility.RemoveComponentsOnDespawned(pawn);
				}
				if (discardMode != PawnDiscardDecideMode.Decide)
				{
					if (discardMode != PawnDiscardDecideMode.KeepForever)
					{
						if (discardMode == PawnDiscardDecideMode.Discard)
						{
							this.DiscardPawn(pawn, false);
						}
					}
					else
					{
						this.pawnsForcefullyKeptAsWorldPawns.Add(pawn);
						this.AddPawn(pawn);
					}
				}
				else
				{
					this.AddPawn(pawn);
				}
			}
		}

		// Token: 0x0600207F RID: 8319 RVA: 0x00116350 File Offset: 0x00114750
		public void RemovePawn(Pawn p)
		{
			if (!this.Contains(p))
			{
				Log.Error(string.Concat(new object[]
				{
					"Tried to remove pawn ",
					p,
					" from ",
					base.GetType(),
					", but it's not here."
				}), false);
			}
			this.gc.CancelGCPass();
			if (this.pawnsMothballed.Contains(p))
			{
				int num = Find.TickManager.TicksGame % 15000;
				if (num != 0)
				{
					try
					{
						p.TickMothballed(Find.TickManager.TicksGame % 15000);
					}
					catch (Exception arg)
					{
						Log.Error("Exception ticking mothballed world pawn (just before removing): " + arg, false);
					}
				}
			}
			this.pawnsAlive.Remove(p);
			this.pawnsMothballed.Remove(p);
			this.pawnsDead.Remove(p);
			this.pawnsForcefullyKeptAsWorldPawns.Remove(p);
		}

		// Token: 0x06002080 RID: 8320 RVA: 0x00116454 File Offset: 0x00114854
		public void RemoveAndDiscardPawnViaGC(Pawn p)
		{
			this.RemovePawn(p);
			this.DiscardPawn(p, true);
		}

		// Token: 0x06002081 RID: 8321 RVA: 0x00116468 File Offset: 0x00114868
		public WorldPawnSituation GetSituation(Pawn p)
		{
			WorldPawnSituation result;
			if (!this.Contains(p))
			{
				result = WorldPawnSituation.None;
			}
			else if (p.Dead || p.Destroyed)
			{
				result = WorldPawnSituation.Dead;
			}
			else if (PawnUtility.IsFactionLeader(p))
			{
				result = WorldPawnSituation.FactionLeader;
			}
			else if (PawnUtility.IsKidnappedPawn(p))
			{
				result = WorldPawnSituation.Kidnapped;
			}
			else if (p.IsCaravanMember())
			{
				result = WorldPawnSituation.CaravanMember;
			}
			else if (PawnUtility.IsTravelingInTransportPodWorldObject(p))
			{
				result = WorldPawnSituation.InTravelingTransportPod;
			}
			else if (PawnUtility.ForSaleBySettlement(p))
			{
				result = WorldPawnSituation.ForSaleBySettlement;
			}
			else
			{
				result = WorldPawnSituation.Free;
			}
			return result;
		}

		// Token: 0x06002082 RID: 8322 RVA: 0x00116508 File Offset: 0x00114908
		public IEnumerable<Pawn> GetPawnsBySituation(WorldPawnSituation situation)
		{
			return from x in this.AllPawnsAliveOrDead
			where this.GetSituation(x) == situation
			select x;
		}

		// Token: 0x06002083 RID: 8323 RVA: 0x00116548 File Offset: 0x00114948
		public int GetPawnsBySituationCount(WorldPawnSituation situation)
		{
			int num = 0;
			foreach (Pawn p in this.pawnsAlive)
			{
				if (this.GetSituation(p) == situation)
				{
					num++;
				}
			}
			foreach (Pawn p2 in this.pawnsDead)
			{
				if (this.GetSituation(p2) == situation)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06002084 RID: 8324 RVA: 0x00116618 File Offset: 0x00114A18
		private bool ShouldAutoTendTo(Pawn pawn)
		{
			return !pawn.Dead && !pawn.Destroyed && pawn.IsHashIntervalTick(7500) && !pawn.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(pawn);
		}

		// Token: 0x06002085 RID: 8325 RVA: 0x0011666C File Offset: 0x00114A6C
		public bool IsBeingDiscarded(Pawn p)
		{
			return this.pawnsBeingDiscarded.Contains(p);
		}

		// Token: 0x06002086 RID: 8326 RVA: 0x00116690 File Offset: 0x00114A90
		public void Notify_PawnDestroyed(Pawn p)
		{
			if (this.pawnsAlive.Contains(p) || this.pawnsMothballed.Contains(p))
			{
				this.pawnsAlive.Remove(p);
				this.pawnsMothballed.Remove(p);
				this.pawnsDead.Add(p);
			}
		}

		// Token: 0x06002087 RID: 8327 RVA: 0x001166EC File Offset: 0x00114AEC
		private bool ShouldMothball(Pawn p)
		{
			return this.DefPreventingMothball(p) == null && !p.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(p);
		}

		// Token: 0x06002088 RID: 8328 RVA: 0x00116724 File Offset: 0x00114B24
		private HediffDef DefPreventingMothball(Pawn p)
		{
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (!hediffs[i].def.AlwaysAllowMothball)
				{
					if (!hediffs[i].IsPermanent())
					{
						return hediffs[i].def;
					}
				}
			}
			return null;
		}

		// Token: 0x06002089 RID: 8329 RVA: 0x001167A8 File Offset: 0x00114BA8
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

		// Token: 0x0600208A RID: 8330 RVA: 0x001167FC File Offset: 0x00114BFC
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

		// Token: 0x0600208B RID: 8331 RVA: 0x00116860 File Offset: 0x00114C60
		private void DoMothballProcessing()
		{
			WorldPawns.tmpPawnsToTick.AddRange(this.pawnsMothballed);
			for (int i = 0; i < WorldPawns.tmpPawnsToTick.Count; i++)
			{
				try
				{
					WorldPawns.tmpPawnsToTick[i].TickMothballed(15000);
				}
				catch (Exception arg)
				{
					Log.ErrorOnce("Exception ticking mothballed world pawn: " + arg, WorldPawns.tmpPawnsToTick[i].thingIDNumber ^ 1535437893, false);
				}
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

		// Token: 0x0600208C RID: 8332 RVA: 0x0011696C File Offset: 0x00114D6C
		public void DebugRunMothballProcessing()
		{
			this.DoMothballProcessing();
			Log.Message(string.Format("World pawn mothball run complete", new object[0]), false);
		}

		// Token: 0x0600208D RID: 8333 RVA: 0x0011698B File Offset: 0x00114D8B
		public void UnpinAllForcefullyKeptPawns()
		{
			this.pawnsForcefullyKeptAsWorldPawns.Clear();
		}

		// Token: 0x0600208E RID: 8334 RVA: 0x0011699C File Offset: 0x00114D9C
		public void LogWorldPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= World Pawns =======");
			stringBuilder.AppendLine("Count: " + this.AllPawnsAliveOrDead.Count<Pawn>());
			stringBuilder.AppendLine(string.Format("(Live: {0} - Mothballed: {1} - Dead: {2}; {3} forcefully kept)", new object[]
			{
				this.pawnsAlive.Count,
				this.pawnsMothballed.Count,
				this.pawnsDead.Count,
				this.pawnsForcefullyKeptAsWorldPawns.Count
			}));
			foreach (WorldPawnSituation worldPawnSituation in (WorldPawnSituation[])Enum.GetValues(typeof(WorldPawnSituation)))
			{
				if (worldPawnSituation != WorldPawnSituation.None)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("== " + worldPawnSituation + " ==");
					foreach (Pawn pawn in from x in this.GetPawnsBySituation(worldPawnSituation)
					orderby (x.Faction != null) ? x.Faction.loadID : -1
					select x)
					{
						string text = (pawn.Name == null) ? pawn.LabelCap : pawn.Name.ToStringFull;
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							text,
							", ",
							pawn.KindLabel,
							", ",
							pawn.Faction
						}));
					}
				}
			}
			stringBuilder.AppendLine("===========================");
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0600208F RID: 8335 RVA: 0x00116B88 File Offset: 0x00114F88
		public void LogWorldPawnMothballPrevention()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= World Pawns Mothball Prevention =======");
			stringBuilder.AppendLine(string.Format("Count: {0}", this.pawnsAlive.Count<Pawn>()));
			int num = 0;
			Dictionary<HediffDef, int> dictionary = new Dictionary<HediffDef, int>();
			foreach (Pawn p in this.pawnsAlive)
			{
				HediffDef hediffDef = this.DefPreventingMothball(p);
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
			foreach (KeyValuePair<HediffDef, int> keyValuePair in from kvp in dictionary
			orderby kvp.Value descending
			select kvp)
			{
				stringBuilder.AppendLine(string.Format("{0}: {1}", keyValuePair.Value, keyValuePair.Key));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x040012A8 RID: 4776
		private HashSet<Pawn> pawnsAlive = new HashSet<Pawn>();

		// Token: 0x040012A9 RID: 4777
		private HashSet<Pawn> pawnsMothballed = new HashSet<Pawn>();

		// Token: 0x040012AA RID: 4778
		private HashSet<Pawn> pawnsDead = new HashSet<Pawn>();

		// Token: 0x040012AB RID: 4779
		private HashSet<Pawn> pawnsForcefullyKeptAsWorldPawns = new HashSet<Pawn>();

		// Token: 0x040012AC RID: 4780
		public WorldPawnGC gc = new WorldPawnGC();

		// Token: 0x040012AD RID: 4781
		private Stack<Pawn> pawnsBeingDiscarded = new Stack<Pawn>();

		// Token: 0x040012AE RID: 4782
		private const float PctOfHumanlikesAlwaysKept = 0.1f;

		// Token: 0x040012AF RID: 4783
		private const float PctOfUnnamedColonyAnimalsAlwaysKept = 0.05f;

		// Token: 0x040012B0 RID: 4784
		private const int TendIntervalTicks = 7500;

		// Token: 0x040012B1 RID: 4785
		private const int MothballUpdateInterval = 15000;

		// Token: 0x040012B2 RID: 4786
		private static List<Pawn> tmpPawnsToTick = new List<Pawn>();

		// Token: 0x040012B3 RID: 4787
		private static List<Pawn> tmpPawnsToRemove = new List<Pawn>();
	}
}
