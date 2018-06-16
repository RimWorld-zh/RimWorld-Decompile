using System;

namespace Verse
{
	// Token: 0x02000BD6 RID: 3030
	public class TimeSlower
	{
		// Token: 0x17000A53 RID: 2643
		// (get) Token: 0x06004204 RID: 16900 RVA: 0x0022C9B0 File Offset: 0x0022ADB0
		public bool ForcedNormalSpeed
		{
			get
			{
				return Find.TickManager.TicksGame < this.forceNormalSpeedUntil;
			}
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x0022C9D7 File Offset: 0x0022ADD7
		public void SignalForceNormalSpeed()
		{
			this.forceNormalSpeedUntil = Find.TickManager.TicksGame + 790;
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x0022C9F0 File Offset: 0x0022ADF0
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
