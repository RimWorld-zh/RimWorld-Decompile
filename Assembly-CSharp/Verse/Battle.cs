using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BBB RID: 3003
	public class Battle : IExposable, ILoadReferenceable
	{
		// Token: 0x04002C7C RID: 11388
		public const int TicksForBattleExit = 5000;

		// Token: 0x04002C7D RID: 11389
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04002C7E RID: 11390
		private string battleName = null;

		// Token: 0x04002C7F RID: 11391
		private Battle absorbedBy;

		// Token: 0x04002C80 RID: 11392
		private HashSet<Pawn> concerns = new HashSet<Pawn>();

		// Token: 0x04002C81 RID: 11393
		private int loadID;

		// Token: 0x04002C82 RID: 11394
		private int creationTimestamp;

		// Token: 0x17000A27 RID: 2599
		// (get) Token: 0x0600410B RID: 16651 RVA: 0x00225A2C File Offset: 0x00223E2C
		public int Importance
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x0600410C RID: 16652 RVA: 0x00225A4C File Offset: 0x00223E4C
		public int CreationTimestamp
		{
			get
			{
				return this.creationTimestamp;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x0600410D RID: 16653 RVA: 0x00225A68 File Offset: 0x00223E68
		public int LastEntryTimestamp
		{
			get
			{
				return (this.entries.Count <= 0) ? 0 : this.entries[this.entries.Count - 1].Timestamp;
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x0600410E RID: 16654 RVA: 0x00225AB4 File Offset: 0x00223EB4
		public Battle AbsorbedBy
		{
			get
			{
				return this.absorbedBy;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x0600410F RID: 16655 RVA: 0x00225AD0 File Offset: 0x00223ED0
		public List<LogEntry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x06004110 RID: 16656 RVA: 0x00225AEC File Offset: 0x00223EEC
		public static Battle Create()
		{
			return new Battle
			{
				loadID = Find.UniqueIDsManager.GetNextBattleID(),
				creationTimestamp = Find.TickManager.TicksGame
			};
		}

		// Token: 0x06004111 RID: 16657 RVA: 0x00225B28 File Offset: 0x00223F28
		public string GetName()
		{
			if (this.battleName.NullOrEmpty())
			{
				HashSet<Faction> hashSet = new HashSet<Faction>(from p in this.concerns
				select p.Faction);
				GrammarRequest request = default(GrammarRequest);
				if (this.concerns.Count == 1)
				{
					if (hashSet.Count((Faction f) => f != null) < 2)
					{
						request.Includes.Add(RulePackDefOf.Battle_Solo);
						request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null));
						goto IL_1D6;
					}
				}
				if (this.concerns.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_Duel);
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT1", this.concerns.First<Pawn>(), null));
					request.Rules.AddRange(GrammarUtility.RulesForPawn("PARTICIPANT2", this.concerns.Last<Pawn>(), null));
				}
				else if (hashSet.Count == 1)
				{
					request.Includes.Add(RulePackDefOf.Battle_Internal);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>()));
				}
				else if (hashSet.Count == 2)
				{
					request.Includes.Add(RulePackDefOf.Battle_War);
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION1", hashSet.First<Faction>()));
					request.Rules.AddRange(GrammarUtility.RulesForFaction("FACTION2", hashSet.Last<Faction>()));
				}
				else
				{
					request.Includes.Add(RulePackDefOf.Battle_Brawl);
				}
				IL_1D6:
				this.battleName = GrammarResolver.Resolve("r_battlename", request, null, false);
			}
			return this.battleName;
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x00225D2C File Offset: 0x0022412C
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			foreach (Thing thing in entry.GetConcerns())
			{
				if (thing is Pawn)
				{
					this.concerns.Add(thing as Pawn);
				}
			}
			this.battleName = null;
		}

		// Token: 0x06004113 RID: 16659 RVA: 0x00225DB4 File Offset: 0x002241B4
		public void Absorb(Battle battle)
		{
			this.creationTimestamp = Mathf.Min(this.creationTimestamp, battle.creationTimestamp);
			this.entries.AddRange(battle.entries);
			this.concerns.AddRange(battle.concerns);
			this.entries = (from e in this.entries
			orderby e.Age
			select e).ToList<LogEntry>();
			battle.entries.Clear();
			battle.concerns.Clear();
			battle.absorbedBy = this;
			this.battleName = null;
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x00225E54 File Offset: 0x00224254
		public bool Concerns(Pawn pawn)
		{
			return this.concerns.Contains(pawn);
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x00225E78 File Offset: 0x00224278
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			if (this.concerns.Contains(p))
			{
				for (int i = this.entries.Count - 1; i >= 0; i--)
				{
					if (this.entries[i].Concerns(p))
					{
						if (!silentlyRemoveReferences)
						{
							Log.Warning(string.Concat(new object[]
							{
								"Discarding pawn ",
								p,
								", but he is referenced by a battle log entry ",
								this.entries[i],
								"."
							}), false);
						}
						this.entries.RemoveAt(i);
					}
				}
				this.concerns.Remove(p);
			}
		}

		// Token: 0x06004116 RID: 16662 RVA: 0x00225F30 File Offset: 0x00224330
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.loadID, "loadID", 0, false);
			Scribe_Values.Look<int>(ref this.creationTimestamp, "creationTimestamp", 0, false);
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, new object[0]);
			Scribe_References.Look<Battle>(ref this.absorbedBy, "absorbedBy", false);
			Scribe_Values.Look<string>(ref this.battleName, "battleName", null, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				foreach (Pawn item in this.entries.SelectMany((LogEntry e) => e.GetConcerns()).OfType<Pawn>())
				{
					this.concerns.Add(item);
				}
			}
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x00226028 File Offset: 0x00224428
		public string GetUniqueLoadID()
		{
			return "Battle_" + this.loadID;
		}
	}
}
