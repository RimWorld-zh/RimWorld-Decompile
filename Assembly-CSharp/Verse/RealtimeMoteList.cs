using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000DE7 RID: 3559
	public class RealtimeMoteList
	{
		// Token: 0x040034D6 RID: 13526
		public List<Mote> allMotes = new List<Mote>();

		// Token: 0x06004FBD RID: 20413 RVA: 0x00297381 File Offset: 0x00295781
		public void Clear()
		{
			this.allMotes.Clear();
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x0029738F File Offset: 0x0029578F
		public void MoteSpawned(Mote newMote)
		{
			this.allMotes.Add(newMote);
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x0029739E File Offset: 0x0029579E
		public void MoteDespawned(Mote oldMote)
		{
			this.allMotes.Remove(oldMote);
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x002973B0 File Offset: 0x002957B0
		public void MoteListUpdate()
		{
			for (int i = this.allMotes.Count - 1; i >= 0; i--)
			{
				this.allMotes[i].RealtimeUpdate();
			}
		}
	}
}
