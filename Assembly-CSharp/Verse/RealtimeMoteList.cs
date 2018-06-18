using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DE8 RID: 3560
	public class RealtimeMoteList
	{
		// Token: 0x06004FA4 RID: 20388 RVA: 0x00295C79 File Offset: 0x00294079
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x00295C87 File Offset: 0x00294087
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x00295C96 File Offset: 0x00294096
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x00295CA8 File Offset: 0x002940A8
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}

		// Token: 0x040034CB RID: 13515
		public List<Mote> allMotes = new List<Mote>();
	}
}
