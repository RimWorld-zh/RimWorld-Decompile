using System;

namespace Verse
{
	// Token: 0x02000BD5 RID: 3029
	public class TimeSlower
	{
		// Token: 0x04002D2E RID: 11566
		private int forceNormalSpeedUntil;

		// Token: 0x04002D2F RID: 11567
		private const int ForceTicksStandard = 790;

		// Token: 0x04002D30 RID: 11568
		private const int ForceTicksShort = 250;

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x0022D500 File Offset: 0x0022B900
		public bool ForcedNormalSpeed
		{
			get
			{
				return Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x0022D527 File Offset: 0x0022B927
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Find.TickManager.TicksGame + 790;
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x0022D540 File Offset: 0x0022B940
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Find.TickManager.TicksGame + 250;
		}
	}
}
