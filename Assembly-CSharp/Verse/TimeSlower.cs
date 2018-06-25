using System;

namespace Verse
{
	// Token: 0x02000BD4 RID: 3028
	public class TimeSlower
	{
		// Token: 0x04002D27 RID: 11559
		private int forceNormalSpeedUntil;

		// Token: 0x04002D28 RID: 11560
		private const int ForceTicksStandard = 790;

		// Token: 0x04002D29 RID: 11561
		private const int ForceTicksShort = 250;

		// Token: 0x17000A54 RID: 2644
		// (get) Token: 0x0600420B RID: 16907 RVA: 0x0022D220 File Offset: 0x0022B620
		public bool ForcedNormalSpeed
		{
			get
			{
				return Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x0600420C RID: 16908 RVA: 0x0022D247 File Offset: 0x0022B647
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Find.TickManager.TicksGame + 790;
		}

		// Token: 0x0600420D RID: 16909 RVA: 0x0022D260 File Offset: 0x0022B660
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Find.TickManager.TicksGame + 250;
		}
	}
}
