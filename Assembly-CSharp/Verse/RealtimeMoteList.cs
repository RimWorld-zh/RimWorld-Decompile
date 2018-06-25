using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DE8 RID: 3560
	public class RealtimeMoteList
	{
		// Token: 0x040034DD RID: 13533
		public List<Mote> allMotes = new List<Mote>();

		// Token: 0x06004FBD RID: 20413 RVA: 0x00297661 File Offset: 0x00295A61
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x0029766F File Offset: 0x00295A6F
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x0029767E File Offset: 0x00295A7E
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x00297690 File Offset: 0x00295A90
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}
	}
}
