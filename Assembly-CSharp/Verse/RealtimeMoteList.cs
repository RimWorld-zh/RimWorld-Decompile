using System.Collections.Generic;

namespace Verse
{
	public class RealtimeMoteList
	{
		public List<Mote> allMotes = new List<Mote>();

		public void Clear()
		{
			this.allMotes.Clear();
		}

		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		public void MoteListUpdate()
		{
			for (int num = this.allMotes.Count - 1; num >= 0; num--)
			{
				this.allMotes[num].RealtimeUpdate();
			}
		}
	}
}
