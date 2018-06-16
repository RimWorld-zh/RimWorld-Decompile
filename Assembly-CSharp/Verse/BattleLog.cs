using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BBD RID: 3005
	public class BattleLog : IExposable
	{
		// Token: 0x17000A2B RID: 2603
		// (get) Token: 0x06004116 RID: 16662 RVA: 0x00225578 File Offset: 0x00223978
		public List<Battle> Battles
		{
			get
			{
				return this.battles;
			}
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x00225594 File Offset: 0x00223994
		public void Add(LogEntry entry)
		{
			Battle battle = null;
			foreach (Thing thing in entry.GetConcerns())
			{
				Pawn pawn = (Pawn)thing;
				Battle battleActive = pawn.records.BattleActive;
				if (battle == null)
				{
					battle = battleActive;
				}
				else if (battleActive != null)
				{
					battle = ((battle.Importance <= battleActive.Importance) ? battleActive : battle);
				}
			}
			if (battle == null)
			{
				battle = Battle.Create();
				this.battles.Insert(0, battle);
			}
			foreach (Thing thing2 in entry.GetConcerns())
			{
				Pawn pawn2 = (Pawn)thing2;
				Battle battleActive2 = pawn2.records.BattleActive;
				if (battleActive2 != null && battleActive2 != battle)
				{
					battle.Absorb(battleActive2);
					this.battles.Remove(battleActive2);
				}
				pawn2.records.EnterBattle(battle);
			}
			battle.Add(entry);
			this.activeEntries = null;
			this.ReduceToCapacity();
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x002256EC File Offset: 0x00223AEC
		private void ReduceToCapacity()
		{
			int num = this.battles.Count((Battle btl) => btl.AbsorbedBy == null);
			while (num > 20 && this.battles[this.battles.Count - 1].LastEntryTimestamp + Mathf.Max(420000, 5000) < Find.TickManager.TicksGame)
			{
				if (this.battles[this.battles.Count - 1].AbsorbedBy == null)
				{
					num--;
				}
				this.battles.RemoveAt(this.battles.Count - 1);
				this.activeEntries = null;
			}
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x002257B3 File Offset: 0x00223BB3
		public void ExposeData()
		{
			Scribe_Collections.Look<Battle>(ref this.battles, "battles", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.battles == null)
			{
				this.battles = new List<Battle>();
			}
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x002257F0 File Offset: 0x00223BF0
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.battles.Count; i++)
			{
				if (this.battles[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x00225844 File Offset: 0x00223C44
		public bool IsEntryActive(LogEntry log)
		{
			if (this.activeEntries == null)
			{
				this.activeEntries = new HashSet<LogEntry>();
				for (int i = 0; i < this.battles.Count; i++)
				{
					List<LogEntry> entries = this.battles[i].Entries;
					for (int j = 0; j < entries.Count; j++)
					{
						this.activeEntries.Add(entries[j]);
					}
				}
			}
			return this.activeEntries.Contains(log);
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x002258DC File Offset: 0x00223CDC
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.battles.Count - 1; i >= 0; i--)
			{
				this.battles[i].Notify_PawnDiscarded(p, silentlyRemoveReferences);
			}
		}

		// Token: 0x04002C7B RID: 11387
		private List<Battle> battles = new List<Battle>();

		// Token: 0x04002C7C RID: 11388
		private const int BattleHistoryLength = 20;

		// Token: 0x04002C7D RID: 11389
		private HashSet<LogEntry> activeEntries = null;
	}
}
