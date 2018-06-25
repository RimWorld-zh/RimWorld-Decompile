using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldPawnGC : IExposable
	{
		private int lastSuccessfulGCTick = 0;

		private int currentGCRate = 1;

		private const float PctOfHumanlikesAlwaysKept = 0.1f;

		private const float PctOfUnnamedColonyAnimalsAlwaysKept = 0.05f;

		private const int AdditionalStoryRelevantPawns = 20;

		private const int GCUpdateInterval = 15000;

		private IEnumerator activeGCProcess = null;

		private StringBuilder logDotgraph = null;

		private HashSet<string> logDotgraphUniqueLinks = null;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, int>, int> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<KeyValuePair<string, int>, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<char, bool> <>f__am$cache2;

		public WorldPawnGC()
		{
		}

		public void WorldPawnGCTick()
		{
			if (this.lastSuccessfulGCTick < Find.TickManager.TicksGame / 15000 * 15000)
			{
				if (this.activeGCProcess == null)
				{
					this.activeGCProcess = this.PawnGCPass().GetEnumerator();
					if (DebugViewSettings.logWorldPawnGC)
					{
						Log.Message(string.Format("World pawn GC started at rate {0}", this.currentGCRate), false);
					}
				}
				if (this.activeGCProcess != null)
				{
					bool flag = false;
					int num = 0;
					while (num < this.currentGCRate && !flag)
					{
						flag = !this.activeGCProcess.MoveNext();
						num++;
					}
					if (flag)
					{
						this.lastSuccessfulGCTick = Find.TickManager.TicksGame;
						this.currentGCRate = 1;
						this.activeGCProcess = null;
						if (DebugViewSettings.logWorldPawnGC)
						{
							Log.Message("World pawn GC complete", false);
						}
					}
				}
			}
		}

		public void CancelGCPass()
		{
			if (this.activeGCProcess != null)
			{
				this.activeGCProcess = null;
				this.currentGCRate = Mathf.Min(this.currentGCRate * 2, 16777216);
				if (DebugViewSettings.logWorldPawnGC)
				{
					Log.Message("World pawn GC cancelled", false);
				}
			}
		}

		private IEnumerable AccumulatePawnGCData(Dictionary<Pawn, string> keptPawns)
		{
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				string criticalPawnReason = this.GetCriticalPawnReason(pawn2);
				if (!criticalPawnReason.NullOrEmpty())
				{
					keptPawns[pawn2] = criticalPawnReason;
					if (this.logDotgraph != null)
					{
						this.logDotgraph.AppendLine(string.Format("{0} [label=<{0}<br/><font point-size=\"10\">{1}</font>> color=\"{2}\" shape=\"{3}\"];", new object[]
						{
							WorldPawnGC.DotgraphIdentifier(pawn2),
							criticalPawnReason,
							(pawn2.relations == null || !pawn2.relations.everSeenByPlayer) ? "grey" : "black",
							(!pawn2.RaceProps.Humanlike) ? "box" : "oval"
						}));
					}
				}
				else if (this.logDotgraph != null)
				{
					this.logDotgraph.AppendLine(string.Format("{0} [color=\"{1}\" shape=\"{2}\"];", WorldPawnGC.DotgraphIdentifier(pawn2), (pawn2.relations == null || !pawn2.relations.everSeenByPlayer) ? "grey" : "black", (!pawn2.RaceProps.Humanlike) ? "box" : "oval"));
				}
			}
			foreach (Pawn key in (from pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where this.AllowedAsStoryPawn(pawn) && !keptPawns.ContainsKey(pawn)
			orderby pawn.records.StoryRelevance descending
			select pawn).Take(20))
			{
				keptPawns[key] = "StoryRelevant";
			}
			Pawn[] criticalPawns = keptPawns.Keys.ToArray<Pawn>();
			foreach (Pawn pawn4 in criticalPawns)
			{
				this.AddAllRelationships(pawn4, keptPawns);
				yield return null;
			}
			foreach (Pawn pawn3 in criticalPawns)
			{
				this.AddAllMemories(pawn3, keptPawns);
			}
			yield break;
		}

		private Dictionary<Pawn, string> AccumulatePawnGCDataImmediate()
		{
			Dictionary<Pawn, string> dictionary = new Dictionary<Pawn, string>();
			this.AccumulatePawnGCData(dictionary).ExecuteEnumerable();
			return dictionary;
		}

		public string PawnGCDebugResults()
		{
			Dictionary<Pawn, string> dictionary = this.AccumulatePawnGCDataImmediate();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			foreach (Pawn key in Find.WorldPawns.AllPawnsAlive)
			{
				string text = "Discarded";
				if (dictionary.ContainsKey(key))
				{
					text = dictionary[key];
				}
				if (!dictionary2.ContainsKey(text))
				{
					dictionary2[text] = 0;
				}
				Dictionary<string, int> dictionary3;
				string key2;
				(dictionary3 = dictionary2)[key2 = text] = dictionary3[key2] + 1;
			}
			return GenText.ToTextList(from kvp in dictionary2
			orderby kvp.Value descending
			select string.Format("{0}: {1}", kvp.Value, kvp.Key), "\n");
		}

		public IEnumerable PawnGCPass()
		{
			Dictionary<Pawn, string> keptPawns = new Dictionary<Pawn, string>();
			Pawn[] worldPawnsSnapshot = Find.WorldPawns.AllPawnsAliveOrDead.ToArray<Pawn>();
			IEnumerator enumerator = this.AccumulatePawnGCData(keptPawns).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object _ = enumerator.Current;
					yield return null;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			foreach (Pawn pawn in worldPawnsSnapshot)
			{
				if (pawn.IsWorldPawn() && !keptPawns.ContainsKey(pawn))
				{
					Find.WorldPawns.RemoveAndDiscardPawnViaGC(pawn);
				}
			}
			yield break;
		}

		private string GetCriticalPawnReason(Pawn pawn)
		{
			string result;
			if (pawn.Discarded)
			{
				result = null;
			}
			else if (PawnUtility.EverBeenColonistOrTameAnimal(pawn) && pawn.RaceProps.Humanlike)
			{
				result = "Colonist";
			}
			else if (PawnGenerator.IsBeingGenerated(pawn))
			{
				result = "Generating";
			}
			else if (PawnUtility.IsFactionLeader(pawn))
			{
				result = "FactionLeader";
			}
			else if (PawnUtility.IsKidnappedPawn(pawn))
			{
				result = "Kidnapped";
			}
			else if (pawn.IsCaravanMember())
			{
				result = "CaravanMember";
			}
			else if (PawnUtility.IsTravelingInTransportPodWorldObject(pawn))
			{
				result = "TransportPod";
			}
			else if (PawnUtility.ForSaleBySettlement(pawn))
			{
				result = "ForSale";
			}
			else if (Find.WorldPawns.ForcefullyKeptPawns.Contains(pawn))
			{
				result = "ForceKept";
			}
			else if (pawn.SpawnedOrAnyParentSpawned)
			{
				result = "Spawned";
			}
			else if (!pawn.Corpse.DestroyedOrNull())
			{
				result = "CorpseExists";
			}
			else
			{
				if (pawn.RaceProps.Humanlike && Current.ProgramState == ProgramState.Playing)
				{
					if (Find.PlayLog.AnyEntryConcerns(pawn))
					{
						return "InPlayLog";
					}
					if (Find.BattleLog.AnyEntryConcerns(pawn))
					{
						return "InBattleLog";
					}
				}
				if (Current.ProgramState == ProgramState.Playing && Find.TaleManager.AnyActiveTaleConcerns(pawn))
				{
					result = "InActiveTale";
				}
				else
				{
					result = null;
				}
			}
			return result;
		}

		private bool AllowedAsStoryPawn(Pawn pawn)
		{
			return pawn.RaceProps.Humanlike;
		}

		public void AddAllRelationships(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.relations != null)
			{
				foreach (Pawn pawn2 in pawn.relations.RelatedPawns)
				{
					if (this.logDotgraph != null)
					{
						string text = string.Format("{0}->{1} [label=<{2}> color=\"purple\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(pawn2), pawn.GetRelations(pawn2).FirstOrDefault<PawnRelationDef>().ToString());
						if (!this.logDotgraphUniqueLinks.Contains(text))
						{
							this.logDotgraphUniqueLinks.Add(text);
							this.logDotgraph.AppendLine(text);
						}
					}
					if (!keptPawns.ContainsKey(pawn2))
					{
						keptPawns[pawn2] = "Relationship";
					}
				}
			}
		}

		public void AddAllMemories(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null && pawn.needs.mood.thoughts.memories != null)
			{
				foreach (Thought_Memory thought_Memory in pawn.needs.mood.thoughts.memories.Memories)
				{
					if (thought_Memory.otherPawn != null)
					{
						if (this.logDotgraph != null)
						{
							string text = string.Format("{0}->{1} [label=<{2}> color=\"orange\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(thought_Memory.otherPawn), thought_Memory.def);
							if (!this.logDotgraphUniqueLinks.Contains(text))
							{
								this.logDotgraphUniqueLinks.Add(text);
								this.logDotgraph.AppendLine(text);
							}
						}
						if (!keptPawns.ContainsKey(thought_Memory.otherPawn))
						{
							keptPawns[thought_Memory.otherPawn] = "Memory";
						}
					}
				}
			}
		}

		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastSuccessfulGCTick, "lastSuccessfulGCTick", 0, false);
			Scribe_Values.Look<int>(ref this.currentGCRate, "nextGCRate", 1, false);
		}

		public void LogGC()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= GC =======");
			stringBuilder.AppendLine(this.PawnGCDebugResults());
			Log.Message(stringBuilder.ToString(), false);
		}

		public void RunGC()
		{
			this.CancelGCPass();
			PerfLogger.Reset();
			IEnumerator enumerator = this.PawnGCPass().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			float num = PerfLogger.Duration() * 1000f;
			PerfLogger.Flush();
			Log.Message(string.Format("World pawn GC run complete in {0} ms", num), false);
		}

		public void LogDotgraph()
		{
			this.logDotgraph = new StringBuilder();
			this.logDotgraphUniqueLinks = new HashSet<string>();
			this.logDotgraph.AppendLine("digraph { rankdir=LR;");
			this.AccumulatePawnGCDataImmediate();
			this.logDotgraph.AppendLine("}");
			GUIUtility.systemCopyBuffer = this.logDotgraph.ToString();
			Log.Message("Dotgraph copied to clipboard", false);
			this.logDotgraph = null;
			this.logDotgraphUniqueLinks = null;
		}

		public static string DotgraphIdentifier(Pawn pawn)
		{
			return new string((from ch in pawn.LabelShort
			where char.IsLetter(ch)
			select ch).ToArray<char>()) + "_" + pawn.thingIDNumber.ToString();
		}

		[CompilerGenerated]
		private static int <PawnGCDebugResults>m__0(KeyValuePair<string, int> kvp)
		{
			return kvp.Value;
		}

		[CompilerGenerated]
		private static string <PawnGCDebugResults>m__1(KeyValuePair<string, int> kvp)
		{
			return string.Format("{0}: {1}", kvp.Value, kvp.Key);
		}

		[CompilerGenerated]
		private static bool <DotgraphIdentifier>m__2(char ch)
		{
			return char.IsLetter(ch);
		}

		[CompilerGenerated]
		private sealed class <AccumulatePawnGCData>c__Iterator0 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal IEnumerator<Pawn> $locvar0;

			internal Dictionary<Pawn, string> keptPawns;

			internal IEnumerator<Pawn> $locvar1;

			internal Pawn[] <criticalPawns>__0;

			internal Pawn[] $locvar2;

			internal int $locvar3;

			internal Pawn <pawn>__1;

			internal Pawn[] $locvar4;

			internal int $locvar5;

			internal WorldPawnGC $this;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			private WorldPawnGC.<AccumulatePawnGCData>c__Iterator0.<AccumulatePawnGCData>c__AnonStorey2 $locvar6;

			private static Func<Pawn, float> <>f__am$cache0;

			[DebuggerHidden]
			public <AccumulatePawnGCData>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					enumerator = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Pawn pawn2 = enumerator.Current;
							string criticalPawnReason = base.GetCriticalPawnReason(pawn2);
							if (!criticalPawnReason.NullOrEmpty())
							{
								keptPawns[pawn2] = criticalPawnReason;
								if (this.logDotgraph != null)
								{
									this.logDotgraph.AppendLine(string.Format("{0} [label=<{0}<br/><font point-size=\"10\">{1}</font>> color=\"{2}\" shape=\"{3}\"];", new object[]
									{
										WorldPawnGC.DotgraphIdentifier(pawn2),
										criticalPawnReason,
										(pawn2.relations == null || !pawn2.relations.everSeenByPlayer) ? "grey" : "black",
										(!pawn2.RaceProps.Humanlike) ? "box" : "oval"
									}));
								}
							}
							else if (this.logDotgraph != null)
							{
								this.logDotgraph.AppendLine(string.Format("{0} [color=\"{1}\" shape=\"{2}\"];", WorldPawnGC.DotgraphIdentifier(pawn2), (pawn2.relations == null || !pawn2.relations.everSeenByPlayer) ? "grey" : "black", (!pawn2.RaceProps.Humanlike) ? "box" : "oval"));
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					enumerator2 = (from pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive
					where this.AllowedAsStoryPawn(pawn) && !keptPawns.ContainsKey(pawn)
					orderby pawn.records.StoryRelevance descending
					select pawn).Take(20).GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Pawn key = enumerator2.Current;
							keptPawns[key] = "StoryRelevant";
						}
					}
					finally
					{
						if (enumerator2 != null)
						{
							enumerator2.Dispose();
						}
					}
					criticalPawns = keptPawns.Keys.ToArray<Pawn>();
					array = criticalPawns;
					i = 0;
					break;
				case 1u:
					i++;
					break;
				default:
					return false;
				}
				if (i < array.Length)
				{
					pawn = array[i];
					base.AddAllRelationships(pawn, <AccumulatePawnGCData>c__AnonStorey.keptPawns);
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				array2 = criticalPawns;
				for (j = 0; j < array2.Length; j++)
				{
					Pawn pawn3 = array2[j];
					base.AddAllMemories(pawn3, <AccumulatePawnGCData>c__AnonStorey.keptPawns);
				}
				this.$PC = -1;
				return false;
			}

			object IEnumerator<object>.Current
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
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldPawnGC.<AccumulatePawnGCData>c__Iterator0 <AccumulatePawnGCData>c__Iterator = new WorldPawnGC.<AccumulatePawnGCData>c__Iterator0();
				<AccumulatePawnGCData>c__Iterator.$this = this;
				<AccumulatePawnGCData>c__Iterator.keptPawns = keptPawns;
				return <AccumulatePawnGCData>c__Iterator;
			}

			private static float <>m__0(Pawn pawn)
			{
				return pawn.records.StoryRelevance;
			}

			private sealed class <AccumulatePawnGCData>c__AnonStorey2
			{
				internal Dictionary<Pawn, string> keptPawns;

				internal WorldPawnGC.<AccumulatePawnGCData>c__Iterator0 <>f__ref$0;

				public <AccumulatePawnGCData>c__AnonStorey2()
				{
				}

				internal bool <>m__0(Pawn pawn)
				{
					return this.<>f__ref$0.$this.AllowedAsStoryPawn(pawn) && !this.keptPawns.ContainsKey(pawn);
				}
			}
		}

		[CompilerGenerated]
		private sealed class <PawnGCPass>c__Iterator1 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			internal Dictionary<Pawn, string> <keptPawns>__0;

			internal Pawn[] <worldPawnsSnapshot>__0;

			internal IEnumerator $locvar0;

			internal object <_>__1;

			internal IDisposable $locvar1;

			internal WorldPawnGC $this;

			internal object $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <PawnGCPass>c__Iterator1()
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
					keptPawns = new Dictionary<Pawn, string>();
					worldPawnsSnapshot = Find.WorldPawns.AllPawnsAliveOrDead.ToArray<Pawn>();
					enumerator = base.AccumulatePawnGCData(keptPawns).GetEnumerator();
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
					}
					if (enumerator.MoveNext())
					{
						_ = enumerator.Current;
						this.$current = null;
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
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				for (int i = 0; i < worldPawnsSnapshot.Length; i++)
				{
					Pawn pawn = worldPawnsSnapshot[i];
					if (pawn.IsWorldPawn() && !keptPawns.ContainsKey(pawn))
					{
						Find.WorldPawns.RemoveAndDiscardPawnViaGC(pawn);
					}
				}
				this.$PC = -1;
				return false;
			}

			object IEnumerator<object>.Current
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
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
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
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				WorldPawnGC.<PawnGCPass>c__Iterator1 <PawnGCPass>c__Iterator = new WorldPawnGC.<PawnGCPass>c__Iterator1();
				<PawnGCPass>c__Iterator.$this = this;
				return <PawnGCPass>c__Iterator;
			}
		}
	}
}
