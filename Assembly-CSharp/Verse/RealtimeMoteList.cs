using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DE5 RID: 3557
	public class RealtimeMoteList
	{
		// Token: 0x06004FB9 RID: 20409 RVA: 0x00297255 File Offset: 0x00295655
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x00297263 File Offset: 0x00295663
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x00297272 File Offset: 0x00295672
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x00297284 File Offset: 0x00295684
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}

		// Token: 0x040034D6 RID: 13526
		public List<Mote> allMotes = new List<Mote>();
	}
}
