using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BCB RID: 3019
	public class PlayLog : IExposable
	{
		// Token: 0x04002CF5 RID: 11509
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04002CF6 RID: 11510
		private const int Capacity = 150;

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x060041D1 RID: 16849 RVA: 0x0022AAF0 File Offset: 0x00228EF0
		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x17000A49 RID: 2633
		// (get) Token: 0x060041D2 RID: 16850 RVA: 0x0022AB0C File Offset: 0x00228F0C
		public int LastTick
		{
			get
			{
				int result;
				if (this.entries.Count == 0)
				{
					result = 0;
				}
				else
				{
					result = this.entries[0].Tick;
				}
				return result;
			}
		}

		// Token: 0x060041D3 RID: 16851 RVA: 0x0022AB49 File Offset: 0x00228F49
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x0022AB5F File Offset: 0x00228F5F
		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		// Token: 0x060041D5 RID: 16853 RVA: 0x0022AB9C File Offset: 0x00228F9C
		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, new object[0]);
		}

		// Token: 0x060041D6 RID: 16854 RVA: 0x0022ABB8 File Offset: 0x00228FB8
		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
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
							", but he is referenced by a play log entry ",
							this.entries[i],
							"."
						}), false);
					}
					this.RemoveEntry(this.entries[i]);
				}
			}
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x0022AC53 File Offset: 0x00229053
		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x0022AC64 File Offset: 0x00229064
		public bool AnyEntryConcerns(Pawn p)
		{
			for (int i = 0; i < this.entries.Count; i++)
			{
				if (this.entries[i].Concerns(p))
				{
					return true;
				}
			}
			return false;
		}
	}
}
