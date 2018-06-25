using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000526 RID: 1318
	public class Pawn_GuiltTracker : IExposable
	{
		// Token: 0x04000E54 RID: 3668
		public int lastGuiltyTick = -99999;

		// Token: 0x04000E55 RID: 3669
		private const int GuiltyDuration = 60000;

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x0600181D RID: 6173 RVA: 0x000D2958 File Offset: 0x000D0D58
		public bool IsGuilty
		{
			get
			{
				return this.TicksUntilInnocent > 0;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x0600181E RID: 6174 RVA: 0x000D2978 File Offset: 0x000D0D78
		public int TicksUntilInnocent
		{
			get
			{
				return Mathf.Max(0, this.lastGuiltyTick + 60000 - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x000D29AA File Offset: 0x000D0DAA
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastGuiltyTick, "lastGuiltyTick", -99999, false);
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x000D29C3 File Offset: 0x000D0DC3
		public void Notify_Guilty()
		{
			this.lastGuiltyTick = Find.TickManager.TicksGame;
		}
	}
}
