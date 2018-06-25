using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BCD RID: 3021
	public class PlayLog : IExposable
	{
		// Token: 0x04002CF5 RID: 11509
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04002CF6 RID: 11510
		private const int Capacity = 150;

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x060041D4 RID: 16852 RVA: 0x0022ABCC File Offset: 0x00228FCC
		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x17000A48 RID: 2632
		// (get) Token: 0x060041D5 RID: 16853 RVA: 0x0022ABE8 File Offset: 0x00228FE8
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

		// Token: 0x060041D6 RID: 16854 RVA: 0x0022AC25 File Offset: 0x00229025
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		// Token: 0x060041D7 RID: 16855 RVA: 0x0022AC3B File Offset: 0x0022903B
		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		// Token: 0x060041D8 RID: 16856 RVA: 0x0022AC78 File Offset: 0x00229078
		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, new object[0]);
		}

		// Token: 0x060041D9 RID: 16857 RVA: 0x0022AC94 File Offset: 0x00229094
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

		// Token: 0x060041DA RID: 16858 RVA: 0x0022AD2F File Offset: 0x0022912F
		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		// Token: 0x060041DB RID: 16859 RVA: 0x0022AD40 File Offset: 0x00229140
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
