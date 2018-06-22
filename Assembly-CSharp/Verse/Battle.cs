using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x02000BB8 RID: 3000
	public class Battle : IExposable, ILoadReferenceable
	{
		// Token: 0x17000A28 RID: 2600
		// (get) Token: 0x06004108 RID: 16648 RVA: 0x00225670 File Offset: 0x00223A70
		public int Importance
		{
			get
			{
				return this.entries.Count;
			}
		}

		// Token: 0x17000A29 RID: 2601
		// (get) Token: 0x06004109 RID: 16649 RVA: 0x00225690 File Offset: 0x00223A90
		public int CreationTimestamp
		{
			get
			{
				return this.creationTimestamp;
			}
		}

		// Token: 0x17000A2A RID: 2602
		// (get) Token: 0x0600410A RID: 16650 RVA: 0x002256AC File Offset: 0x00223AAC
		public int LastEntryTimestamp
		{
			get
			{
				return (this.entries.Count <= 0) ? 0 : this.entries[this.entries.Count - 1].Timestamp;
			}
		}

		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x0600410B RID: 16651 RVA: 0x002256F8 File Offset: 0x00223AF8
		public Battle AbsorbedBy
		{
			get
			{
				return this.absorbedBy;
			}
		}

		// Token: 0x17000A2C RID: 2604
		// (get) Token: 0x0600410C RID: 16652 RVA: 0x00225714 File Offset: 0x00223B14
		public List<LogEntry> Entries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x00225730 File Offset: 0x00223B30
		public static Battle Create()
		{
			return new Battle
			{
				loadID = Find.UniqueIDsManager.GetNextBattleID(),
				creationTimestamp = Find.TickManager.TicksGame
			};
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x0022576C File Offset: 0x00223B6C
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

		// Token: 0x0600410F RID: 16655 RVA: 0x00225970 File Offset: 0x00223D70
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

		// Token: 0x06004110 RID: 16656 RVA: 0x002259F8 File Offset: 0x00223DF8
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

		// Token: 0x06004111 RID: 16657 RVA: 0x00225A98 File Offset: 0x00223E98
		public bool Concerns(Pawn pawn)
		{
			return this.concerns.Contains(pawn);
		}

		// Token: 0x06004112 RID: 16658 RVA: 0x00225ABC File Offset: 0x00223EBC
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

		// Token: 0x06004113 RID: 16659 RVA: 0x00225B74 File Offset: 0x00223F74
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

		// Token: 0x06004114 RID: 16660 RVA: 0x00225C6C File Offset: 0x0022406C
		public string GetUniqueLoadID()
		{
			return "Battle_" + this.loadID;
		}

		// Token: 0x04002C75 RID: 11381
		public const int TicksForBattleExit = 5000;

		// Token: 0x04002C76 RID: 11382
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04002C77 RID: 11383
		private string battleName = null;

		// Token: 0x04002C78 RID: 11384
		private Battle absorbedBy;

		// Token: 0x04002C79 RID: 11385
		private HashSet<Pawn> concerns = new HashSet<Pawn>();

		// Token: 0x04002C7A RID: 11386
		private int loadID;

		// Token: 0x04002C7B RID: 11387
		private int creationTimestamp;
	}
}
