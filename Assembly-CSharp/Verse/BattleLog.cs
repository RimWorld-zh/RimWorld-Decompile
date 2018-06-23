using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000BB9 RID: 3001
	public class BattleLog : IExposable
	{
		// Token: 0x04002C80 RID: 11392
		private List<Battle> battles = new List<Battle>();

		// Token: 0x04002C81 RID: 11393
		private const int BattleHistoryLength = 20;

		// Token: 0x04002C82 RID: 11394
		private HashSet<LogEntry> activeEntries = null;

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x0600411A RID: 16666 RVA: 0x00225D20 File Offset: 0x00224120
		public List<Battle> Battles
		{
			get
			{
				return this.battles;
			}
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x00225D3C File Offset: 0x0022413C
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

		// Token: 0x0600411C RID: 16668 RVA: 0x00225E94 File Offset: 0x00224294
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

		// Token: 0x0600411D RID: 16669 RVA: 0x00225F5B File Offset: 0x0022435B
		public void ExposeData()
		{
			Scribe_Collections.Look<Battle>(ref this.battles, "battles", LookMode.Deep, new object[0]);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.battles == null)
			{
				this.battles = new List<Battle>();
			}
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x00225F98 File Offset: 0x00224398
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

		// Token: 0x0600411F RID: 16671 RVA: 0x00225FEC File Offset: 0x002243EC
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

		// Token: 0x06004120 RID: 16672 RVA: 0x00226084 File Offset: 0x00224484
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int i = this.battles.Count - 1; i >= 0; i--)
			{
				this.battles[i].Notify_PawnDiscarded(p, silentlyRemoveReferences);
			}
		}
	}
}
