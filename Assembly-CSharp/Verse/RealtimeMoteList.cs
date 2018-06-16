using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DE9 RID: 3561
	public class RealtimeMoteList
	{
		// Token: 0x06004FA6 RID: 20390 RVA: 0x00295C99 File Offset: 0x00294099
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x00295CA7 File Offset: 0x002940A7
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x00295CB6 File Offset: 0x002940B6
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x00295CC8 File Offset: 0x002940C8
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}

		// Token: 0x040034CD RID: 13517
		public List<Mote> allMotes = new List<Mote>();
	}
}
