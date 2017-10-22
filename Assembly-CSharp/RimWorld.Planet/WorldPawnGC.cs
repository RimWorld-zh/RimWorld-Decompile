using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public void WorldPawnGCTick()
		{
			if (this.lastSuccessfulGCTick < Find.TickManager.TicksGame / 15000 * 15000)
			{
				if (this.activeGCProcess == null)
				{
					this.activeGCProcess = this.PawnGCPass().GetEnumerator();
					if (DebugViewSettings.logWorldPawnGC)
					{
						Log.Message(string.Format("World pawn GC started at rate {0}", this.currentGCRate));
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
							Log.Message("World pawn GC complete");
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
					Log.Message("World pawn GC cancelled");
				}
			}
		}

		private IEnumerable AccumulatePawnGCData(Dictionary<Pawn, string> keptPawns)
		{
			_003CAccumulatePawnGCData_003Ec__Iterator0 _003CAccumulatePawnGCData_003Ec__Iterator = (_003CAccumulatePawnGCData_003Ec__Iterator0)/*Error near IL_0032: stateMachine*/;
			foreach (Pawn item in PawnsFinder.AllMapsAndWorld_AliveOrDead)
			{
				string criticalPawnReason = this.GetCriticalPawnReason(item);
				if (!criticalPawnReason.NullOrEmpty())
				{
					keptPawns[item] = criticalPawnReason;
					if (this.logDotgraph != null)
					{
						this.logDotgraph.AppendLine(string.Format("{0} [label=<{0}<br/><font point-size=\"10\">{1}</font>> color=\"{2}\" shape=\"{3}\"];", WorldPawnGC.DotgraphIdentifier(item), criticalPawnReason, (item.relations == null || !item.relations.everSeenByPlayer) ? "grey" : "black", (!item.RaceProps.Humanlike) ? "box" : "oval"));
					}
				}
				else if (this.logDotgraph != null)
				{
					this.logDotgraph.AppendLine(string.Format("{0} [color=\"{1}\" shape=\"{2}\"];", WorldPawnGC.DotgraphIdentifier(item), (item.relations == null || !item.relations.everSeenByPlayer) ? "grey" : "black", (!item.RaceProps.Humanlike) ? "box" : "oval"));
				}
			}
			foreach (Pawn item2 in (from pawn in PawnsFinder.AllMapsAndWorld_Alive
			where _003CAccumulatePawnGCData_003Ec__Iterator._0024this.AllowedAsStoryPawn(pawn) && !keptPawns.ContainsKey(pawn)
			orderby pawn.records.StoryRelevance descending
			select pawn).Take(20))
			{
				keptPawns[item2] = "StoryRelevant";
			}
			Pawn[] array;
			Pawn[] criticalPawns = array = keptPawns.Keys.ToArray();
			int num = 0;
			if (num < array.Length)
			{
				Pawn pawn2 = array[num];
				this.AddAllRelationships(pawn2, keptPawns);
				yield return (object)null;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			Pawn[] array2 = criticalPawns;
			for (int i = 0; i < array2.Length; i++)
			{
				Pawn pawn3 = array2[i];
				this.AddAllMemories(pawn3, keptPawns);
			}
		}

		private Dictionary<Pawn, string> AccumulatePawnGCDataImmediate()
		{
			Dictionary<Pawn, string> dictionary = new Dictionary<Pawn, string>();
			IEnumerator enumerator = this.AccumulatePawnGCData(dictionary).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.Current;
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
			return dictionary;
		}

		public string PawnGCDebugResults()
		{
			Dictionary<Pawn, string> dictionary = this.AccumulatePawnGCDataImmediate();
			Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
			foreach (Pawn item in Find.WorldPawns.AllPawnsAlive)
			{
				string text = "Discarded";
				if (dictionary.ContainsKey(item))
				{
					text = dictionary[item];
				}
				if (!dictionary2.ContainsKey(text))
				{
					dictionary2[text] = 0;
				}
				Dictionary<string, int> dictionary3;
				string key;
				(dictionary3 = dictionary2)[key = text] = dictionary3[key] + 1;
			}
			return GenText.ToTextList(from kvp in dictionary2
			orderby kvp.Value descending
			select string.Format("{0}: {1}", kvp.Value, kvp.Key), "\n");
		}

		public IEnumerable PawnGCPass()
		{
			Dictionary<Pawn, string> keptPawns = new Dictionary<Pawn, string>();
			IEnumerator enumerator = this.AccumulatePawnGCData(keptPawns).GetEnumerator();
			try
			{
				if (enumerator.MoveNext())
				{
					object _ = enumerator.Current;
					yield return (object)null;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			finally
			{
				IDisposable disposable;
				IDisposable disposable2 = disposable = (enumerator as IDisposable);
				if (disposable != null)
				{
					disposable2.Dispose();
				}
			}
			Pawn[] array = Find.WorldPawns.AllPawnsAlive.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				Pawn pawn = array[i];
				if (!keptPawns.ContainsKey(pawn))
				{
					Find.WorldPawns.RemoveAndDiscardPawnViaGC(pawn);
				}
			}
			yield break;
			IL_0144:
			/*Error near IL_0145: Unexpected return in MoveNext()*/;
		}

		private string GetCriticalPawnReason(Pawn pawn)
		{
			string result;
			if (pawn.Discarded)
			{
				result = (string)null;
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
						result = "InPlayLog";
						goto IL_018e;
					}
					if (Find.BattleLog.AnyEntryConcerns(pawn))
					{
						result = "InBattleLog";
						goto IL_018e;
					}
				}
				result = ((Current.ProgramState != ProgramState.Playing || !Find.TaleManager.AnyActiveTaleConcerns(pawn)) ? null : "InActiveTale");
			}
			goto IL_018e;
			IL_018e:
			return result;
		}

		private bool AllowedAsStoryPawn(Pawn pawn)
		{
			return (byte)(pawn.RaceProps.Humanlike ? 1 : 0) != 0;
		}

		public void AddAllRelationships(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.relations != null)
			{
				foreach (Pawn relatedPawn in pawn.relations.RelatedPawns)
				{
					if (this.logDotgraph != null)
					{
						string text = string.Format("{0}->{1} [label=<{2}> color=\"purple\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(relatedPawn), pawn.GetRelations(relatedPawn).FirstOrDefault().ToString());
						if (!this.logDotgraphUniqueLinks.Contains(text))
						{
							this.logDotgraphUniqueLinks.Add(text);
							this.logDotgraph.AppendLine(text);
						}
					}
					if (!keptPawns.ContainsKey(relatedPawn))
					{
						keptPawns[relatedPawn] = "Relationship";
					}
				}
			}
		}

		public void AddAllMemories(Pawn pawn, Dictionary<Pawn, string> keptPawns)
		{
			if (pawn.needs != null && pawn.needs.mood != null && pawn.needs.mood.thoughts != null && pawn.needs.mood.thoughts.memories != null)
			{
				foreach (Thought_Memory memory in pawn.needs.mood.thoughts.memories.Memories)
				{
					if (memory.otherPawn != null)
					{
						if (this.logDotgraph != null)
						{
							string text = string.Format("{0}->{1} [label=<{2}> color=\"orange\"];", WorldPawnGC.DotgraphIdentifier(pawn), WorldPawnGC.DotgraphIdentifier(memory.otherPawn), memory.def);
							if (!this.logDotgraphUniqueLinks.Contains(text))
							{
								this.logDotgraphUniqueLinks.Add(text);
								this.logDotgraph.AppendLine(text);
							}
						}
						if (!keptPawns.ContainsKey(memory.otherPawn))
						{
							keptPawns[memory.otherPawn] = "Memory";
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
			Log.Message(stringBuilder.ToString());
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
					object current = enumerator.Current;
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
			float num = (float)(PerfLogger.Duration() * 1000.0);
			PerfLogger.Flush();
			Log.Message(string.Format("World pawn GC run complete in {0} ms", num));
		}

		public void LogDotgraph()
		{
			this.logDotgraph = new StringBuilder();
			this.logDotgraphUniqueLinks = new HashSet<string>();
			this.logDotgraph.AppendLine("digraph { rankdir=LR;");
			this.AccumulatePawnGCDataImmediate();
			this.logDotgraph.AppendLine("}");
			GUIUtility.systemCopyBuffer = this.logDotgraph.ToString();
			Log.Message("Dotgraph copied to clipboard");
			this.logDotgraph = null;
			this.logDotgraphUniqueLinks = null;
		}

		public static string DotgraphIdentifier(Pawn pawn)
		{
			return new string((from ch in pawn.NameStringShort
			where char.IsLetter(ch)
			select ch).ToArray()) + "_" + pawn.thingIDNumber.ToString();
		}
	}
}
