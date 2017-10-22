using System.Collections.Generic;

namespace Verse
{
	public class PlayLog : IExposable
	{
		private List<LogEntry> entries = new List<LogEntry>();

		private const int Capacity = 150;

		public List<LogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		public void Add(LogEntry entry)
		{
			this.entries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		private void ReduceToCapacity()
		{
			while (this.entries.Count > 150)
			{
				this.RemoveEntry(this.entries[this.entries.Count - 1]);
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.entries, "entries", LookMode.Deep, new object[0]);
		}

		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int num = this.entries.Count - 1; num >= 0; num--)
			{
				if (this.entries[num].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						Log.Warning("Discarding pawn " + p + ", but he is referenced by a play log entry " + this.entries[num] + ".");
					}
					this.RemoveEntry(this.entries[num]);
				}
			}
		}

		private void RemoveEntry(LogEntry entry)
		{
			this.entries.Remove(entry);
		}

		public bool AnyEntryConcerns(Pawn p)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.entries.Count)
				{
					if (this.entries[num].Concerns(p))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				result = false;
				break;
			}
			return result;
		}
	}
}
