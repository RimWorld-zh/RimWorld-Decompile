using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000524 RID: 1316
	public class Pawn_GuiltTracker : IExposable
	{
		// Token: 0x1700035C RID: 860
		// (get) Token: 0x06001819 RID: 6169 RVA: 0x000D2808 File Offset: 0x000D0C08
		public bool IsGuilty
		{
			get
			{
				return this.TicksUntilInnocent > 0;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600181A RID: 6170 RVA: 0x000D2828 File Offset: 0x000D0C28
		public int TicksUntilInnocent
		{
			get
			{
				return Mathf.Max(0, this.lastGuiltyTick + 60000 - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x000D285A File Offset: 0x000D0C5A
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastGuiltyTick, "lastGuiltyTick", -99999, false);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x000D2873 File Offset: 0x000D0C73
		public void Notify_Guilty()
		{
			this.lastGuiltyTick = Find.TickManager.TicksGame;
		}

		// Token: 0x04000E54 RID: 3668
		public int lastGuiltyTick = -99999;

		// Token: 0x04000E55 RID: 3669
		private const int GuiltyDuration = 60000;
	}
}
