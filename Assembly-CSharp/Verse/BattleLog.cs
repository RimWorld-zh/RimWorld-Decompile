using System.Collections.Generic;

namespace Verse
{
	public class BattleLog : IExposable
	{
		private List<LogEntry> rawEntries = new List<LogEntry>();

		private const int BattleHistoryLength = 200;

		public List<LogEntry> RawEntries
		{
			get
			{
				return this.rawEntries;
			}
		}

		public void Add(LogEntry entry)
		{
			this.rawEntries.Insert(0, entry);
			this.ReduceToCapacity();
		}

		private void ReduceToCapacity()
		{
			if (this.rawEntries.Count > 200)
			{
				this.rawEntries.RemoveRange(200, this.rawEntries.Count - 200);
			}
		}

		public void ExposeData()
		{
			Scribe_Collections.Look<LogEntry>(ref this.rawEntries, "rawEntries", LookMode.Deep, new object[0]);
		}

		public bool AnyEntryConcerns(Pawn p)
		{
			int num = 0;
			bool result;
			while (true)
			{
				if (num < this.rawEntries.Count)
				{
					if (this.rawEntries[num].Concerns(p))
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

		public void Notify_PawnDiscarded(Pawn p, bool silentlyRemoveReferences)
		{
			for (int num = this.rawEntries.Count - 1; num >= 0; num--)
			{
				if (this.rawEntries[num].Concerns(p))
				{
					if (!silentlyRemoveReferences)
					{
						Log.Warning("Discarding pawn " + p + ", but he is referenced by a battle log entry " + this.rawEntries[num] + ".");
					}
					this.rawEntries.Remove(this.rawEntries[num]);
				}
			}
		}
	}
}
