using System.Collections.Generic;

namespace Verse
{
	public class PlayLog : IExposable
	{
		private const int Capacity = 150;

		private List<PlayLogEntry> entries = new List<PlayLogEntry>();

		public List<PlayLogEntry> AllEntries
		{
			get
			{
				return this.entries;
			}
		}

		public void Add(PlayLogEntry entry)
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
			Scribe_Collections.Look<PlayLogEntry>(ref this.entries, "entries", LookMode.Deep, new object[0]);
		}

		public void Notify_PawnDiscarded(Pawn p)
		{
			for (int num = this.entries.Count - 1; num >= 0; num--)
			{
				if (this.entries[num].Concerns(p))
				{
					Log.Warning("Discarding pawn " + p + ", but he is referenced by a play log entry " + this.entries[num] + ".");
					this.RemoveEntry(this.entries[num]);
				}
			}
		}

		private void RemoveEntry(PlayLogEntry entry)
		{
			this.entries.Remove(entry);
			entry.PostRemove();
		}

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
