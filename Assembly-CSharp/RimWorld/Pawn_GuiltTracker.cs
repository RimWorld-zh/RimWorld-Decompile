using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000528 RID: 1320
	public class Pawn_GuiltTracker : IExposable
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001822 RID: 6178 RVA: 0x000D2810 File Offset: 0x000D0C10
		public bool IsGuilty
		{
			get
			{
				return this.TicksUntilInnocent > 0;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x06001823 RID: 6179 RVA: 0x000D2830 File Offset: 0x000D0C30
		public int TicksUntilInnocent
		{
			get
			{
				return Mathf.Max(0, this.lastGuiltyTick + 60000 - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x000D2862 File Offset: 0x000D0C62
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastGuiltyTick, "lastGuiltyTick", -99999, false);
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x000D287B File Offset: 0x000D0C7B
		public void Notify_Guilty()
		{
			this.lastGuiltyTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04000E57 RID: 3671
		public int lastGuiltyTick = -99999;

		// Token: 0x04000E58 RID: 3672
		private const int GuiltyDuration = 60000;
	}
}
