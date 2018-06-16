using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000BCF RID: 3023
	public class PlayLog : IExposable
	{
		// Token: 0x17000A46 RID: 2630
		// (get) Token: 0x060041CD RID: 16845 RVA: 0x0022A3A4 File Offset: 0x002287A4
		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		// Token: 0x17000A47 RID: 2631
		// (get) Token: 0x060041CE RID: 16846 RVA: 0x0022A3C0 File Offset: 0x002287C0
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

		// Token: 0x060041CF RID: 16847 RVA: 0x0022A3FD File Offset: 0x002287FD
		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		// Token: 0x060041D0 RID: 16848 RVA: 0x0022A413 File Offset: 0x00228813
		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		// Token: 0x060041D1 RID: 16849 RVA: 0x0022A450 File Offset: 0x00228850
		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, new object[0]);
		}

		// Token: 0x060041D2 RID: 16850 RVA: 0x0022A46C File Offset: 0x0022886C
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

		// Token: 0x060041D3 RID: 16851 RVA: 0x0022A507 File Offset: 0x00228907
		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		// Token: 0x060041D4 RID: 16852 RVA: 0x0022A518 File Offset: 0x00228918
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

		// Token: 0x04002CF0 RID: 11504
		private List<LogEntry> entries = new List<LogEntry>();

		// Token: 0x04002CF1 RID: 11505
		private const int Capacity = 150;
	}
}
