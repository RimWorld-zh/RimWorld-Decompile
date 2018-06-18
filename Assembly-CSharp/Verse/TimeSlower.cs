using System;

namespace Verse
{
	// Token: 0x02000BD6 RID: 3030
	public class TimeSlower
	{
		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06004206 RID: 16902 RVA: 0x0022CA28 File Offset: 0x0022AE28
		public bool ForcedNormalSpeed
		{
			get
			{
				return Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x0022CA4F File Offset: 0x0022AE4F
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Find.TickManager.TicksGame + 790;
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x0022CA68 File Offset: 0x0022AE68
		public void SignalForceNormalSpeedShort()
		{
			this.forceNormalSpeedUntil = Find.TickManager.TicksGame + 250;
		}

		// Token: 0x04002D22 RID: 11554
		private int forceNormalSpeedUntil;

		// Token: 0x04002D23 RID: 11555
		private const int ForceTicksStandard = 790;

		// Token: 0x04002D24 RID: 11556
		private const int ForceTicksShort = 250;
	}
}
